using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    // NOT NEEDED: USE MovementVariables instead. Saved and commented for values
    //public float minJumpHeight = 1f, maxJumpHeight = 3.5f;
    //public float acceleration = .25f;
    //public float airSpeed = 6.4f, groundSpeed = 9.4f;
    //public float gravity;
    //public float gravityModifierFall;
    //public float gravityModifierJump;
    //public float maxJumpVelocity, minJumpVelocity;
    //public float velocitySmoothing;
    public bool isJumping = false;

    //public float timeToJumpApex = .65f;
    public float moveSpeed = 9;

    private Vector2 playerInput;

    public void Start()
    {
    }

    public void Update()
    {
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, Input.GetAxisRaw("Vertical") * moveSpeed);

        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }
    }

    public void FixedUpdate()
    {
        controller.Move(playerInput * Time.deltaTime, isJumping);
        isJumping = false;
    }
}
