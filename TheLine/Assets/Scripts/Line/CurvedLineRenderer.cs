using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent( typeof(LineRenderer) )]
public class CurvedLineRenderer : MonoBehaviour 
{
	public float lineSegmentSize = 0.15f;
	public float lineWidth = 0.1f;
	[Header("Gizmos")]
	public bool showGizmos = true;
	public float gizmoSize = 0.1f;
	public Color gizmoColor = new Color(1,0,0,0.5f);

	private CurvedLinePoint[] linePoints = new CurvedLinePoint[0];
	private Vector3[] linePositions = new Vector3[0];
	private Vector3[] linePositionsOld = new Vector3[0];

	public void Update () 
	{
		GetPoints();
		SetPointsToLine();
	}

	void GetPoints()
	{
		// Find curved points in children
		linePoints = GetComponentsInChildren<CurvedLinePoint>();

		// Add positions
		linePositions = new Vector3[linePoints.Length];
		for( int i = 0; i < linePoints.Length; i++ )
		{
			linePositions[i] = linePoints[i].transform.position;
		}
	}

	void SetPointsToLine()
	{
		// Create old positions if they dont match
		if( linePositionsOld.Length != linePositions.Length )
		{
			linePositionsOld = new Vector3[linePositions.Length];
		}

		// Check if line points have moved
		bool moved = false;
		for( int i = 0; i < linePositions.Length; i++ )
		{
			// Compare
			if( linePositions[i] != linePositionsOld[i] )
			{
				moved = true;
			}
		}

		// Update if moved
		if( moved == true )
		{
			LineRenderer line = this.GetComponent<LineRenderer>();

			// Get smoothed values
			Vector3[] smoothedPoints = LineSmoother.SmoothLine( linePositions, lineSegmentSize );

			// Set line settings
			line.positionCount = smoothedPoints.Length;
			line.SetPositions( smoothedPoints );
			line.startWidth = lineWidth;
            line.endWidth = lineWidth;
        }
	}

	void OnDrawGizmosSelected()
	{
		Update();
	}

	void OnDrawGizmos()
	{
		if (linePoints.Length == 0)
		{
			GetPoints();
		}

		// Settings for gizmos
		foreach (CurvedLinePoint linePoint in linePoints)
		{
			linePoint.showGizmo = showGizmos;
			linePoint.gizmoSize = gizmoSize;
			linePoint.gizmoColor = gizmoColor;
		}
	}
}
