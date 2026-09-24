using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Small procedural sound bank for match feedback (sirens, heartbeat, drone rotors, impacts).
/// Clips are synthesised once on first use, so there are no extra audio assets to ship.
/// </summary>
public static class SynthAudio
{
    private const int Rate = 44100;
    private static readonly Dictionary<string, AudioClip> cache = new Dictionary<string, AudioClip>();
    private static uint noiseState = 0x9E3779B9u;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics() => cache.Clear();

    private delegate float Sample(float t, float duration);

    private static AudioClip Build(string name, float seconds, Sample sample, float gain = 1f)
    {
        if (cache.TryGetValue(name, out AudioClip existing) && existing != null) return existing;
        int count = Mathf.CeilToInt(seconds * Rate);
        var data = new float[count];
        float peak = 0.0001f;
        for (int i = 0; i < count; i++)
        {
            data[i] = sample(i / (float)Rate, seconds);
            peak = Mathf.Max(peak, Mathf.Abs(data[i]));
        }
        float normalise = gain * 0.9f / peak;
        for (int i = 0; i < count; i++) data[i] *= normalise;
        AudioClip clip = AudioClip.Create(name, count, 1, Rate, false);
        clip.SetData(data, 0);
        cache[name] = clip;
        return clip;
    }

    private static float Noise()
    {
        noiseState ^= noiseState << 13;
        noiseState ^= noiseState >> 17;
        noiseState ^= noiseState << 5;
        return (noiseState / (float)uint.MaxValue) * 2f - 1f;
    }

    private static float Sine(float hz, float t) => Mathf.Sin(2f * Mathf.PI * hz * t);
    private static float Saw(float hz, float t) => 2f * (t * hz - Mathf.Floor(0.5f + t * hz));
    private static float Tri(float hz, float t) => 2f * Mathf.Abs(Saw(hz, t)) - 1f;

    /// <summary>Two-tone alarm sweep, loops seamlessly (2 s).</summary>
    public static AudioClip Siren() => Build("siren", 2f, (t, d) =>
    {
        float sweep = 0.5f - 0.5f * Mathf.Cos(2f * Mathf.PI * t / d);
        float phase = 2f * Mathf.PI * (520f * t + 480f * (t / 2f - d / (4f * Mathf.PI) * Mathf.Sin(2f * Mathf.PI * t / d)));
        float tone = Mathf.Sin(phase) + 0.45f * Mathf.Sin(phase * 2f) + 0.2f * Mathf.Sin(phase * 3f);
        return Mathf.Clamp(tone * 0.8f, -1f, 1f) * (0.75f + 0.25f * sweep);
    }, 0.8f);

    /// <summary>Lub-dub heartbeat (1 s loop at 60 bpm).</summary>
    public static AudioClip Heartbeat() => Build("heartbeat", 1f, (t, d) =>
    {
        float Thump(float start, float amp)
        {
            float x = t - start;
            if (x < 0f) return 0f;
            float pitch = 48f + 40f * Mathf.Exp(-x * 30f);
            return amp * Mathf.Sin(2f * Mathf.PI * pitch * x) * Mathf.Exp(-x * 14f) * Mathf.Min(1f, x * 400f);
        }
        return Thump(0f, 1f) + Thump(0.26f, 0.7f);
    });

    /// <summary>Rotor hum with blade chop (1 s loop).</summary>
    public static AudioClip DroneHum() => Build("drone-hum", 1f, (t, d) =>
    {
        float chop = 0.6f + 0.4f * Mathf.Sin(2f * Mathf.PI * 46f * t);
        float body = Saw(92f, t) * 0.35f + Sine(184f, t) * 0.4f + Sine(276f, t) * 0.18f + Tri(552f, t) * 0.06f;
        return (body * chop + Noise() * 0.12f * chop) * 0.9f;
    }, 0.7f);

    public static AudioClip Whoosh() => Build("whoosh", 0.8f, (t, d) =>
    {
        float env = Mathf.Sin(Mathf.PI * Mathf.Clamp01(t / d)) * Mathf.Exp(-t * 1.5f);
        return LowNoise(0.06f + 0.18f * env) * env;
    });

    /// <summary>Heavy metal drop: low thud plus a short metallic ring.</summary>
    public static AudioClip Clunk() => Build("clunk", 0.7f, (t, d) =>
    {
        float thud = Mathf.Sin(2f * Mathf.PI * (55f + 60f * Mathf.Exp(-t * 25f)) * t) * Mathf.Exp(-t * 9f);
        float ring = (Sine(870f, t) + 0.6f * Sine(1333f, t) + 0.4f * Sine(2011f, t)) * Mathf.Exp(-t * 11f) * 0.25f;
        float click = Noise() * Mathf.Exp(-t * 90f) * 0.6f;
        return thud + ring + click;
    });

