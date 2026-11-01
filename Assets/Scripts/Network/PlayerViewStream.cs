using System;
using System.IO;

/// <summary>
/// Live player views for the spectator PC: each headset sends what its player sees (passthrough
/// camera plus the game's virtual content) as JPEG frames over TCP. The spectator broadcasts
/// "THERMAL-SPECTATOR-1:&lt;port&gt;" to UDP <see cref="BeaconPort"/> once a second; a headset that hears
/// it connects to that port. Each frame is: int32 player network ID, int32 JPEG length, the JPEG
/// (little-endian). See <see cref="PlayerViewSender"/> and <see cref="PlayerViewReceiver"/>.
/// </summary>
public static class PlayerViewStream
{
    public const int BeaconPort = 7783, StreamPort = 7784;
    public const string BeaconPrefix = "THERMAL-SPECTATOR-1:";
    public const int Width = 960, FramesPerSecond = 15, JpegQuality = 65;
    public const int MaxFrameBytes = 4 * 1024 * 1024;
    // A spectator or a stream unheard for this long is treated as gone.
    public const float TimeoutSeconds = 4f;

    public static bool TryParseBeacon(string text, out int port)
    {
        port = 0;
        return text != null && text.StartsWith(BeaconPrefix, StringComparison.Ordinal)
            && int.TryParse(text.Substring(BeaconPrefix.Length), out port) && port > 0 && port < 65536;
    }

    public static void WriteFrame(Stream stream, int playerId, byte[] jpeg)
    {
        var header = new byte[8];
        BitConverter.TryWriteBytes(new Span<byte>(header, 0, 4), playerId);
        BitConverter.TryWriteBytes(new Span<byte>(header, 4, 4), jpeg.Length);
        if (!BitConverter.IsLittleEndian) { Array.Reverse(header, 0, 4); Array.Reverse(header, 4, 4); }
        stream.Write(header, 0, header.Length);
        stream.Write(jpeg, 0, jpeg.Length);
        stream.Flush();
    }

    /// <summary>Reads one frame; false at the end of the stream. Throws on a malformed frame.</summary>
    public static bool TryReadFrame(Stream stream, out int playerId, out byte[] jpeg)
    {
        playerId = 0;
        jpeg = null;
        var header = new byte[8];
        if (!ReadExactly(stream, header)) return false;
        if (!BitConverter.IsLittleEndian) { Array.Reverse(header, 0, 4); Array.Reverse(header, 4, 4); }
        playerId = BitConverter.ToInt32(header, 0);
        int length = BitConverter.ToInt32(header, 4);
        if (length <= 0 || length > MaxFrameBytes) throw new InvalidDataException($"Bad player view frame length {length}.");
        jpeg = new byte[length];
        return ReadExactly(stream, jpeg);
    }

    private static bool ReadExactly(Stream stream, byte[] buffer)
    {
        int read = 0;
        while (read < buffer.Length)
        {
            int n = stream.Read(buffer, read, buffer.Length - read);
            if (n <= 0) return false;
            read += n;
        }
        return true;
    }
}
