using UnityEngine;

public class SaveSettings
{
    public static void SaveFullScreen(bool isFull = false)
    {
        if (isFull)
            PlayerPrefs.SetInt("fullScreen", 1);
        else
            PlayerPrefs.SetInt("fullScreen", 0);
    }

    public static bool LoadFullScreen()
    {
        if (PlayerPrefs.GetInt("fullScreen", 1) == 1)
            return true;

        return false;
    }

    public static int LoadResolution()
    {
        return PlayerPrefs.GetInt("resolucion", 0); // 0 como valor predeterminado
    }

    public static void SaveResolution(int index)
    {
        PlayerPrefs.SetInt("resolucion", index);
        PlayerPrefs.Save();
    }

}
