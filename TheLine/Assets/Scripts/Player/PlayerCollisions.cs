using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    public CollisionInfo collisionInfo;
    public EdgeCollider2D lineCollider;
    public LayerMask collisionMask;
    public float skinWidth = -0.3f;


    [HideInInspector]
    public BoxCollider2D collider;                  //Calling boxcollider

    [HideInInspector]
    public Vector2 horizontalRayOrigin, verticalRayOrigin;

    private Vector2 raycastOrigin;

    public virtual void Awake()
    {
        collider = GetComponent<BoxCollider2D>();	//Getting boxcollider
    }

    private void FixedUpdate()
    {
        if (IsCollidingWithLine())
        {
            collisionInfo.below = true;
        }
        else
        {
            collisionInfo.below = false;
        }
    }

    /*public void VerticalCollisions(ref Vector2 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Clamp(Mathf.Abs(velocity.y) + 0.2f, 0, .6f);

        verticalRayOrigin = raycastOrigin + (Vector2.up * skinWidth);

        RaycastHit2D hit = Physics2D.Raycast(verticalRayOrigin, transform.up * directionY, rayLength, collisionMask);
        Debug.DrawRay(verticalRayOrigin, transform.up * directionY * rayLength, Color.red);
    }

    public void HorizontalCollisions(ref Vector2 velocity)
    {
        float directionX = collisionInfo.faceDirection;
        float rayLength = Mathf.Abs(velocity.x) + 0.2f;

        horizontalRayOrigin = raycastOrigin;
        horizontalRayOrigin += (Vector2)transform.up;

        RaycastHit2D hit = Physics2D.Raycast(horizontalRayOrigin, transform.right * directionX, rayLength, collisionMask);     //Raycast hit - from origin in directionX with rayLength and only on collision layer
        Debug.DrawRay(horizontalRayOrigin, transform.right * directionX * rayLength, Color.red);
    }*/

    public bool IsCollidingWithLine()
    {
        return LineCollision();
    }


    public RaycastHit2D LineCollision()
    {
        verticalRayOrigin = transform.position;

        Debug.DrawRay(verticalRayOrigin, transform.up * -0.8f, Color.red);
        return Physics2D.Raycast(verticalRayOrigin, transform.up, -0.8f, collisionMask);
    }

    //RAYCAST ORIGIN
    public void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        raycastOrigin = new Vector2(bounds.center.x, bounds.center.y);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Line")
        {
            PlayerEvents.Instance.OnLandEvent.Invoke();
            Debug.Log("Collision!!!!! with: " + collision.gameObject.layer);
        }
    }

    public struct CollisionInfo
    {
        public bool above, below;                       //Collision on above or below?
        public bool left, right;                        //Collision on right or left?
        public int faceDirection;

        public void Reset()                             //Function for resetting collisiondetection
        {
            above = below = false;
            left = right = false;
            faceDirection = 1;
        }
    }
}
