using UnityEngine;

/// <summary>
/// Editor/dev preview: plays the sudden-death sequence without a networked match.
/// Add to any object in Play mode; the clock starts a few seconds before sudden death.
/// </summary>
public sealed class SuddenDeathPreview : MonoBehaviour
{
    public float startClock = -6f;
    public float speed = 1f;
    public int shootDroneIndex = -1;
    public float shootAtClock = 3f;
    public float clock;
    private int downMask;

    private void OnEnable() => clock = startClock;

    private void Update()
    {
        clock += Time.deltaTime * speed;
        if (shootDroneIndex >= 0 && clock >= shootAtClock) downMask |= 1 << shootDroneIndex;
        SuddenDeathDirector.Ensure().Drive(FusionRoundDirector.RoundPhase.Fighting, clock, downMask);
    }
}
