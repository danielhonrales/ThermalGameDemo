using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Local mirror of what the forearm hardware is doing. Every Pi event starts a build-up that
/// peaks <see cref="CombatEventOutput.HardwareLeadSeconds"/> later, when the Peltiers and
/// vibros are expected to be felt. Visuals read <see cref="Sample"/> instead of combat code.
/// </summary>
public static class ArmActivationSignal
{
    public enum Kind { Heat, Cold, Shield, Hit, Heal }

    public struct Reading
    {
        /// <summary>0 idle .. ~1.5 maximum; peltier-style slow intensity.</summary>
        public float Intensity;
        /// <summary>0..1 fast buzz amplitude, matching vibro bursts.</summary>
        public float Vibration;
        /// <summary>0..1 inside a build-up window, rising toward the peak.</summary>
        public float Anticipation;
        public Color Color;
    }

    private sealed class Pulse
    {
        public Kind Kind;
        public string State;
        public float StartedAt, Strength, Decay, Vibration;
        public float ReleasedAt = -1f;
    }

    private static readonly List<Pulse> pulses = new List<Pulse>();
    private static bool subscribed;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Subscribe()
    {
        pulses.Clear();
        CombatEventOutput.Signaled -= OnSignal;
        CombatEventOutput.Signaled += OnSignal;
        subscribed = true;
    }

    public static Color ColorFor(Kind kind)
    {
        switch (kind)
        {
            case Kind.Cold: return CombatVfxStyle.Cold;
            case Kind.Shield: return CombatVfxStyle.Shield;
            case Kind.Hit: return CombatVfxStyle.Critical;
            case Kind.Heal: return HealPickupView.Green;
            default: return CombatVfxStyle.Heat;
        }
    }

    private static void OnSignal(string name, string source)
    {
        float now = Time.time;
        switch (name)
        {
            // Sustained states: build up, hold while active, fade on _stop.
            case "fire_charge_start": Hold("fire_charge", Kind.Heat, 0.35f, 0.1f, now); break;
            case "fire_start": Hold("fire", Kind.Heat, 1.1f, 0.55f, now); break;
            case "ice_charge_start": Hold("ice_charge", Kind.Cold, 0.35f, 0.1f, now); break;
            case "ice_flight_start": Hold("ice_flight", Kind.Cold, 0.45f, 0.15f, now); break;
            case "shield_start": Hold("shield", Kind.Shield, 0.55f, 0.2f, now); break;
            case "hazard_warning_start": Hold("hazard_warning", Kind.Heat, 0.3f, 0.35f, now); break;
            case "hazard_start": Hold("hazard", Kind.Heat, 0.9f, 0.7f, now); break;
            // One-shot pulses.
            case "ice_shot": Burst(Kind.Cold, 1.2f, 0.7f, 0.8f, now); break;
            case "shield_block": Burst(Kind.Shield, 1.1f, 0.5f, 1f, now); break;
            case "hit_received":
                Burst(source == "ice" ? Kind.Cold : Kind.Hit, 1.5f, 0.9f, 1f, now); break;
            case "heal_received": Burst(Kind.Heal, 1.3f, 1.2f, 0.5f, now); break;
            case "fire_cancel": Cancel(Kind.Heat, now); break;
            case "ice_cancel": Cancel(Kind.Cold, now); break;
            case "session_pause":
            case "round_disconnected":
                pulses.Clear(); break;
            default:
                if (name.EndsWith("_stop")) Release(name.Substring(0, name.Length - 5), now);
                break;
        }
    }

    private static void Hold(string state, Kind kind, float strength, float vibration, float now)
    {
        Release(state, now);
        pulses.Add(new Pulse { Kind = kind, State = state, StartedAt = now,
            Strength = strength, Decay = 0.35f, Vibration = vibration });
    }

    private static void Burst(Kind kind, float strength, float decay, float vibration, float now)
        => pulses.Add(new Pulse { Kind = kind, StartedAt = now, Strength = strength,
            Decay = decay, Vibration = vibration, ReleasedAt = now + CombatEventOutput.HardwareLeadSeconds });

    private static void Release(string state, float now)
    {
        foreach (Pulse pulse in pulses)
            if (pulse.State == state && pulse.ReleasedAt < 0f)
                pulse.ReleasedAt = Mathf.Max(now, pulse.StartedAt + CombatEventOutput.HardwareLeadSeconds);
    }

    // A cancelled early signal never reached its peak: remove the pending build-up quietly.
    private static void Cancel(Kind kind, float now)
    {
        float lead = CombatEventOutput.HardwareLeadSeconds;
        pulses.RemoveAll(p => p.Kind == kind && now - p.StartedAt < lead);
    }

    /// <summary>Envelope of one pulse: eased build-up, sharp peak, hold, then exponential fade.</summary>
    private static float Envelope(Pulse pulse, float now, out float anticipation, out float spike)
    {
        float lead = CombatEventOutput.HardwareLeadSeconds;
        float age = now - pulse.StartedAt;
        anticipation = 0f;
        spike = 0f;
        if (age < lead)
        {
            float t = age / lead;
            anticipation = t;
            return pulse.Strength * 0.35f * t * t;
        }
        float sincePeak = age - lead;
        spike = Mathf.Exp(-sincePeak * 9f);
        float level = pulse.Strength * (1f + 0.6f * spike);
        if (pulse.ReleasedAt >= 0f && now > pulse.ReleasedAt)
            level *= Mathf.Exp(-(now - pulse.ReleasedAt) / Mathf.Max(0.05f, pulse.Decay) * 2.3f);
        return level;
    }

    public static Reading Sample(float now)
    {
        if (!subscribed) Subscribe();
        var reading = new Reading { Color = CombatVfxStyle.Neutral };
        float total = 0f;
        Color mix = Color.black;
        for (int i = pulses.Count - 1; i >= 0; i--)
        {
            Pulse pulse = pulses[i];
            float level = Envelope(pulse, now, out float anticipation, out float spike);
            if (pulse.ReleasedAt >= 0f && level < 0.01f && now > pulse.ReleasedAt)
            {
                pulses.RemoveAt(i);
                continue;
            }
            total += level;
            mix += ColorFor(pulse.Kind) * level;
            reading.Anticipation = Mathf.Max(reading.Anticipation, anticipation);
            reading.Vibration = Mathf.Max(reading.Vibration,
                pulse.Vibration * Mathf.Max(spike, level / Mathf.Max(0.01f, pulse.Strength) * 0.4f));
        }
        reading.Intensity = Mathf.Min(1.5f, total);
        if (total > 0.001f)
            reading.Color = Color.Lerp(CombatVfxStyle.Neutral, mix / total, Mathf.Clamp01(total * 2.5f));
        reading.Color.a = 1f;
        return reading;
    }
}
