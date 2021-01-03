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

    public Transform glue;

    [HideInInspector]
    public MovementVariables movementVariables;

    private void Awake()
    {
        playerCollisions = GetComponentInChildren<PlayerCollisions>();
        playerCollisions.collisionInfo.Reset();
        glue = transform.Find("Glue");
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

        // If the player should jump...
        if (playerCollisions.collisionInfo.below && movementVariables.Jump)
        {
            PlayerEvents.Instance.OnJumpEvent.Invoke();

            // Add a vertical force to the player.
            movementVariables.Velocity.y = 5f;
        }

        if (playerCollisions.collisionInfo.below)
        {
            movementVariables.Velocity.y = 0;
            StickToLine();
        }
        else
        {
            movementVariables.Velocity.y += movementVariables.Gravity * Time.deltaTime;
        }
    }

    private void StickToLine()
    {
        var hit = playerCollisions.LineCollision();

        if (hit)
        {
            transform.localRotation = Quaternion.LookRotation(transform.forward, hit.normal);
            transform.localPosition = new Vector3(transform.localPosition.x, hit.point.y, transform.localPosition.z);
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