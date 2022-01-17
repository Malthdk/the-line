using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour
{
    
    [Header("Movement variables")]
    [Space]
    [Range(0, .3f)] public float movementSmoothing = .05f;  // How much to smooth out the movement
    [Range(0, .2f)] public float moveSpeed = .05f;
    public float acceleration = 2f;
    public float maxJumpHeight = 3.5f;              //Max JumpHeight
    public float minJumpHeight = 1f;
    public float timeToJumpApex = .65f;
    public float gravity;
    public float velocitySmoothing;
    public Vector2 velocity;
    private float maxJumpVelocity, minJumpVelocity, targetVelocity;

    [Header("Events")]
    [Space]
    public UnityEvent OnLandEvent;
    public UnityEvent OnJumpEvent;

    private PlayerCollisions playerCollisions;
    private bool facingRight = true;
    private bool hasLanded = false;


    private void Awake()
    {
        playerCollisions = GetComponentInChildren<PlayerCollisions>();
        playerCollisions.collisionInfo.Reset();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnJumpEvent == null)
            OnJumpEvent = new UnityEvent();

        OnLandEvent.AddListener(Landed);
        OnJumpEvent.AddListener(Jumped);
    }

    private void FixedUpdate()
    {
        if (!playerCollisions.collisionInfo.IsCollidingWithLine)
        {
            gravity =  -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            //movementVariables.Gravity = -(1 * 0.5f) / Mathf.Pow(0.65f, 1);
        }
    }

    public void Move(Vector2 playerInput, bool isJumping)
    {
        playerCollisions.UpdateRaycastOrigins();
        playerCollisions.collisionInfo.VelocityOld = velocity;

        acceleration = (playerInput.x == 1 || playerInput.x == -1) ? acceleration : (acceleration / 5f);
        targetVelocity = playerInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocity, ref velocitySmoothing, acceleration);
        velocity.y += gravity * Time.deltaTime;

        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;              //Max jump velocity defined based on gravity and time to reach highest point
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);   //Min jump velocity defined based on gravity and min jump height

        ChangePlayerDirection(playerInput);

        playerCollisions.HorizontalCollisions(ref velocity);
        playerCollisions.VerticalCollisions(ref velocity);

        // If the player should jump...
        if (playerCollisions.collisionInfo.IsCollidingWithLine && isJumping)
        {
            Debug.Log("JUMPING!");
            OnJumpEvent.Invoke();
        }

        if (playerCollisions.collisionInfo.IsCollidingWithLine)
        {
            if (!hasLanded)
            {
                OnLandEvent.Invoke();
            }

            transform.position = new Vector2(transform.position.x, playerCollisions.collisionInfo.LineCollisionPosition.y);
            transform.rotation = Quaternion.LookRotation(transform.forward, playerCollisions.collisionInfo.LineCollisionNormal);

            velocity.y = 0;
        }



        transform.Translate(velocity);
    }

    private void ChangePlayerDirection(Vector2 playerInput)
    {
        if (playerInput.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (playerInput.x < 0 && facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        playerCollisions.collisionInfo.FaceDirection = facingRight ? 1 : -1;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void Landed()
    {
        transform.position = new Vector2(transform.position.x, playerCollisions.collisionInfo.LineCollisionPosition.y);
        hasLanded = true;
        Debug.Log("Player has landed!");
    }

    private void Jumped()
    {
        playerCollisions.collisionInfo.IsCollidingWithLine = false;
        hasLanded = false;
        velocity.y = minJumpVelocity;
    }
}