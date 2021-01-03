using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMask : MonoBehaviour
{
    public EdgeCollider2D lineCollider;
    public LineRenderer lineRenderer;

    private Vector2 startPoint;
    private Vector2 endPoint;
    private Vector2 playerStartPoint;
    private Vector2 playerEndPoint;

    private LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));


        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.green, 0.2f), new GradientColorKey(Color.red, 0.20001f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );

        gradient.mode = GradientMode.Fixed;
        lr.colorGradient = gradient;
    }
}
