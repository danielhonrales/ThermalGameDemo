using UnityEngine;

/// <summary>Scene-authored starting cover and the delivery poses for sudden death.</summary>
[DefaultExecutionOrder(-500)]
public sealed class ArenaLayouts : MonoBehaviour
{
    public Transform initialCover;
    public Transform suddenDeathCover;

    private void Awake()
    {
        // Editor preview visibility must never alter the match's starting layout.
        if (initialCover != null) { initialCover.gameObject.SetActive(true);
            foreach (Transform child in initialCover) child.gameObject.SetActive(true);
            RemoveRigidbodies(initialCover); }
        if (suddenDeathCover != null) { suddenDeathCover.gameObject.SetActive(false); RemoveRigidbodies(suddenDeathCover); }
    }

    public static void RemoveRigidbodies(Transform root)
    {
        foreach (var body in root.GetComponentsInChildren<Rigidbody>(true))
        {
            body.isKinematic = true;
            body.detectCollisions = false;
#if UNITY_EDITOR
            if (!Application.isPlaying) { DestroyImmediate(body); continue; }
#endif
            Destroy(body);
        }
    }
}
