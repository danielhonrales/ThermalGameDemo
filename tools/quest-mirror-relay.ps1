<#
.SYNOPSIS
Sits between one scrcpy window and its headset and skips late video instead of fast-forwarding through it.

.DESCRIPTION
Started by quest-mirror.ps1, one per window; not meant to be run by hand. scrcpy connects to
ListenPort (its --tunnel-port) and the relay connects on to TargetPort, scrcpy's adb forward to the
headset (its --port). After a Wi-Fi stall the headset's queued frames all arrive at once, and scrcpy
would show them fast-forward. The relay compares each frame's capture time with when it arrives:
once a frame is more than LateMs behind, it drops frames until a key frame arrives on time (or
MaxFreezeMs has passed), so the window holds its last picture and then jumps back to live. Dropping
has to run to a key frame, as the frames in between are built on the dropped ones; quest-mirror.ps1
asks the headset for a key frame every second. It exits once its window's stream ends, or if nothing
connects within IdleExitSeconds. It logs each skip to %TEMP%\quest-mirror-relay-<ListenPort>.log.
#>
param(
    [Parameter(Mandatory)][int]$ListenPort,
    [Parameter(Mandatory)][int]$TargetPort,
    [int]$LateMs = 200,
    [int]$MaxFreezeMs = 2500,
    [int]$IdleExitSeconds = 30
)
$ErrorActionPreference = 'Stop'

