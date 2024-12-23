using System;
using UnityEngine;
using UnityEngine.Events;

public class GameLoop : MonoBehaviour
{
    public event Action OnResetGame;
    public event Action<bool> OnPauseGame;
    public UnityEvent OnResetGameUnityEvent;

    private bool inPause;

    public bool InPause => inPause;

    public void CallOnResetGame()
    {
        OnResetGame?.Invoke();
        OnResetGameUnityEvent?.Invoke();
    }

    public void SetScaleTime(bool inPause)
    {

        this.inPause = inPause;

        OnPauseGame?.Invoke(this.inPause);

        Utils.SetPauseGame(this.inPause);
    }
}
