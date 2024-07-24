using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    
    private bool _isWalking;
    private Vector3 _lastInteractDirection;
    
    private const float PlayerSize = 0.7f;
    private const float PlayerHeight = 2.0f;
    
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

        // If unable to move, check for movement independently along X-axis or Z-axis
        if (!canMove)
        {
            do
            {
                // Test X movement
                Vector3 moveDirectionX = new Vector3(moveDirection.x, 0, 0).normalized;
                canMove = !MovementCollides(moveDirectionX, moveDistance);
                if (canMove)
                {
                    moveDirection = moveDirectionX;
                    break;
                }

                // Test Z movement
                Vector3 moveDirectionZ = new Vector3(0, 0, moveDirection.z).normalized;
                canMove = !MovementCollides(moveDirectionZ, moveDistance);
                if (canMove)
                {
                    moveDirection = moveDirectionZ;
                    break;
                }
            } while (false);
        }
        
        if (canMove)
        {
            transform.position += moveDirection * moveDistance;
        }

        _isWalking = (moveDirection != Vector3.zero);

        float rotateSpeed = 10.0f;
        transform.forward = Vector3.Slerp(
            transform.forward, moveDirection, Time.deltaTime*rotateSpeed);
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
        if (Physics.Raycast(transform.position, _lastInteractDirection, 
                out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                clearCounter.Interact();
            }
        }
        else
        {
            //Debug.Log("-"); DEBUG
        }
    }

    /**
     * Uses Physics.CapsuleCast to check if player movement along the inputted Vector3
     * collides with anything during its movement.
     */
    private bool MovementCollides(Vector3 moveDirection, float moveDistance)
    {
        return Physics.CapsuleCast(
            transform.position,
            transform.position + Vector3.up * PlayerHeight,
            PlayerSize,
            moveDirection,
            moveDistance);
    }

    public bool IsWalking()
    {
        return _isWalking;
    }
}