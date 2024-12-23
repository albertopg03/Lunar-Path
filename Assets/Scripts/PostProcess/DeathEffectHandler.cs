using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DeathEffectHandler
{
    private readonly Vignette vignette;
    private readonly ChromaticAberration chromaticAberration;
    private readonly DepthOfField depthOfField;
    private readonly Camera camera;

    private readonly float initialVignetteIntensity;
    private readonly Color initialVignetteColor;
    private readonly float initialChromaticIntensity;
    private readonly float initialFOV;

    private readonly float targetIntensity;
    private readonly float effectDuration;
    private readonly Color targetColor;
    private readonly float targetChromaticIntensity;
    private readonly float targetSizeCamera;

    public DeathEffectHandler(Vignette vignette, ChromaticAberration chromaticAberration, DepthOfField depthOfField, Camera camera,
        float targetIntensity, float effectDuration, Color targetColor, float targetFOV, float targetChromaticIntensity)
    {
        this.vignette = vignette;
        this.chromaticAberration = chromaticAberration;
        this.depthOfField = depthOfField;
        this.camera = camera;

        // Guardar valores iniciales
        initialVignetteIntensity = vignette.intensity.value;
        initialVignetteColor = vignette.color.value;
        initialChromaticIntensity = chromaticAberration.intensity.value;
        initialFOV = camera.fieldOfView;

        this.targetIntensity = targetIntensity;
        this.effectDuration = effectDuration;
        this.targetColor = targetColor;
        this.targetChromaticIntensity = targetChromaticIntensity;
        this.targetSizeCamera = targetFOV;
    }

    public void TriggerEffect()
    {

        depthOfField.active = true;

        Sequence sequence = DOTween.Sequence();

        // Efecto de Vignette
        sequence.Join(DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, targetIntensity, effectDuration));
        sequence.Join(DOTween.To(() => vignette.color.value, x => vignette.color.value = x, targetColor, effectDuration));

        // Efecto de Aberración Cromática
        sequence.Join(DOTween.To(() => chromaticAberration.intensity.value, x => chromaticAberration.intensity.value = x, targetChromaticIntensity, effectDuration));

        // Zoom de la cámara
        sequence.Join(DOTween.To(() => camera.orthographicSize, x => camera.orthographicSize = x, targetSizeCamera, effectDuration));
    }
}
