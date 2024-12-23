using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpeedEffectHandler
{
    private readonly Vignette vignette;
    private readonly ChromaticAberration chromaticAberration;
    private readonly float targetIntensity;
    private readonly float duration;
    private readonly Color targetColor;
    private readonly float chromaticIntensity;

    private Sequence speedSequence;

    public SpeedEffectHandler(
        Vignette vignette, ChromaticAberration chromaticAberration,
        float targetIntensity, float duration, Color targetColor, float chromaticIntensity)
    {
        this.vignette = vignette;
        this.chromaticAberration = chromaticAberration;
        this.targetIntensity = targetIntensity;
        this.duration = duration;
        this.targetColor = targetColor;
        this.chromaticIntensity = chromaticIntensity;
    }

    public void TriggerEffect(float effectDuration)
    {
        speedSequence?.Kill();

        speedSequence = DOTween.Sequence();

        speedSequence.Append(
            DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, targetIntensity, duration)
                   .SetLoops((int)(effectDuration / duration), LoopType.Yoyo)
        );

        speedSequence.Join(
            DOTween.To(() => vignette.color.value, x => vignette.color.value = x, targetColor, duration)
                   .SetLoops((int)(effectDuration / duration), LoopType.Yoyo)
        );

        speedSequence.Join(
            DOTween.To(() => chromaticAberration.intensity.value, x => chromaticAberration.intensity.value = x, chromaticIntensity, duration)
                   .SetLoops((int)(effectDuration / duration), LoopType.Yoyo)
        );

        // Limpia los valores después del efecto
        DOVirtual.DelayedCall(effectDuration, () =>
        {
            speedSequence?.Kill();
            ResetEffects();
        });
    }

    private void ResetEffects()
    {
        DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, 0f, duration);
        DOTween.To(() => vignette.color.value, x => vignette.color.value = x, Color.black, duration);
        DOTween.To(() => chromaticAberration.intensity.value, x => chromaticAberration.intensity.value = x, 0f, duration);
    }
}
