using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{   
    public float Speed = 10f;
    public float TurnSpeed = 125f;
    
    public PlayerState State;
    public TMP_Text TextField;
    public CameraRotation CamRotation;


    void Awake()
    {
        State = new Playing(gameObject);
        TextField = gameObject.GetComponentInChildren<TMP_Text>();
        CamRotation = gameObject.GetComponentInChildren<CameraRotation>();
    }

    public bool IsPaused() 
    {
        return (State.GetType() == typeof(Paused));
    }

    public bool Interact(Transform lookAtTarget=null)
    {
        if (State.GetType() == typeof(Playing))
        {
            State = new Interacting(gameObject);
            if (lookAtTarget != null) CamRotation.LookAt(lookAtTarget);
            return true;
        } 
        else return false;
    }

    public void ReturnToPlayState()
    {
        State = new Playing(gameObject);
    }

    void Update()
    {
        State.HandlePlayerInput();

        if (Input.GetKeyDown("escape"))
        {
            if (State.GetType() == typeof(Playing))
            {
                State = new Paused(gameObject);
                Time.timeScale = 0;
            }
            else if (IsPaused())
            {
                ReturnToPlayState();
                Time.timeScale = 1;
            }
        }
    }
}