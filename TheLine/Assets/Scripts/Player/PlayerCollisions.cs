using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : RaycastController
{
    public CollisionInfo collisionInfo;
    public EdgeCollider2D lineCollider;

    public void VerticalCollisions(ref Vector2 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            verticalRayOrigin = (directionY >= -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            verticalRayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.y);
            Debug.DrawLine(raycastOrigins.bottomLeft, gameObject.transform.position, Color.red);

            RaycastHit2D hit = Physics2D.Raycast(verticalRayOrigin, transform.up * directionY, rayLength, collisionMask);
            Debug.DrawRay(verticalRayOrigin, transform.up * directionY * rayLength * 5, Color.green);

            if (hit)
            {
                collisionInfo.below = true;
                if (hit.collider is EdgeCollider2D)
                {
                    lineCollider = (EdgeCollider2D)hit.collider;
                }
                
                Debug.DrawRay(hit.point, gameObject.transform.position, Color.red);
                Debug.Log(collisionInfo.below + " | " + hit.transform.gameObject.layer + " | " + lineCollider);

                transform.rotation = Quaternion.LookRotation(transform.forward, hit.normal);
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
            horizontalRayOrigin += Vector2.up * (horizontalRaySpacing * i);

            RaycastHit2D hit = Physics2D.Raycast(horizontalRayOrigin, transform.right * directionX, rayLength, collisionMask);     //Raycast hit - from origin in directionX with rayLength and only on collision layer
            Debug.DrawRay(horizontalRayOrigin, transform.right * directionX * rayLength * 5, Color.red);

            //collisionInfo.below = hit;
        }
    }
}
