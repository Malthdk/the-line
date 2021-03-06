using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineSmoother : MonoBehaviour 
{
	public static Vector3[] SmoothLine( Vector3[] inputPoints, float segmentSize )
	{
		// Create curves
		AnimationCurve curveX = new AnimationCurve();
		AnimationCurve curveY = new AnimationCurve();
		AnimationCurve curveZ = new AnimationCurve();

		// Create keyframe sets
		Keyframe[] keysX = new Keyframe[inputPoints.Length];
		Keyframe[] keysY = new Keyframe[inputPoints.Length];
		Keyframe[] keysZ = new Keyframe[inputPoints.Length];

		// Set keyframes
		for( int i = 0; i < inputPoints.Length; i++ )
		{
			keysX[i] = new Keyframe( i, inputPoints[i].x );
			keysY[i] = new Keyframe( i, inputPoints[i].y );
			keysZ[i] = new Keyframe( i, inputPoints[i].z );
		}

		// Apply keyframes to curves
		curveX.keys = keysX;
		curveY.keys = keysY;
		curveZ.keys = keysZ;

		// Smooth curve tangents
		for( int i = 0; i < inputPoints.Length; i++ )
		{
			curveX.SmoothTangents( i, 0 );
			curveY.SmoothTangents( i, 0 );
			curveZ.SmoothTangents( i, 0 );
		}

		// List to write smoothed values to
		List<Vector3> lineSegments = new List<Vector3>();

		// Find segments in each section
		for( int i = 0; i < inputPoints.Length; i++ )
		{
			// Add first point
			lineSegments.Add( inputPoints[i] );

			// Make sure within range of array
			if( i+1 < inputPoints.Length )
			{
				// Find distance to next point
				float distanceToNext = Vector3.Distance(inputPoints[i], inputPoints[i+1]);

				// Number of segments
				int segments = (int)(distanceToNext / segmentSize);

				// Add segments
				for( int s = 1; s < segments; s++ )
				{
					// Interpolated time on curve
					float time = ((float)s/(float)segments) + (float)i;

					// Sample curves to find smoothed position
					Vector3 newSegment = new Vector3( curveX.Evaluate(time), curveY.Evaluate(time), curveZ.Evaluate(time) );

					// Add to list
					lineSegments.Add( newSegment );
				}
			}
		}

		return lineSegments.ToArray();
	}

}
