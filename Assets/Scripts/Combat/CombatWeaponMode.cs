using UnityEngine;

[DisallowMultipleComponent]
public sealed class CombatWeaponMode : MonoBehaviour
{
    public enum WeaponMode
    {
        ThermalBeam,
        IceGrenade
    }

    [SerializeField] private WeaponMode activeMode = WeaponMode.ThermalBeam;
    [SerializeField] private PalmBeamShooter beamShooter;
    [SerializeField] private IceGrenadeLauncher grenadeLauncher;

    public WeaponMode ActiveMode => activeMode;

    private void Reset()
    {
        beamShooter = GetComponent<PalmBeamShooter>();
        grenadeLauncher = GetComponent<IceGrenadeLauncher>();
    }

    private void Awake()
    {
        if (beamShooter == null)
        {
            beamShooter = GetComponent<PalmBeamShooter>();
        }

        if (grenadeLauncher == null)
        {
            grenadeLauncher = GetComponent<IceGrenadeLauncher>();
        }

        ApplyMode();
    }

    private void OnValidate()
    {
        ApplyMode();
    }

    public void SetMode(WeaponMode mode)
    {
        activeMode = mode;
        ApplyMode();
    }

    private void ApplyMode()
    {
        bool useBeam = activeMode == WeaponMode.ThermalBeam;

        if (beamShooter != null)
        {
            beamShooter.enabled = useBeam;
        }

        if (grenadeLauncher != null)
        {
            grenadeLauncher.enabled = !useBeam;
        }
    }
}
