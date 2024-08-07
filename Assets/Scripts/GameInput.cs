using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractPerformed;
    public event EventHandler OnPausePerformed;
    private PlayerInputActions _playerInputActions;

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
}
