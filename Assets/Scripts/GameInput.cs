using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PlayerPrefsBindings = "InputBindings";
    
    public static GameInput Instance { get; private set; }
    
    public event EventHandler OnInteractPerformed;
    public event EventHandler OnPausePerformed;
    private PlayerInputActions _playerInputActions;

    [Serializable]
    public enum Binding
    {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Interact,
        InteractAlternate,
        Pause,
        Gamepad_Interact,
        Gamepad_InteractAlternate,
        Gamepad_Pause
    }
    
    private bool _interactAlternateHeld;

    private void Awake()
    {
        Instance = this;

        _playerInputActions = new PlayerInputActions();
        if (PlayerPrefs.HasKey(PlayerPrefsBindings))
        {
            _playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PlayerPrefsBindings));
        }
        _playerInputActions.Player.Enable();

        _playerInputActions.Player.Interact.performed += Interact_performed;
        _playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        _playerInputActions.Player.InteractAlternate.canceled += InteractAlternate_canceled;
        _playerInputActions.Player.Pause.performed += Pause_performed;

        _interactAlternateHeld = false;
    }

    private void OnDestroy()
    {
        _playerInputActions.Player.Interact.performed -= Interact_performed;
        _playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        _playerInputActions.Player.InteractAlternate.canceled -= InteractAlternate_canceled;
        _playerInputActions.Player.Pause.performed -= Pause_performed;
        
        _playerInputActions.Dispose();
    }

    public bool GetInteractAlternateHeld() => _interactAlternateHeld;

    private void InteractAlternate_canceled(InputAction.CallbackContext obj) => _interactAlternateHeld = false;

    private void InteractAlternate_performed(InputAction.CallbackContext obj) => _interactAlternateHeld = true;

    private void Interact_performed(InputAction.CallbackContext obj) => 
        OnInteractPerformed?.Invoke(this, EventArgs.Empty);

    private void Pause_performed(InputAction.CallbackContext obj) => 
        OnPausePerformed?.Invoke(this, EventArgs.Empty);

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        
        inputVector = inputVector.normalized;

        return inputVector;
    }

    public string GetBindingText(Binding binding)
    {
        GetBinding(binding, out var inputAction, out var bindingIndex);
        return inputAction.bindings[bindingIndex].ToDisplayString();
    }

    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        _playerInputActions.Player.Disable();

        GetBinding(binding, out var inputAction, out var bindingIndex);

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback =>
            {
                callback.Dispose();
                _playerInputActions.Player.Enable();
                onActionRebound();
                
                PlayerPrefs.SetString(PlayerPrefsBindings, _playerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
            })
            .Start();
    }

    public void ResetBinding(Binding binding, Action onActionRebound)
    {
        GetBinding(binding, out var inputAction, out var bindingIndex);
        
        inputAction.RemoveBindingOverride(bindingIndex);

        onActionRebound();
                
        PlayerPrefs.SetString(PlayerPrefsBindings, _playerInputActions.SaveBindingOverridesAsJson());
        PlayerPrefs.Save();
    }

    private void GetBinding(Binding binding, out InputAction inputAction, out int bindingIndex)
    {
        switch (binding)
        {
            default:
            case Binding.MoveUp:
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.MoveDown:
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.MoveLeft:
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.MoveRight:
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = _playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternate:
                inputAction = _playerInputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = _playerInputActions.Player.Pause;
                bindingIndex = 0;
                break;
            case Binding.Gamepad_Interact:
                inputAction = _playerInputActions.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_InteractAlternate:
                inputAction = _playerInputActions.Player.InteractAlternate;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_Pause:
                inputAction = _playerInputActions.Player.Pause;
                bindingIndex = 1;
                break;
        }
    }
}
