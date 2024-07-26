using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    
    public event EventHandler<OnSelectedCounterChangeEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangeEventArgs : EventArgs
    {
        public ClearCounter SelectedCounter;
    }
    
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    
    private bool _isWalking;
    private Vector3 _lastInteractDirection;
    /** Current counter that player can interact with */
    private ClearCounter _selectedCounter;
    
    private const float PlayerSize = 0.7f;
    private const float PlayerHeight = 2.0f;

    private void Awake()
    {
        if (Instance is not null)
        {
            Debug.LogError("There is more than one Player instance!");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (_selectedCounter is not null)
        {
            _selectedCounter.Interact();
        }
    }

    private void Update()
    {
        UpdateMovement();
        UpdateInteractions();
    }

    /**
     * Updates player movement, stopping or sliding movement if collision with an object occurs.
     */
    private void UpdateMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

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
                if (moveDirection.x != 0)
                {
                    Vector3 moveDirectionX = new Vector3(moveDirection.x, 0, 0).normalized;
                    canMove = !MovementCollides(moveDirectionX, moveDistance);
                    if (canMove)
                    {
                        moveDirection = moveDirectionX;
                        break;
                    }
                }

                // Test Z movement
                if (moveDirection.z != 0)
                {
                    Vector3 moveDirectionZ = new Vector3(0, 0, moveDirection.z).normalized;
                    canMove = !MovementCollides(moveDirectionZ, moveDistance);
                    if (canMove)
                    {
                        moveDirection = moveDirectionZ;
                        break;
                    }
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
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

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

        // Check if collided object is a ClearCounter
        if (!raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
        {
            SetSelectedCounter(null);
            return;
        }
        
        if (clearCounter != _selectedCounter)
        {
            SetSelectedCounter(clearCounter);
        }
    }

    private void SetSelectedCounter(ClearCounter selectedCounter)
    {
        _selectedCounter = selectedCounter;
        
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangeEventArgs
        {
            SelectedCounter = _selectedCounter
        });
    }

    public bool IsWalking()
    {
        return _isWalking;
    }
}