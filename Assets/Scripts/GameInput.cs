using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    
    public event EventHandler OnInteractPerformed;
    public event EventHandler OnPausePerformed;
    private PlayerInputActions _playerInputActions;

    public enum Binding
    {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Interact,
        InteractAlternate,
        Pause
    }
    
    private bool _interactAlternateHeld;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();

        _playerInputActions.Player.Interact.performed += Interact_performed;
        _playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        _playerInputActions.Player.InteractAlternate.canceled += InteractAlternate_canceled;
        _playerInputActions.Player.Pause.performed += Pause_performed;

        _interactAlternateHeld = false;

        Instance = this;
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
        switch (binding)
        {
            default:
            case Binding.MoveUp:
                return _playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.MoveDown:
                return _playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.MoveLeft:
                return _playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.MoveRight:
                return _playerInputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return _playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return _playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return _playerInputActions.Player.Pause.bindings[0].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding)
    {
        _playerInputActions.Player.Disable();

        _playerInputActions.Player.Move.PerformInteractiveRebinding(1)
            .OnComplete(callback =>
            {
                Debug.Log(callback.action.bindings[1].path);
                Debug.Log(callback.action.bindings[1].overridePath);
                callback.Dispose();
                _playerInputActions.Player.Enable();
            })
            .Start();
    }
}
