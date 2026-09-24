using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Exaggerated, readable arm reactions that land exactly on the hardware peak (one lead after the
/// Pi is told): an electric shock with red shockwaves climbing the arm on a hit, purple sparks on a
/// block, heat or frost rings racing down to the hand on a shot, and green sparkles on a heal.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(ForearmPulseSpiral))]
public sealed class ForearmBursts : MonoBehaviour
{
    private const int RingCount = 8;
    private const float RingSeconds = 0.35f;

    private struct Ring { public float startedAt, delay; public Color color; public bool towardHand; public float width; }

    private struct Pending { public float at; public string name, source; }

    private ForearmPulseSpiral arm;
    private readonly LineRenderer[] lines = new LineRenderer[RingCount];
    private readonly Ring[] rings = new Ring[RingCount];
    private readonly List<Pending> pending = new List<Pending>();
    private Material material;
    private int nextRing;

    private void Awake()
    {
        arm = GetComponent<ForearmPulseSpiral>();
        material = CombatVfxStyle.CreateMaterial("Forearm bursts", Color.white);
        var root = new GameObject("ForearmBurstVFX").transform;
        root.SetParent(transform, false);
        for (int i = 0; i < RingCount; i++)
        {
            lines[i] = CombatVfxStyle.CreateLine(root, "Shock ring", material, true, 0.01f);
            rings[i].startedAt = -10f;
        }
    }

    private void OnEnable() => CombatEventOutput.Signaled += OnSignal;
    private void OnDisable() => CombatEventOutput.Signaled -= OnSignal;

    private void OnSignal(string name, string source)
    {
        switch (name)
        {
            case "hit_received": case "shield_block": case "fire_shot": case "ice_shot": case "heal_received":
                pending.Add(new Pending { at = Time.time + CombatEventOutput.HardwareLeadSeconds, name = name, source = source });
                break;
            case "fire_cancel": pending.RemoveAll(p => p.name == "fire_shot"); break;
            case "ice_cancel": pending.RemoveAll(p => p.name == "ice_shot"); break;
        }
    }

    private void Update()
    {
        for (int i = pending.Count - 1; i >= 0; i--)
            if (Time.time >= pending[i].at) { Fire(pending[i]); pending.RemoveAt(i); }
        DrawRings();
    }

    private void Fire(Pending burst)
    {
        if (!arm.HasPose) return;
        ThermalFxLibrary fx = ThermalFxLibrary.Instance;
        Vector3 mid = Vector3.Lerp(arm.Elbow, arm.Wrist, 0.5f);
        switch (burst.name)
        {
            case "hit_received":
                // Stay in the weapon palette with a red flash, so the arm never "changes weapon".
                Color hit = Color.Lerp(ArmActivationSignal.WeaponColor, CombatVfxStyle.Critical, 0.35f);
                ThermalFxLibrary.Spawn(fx?.electroHit, mid, 0.3f, 2f);
                for (int i = 0; i < 3; i++) AddRing(hit, false, i * 0.07f, 0.016f);
                break;
            case "shield_block":
                ThermalFxLibrary.Spawn(fx?.shieldSparks, arm.Wrist, 0.4f, 2f);
                for (int i = 0; i < 2; i++) AddRing(CombatVfxStyle.Shield, true, i * 0.06f, 0.014f);
                break;
            case "fire_shot":
                AddRing(CombatVfxStyle.Heat, true, 0f, 0.018f);
                AddRing(CombatVfxStyle.HeatCore, true, 0.05f, 0.008f);
                break;
            case "ice_shot":
                ThermalFxLibrary.Spawn(fx?.coldSparks, arm.Wrist, 0.35f, 2f);
                AddRing(CombatVfxStyle.Cold, true, 0f, 0.018f);
                AddRing(CombatVfxStyle.ColdCore, true, 0.05f, 0.008f);
                break;
            case "heal_received":
                ThermalFxLibrary.Spawn(fx?.healSparks, mid, 0.5f, 2.5f);
                for (int i = 0; i < 3; i++)
                    AddRing(Color.Lerp(ArmActivationSignal.WeaponColor, HealPickupView.Green, 0.4f), false, i * 0.09f, 0.012f);
                break;
        }
    }

    private void AddRing(Color color, bool towardHand, float delay, float width)
    {
        rings[nextRing] = new Ring { startedAt = Time.time, delay = delay, color = color, towardHand = towardHand, width = width };
        nextRing = (nextRing + 1) % RingCount;
    }

    private void DrawRings()
    {
        bool pose = arm.HasPose;
        Vector3 elbow = arm.Elbow, wrist = arm.Wrist;
        Quaternion plane = arm.ArmRotation;
        for (int i = 0; i < RingCount; i++)
        {
            float t = (Time.time - rings[i].startedAt - rings[i].delay) / RingSeconds;
            LineRenderer line = lines[i];
            if (!pose || t < 0f || t > 1f) { line.enabled = false; continue; }
            // Rings race along the arm and flare outward as they travel.
            float along = rings[i].towardHand ? t : 1f - t;
            Vector3 centre = Vector3.Lerp(elbow, wrist + (wrist - elbow).normalized * 0.06f, along);
            float radius = arm.ArmRadius * (1.1f + 1.4f * t);
            CombatVfxStyle.SetRing(line, centre, plane, radius, 32, t * 180f, 360f);
            line.widthMultiplier = rings[i].width * (1f - t * 0.6f);
            Color c = Color.Lerp(Color.white, rings[i].color, Mathf.Clamp01(t * 3f));
            line.startColor = line.endColor = CombatVfxStyle.WithAlpha(c, 1f - t * t);
            line.enabled = true;
        }
    }

    private void OnDestroy()
    {
        if (material != null) Destroy(material);
    }
}
