using UnityEngine;

public sealed class SleevePositioner : MonoBehaviour
{
    public Transform target;
    public Vector3 localOffset;
    public Vector3 rotationOffset;

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        transform.SetPositionAndRotation(
            target.TransformPoint(localOffset),
            target.rotation * Quaternion.Euler(rotationOffset));
    }
}
