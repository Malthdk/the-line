using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PlayerEvents : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnLandEvent;
    public UnityEvent OnJumpEvent;
    private PlayerCollisions playerCollisions;
    private Transform glue;

    private static PlayerEvents _instance;

    public static PlayerEvents Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }

        if (OnLandEvent == null)
        {
            OnLandEvent = new UnityEvent();
        }

        if (OnJumpEvent == null)
        {
            OnJumpEvent = new UnityEvent();
        }

        OnLandEvent.AddListener(Landed);
        OnJumpEvent.AddListener(Jumped);

        playerCollisions = GetComponentInChildren<PlayerCollisions>();
        glue = transform.Find("Glue");
    }

    public void Landed()
    {
        Debug.Log("Player has landed!");

    }

    public void Jumped()
    {
        Debug.Log("Player has jumped!");
    }
}
