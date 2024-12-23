using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private InputMaster controls;

    public delegate void OnChangeDirection();
    public event OnChangeDirection ChangeDirectionEvent;

    public delegate void OnMenuPause();
    public event OnMenuPause ActivateMenuPause;

    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.ChangeDirection.performed += context => ChangeDirectionEvent?.Invoke();
        controls.Player.ActivatePause.performed += context => ActivateMenuPause?.Invoke();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
