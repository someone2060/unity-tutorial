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

    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        _playerInputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;
        
        switch (binding)
        {
            default:
            case GameInput.Binding.MoveUp:
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case GameInput.Binding.MoveDown:
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case GameInput.Binding.MoveLeft:
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case GameInput.Binding.MoveRight:
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case GameInput.Binding.Interact:
                inputAction = _playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case GameInput.Binding.InteractAlternate:
                inputAction = _playerInputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case GameInput.Binding.Pause:
                inputAction = _playerInputActions.Player.Pause;
                bindingIndex = 0;
                break;
        }
        
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
}