    /// <summary>Sudden-death slam: sub drop and a dissonant brass swell.</summary>
    public static AudioClip Stinger() => Build("stinger", 2.6f, (t, d) =>
    {
        float boom = Mathf.Sin(2f * Mathf.PI * (32f + 70f * Mathf.Exp(-t * 6f)) * t) * Mathf.Exp(-t * 1.6f);
        float swell = Mathf.Min(1f, t * 8f) * Mathf.Exp(-t * 1.1f);
        float brass = (Saw(55f, t) + Saw(58.3f, t) * 0.8f + Saw(82.4f, t) * 0.6f + Saw(110.5f, t) * 0.4f) * 0.25f;
        float hit = LowNoise(0.3f) * Mathf.Exp(-t * 7f) * 0.6f;
        return boom * 1.1f + brass * swell + hit;
    });

    public static AudioClip Tick() => Build("tick", 0.09f, (t, d) =>
        (Sine(1400f, t) + 0.4f * Sine(2800f, t)) * Mathf.Exp(-t * 55f));

    /// <summary>Deeper countdown beat for the last seconds.</summary>
    public static AudioClip CountBeat() => Build("count-beat", 0.35f, (t, d) =>
        (Sine(660f, t) * 0.6f + Sine(90f + 60f * Mathf.Exp(-t * 30f), t)) * Mathf.Exp(-t * 12f));

    public static AudioClip HitConfirm() => Build("hit-confirm", 0.16f, (t, d) =>
        (Sine(2300f, t) * 0.7f + Sine(3450f, t) * 0.4f + Noise() * Mathf.Exp(-t * 200f)) * Mathf.Exp(-t * 32f));

    public static AudioClip Explosion() => Build("explosion", 1.4f, (t, d) =>
    {
        float body = LowNoise(0.08f + 0.5f * Mathf.Exp(-t * 6f)) * Mathf.Exp(-t * 3.2f);
        float sub = Mathf.Sin(2f * Mathf.PI * (40f + 50f * Mathf.Exp(-t * 10f)) * t) * Mathf.Exp(-t * 4f);
        return body + sub * 0.9f;
    });

    public static AudioClip Rattle() => Build("rattle", 0.6f, (t, d) =>
    {
        float gate = Mathf.Sin(2f * Mathf.PI * 22f * t) > 0.3f ? 1f : 0.15f;
        return (Noise() * 0.5f + Sine(640f, t) * 0.3f) * gate * Mathf.Exp(-t * 3f);
    });

    public static AudioClip HealChime() => Build("heal-chime", 1.2f, (t, d) =>
    {
        float n1 = Sine(784f, t) * Mathf.Exp(-t * 3f);
        float n2 = t > 0.09f ? Sine(988f, t) * Mathf.Exp(-(t - 0.09f) * 3f) : 0f;
        float n3 = t > 0.18f ? Sine(1319f, t) * Mathf.Exp(-(t - 0.18f) * 2.5f) : 0f;
        return (n1 + n2 + n3) * 0.6f;
    });

    /// <summary>Low rumbling fire bed (2 s loop).</summary>
    public static AudioClip FireRoar() => Build("fire-roar", 2f, (t, d) =>
    {
        float crackle = Noise() > 0.985f ? Noise() * 0.8f : 0f;
        float roar = LowNoise(0.035f) * (0.8f + 0.2f * Mathf.Sin(2f * Mathf.PI * t / d * 3f));
        return roar + crackle;
    }, 0.8f);

    public static AudioClip PowerDown() => Build("power-down", 1.1f, (t, d) =>
        Saw(Mathf.Lerp(240f, 30f, t / d), t) * Mathf.Exp(-t * 2.2f) * 0.8f + LowNoise(0.2f) * Mathf.Exp(-t * 5f) * 0.3f);

    private static float lowState;
    private static float LowNoise(float cutoff)
    {
        lowState += (Noise() - lowState) * Mathf.Clamp01(cutoff);
        return lowState * 2.5f;
    }

    /// <summary>Plays a 2D one-shot through a shared source on the main camera.</summary>
    public static void Play2D(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        AudioSource source = SharedSource();
        if (source == null || clip == null) return;
        source.pitch = pitch;
        source.PlayOneShot(clip, volume);
    }

    /// <summary>Plays a spatial one-shot at a world position.</summary>
    public static void PlayAt(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f,
        float maxDistance = 12f)
    {
        if (clip == null) return;
        var go = new GameObject("SFX " + clip.name);
        go.transform.position = position;
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.spatialBlend = 1f;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.minDistance = 0.5f;
        source.maxDistance = maxDistance;
        source.dopplerLevel = 0f;
        source.Play();
        Object.Destroy(go, clip.length / Mathf.Max(0.1f, pitch) + 0.1f);
    }

    private static AudioSource shared;
    private static AudioSource SharedSource()
    {
        if (shared != null) return shared;
        Camera camera = Camera.main;
        if (camera == null) return null;
        var go = new GameObject("Synth UI audio");
        go.transform.SetParent(camera.transform, false);
        shared = go.AddComponent<AudioSource>();
        shared.playOnAwake = false;
        shared.spatialBlend = 0f;
        return shared;
    }
}
