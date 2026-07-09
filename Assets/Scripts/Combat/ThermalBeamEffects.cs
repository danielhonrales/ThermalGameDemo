using UnityEngine;

[DisallowMultipleComponent]
public sealed class ThermalBeamEffects : MonoBehaviour
{
    [Header("ThermalInMotion Source Prefab")]
    [SerializeField] private bool useThermalInMotionBeamPrefab = true;
    [SerializeField] private string beamPrefabResourcePath = "CustomAssets/Beam/Beam";
    [SerializeField] private bool useHotBeam = true;
    [SerializeField] private Vector3 beamPrefabRotationOffset;
    [SerializeField] private bool useProceduralFallbackEffects;

    [Header("Procedural Fallback")]
    [SerializeField] private bool createGlowLine;
    [SerializeField, Min(0.001f)] private float coreWidth = 0.025f;
    [SerializeField, Min(0.001f)] private float glowWidth = 0.075f;
    [SerializeField] private float pulseSpeed = 18f;
    [SerializeField] private float pulseAmount = 0.35f;

    [Header("Procedural Fallback Particles")]
    [SerializeField] private bool createParticles;
    [SerializeField] private float chargeParticleRate = 35f;
    [SerializeField] private float impactCooldownSeconds = 0.08f;
    [SerializeField] private Color impactColor = new Color(1f, 0.2f, 0.05f, 1f);

    [Header("Audio")]
    [SerializeField] private bool playBeamAudio = true;
    [SerializeField, Range(0f, 1f)] private float beamVolume = 0.25f;
    [SerializeField] private string beamLoopResourcePath = "CustomAssets/Audio/fireBeam";

    private LineRenderer glowLine;
    private ParticleSystem chargeParticles;
    private ParticleSystem impactParticles;
    private AudioSource beamAudio;
    private GameObject sourceBeamInstance;
    private BeamController sourceBeamController;
    private float lastImpactTime;

    private void Awake()
    {
        EnsureEffects();
        SetVisible(false);
    }

    public void ShowBeam(Vector3 start, Vector3 end, Color color, bool hitSomething)
    {
        EnsureEffects();

        ShowThermalInMotionBeam(start, end);

        if (useProceduralFallbackEffects && createGlowLine && glowLine != null)
        {
            float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
            glowLine.enabled = true;
            glowLine.startWidth = glowWidth * pulse;
            glowLine.endWidth = glowWidth * pulse;
            glowLine.startColor = new Color(color.r, color.g, color.b, 0.34f);
            glowLine.endColor = new Color(color.r, color.g, color.b, 0.08f);
            glowLine.SetPosition(0, start);
            glowLine.SetPosition(1, end);
        }

        if (useProceduralFallbackEffects && chargeParticles != null)
        {
            chargeParticles.transform.position = start;
            ParticleSystem.MainModule main = chargeParticles.main;
            main.startColor = color;

            if (!chargeParticles.isPlaying)
            {
                chargeParticles.Play();
            }
        }

        if (useProceduralFallbackEffects && hitSomething && impactParticles != null && Time.time >= lastImpactTime + impactCooldownSeconds)
        {
            lastImpactTime = Time.time;
            impactParticles.transform.position = end;
            ParticleSystem.MainModule main = impactParticles.main;
            main.startColor = Color.Lerp(color, impactColor, 0.35f);
            impactParticles.Play();
        }

        if (beamAudio != null && !beamAudio.isPlaying)
        {
            beamAudio.Play();
        }
    }

    public void ShowCharge(Vector3 start, Color color)
    {
        EnsureEffects();

        if (glowLine != null)
        {
            glowLine.enabled = false;
        }

        if (beamAudio != null && beamAudio.isPlaying)
        {
            beamAudio.Stop();
        }

        if (sourceBeamInstance != null)
        {
            sourceBeamInstance.SetActive(true);
            sourceBeamInstance.transform.SetPositionAndRotation(
                start,
                Quaternion.LookRotation(transform.forward, Vector3.up) * Quaternion.Euler(beamPrefabRotationOffset));
        }

        if (sourceBeamController != null)
        {
            sourceBeamController.SetBeamMode(useHotBeam, false);
            sourceBeamController.SetChargeActive(true);
        }

        if (useProceduralFallbackEffects && chargeParticles != null)
        {
            chargeParticles.transform.position = start;
            ParticleSystem.MainModule main = chargeParticles.main;
            main.startColor = color;

            if (!chargeParticles.isPlaying)
            {
                chargeParticles.Play();
            }
        }
    }

    public void HideBeam()
    {
        SetVisible(false);
    }

