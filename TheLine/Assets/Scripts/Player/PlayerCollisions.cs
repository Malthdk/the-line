using UnityEngine;

public class PlayerCollisions : RaycastController
{
    public CollisionInfo collisionInfo;
    public EdgeCollider2D lineCollider;

    public void VerticalCollisions(ref Vector2 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Clamp(Mathf.Abs(velocity.y) - skinWidth, 0, 0.4f);

        for (int i = 0; i < verticalRayCount; i++)
        {
            verticalRayOrigin = (directionY >= -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            verticalRayOrigin += (Vector2)transform.right * collisionInfo.faceDirection * (verticalRaySpacing * i);

            RaycastHit2D hit = Physics2D.Raycast(verticalRayOrigin, transform.up * directionY, rayLength, collisionMask);
            Debug.DrawRay(verticalRayOrigin, transform.up * directionY * rayLength, Color.red);

            if (hit)
            {
                collisionInfo.below = true;
                if (hit.collider is EdgeCollider2D)
                {
                    lineCollider = (EdgeCollider2D)hit.collider;
                }
                
                Debug.DrawRay(hit.point, gameObject.transform.position, Color.red);
            }
        }
    }

    public void HorizontalCollisions(ref Vector2 velocity)
    {
        float directionX = collisionInfo.faceDirection;
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            horizontalRayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            horizontalRayOrigin += (Vector2)transform.up * (horizontalRaySpacing * i);

            RaycastHit2D hit = Physics2D.Raycast(horizontalRayOrigin, transform.right * directionX, rayLength, collisionMask);     //Raycast hit - from origin in directionX with rayLength and only on collision layer
            Debug.DrawRay(horizontalRayOrigin, transform.right * directionX * rayLength, Color.red);

            //collisionInfo.below = hit;
        }
    }

    public RaycastHit2D LineCollistion()
    {
        Debug.DrawRay(raycastOrigins.center, transform.up * -1 * 0.6f, Color.cyan);
        return Physics2D.Raycast(raycastOrigins.center, transform.up * -1, 0.6f, collisionMask);
    }
}
