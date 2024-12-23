using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DamageEffectHandler
{
    private readonly Vignette vignette;
    private readonly ChromaticAberration chromaticAberration;

    private readonly float initialVignetteIntensity;
    private readonly Color initialVignetteColor;
    private readonly float initialChromaticIntensity;

    private readonly float targetIntensity;
    private readonly float effectDuration;
    private readonly Color targetColor;
    private readonly float targetChromaticIntensity;

    public DamageEffectHandler(Vignette vignette, ChromaticAberration chromaticAberration,
        float targetIntensity, float effectDuration, Color targetColor, float targetChromaticIntensity)
    {
        this.vignette = vignette;
        this.chromaticAberration = chromaticAberration;

        // Save initial values
        initialVignetteIntensity = vignette.intensity.value;
        initialVignetteColor = vignette.color.value;
        initialChromaticIntensity = chromaticAberration.intensity.value;

        this.targetIntensity = targetIntensity;
        this.effectDuration = effectDuration;
        this.targetColor = targetColor;
        this.targetChromaticIntensity = targetChromaticIntensity;
    }

    public void TriggerEffect()
    {
        Sequence sequence = DOTween.Sequence();

        // Vignette
        sequence.Join(DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, targetIntensity, effectDuration));
        sequence.Join(DOTween.To(() => vignette.color.value, x => vignette.color.value = x, targetColor, effectDuration));

        // Chromatic Aberration
        sequence.Join(DOTween.To(() => chromaticAberration.intensity.value, x => chromaticAberration.intensity.value = x, targetChromaticIntensity, effectDuration));

        // Reset 
        sequence.OnKill(() =>
        {
            DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, initialVignetteIntensity, effectDuration);
            DOTween.To(() => vignette.color.value, x => vignette.color.value = x, initialVignetteColor, effectDuration);
            DOTween.To(() => chromaticAberration.intensity.value, x => chromaticAberration.intensity.value = x, initialChromaticIntensity, effectDuration);
        });
    }
}
