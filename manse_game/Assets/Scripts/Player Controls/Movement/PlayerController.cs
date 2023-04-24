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

    void Awake()
    {
        State = new Playing(gameObject);
        TextField = gameObject.GetComponentInChildren<TMP_Text>();
    }

    public bool isPaused() 
    {
        return (State.GetType() == typeof(Paused));
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
            else if (isPaused())
            {
                State = new Playing(gameObject);
                Time.timeScale = 1;
            }
        }
    }
}