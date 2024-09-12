using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveOn : MonoBehaviour
{

    [SerializeField] private float speed = 4;
    
    //Rotation speed in degrees per second
    [SerializeField] private float rotationSpeed = 50;
    
    private Vector3 moveDirection;
    
    public PlayerInputActions playerInputActions;
    
    private InputAction moveAction;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        moveDirection = transform.forward;
    }

    private void OnEnable()
    {
        moveAction = playerInputActions.Player.Move;
        moveAction.Enable();
    }
    
    private void OnDisable()
    {
        moveAction.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputMoveDirection = moveAction.ReadValue<Vector2>();

        if (inputMoveDirection.x > 0.5)
        {
            moveDirection = Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0) * moveDirection;
        }
        else if (inputMoveDirection.x < -0.5)
        {
            moveDirection = Quaternion.Euler(0, -rotationSpeed * Time.deltaTime, 0) * moveDirection;
        }
        
        
        
        transform.LookAt(transform.position + moveDirection, Vector3.up);
        transform.position += transform.forward * (speed * Time.deltaTime);
    }
    
    
}