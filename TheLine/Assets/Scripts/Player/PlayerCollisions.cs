using UnityEngine;

public class PlayerCollisions : RaycastController
{
    public CollisionInfo collisionInfo;
    public EdgeCollider2D lineCollider;

    public void VerticalCollisions(ref Vector2 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y);

        for (int i = 0; i < verticalRayCount; i++)
        {
            verticalRayOrigin = ((directionY == -1) ? raycastOrigins.BottomLeft : raycastOrigins.TopLeft) + Vector2.up * velocity.y;
            verticalRayOrigin += (Vector2)transform.right * collisionInfo.FaceDirection * (verticalRaySpacing * i);

            RaycastHit2D hit = Physics2D.Raycast(verticalRayOrigin, transform.up * directionY, rayLength, collisionMask);
            Debug.DrawRay(verticalRayOrigin, transform.up * directionY * rayLength, Color.red);

            if (hit)
            {
                Debug.Log("Collided with sumthn");
                if (!collisionInfo.IsCollidingWithLine && hit.transform.tag == "Line") {
                    Debug.Log("Collided with line");
                    collisionInfo.IsCollidingWithLine = true;
                    collisionInfo.LineCollisionPosition = hit.point;
                    collisionInfo.LineCollisionNormal = hit.normal;
                }

                if (hit.collider is EdgeCollider2D)
                {
                    lineCollider = (EdgeCollider2D)hit.collider;
                }
            }
        }
    }

    public void HorizontalCollisions(ref Vector2 velocity)
    {
        float directionX = collisionInfo.FaceDirection;
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            horizontalRayOrigin = (directionX == -1) ? raycastOrigins.BottomLeft : raycastOrigins.BottomRight;
            horizontalRayOrigin += (Vector2)transform.up * (horizontalRaySpacing * i);

            RaycastHit2D hit = Physics2D.Raycast(horizontalRayOrigin, transform.right * directionX, rayLength, collisionMask);     //Raycast hit - from origin in directionX with rayLength and only on collision layer
            Debug.DrawRay(horizontalRayOrigin, transform.right * directionX * rayLength, Color.red);

            //collisionInfo.below = hit;
        }
    }


    public void LineCollistion()
    {

        var rayDistance = 0.3f;
        Debug.DrawRay(raycastOrigins.Center, transform.up * -1 * rayDistance, Color.magenta);
        var hit = Physics2D.Raycast(raycastOrigins.Center, transform.up * -1, rayDistance, collisionMask);

        if (hit.collider != null && hit.transform.tag == "Line")
        {
            var hitPosition = new Vector2(hit.point.x, hit.point.y + (rayDistance / 2) * hit.normal.y);
            collisionInfo.IsCollidingWithLine = true;
            collisionInfo.LineCollisionPosition = hitPosition;
            collisionInfo.LineCollisionNormal = hit.normal;
        }
        else
        {
            collisionInfo.IsCollidingWithLine = false;
        }

        //Debug.DrawRay(raycastOrigins.BottomLeft, transform.up * -1 * 0.3f, Color.magenta);
        //Debug.DrawRay(raycastOrigins.BottomRight, transform.up * -1 * 0.3f, Color.magenta);
        //var leftHit = Physics2D.Raycast(raycastOrigins.BottomLeft, transform.up * -1, 0.3f, collisionMask);
        //var rightHit = Physics2D.Raycast(raycastOrigins.BottomRight, transform.up * -1, 0.3f, collisionMask);

        //if (leftHit.collider != null && rightHit.collider != null && rightHit.collider == leftHit.collider) {
        //    Debug.Log("Collided with line!");
        //    var collisionPoint = Vector2.Lerp(leftHit.point, rightHit.point, 0.5f);
        //    collisionInfo.IsCollidingWithLine = true;
        //    collisionInfo.LineCollisionPosition = new Vector2(collisionPoint.x, collisionPoint.y + 0.4f);
        //    collisionInfo.LineCollisionNormal = (leftHit.normal + rightHit.normal) * 0.5f;

        //}
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawSphere(collisionInfo.LineCollisionPosition, 0.05f);

    }

    public struct CollisionInfo
    {
        public bool Above, Below;                       //Collision on above or below?
        public bool Left, Right;                        //Collision on right or left?
        public bool IsCollidingWithLine;
        public int FaceDirection;
        public Vector2 LineCollisionPosition, LineCollisionNormal, VelocityOld;

        public void Reset()                             //Function for resetting collisiondetection
        {
            Above = Below = Left = Right = false;
            FaceDirection = 1;
        }
    }
}
