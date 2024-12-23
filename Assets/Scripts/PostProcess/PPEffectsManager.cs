using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PPEffectsManager : MonoBehaviour
{
    [Header("SUBJECTS")]
    [SerializeField] private PlayerHealth subjectPlayerHealth;
    [SerializeField] private PlayerMovement subjectPlayerMovement;
    [SerializeField] private GameLoop subjectGameLoop;

    [Header("DAMAGE EFFECT")]
    [SerializeField] private float dmgTargetIntensity;
    [SerializeField] private float dmgDuration;
    [SerializeField] private Color dmgColor;
    [SerializeField] [Range(0, 1)] private float dmgChromaticIntensity;

    [Header("SPEED EFFECT")]
    [SerializeField] private float spdTargetIntensity;
    [SerializeField] private float spdDuration;
    [SerializeField] private Color spdColor;
    [SerializeField] [Range(0, 1)] private float spdChromaticIntensity;

    [Header("GOD EFFECT")]
    [SerializeField] private float godTargetIntensity;
    [SerializeField] private float godDuration;
    [SerializeField] private Color godColor;
    [SerializeField] [Range(0, 1)] private float godChromaticIntensity;

    [Header("DEATH EFFECT")]
    [SerializeField] private float deathTargetIntensity;
    [SerializeField] private float deathDuration;
    [SerializeField] private Color deathColor;
    [SerializeField] private float deathTargetFOV;
    [SerializeField] [Range(0, 1)] private float deathChromaticIntensity;

    private DamageEffectHandler damageEffectHandler;
    private SpeedEffectHandler speedEffectHandler;
    private GodEffectHandler godEffectHandler;
    private DeathEffectHandler deathEffectHandler;

    private Vignette vignette;
    private ChromaticAberration chromaticAberration;
    private DepthOfField depthOfField;
    private Camera mainCamera;

    private float initSizeCamera;

    private void Init()
    {
        DOTween.KillAll();

        if (vignette != null)
        {
            vignette.intensity.Override(0f);
            vignette.color.Override(Color.black);
        }

        if (chromaticAberration != null)
        {
            chromaticAberration.intensity.Override(0f);
        }

        if (depthOfField != null)
        {
            depthOfField.active = false;
        }

        if (mainCamera != null)
        {
            mainCamera.orthographicSize = initSizeCamera;
        }
    }

    private void OnEnable()
    {
        subjectGameLoop.OnResetGame += Init;
    }

    private void OnDisable()
    {
        subjectGameLoop.OnResetGame -= Init;
        subjectPlayerHealth.OnDamage -= damageEffectHandler.TriggerEffect;
        subjectPlayerMovement.OnSpeedEffect -= speedEffectHandler.TriggerEffect;
        subjectPlayerHealth.OnGodMode -= godEffectHandler.TriggerEffect;
        subjectPlayerHealth.OnDeath -= deathEffectHandler.TriggerEffect;
    }

    private void Start()
    {
        // Obtén el componente Volume y asegúrate de que tiene un perfil asignado
        var volume = GetComponent<Volume>();
        if (volume == null || volume.profile == null)
        {
            Debug.LogError("PPEffectsManager: No encontrado");
            return;
        }

        // Obtén los efectos del perfil
        if (!volume.profile.TryGet(out vignette) || !volume.profile.TryGet(out chromaticAberration) || !volume.profile.TryGet(out depthOfField))
        {
            Debug.LogError("PPEffectsManager: No encontrado");
            return;
        }

        // Configurar la cámara principal
        mainCamera = Camera.main;
        initSizeCamera = mainCamera.orthographicSize;

        if (mainCamera == null)
        {
            Debug.LogError("PPEffectsManager: Cámara principal no encontrada.");
            return;
        }

        // Inicializa los manejadores
        damageEffectHandler = new DamageEffectHandler(
            vignette, chromaticAberration,
            dmgTargetIntensity, dmgDuration, dmgColor, dmgChromaticIntensity);

        speedEffectHandler = new SpeedEffectHandler(
            vignette, chromaticAberration,
            spdTargetIntensity, spdDuration, spdColor, spdChromaticIntensity);

        godEffectHandler = new GodEffectHandler(
            vignette, chromaticAberration,
            godTargetIntensity, godDuration, godColor, godChromaticIntensity);

        deathEffectHandler = new DeathEffectHandler(
            vignette, chromaticAberration, depthOfField, mainCamera,
            deathTargetIntensity, deathDuration, deathColor, deathTargetFOV, deathChromaticIntensity);

        // Vincula los eventos
        subjectPlayerHealth.OnDamage += damageEffectHandler.TriggerEffect;
        subjectPlayerMovement.OnSpeedEffect += speedEffectHandler.TriggerEffect;
        subjectPlayerHealth.OnGodMode += godEffectHandler.TriggerEffect;
        subjectPlayerHealth.OnDeath += deathEffectHandler.TriggerEffect;
    }
}
