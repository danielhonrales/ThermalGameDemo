using TMPro;
using UnityEngine;

/// <summary>World-space hit confirmation: an expanding X marker and a floating damage number.</summary>
public sealed class HitMarkerFx : MonoBehaviour
{
    private const float Lifetime = 0.9f;
    private readonly LineRenderer[] ticks = new LineRenderer[4];
    private TextMeshPro number;
    private Material material;
    private Color color;
    private float startedAt;
    private Vector3 drift;

    public static void Spawn(Vector3 position, int damage, Color color)
    {
        var go = new GameObject("Hit marker");
        go.transform.position = position;
        go.AddComponent<HitMarkerFx>().Init(damage, color);
    }

    private void Init(int damage, Color tint)
    {
        color = tint;
        startedAt = Time.time;
        drift = new Vector3(Random.Range(-0.12f, 0.12f), 0.35f, 0f);
        material = CombatVfxStyle.CreateMaterial("Hit marker", Color.white);
        for (int i = 0; i < ticks.Length; i++)
        {
            ticks[i] = CombatVfxStyle.CreateLine(transform, "Tick", material, false, 0.012f);
            ticks[i].positionCount = 2;
            ticks[i].enabled = true;
        }
        var textObject = new GameObject("Damage");
        textObject.transform.SetParent(transform, false);
        number = textObject.AddComponent<TextMeshPro>();
        number.font = HudKit.Heavy;
        number.text = damage > 0 ? damage.ToString() : "BLOCK";
        number.fontSize = damage > 0 ? 2.4f : 1.5f;
        number.alignment = TextAlignmentOptions.Center;
        number.outlineWidth = 0.18f;
        number.outlineColor = new Color32(0, 0, 0, 200);
        number.rectTransform.sizeDelta = new Vector2(3f, 1f);
        Destroy(gameObject, Lifetime);
    }

    private void LateUpdate()
    {
        float t = Mathf.Clamp01((Time.time - startedAt) / Lifetime);
        Camera camera = Camera.main;
        if (camera != null)
            transform.rotation = Quaternion.LookRotation(transform.position - camera.transform.position);
        float pop = 1f + 0.5f * Mathf.Exp(-t * 18f);
        float inner = Mathf.Lerp(0.05f, 0.11f, 1f - Mathf.Exp(-t * 12f));
        float outer = inner + 0.07f * (1f - t);
        for (int i = 0; i < ticks.Length; i++)
        {
            float angle = (45f + 90f * i) * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
            ticks[i].SetPosition(0, dir * inner * pop);
            ticks[i].SetPosition(1, dir * outer * pop);
            Color c = Color.Lerp(Color.white, color, t * 2f);
            ticks[i].startColor = ticks[i].endColor = CombatVfxStyle.WithAlpha(c, 1f - t);
        }
        number.transform.localPosition = new Vector3(0.22f, 0.08f, 0f) + drift * (1f - Mathf.Exp(-t * 4f));
        number.transform.localScale = Vector3.one * 0.1f * (1f + 0.7f * Mathf.Exp(-t * 14f));
        number.color = CombatVfxStyle.WithAlpha(Color.Lerp(Color.white, color, t * 3f), 1f - t * t);
    }

    private void OnDestroy()
    {
        if (material != null) Destroy(material);
    }
}
