using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AnimationLightCheck : MonoBehaviour
{
    [SerializeField] private Light2D lightComponent;

    [Tooltip("x => valor mínimo. y => valor máximo")]
    [SerializeField] private Vector2 valuesRadius;

    private Tween lightTween;

    private void OnEnable()
    {
        if(lightTween != null)
        {
            lightTween.Play();
        }
        else
        {
            Animation();
        }
    }

    private void OnDisable()
    {
        lightTween.Pause();
    }

    private void Animation()
    {
        // Crear un nuevo Tween
        lightTween = DOTween.To(
            () => lightComponent.pointLightOuterRadius,
            x => lightComponent.pointLightOuterRadius = x,
            valuesRadius.y,
            valuesRadius.x
        )
        .SetLoops(-1, LoopType.Yoyo)
        .SetEase(Ease.InOutSine);
    }
}