    private void SetVisible(bool visible)
    {
        if (sourceBeamController != null)
        {
            sourceBeamController.SetBeamMode(useHotBeam, visible);
            sourceBeamController.SetChargeActive(visible);
        }

        if (sourceBeamInstance != null)
        {
            sourceBeamInstance.SetActive(visible);
        }

        if (glowLine != null)
        {
            glowLine.enabled = visible;
        }

        if (!visible)
        {
            if (chargeParticles != null)
            {
                chargeParticles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            }

            if (beamAudio != null && beamAudio.isPlaying)
            {
                beamAudio.Stop();
            }
        }
    }

    private void EnsureEffects()
    {
        EnsureThermalInMotionBeam();

        if (useProceduralFallbackEffects && createGlowLine && glowLine == null)
        {
            GameObject lineObject = new GameObject("ThermalBeamGlow");
            lineObject.transform.SetParent(transform, false);
            glowLine = lineObject.AddComponent<LineRenderer>();
            glowLine.positionCount = 2;
            glowLine.useWorldSpace = true;
            glowLine.startWidth = coreWidth;
            glowLine.endWidth = coreWidth;
            glowLine.material = new Material(Shader.Find("Sprites/Default"));
            glowLine.enabled = false;
        }

        if (useProceduralFallbackEffects && createParticles && chargeParticles == null)
        {
            chargeParticles = CreateChargeParticles("ThermalBeamCharge");
            chargeParticles.transform.SetParent(transform, false);
            chargeParticles.transform.localPosition = Vector3.zero;
        }

        if (useProceduralFallbackEffects && createParticles && impactParticles == null)
        {
            impactParticles = CreateImpactParticles("ThermalBeamImpact");
            impactParticles.transform.SetParent(transform, false);
            impactParticles.transform.localPosition = Vector3.zero;
        }

        if (playBeamAudio && beamAudio == null)
        {
            beamAudio = gameObject.AddComponent<AudioSource>();
            beamAudio.clip = Resources.Load<AudioClip>(beamLoopResourcePath);
            beamAudio.loop = true;
            beamAudio.playOnAwake = false;
            beamAudio.spatialBlend = 1f;
            beamAudio.volume = beamVolume;
            beamAudio.minDistance = 0.2f;
            beamAudio.maxDistance = 8f;
        }
    }

    private void EnsureThermalInMotionBeam()
    {
        if (!useThermalInMotionBeamPrefab || sourceBeamInstance != null)
        {
            return;
        }

        GameObject sourcePrefab = Resources.Load<GameObject>(beamPrefabResourcePath);
        if (sourcePrefab == null)
        {
            return;
        }

        sourceBeamInstance = Instantiate(sourcePrefab, transform);
        sourceBeamInstance.name = "ThermalInMotionBeam";
        sourceBeamInstance.SetActive(false);
        sourceBeamController = sourceBeamInstance.GetComponent<BeamController>();

        foreach (Collider beamCollider in sourceBeamInstance.GetComponentsInChildren<Collider>(true))
        {
            beamCollider.enabled = false;
        }

        if (sourceBeamController != null)
        {
            sourceBeamController.hand = null;
            sourceBeamController.SetAllInactive();
        }
    }

    private void ShowThermalInMotionBeam(Vector3 start, Vector3 end)
    {
        if (sourceBeamInstance == null)
        {
            return;
        }

        Vector3 direction = end - start;
        if (direction.sqrMagnitude <= 0.0001f)
        {
            return;
        }

        sourceBeamInstance.SetActive(true);
        sourceBeamInstance.transform.SetPositionAndRotation(
            start,
            Quaternion.LookRotation(direction.normalized, Vector3.up) * Quaternion.Euler(beamPrefabRotationOffset));

        if (sourceBeamController != null)
        {
            sourceBeamController.SetBeamMode(useHotBeam, true);
            sourceBeamController.SetChargeActive(true);
        }
    }

    private ParticleSystem CreateChargeParticles(string objectName)
    {
        GameObject particleObject = new GameObject(objectName);
        ParticleSystem particles = particleObject.AddComponent<ParticleSystem>();

        ParticleSystem.MainModule main = particles.main;
        main.loop = true;
        main.startLifetime = 0.18f;
        main.startSpeed = 0.12f;
        main.startSize = 0.035f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = chargeParticleRate;

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.035f;

        ConfigureParticleRenderer(particles);
        return particles;
    }

    private ParticleSystem CreateImpactParticles(string objectName)
    {
        GameObject particleObject = new GameObject(objectName);
        ParticleSystem particles = particleObject.AddComponent<ParticleSystem>();

        ParticleSystem.MainModule main = particles.main;
        main.loop = false;
        main.startLifetime = 0.18f;
        main.startSpeed = 0.7f;
        main.startSize = 0.05f;
        main.maxParticles = 80;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 14) });

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.04f;

        ConfigureParticleRenderer(particles);
        return particles;
    }

    private void ConfigureParticleRenderer(ParticleSystem particles)
    {
        ParticleSystemRenderer renderer = particles.GetComponent<ParticleSystemRenderer>();
        renderer.material = new Material(Shader.Find("Sprites/Default"));
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
    }
}
