using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    InputMaster controls;
    float moveSpeed = 6f;
    Vector3 velocity;
    float gravity = -9.81f;
    Vector2 move;
    float jumpHeight = 2.4f;
    CharacterController controller;
    bool isGrounded;

    public Transform ground;
    public float distanceToGround = 0.4f;
    public LayerMask groundMask;
    private void Awake()
    {
        controls = new InputMaster();
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        Grav();
        PlayerMovement();
        Jump();
    }
    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
    private void Grav()
    {
        isGrounded = Physics.CheckSphere(ground.position, distanceToGround, groundMask);
        if (isGrounded && velocity.y < 0) 
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    private void PlayerMovement()
    {
        move = controls.Player.Move.ReadValue<Vector2>();
        Vector3 movement = (move.y * transform.forward) + (move.x * transform.right);
        controller.Move(movement * moveSpeed * Time.deltaTime);
    }
    private void Jump()
    {
        if (controls.Player.Jump.triggered)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
