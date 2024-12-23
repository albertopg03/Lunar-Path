using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessEffects : MonoBehaviour
{
    private Volume postProcessVolume;
    public Vignette Vignette { get; private set; }
    public ChromaticAberration ChromaticAberration { get; private set; }

    private void Awake()
    {
        postProcessVolume = GetComponent<Volume>();
        if (postProcessVolume.profile.TryGet(out Vignette vignette) &&
            postProcessVolume.profile.TryGet(out ChromaticAberration chromaticAberration))
        {
            Vignette = vignette;
            ChromaticAberration = chromaticAberration;
        }
    }
}
