using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangeEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangeEventArgs : EventArgs
    {
        public BaseCounter SelectedCounter;
    }

    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool _isWalking;
    private Vector3 _lastInteractDirection;
    /** Current counter that player can interact with */
    private BaseCounter _selectedCounter;
    private KitchenObject _kitchenObject;

    private const float PlayerSize = 0.7f;
    private const float PlayerHeight = 2.0f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameInput.Instance.OnInteractPerformed += GameInput_OnInteractPerformed;
    }

    private void GameInput_OnInteractPerformed(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        if (GameManager.Instance.IsGamePaused()) return;

        _selectedCounter?.Interact(this);
    }

    private void Update()
    {
        UpdateMovement();
        UpdateInteractions();
        UpdateInteractAlternate();
    }

    /**
     * Updates player movement, stopping or sliding movement if collision with an object occurs.
     */
    private void UpdateMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        Vector3 moveDirection = new Vector3(inputVector.x, 0.0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        
        bool canMove = !MovementCollides(moveDirection, moveDistance);

        // If unable to move in wanted direction,
        // check for movement along X-axis or Z-axis.
        if (!canMove)
        {
            do
            {
                // Test X movement
                Vector3 moveDirectionX = new Vector3(moveDirection.x, 0, 0).normalized;
                canMove = moveDirection.x is < -.5f or > .5f && !MovementCollides(moveDirectionX, moveDistance);
                if (canMove)
                {
                    moveDirection = moveDirectionX;
                    break;
                }

                // Test Z movement
                Vector3 moveDirectionZ = new Vector3(0, 0, moveDirection.z).normalized;
                canMove = moveDirection.z is < -.5f or > .5f && !MovementCollides(moveDirectionZ, moveDistance);
                if (canMove)
                {
                    moveDirection = moveDirectionZ;
                    break;
                }
            } while (false);
        }

        // Able to fully move in wanted direction
        if (canMove)
        {
            transform.position += moveDirection * moveDistance;
        }

        _isWalking = (moveDirection != Vector3.zero) && canMove;

        var rotateSpeed = 10.0f;
        transform.forward = Vector3.Slerp(
            transform.forward, moveDirection, Time.deltaTime*rotateSpeed);
    }

    /**
     * Uses Physics.CapsuleCast to check if player movement along the inputted Vector3
     * collides with anything during its movement.
     */
    private bool MovementCollides(Vector3 moveDirection, float moveDistance)
    {
        return Physics.CapsuleCast(
            transform.position,
            transform.position + (Vector3.up * PlayerHeight),
            PlayerSize,
            moveDirection,
            moveDistance);
    }

    /**
     * Sends a raycast in the movement direction to check for collision with other objects.
     * If the player is not moving, stores the last collided object.
     */
    private void UpdateInteractions()
    {
        if (GameManager.Instance.IsGamePaused()) return;
        
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        Vector3 moveDirection = new Vector3(inputVector.x, 0.0f, inputVector.y);

        if (moveDirection != Vector3.zero)
        {
            _lastInteractDirection = moveDirection;
        }
        
        float interactDistance = 2.0f;
        
        // Check for collision with counters
        if (!Physics.Raycast(transform.position, _lastInteractDirection,
                out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            SetSelectedCounter(null);
            return;
        }

        // Check if collided object is a BaseCounter
        if (!raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
        {
            SetSelectedCounter(null);
            return;
        }
        
        if (baseCounter != _selectedCounter)
        {
            SetSelectedCounter(baseCounter);
        }
    }

    /**
     * If player has selected a counter and InteractAlternate is held,
     * update InteractAlternate for the appropriate counter.
     */
    private void UpdateInteractAlternate()
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        if (GameManager.Instance.IsGamePaused()) return;

        if (!GameInput.Instance.GetInteractAlternateHeld()) return;

        _selectedCounter?.InteractAlternate(this);
    }

    private void SetSelectedCounter(BaseCounter baseCounter)
    {
        _selectedCounter = baseCounter;
        
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangeEventArgs
        {
            SelectedCounter = _selectedCounter
        });
    }

    public bool IsWalking() => _isWalking;

    public Transform GetKitchenObjectFollowTransform() => kitchenObjectHoldPoint;

    public KitchenObject GetKitchenObject() => _kitchenObject;

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;

        if (kitchenObject is null) return;
        
        OnPickedSomething?.Invoke(this, EventArgs.Empty);
    }

    public void ClearKitchenObject() => _kitchenObject = null;

    public bool HasKitchenObject() => _kitchenObject is not null;
}