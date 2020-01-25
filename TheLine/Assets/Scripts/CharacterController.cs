using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 400f;                          // Amount of force added when the player jumps.
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;  // How much to smooth out the movement
    [Range(0, .2f)] [SerializeField] private float moveSpeed = .05f;        

    private PlayerCollisions playerCollisions;

    private float maxJumpVelocity, minJumpVelocity, acceleration, targetVelocity;
    private bool facingRight = true;

    [HideInInspector]
    public MovementVariables movementVariables;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnJumpEvent;

    private void Awake()
    {
        playerCollisions = GetComponentInChildren<PlayerCollisions>();
        playerCollisions.collisionInfo.Reset();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnJumpEvent == null)
            OnJumpEvent = new BoolEvent();
    }

    private void FixedUpdate()
    {
        movementVariables.Gravity = -(1 * 0.5f) / Mathf.Pow(0.65f, 1);
    }

    public void Move(Vector2 playerInput)
    {
        acceleration = (playerInput.x == 1 || playerInput.x == -1) ? movementVariables.Acceleration : (movementVariables.Acceleration / 5f);
        targetVelocity = playerInput.x * moveSpeed;
        movementVariables.Velocity.x = Mathf.SmoothDamp(movementVariables.Velocity.x, targetVelocity, ref movementVariables.VelocitySmoothing, acceleration);

        transform.Translate(movementVariables.Velocity);

        playerCollisions.UpdateRaycastOrigins();

        if (playerInput.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (playerInput.x < 0 && facingRight)
        {
            Flip();
        }

        playerCollisions.HorizontalCollisions(ref movementVariables.Velocity);
        playerCollisions.VerticalCollisions(ref movementVariables.Velocity);

        // If the player should jump...
        if (playerCollisions.collisionInfo.below && movementVariables.Jump)
        {
            // Add a vertical force to the player.
            movementVariables.Velocity.y = 5f;
        }

        //Debug.Log(playerCollisions.collisionInfo.below);
        if (playerCollisions.collisionInfo.below)
        {
            movementVariables.Velocity.y = 0;
            
        }
        else
        {
            movementVariables.Velocity.y += movementVariables.Gravity * Time.deltaTime;
        }
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        playerCollisions.collisionInfo.faceDirection = facingRight ? 1 : -1;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}