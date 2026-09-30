using UnityEngine;

/// <summary>
/// A fixed arena camera for the spectator gallery; its transform is the camera pose. Create one from
/// the Scene view with "Thermal Demo/Add Spectator Viewpoint From Scene View". When the scene has
/// none, the spectator generates viewpoints around the cover layout instead.
/// </summary>
[DisallowMultipleComponent]
public sealed class SpectatorViewpoint : MonoBehaviour
{
    [SerializeField] private string label = "";
    [SerializeField, Range(20f, 110f)] private float fieldOfView = 60f;
    [Tooltip("Hide the virtual ceiling for this shot, for views from above it.")]
    [SerializeField] private bool hideCeiling = true;

    public string Label => string.IsNullOrWhiteSpace(label) ? name.ToUpperInvariant() : label;
    public float FieldOfView => fieldOfView;
    public bool HideCeiling => hideCeiling;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.3f, 0.9f, 0.55f, 0.9f);
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, fieldOfView, 0.8f, 0.05f, 16f / 9f);
    }
}
