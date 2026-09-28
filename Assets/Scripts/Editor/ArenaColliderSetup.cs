using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class ArenaColliderSetup
{
    [MenuItem("Thermal/Arena/Sync Colliders To Meshes")]
    public static void Sync()
    {
        if (Application.isPlaying) throw new Exception("Stop Play Mode before syncing cover colliders.");
        var layouts = UnityEngine.Object.FindFirstObjectByType<ArenaLayouts>();
        if (layouts == null) throw new Exception("Open the authored Game scene first.");
        int removed = 0, fitted = 0;
        foreach (Transform root in Roots(layouts))
        {
            Undo.RegisterFullObjectHierarchyUndo(root.gameObject, "Sync arena colliders");
            ArenaLayouts.RemoveRigidbodies(root);
            if (root != layouts.initialCover && root != layouts.suddenDeathCover
                && root.GetComponent<GameplayCoverMarker>() == null)
                Undo.AddComponent<GameplayCoverMarker>(root.gameObject);
            // Parent boxes are stale when artists move visual children independently.
            // Attach collision to each visual mesh so all future child edits follow exactly.
            foreach (var collider in root.GetComponentsInChildren<Collider>(true))
            {
                if (collider.GetComponentInParent<BurnableWood>() != null) continue;
                var filter = collider.GetComponent<MeshFilter>();
                bool matches = collider is MeshCollider meshCollider && filter != null
                    && filter.sharedMesh != null && meshCollider.sharedMesh == filter.sharedMesh
                    && !meshCollider.convex && !meshCollider.isTrigger;
                if (!matches) { Undo.DestroyObjectImmediate(collider); removed++; }
            }
            foreach (var filter in root.GetComponentsInChildren<MeshFilter>(true))
            {
                if (filter.sharedMesh == null || filter.GetComponentInParent<BurnableWood>() != null
                    || filter.GetComponentInParent<TMPro.TMP_Text>() != null) continue;
                var renderer = filter.GetComponent<MeshRenderer>();
                if (renderer == null || !renderer.enabled) continue;
                var collider = filter.GetComponent<MeshCollider>();
                if (collider == null) collider = Undo.AddComponent<MeshCollider>(filter.gameObject);
                collider.sharedMesh = filter.sharedMesh;
                collider.convex = false;
                collider.isTrigger = false;
                collider.enabled = true;
                filter.gameObject.layer = CombatLayers.GameplayCoverLayer;
                EditorUtility.SetDirty(collider);
                fitted++;
            }
        }
        Validate(layouts);
        ValidateDeckAbilityQueries(layouts);
        EditorSceneManager.MarkSceneDirty(layouts.gameObject.scene);
        Debug.Log($"ARENA COLLIDERS SYNCED: {fitted} mesh colliders; {removed} stale shapes removed; visual transforms unchanged.");
    }

    // Artists can author persistent deck props beside GameplayCover as well as inside it.
    private static IEnumerable<Transform> Roots(ArenaLayouts layouts)
    {
        yield return layouts.initialCover;
        yield return layouts.suddenDeathCover;
        foreach (Transform child in layouts.transform)
            if (child != layouts.initialCover && child != layouts.suddenDeathCover
                && child.name.StartsWith("Deck details", StringComparison.OrdinalIgnoreCase))
                yield return child;
    }

    private static void ValidateDeckAbilityQueries(ArenaLayouts layouts)
    {
        Physics.SyncTransforms();
        int tested = 0;
        foreach (Transform root in Roots(layouts))
        {
            if (root == layouts.initialCover || root == layouts.suddenDeathCover) continue;
            foreach (var collider in root.GetComponentsInChildren<MeshCollider>())
            {
                Mesh mesh = collider.sharedMesh;
                Vector3[] vertices = mesh.vertices;
                int[] triangles = mesh.triangles;
                bool beam = false, ice = false;
                int stride = Mathf.Max(1, triangles.Length / (3 * 64)) * 3;
                for (int i = 0; i + 2 < triangles.Length && !(beam && ice); i += stride)
                {
                    Vector3 a = collider.transform.TransformPoint(vertices[triangles[i]]);
                    Vector3 b = collider.transform.TransformPoint(vertices[triangles[i + 1]]);
                    Vector3 c = collider.transform.TransformPoint(vertices[triangles[i + 2]]);
                    Vector3 normal = Vector3.Cross(b - a, c - a).normalized;
                    if (normal.sqrMagnitude < 0.5f) continue;
                    Vector3 origin = (a + b + c) / 3f + normal * 0.1f;
                    beam |= Physics.Raycast(origin, -normal, out var hit, 0.2f,
                        CombatLayers.CombatHitMask, QueryTriggerInteraction.Ignore) && hit.collider == collider;
                    ice |= Physics.SphereCast(origin, 0.01f, -normal, out hit, 0.2f,
                        CombatLayers.CombatHitMask, QueryTriggerInteraction.Ignore) && hit.collider == collider;
                }
                if (!beam || !ice) throw new Exception("Deck prop failed beam/ice hit detection: " + collider.name);
                tested++;
            }
        }
        Debug.Log($"DECK ABILITY CHECK PASSED: beam and ice casts hit {tested} additional deck meshes.");
    }

    public static void Validate(ArenaLayouts layouts)
    {
        foreach (Transform root in Roots(layouts))
        {
            foreach (var filter in root.GetComponentsInChildren<MeshFilter>(true))
            {
                var renderer = filter.GetComponent<MeshRenderer>();
                if (filter.sharedMesh == null || renderer == null || !renderer.enabled
                    || filter.GetComponentInParent<TMPro.TMP_Text>() != null
                    || filter.GetComponentInParent<BurnableWood>() != null) continue;
                var collider = filter.GetComponent<MeshCollider>();
                if (collider == null || collider.sharedMesh != filter.sharedMesh || collider.convex
                    || collider.isTrigger || !collider.enabled || collider.gameObject.layer != CombatLayers.GameplayCoverLayer)
                    throw new Exception("Visual mesh lacks matching shot collider: " + filter.name);
            }
            foreach (var collider in root.GetComponentsInChildren<Collider>(true))
            {
                if (collider.GetComponentInParent<BurnableWood>() != null) continue;
                var filter = collider.GetComponent<MeshFilter>();
                if (!(collider is MeshCollider mesh) || filter == null || mesh.sharedMesh != filter.sharedMesh)
                    throw new Exception("Stale collider without matching visual: " + collider.name);
            }
        }
        Debug.Log("ARENA COLLIDER CHECK PASSED: both layouts match their visual meshes.");
    }
}
