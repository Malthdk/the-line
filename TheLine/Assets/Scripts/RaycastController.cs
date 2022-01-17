using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour
{
    public const float skinWidth = 0f;           //Small field within the character for the ray to start in
    public int horizontalRayCount = 4;              //Number of horizontal rays
    public int verticalRayCount = 4;                //Number of Vertical rays
    public LayerMask collisionMask;

    internal float horizontalRaySpacing;              //Space betweeen rays on horizontal rays
    internal float verticalRaySpacing;                //Space between rays on vertical rays

    [HideInInspector]
    public BoxCollider2D originCollider;                  //Calling boxcollider
    [HideInInspector]
    public RaycastOrigins raycastOrigins;           //Calling raycastorigins
    [HideInInspector]
    public Vector2 horizontalRayOrigin, verticalRayOrigin;

    public virtual void Awake()
    {
        originCollider = GetComponent<BoxCollider2D>();	//Getting boxcollider
        CalculateRaySpacing();
    }

    public virtual void Update()
    {
        UpdateRaycastOrigins();                     //Now capable of scaling at run time
    }

    //RAYCAST ORIGIN
    public void UpdateRaycastOrigins()
    {
        Bounds bounds = originCollider.bounds;

        var offset = originCollider.offset;
        var extents = originCollider.size * 0.5f;

        raycastOrigins.BottomLeft = transform.TransformPoint(new Vector2(-extents.x, -extents.y) + offset);        //Setting bottom left corner origin as minium x and minimum y
        raycastOrigins.BottomRight = transform.TransformPoint(new Vector2(extents.x, -extents.y) + offset);       //Setting bottom right corner origin as maximum x and minimum y
        raycastOrigins.TopLeft = transform.TransformPoint(new Vector2(-extents.x, extents.y) + offset);           //Setting top left corner origin as minimum x and maximum y
        raycastOrigins.TopRight = transform.TransformPoint(new Vector2(extents.x, extents.y) + offset);          //Setting top right corner origin as maximum x and maximum y

        raycastOrigins.Bottom = new Vector2(bounds.center.x, bounds.min.y);        
        raycastOrigins.Top = new Vector2(bounds.center.x, bounds.max.y);       
        raycastOrigins.Left = new Vector2(bounds.min.x, bounds.center.y);           
        raycastOrigins.Right = new Vector2(bounds.max.x, bounds.center.y);

        raycastOrigins.Center = new Vector2(bounds.center.x, bounds.center.y);
    }

    //SPACE BETWEEN RAYS
    public void CalculateRaySpacing()
    {
        Bounds bounds = originCollider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    public struct RaycastOrigins
    {
        public Vector2 TopLeft, TopRight;
        public Vector2 BottomLeft, BottomRight;
        public Vector2 Top, Bottom, Left, Right, Center;
    }
}
