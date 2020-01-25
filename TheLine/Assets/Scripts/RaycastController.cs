using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour
{
    public const float skinWidth = .015f;           //Small field within the character for the ray to start in
    public int horizontalRayCount = 4;              //Number of horizontal rays
    public int verticalRayCount = 4;                //Number of Vertical rays
    public LayerMask collisionMask;

    internal float horizontalRaySpacing;              //Space betweeen rays on horizontal rays
    internal float verticalRaySpacing;                //Space between rays on vertical rays

    [HideInInspector]
    public BoxCollider2D collider;                  //Calling boxcollider
    [HideInInspector]
    public RaycastOrigins raycastOrigins;           //Calling raycastorigins
    [HideInInspector]
    public Vector2 horizontalRayOrigin, verticalRayOrigin;

    public virtual void Awake()
    {
        collider = GetComponent<BoxCollider2D>();	//Getting boxcollider
        CalculateRaySpacing();
    }

    public virtual void FixedUpdate()
    {
        UpdateRaycastOrigins();                     //Now capable of scaling at run time
    }

    //RAYCAST ORIGIN
    public void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        //bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);        //Setting bottom left corner origin as minium x and minimum y
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);       //Setting bottom right corner origin as maximum x and minimum y
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);           //Setting top left corner origin as minimum x and maximum y
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);          //Setting top right corner origin as maximum x and maximum y

        raycastOrigins.bottom = new Vector2(bounds.center.x, bounds.min.y);        
        raycastOrigins.top = new Vector2(bounds.center.x, bounds.max.y);       
        raycastOrigins.left = new Vector2(bounds.min.x, bounds.center.y);           
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.center.y);          
    }

    //SPACE BETWEEN RAYS
    public void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
        public Vector2 top, bottom;
        public Vector2 left, right;
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
