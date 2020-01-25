using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMask : MonoBehaviour
{
    public EdgeCollider2D lineCollider;

    private CircleCollider2D circleCollider;
    private Vector2 startPoint;
    private Vector2 endPoint;

    void Start()
    {
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log(collision.contacts.Length);
        foreach (ContactPoint2D contactPoint in collision.contacts)
        {
            Debug.DrawLine(contactPoint.point, contactPoint.normal, Color.red);
        }
    }
}