Add-Type -TypeDefinition @'
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public static class QuestMirrorRelay
{
    // scrcpy's video stream (forward tunnel): a dummy byte, 64 bytes of device name, 12 bytes of codec
    // id and size, then packets of a 12-byte header (8: PTS in microseconds, top bits flags; 4: length)
    // and the encoded frame.
    const ulong ConfigFlag = 1UL << 63, KeyFlag = 1UL << 62, PtsMask = (1UL << 62) - 1;
    const int MaxPacket = 32 * 1024 * 1024;
    const int WindowSeconds = 10;

    static TextWriter log;

    public static void Run(int listenPort, int targetPort, int lateMs, int maxFreezeMs, int idleExitSeconds, string logPath)
    {
        log = TextWriter.Synchronized(new StreamWriter(logPath, false) { AutoFlush = true });
        var listener = new TcpListener(IPAddress.Loopback, listenPort);
        listener.Start();
        Log("listening on " + listenPort + ", forwarding to " + targetPort + "; skipping frames over " + lateMs + " ms late");
        var idleSince = Stopwatch.StartNew();
        while (true)
        {
            if (!listener.Pending())
            {
                if (idleSince.Elapsed.TotalSeconds > idleExitSeconds) { Log("nothing connected; exiting"); break; }
                Thread.Sleep(20);
                continue;
            }
            TcpClient client = listener.AcceptTcpClient();
            bool streamed = Session(client, targetPort, lateMs, maxFreezeMs);
            // scrcpy retries until the headset's server is up; the first session that carried video
            // is the window's, and it ending means the window closed.
            if (streamed) { Log("stream ended; exiting"); break; }
            idleSince.Restart();
        }
        listener.Stop();
    }

    static void Log(string text)
    {
        log.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + "  " + text);
    }

    static bool Session(TcpClient client, int targetPort, int lateMs, int maxFreezeMs)
    {
        TcpClient upstream = new TcpClient();
        try
        {
            upstream.Connect(IPAddress.Loopback, targetPort);
        }
        catch (SocketException)
        {
            client.Close();
            return false;
        }
        client.NoDelay = upstream.NoDelay = true;
        NetworkStream fromHeadset = upstream.GetStream(), toWindow = client.GetStream();
        // Anything the window sends goes straight through.
        var back = new Thread(() =>
        {
            try { client.GetStream().CopyTo(upstream.GetStream()); } catch (Exception) { }
        }) { IsBackground = true };
        back.Start();
        bool streamed = false;
        try
        {
            streamed = Pump(fromHeadset, toWindow, lateMs, maxFreezeMs);
        }
        catch (IOException) { }
        catch (ObjectDisposedException) { }
        client.Close();
        upstream.Close();
        return streamed;
    }

    static bool ReadExact(Stream stream, byte[] buffer, int count)
    {
        int read = 0;
        while (read < count)
        {
            int n = stream.Read(buffer, read, count - read);
            if (n <= 0) return false;
            read += n;
        }
        return true;
    }

    static ulong BigEndian(byte[] b, int at, int bytes)
    {
        ulong value = 0;
        for (int i = 0; i < bytes; i++) value = (value << 8) | b[at + i];
        return value;
    }

    static bool StartsWithStartCode(byte[] b, int length)
    {
        return length >= 4 && b[0] == 0 && b[1] == 0 && (b[2] == 1 || (b[2] == 0 && b[3] == 1));
    }

    // Returns true once at least one frame went through.
    static bool Pump(Stream input, Stream output, int lateMs, int maxFreezeMs)
    {
        var head = new byte[77];
        if (!ReadExact(input, head, 1)) return false;           // dummy byte: the headset is up
        output.Write(head, 0, 1);
        if (!ReadExact(input, head, 76)) return false;          // device name, codec id, size
        output.Write(head, 0, 76);
        bool known = true;
        for (int i = 64; i < 68; i++)
            if (head[i] < 0x20 || head[i] > 0x7e) known = false;    // codec ids are ASCII ("h265")
        if (!known)
        {
            Log("unexpected stream header; passing everything through");
            input.CopyTo(output);
            return true;
        }

        var clock = Stopwatch.StartNew();
        var bucketMin = new long[WindowSeconds];
        var bucketSecond = new long[WindowSeconds];
        for (int i = 0; i < WindowSeconds; i++) bucketSecond[i] = -1;
        var header = new byte[12];
        byte[] payload = new byte[1 << 20];
        bool dropping = false, streamed = false;
        long droppingSince = 0, dropped = 0, skips = 0, latest = 0;
        while (true)
        {
            if (!ReadExact(input, header, 12)) return streamed;
            ulong ptsFlags = BigEndian(header, 0, 8);
            int length = (int)BigEndian(header, 8, 4);
            if (length < 0 || length > MaxPacket)
            {
                Log("unexpected packet length " + length + "; passing everything through");
                output.Write(header, 0, 12);
                input.CopyTo(output);
                return streamed;
            }
            if (payload.Length < length) payload = new byte[Math.Max(length, payload.Length * 2)];
            if (!ReadExact(input, payload, length)) return streamed;
            if (!StartsWithStartCode(payload, length))
            {
                Log("packet without a start code; passing everything through");
                output.Write(header, 0, 12);
                output.Write(payload, 0, length);
                input.CopyTo(output);
                return streamed;
            }

            bool forward = true;
            if ((ptsFlags & ConfigFlag) == 0)
            {
                bool key = (ptsFlags & KeyFlag) != 0;
                long nowUs = clock.ElapsedTicks * 1000000L / Stopwatch.Frequency;
                long offset = nowUs - (long)(ptsFlags & PtsMask);
                // The quickest a frame has come through in the last few seconds is "on time"; how
                // much later than that this one is is its delay.
                long second = nowUs / 1000000L;
                int slot = (int)(second % WindowSeconds);
                if (bucketSecond[slot] != second) { bucketSecond[slot] = second; bucketMin[slot] = offset; }
                else if (offset < bucketMin[slot]) bucketMin[slot] = offset;
                long baseline = long.MaxValue;
                for (int i = 0; i < WindowSeconds; i++)
                    if (bucketSecond[i] > second - WindowSeconds && bucketMin[i] < baseline) baseline = bucketMin[i];
                long lateUs = offset - baseline;
                long nowMs = nowUs / 1000;

                if (!dropping && lateUs > lateMs * 1000L)
                {
                    dropping = true;
                    droppingSince = nowMs;
                    dropped = 0;
                    latest = 0;
                    skips++;
                }
                if (dropping && lateUs > latest) latest = lateUs;
                if (dropping)
                {
                    if (key && (lateUs <= lateMs * 1000L || nowMs - droppingSince > maxFreezeMs))
                    {
                        dropping = false;
                        // Still late after the longest freeze: the link has slowed for good, so this is
                        // the new "on time" rather than a backlog to skip again and again.
                        if (lateUs > lateMs * 1000L)
                            for (int i = 0; i < WindowSeconds; i++) bucketSecond[i] = -1;
                        Log("skip " + skips + ": dropped " + dropped + " frames up to " + latest / 1000 + " ms late; held the picture " + (nowMs - droppingSince) + " ms");
                    }
                    else
                    {
                        forward = false;
                        dropped++;
                    }
                }
            }
            if (forward)
            {
                output.Write(header, 0, 12);
                output.Write(payload, 0, length);
                streamed = true;
            }
        }
    }
}
'@

$logPath = Join-Path $env:TEMP "quest-mirror-relay-$ListenPort.log"
[QuestMirrorRelay]::Run($ListenPort, $TargetPort, $LateMs, $MaxFreezeMs, $IdleExitSeconds, $logPath)
