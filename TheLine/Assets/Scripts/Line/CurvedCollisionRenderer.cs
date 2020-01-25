using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(EdgeCollider2D))]
public class CurvedCollisionRenderer : MonoBehaviour 
{

    private EdgeCollider2D edgeCollider;
    private LineRenderer lineRenderer;
    private Vector2[] linePositions = new Vector2[0];

    private void Start()
    {
        edgeCollider = gameObject.GetComponent<EdgeCollider2D>();
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        CalculateCollisionPoints();
    }
     
    void CalculateCollisionPoints()
	{
        linePositions = new Vector2[lineRenderer.positionCount];
        edgeCollider.offset = transform.position*-1;

        for ( int i = 0; i < lineRenderer.positionCount; i++ )
		{
            linePositions[i] = lineRenderer.GetPosition(i);
        }

        edgeCollider.points = linePositions;
    }
}
