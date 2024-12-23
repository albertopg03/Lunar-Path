using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    // variables para el contador de tiempo
    private static float mili = 0f;
    private static float seg = 0f;
    private static float min = 0f;

    public static event Action OnMinuteChange;

    public static string GetCurrentTimer()
    {
        mili += Time.deltaTime;

        if (mili >= 1f)
        {
            seg++;
            mili = 0f;
        }

        if (seg > 59)
        {
            min++;
            seg = 0f;
            mili = 0f;

            OnMinuteChange?.Invoke();
        }

        return min.ToString("00") + ":" +
                seg.ToString("00") + ":" +
                (mili * 1000).ToString("000");
    }

    public static void ResetTimer()
    {
        mili = 0f;
        seg = 0f;
        min = 0f;
    }

    public static void SetPauseGame(bool inPause)
    {
        Time.timeScale = inPause ? 0f : 1f;
    }
}
