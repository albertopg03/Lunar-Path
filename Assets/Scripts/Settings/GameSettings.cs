using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [SerializeField] private Toggle toggleFullScreen;
    [SerializeField] private TMP_Dropdown dropdownResolutions;
    [SerializeField] private Button saveButton;

    Resolution[] resolutions;

    private void Start()
    {
        // FPS
        SetFPS(60);

        // Listeners
        toggleFullScreen.onValueChanged.AddListener(FullScreen);
        dropdownResolutions.onValueChanged.AddListener(SetResolution);
        saveButton.onClick.AddListener(Save);

        // Full screen
        toggleFullScreen.isOn = SaveSettings.LoadFullScreen();
        FullScreen(SaveSettings.LoadFullScreen());

        // Resolutions
        LoadCurrentResolutions();
    }

    private void SetFPS(int fps)
    {
        Application.targetFrameRate = fps;
        //QualitySettings.vSyncCount = 0;
    }

    private void FullScreen(bool isFull)
    {
        Screen.fullScreen = isFull;
    }

    /*
    private void LoadCurrentResolutions()
    {
        int resolutionSaved = SaveSettings.LoadResolution(); 
        resolutions = Screen.resolutions;
        dropdownResolutions.ClearOptions();

        List<string> listResolutions = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            listResolutions.Add(option);

            // Si no hay resolución guardada, detectar la resolución actual del sistema
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height &&
                !PlayerPrefs.HasKey("resolucion"))
            {
                currentResolutionIndex = i;
            }

            // Usar la resolución guardada si existe
            if (i == resolutionSaved)
            {
                currentResolutionIndex = resolutionSaved;
            }
        }

        dropdownResolutions.AddOptions(listResolutions);

        // Actualiza el índice del dropdown para reflejar la resolución guardada
        dropdownResolutions.value = currentResolutionIndex;
        dropdownResolutions.RefreshShownValue();

        // Aplica la resolución guardada
        SetResolution(currentResolutionIndex);
    }
    */

    private void LoadCurrentResolutions()
    {
        int resolutionSaved = SaveSettings.LoadResolution();
        dropdownResolutions.ClearOptions();

        List<Resolution> webGLResolutions = new List<Resolution>
        {
            new Resolution { width = 1920, height = 1080 },
            new Resolution { width = 1600, height = 900 },
            new Resolution { width = 1280, height = 720 }
        };

        resolutions = webGLResolutions.ToArray();

        List<string> listResolutions = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            listResolutions.Add(option);

            if (i == resolutionSaved)
            {
                currentResolutionIndex = resolutionSaved;
            }
        }

        dropdownResolutions.AddOptions(listResolutions);

        dropdownResolutions.value = currentResolutionIndex;
        dropdownResolutions.RefreshShownValue();

        SetResolution(currentResolutionIndex);
    }

    private void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void Save()
    {
        // Guarda el estado de pantalla completa
        SaveSettings.SaveFullScreen(toggleFullScreen.isOn);

        // Guarda la resolución seleccionada
        SaveSettings.SaveResolution(dropdownResolutions.value);

        Debug.Log("Configuración guardada: Resolución: " + resolutions[dropdownResolutions.value].width + " x " + resolutions[dropdownResolutions.value].height + ", FullScreen: " + toggleFullScreen.isOn);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
