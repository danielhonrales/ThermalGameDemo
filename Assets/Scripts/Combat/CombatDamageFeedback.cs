using UnityEngine;
using UnityEngine.Rendering;

/// <summary>Pooled, short-lived hit pixels around a damaged player's head.</summary>
[DisallowMultipleComponent]
public sealed class CombatDamageFeedback : MonoBehaviour
{
    private const int PixelCount = 22;
    private readonly Transform[] pixels = new Transform[PixelCount];
    private readonly Vector3[] directions = new Vector3[PixelCount];
    private Transform burstRoot;
    private LineRenderer ring;
    private Material pixelMaterial;
    private Material lineMaterial;
    private Mesh pixelMesh;
    private float startedAt = -1f;

    private void Awake()
    {
        burstRoot = new GameObject("HitPixels").transform;
        pixelMesh = CombatVfxStyle.CreateShard();
        pixelMaterial = CombatVfxStyle.CreateMaterial("Hit pixels", CombatVfxStyle.HeatCore);
        lineMaterial = CombatVfxStyle.CreateMaterial("Hit pulse", Color.white);
        ring = CombatVfxStyle.CreateLine(burstRoot, "Hit pulse", lineMaterial, false, 0.022f);
        for (int i = 0; i < PixelCount; i++)
        {
            GameObject pixel = new GameObject("Impact pixel");
            pixel.transform.SetParent(burstRoot, false);
            pixel.AddComponent<MeshFilter>().sharedMesh = pixelMesh;
            MeshRenderer renderer = pixel.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = pixelMaterial;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            pixels[i] = pixel.transform;
            float yaw = i * 2.399963f;
            float elevation = Mathf.Lerp(-0.3f, 0.8f, (i % 7) / 6f);
            directions[i] = new Vector3(Mathf.Cos(yaw), elevation,
                Mathf.Sin(yaw)).normalized;
        }
        burstRoot.gameObject.SetActive(false);
    }

    public void Play(Vector3 worldPosition)
    {
        burstRoot.position = worldPosition;
        burstRoot.gameObject.SetActive(true);
        startedAt = Time.time;
    }

    private void Update()
    {
        if (startedAt < 0f) return;
        float age = (Time.time - startedAt) / 0.58f;
        if (age >= 1f)
        {
            startedAt = -1f;
            burstRoot.gameObject.SetActive(false);
            return;
        }
        Camera camera = Camera.main;
        if (camera != null)
            burstRoot.rotation = Quaternion.LookRotation(camera.transform.position - burstRoot.position);
        CombatVfxStyle.SetRing(ring, Vector3.zero, Quaternion.identity,
            Mathf.Lerp(0.07f, 0.36f, age), 40);
        Color tint = CombatVfxStyle.WithAlpha(CombatVfxStyle.HeatCore, 1f - age);
        ring.startColor = ring.endColor = tint;
        for (int i = 0; i < PixelCount; i++)
        {
            Transform pixel = pixels[i];
            float distance = (0.06f + age * 0.52f) * (0.65f + (i % 5) * 0.11f);
            pixel.localPosition = directions[i] * distance;
            pixel.localRotation = Quaternion.Euler(Time.time * (90f + i * 7f),
                i * 41f, Time.time * 70f);
            pixel.localScale = Vector3.one * (0.055f + (i % 4) * 0.015f) * (1f - age);
        }
    }

    private void OnDestroy()
    {
        if (burstRoot != null) Destroy(burstRoot.gameObject);
        if (pixelMaterial != null) Destroy(pixelMaterial);
        if (lineMaterial != null) Destroy(lineMaterial);
        if (pixelMesh != null) Destroy(pixelMesh);
    }
}
