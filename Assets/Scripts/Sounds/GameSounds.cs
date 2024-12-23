using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MelenitasDev.SoundsGood;
using UnityEngine.UI;

public class GameSounds : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider sliderGeneralMusic;
    [SerializeField] private Button btnSave;
    [SerializeField] private GameLoop gameLoop;

    private float value;

    DynamicMusic dynamicMusic;

    private void OnEnable()
    {
        gameLoop.OnPauseGame += MusicLowResolution;
    }

    private void OnDisable()
    {
        gameLoop.OnPauseGame -= MusicLowResolution;
    }

    private void Awake()
    {
        sliderGeneralMusic.onValueChanged.AddListener(OnChangeDynamicVolumeGeneral);
        btnSave.onClick.AddListener(SaveSoundSettings);
    }

    void Start()
    {
        // música única de ambiente
        //ambient = new Music(Track.ambient);
        //ambient.SetVolume(0.25f).SetSpatialSound(false).SetLoop(true).SetFadeOut(2f).Play(3f);

        // música dinámica
        dynamicMusic = new DynamicMusic(new Track[] { Track.alert, Track.bass, Track.cyberline, Track.guitar, Track.hihat, Track.kick, Track.percussion, Track.snare });
        dynamicMusic.SetAllVolumes(PlayerPrefs.GetFloat("GeneralMusicValue", 0.5f)).SetLoop(true);

        sliderGeneralMusic.value = PlayerPrefs.GetFloat("GeneralMusicValue", 0.5f);

        dynamicMusic.Play();
    }   

    private void OnChangeDynamicVolumeGeneral(float value)
    {
        dynamicMusic.ChangeAllVolumes(value);
        this.value = value;
    }

    private void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat("GeneralMusicValue", value);
    }

    // quitamos algunos instrumentos y bajamos el volumen
    private void MusicLowResolution(bool inPause)
    {
        dynamicMusic.ChangeTrackVolume(Track.cyberline, inPause ? 0 : PlayerPrefs.GetFloat("GeneralMusicValue", 0.5f));
        dynamicMusic.ChangeTrackVolume(Track.guitar, inPause ? 0 : PlayerPrefs.GetFloat("GeneralMusicValue", 0.5f));
        dynamicMusic.ChangeTrackVolume(Track.snare, inPause ? 0 : PlayerPrefs.GetFloat("GeneralMusicValue", 0.5f));
    }
}
