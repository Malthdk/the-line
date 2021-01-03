using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float minJumpHeight = 1f, maxJumpHeight = 3.5f;
    public float acceleration = .25f;
    public float airSpeed = 6.4f, groundSpeed = 9.4f;
    public float gravity;
    public float gravityModifierFall;
    public float gravityModifierJump;
    public float maxJumpVelocity, minJumpVelocity;
    public float velocitySmoothing;
    public bool isLanded = true;

    public float timeToJumpApex = .65f;
    public float moveSpeed = 9;

    private Vector2 playerInput;

    public void Update()
    {
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetButtonDown("Jump"))
        {
            controller.movementVariables.Jump = true;
        }
    }

    public void FixedUpdate()
    {
        controller.Move(playerInput);
    }
}
