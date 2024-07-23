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

    private bool _isWalking;

    private void Update()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDirection = new Vector3(inputVector.x, 0.0f, inputVector.y);
        transform.position += moveDirection * (moveSpeed * Time.deltaTime);

        _isWalking = (moveDirection != Vector3.zero);

        float rotateSpeed = 10.0f;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime*rotateSpeed);
    }

    public bool IsWalking()
    {
        return _isWalking;
    }
}