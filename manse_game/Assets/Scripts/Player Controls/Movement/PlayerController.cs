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
        State = new BaseState(gameObject);
        TextField = gameObject.GetComponentInChildren<TMP_Text>();
    }
    
    void Update()
    {
        State.HandlePlayerInput();
    }

    void FixedUpdate()
    {
        if (Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f)
        {
            if(!gameObject.GetComponent<AudioSource>().isPlaying)
            {
                gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            if(gameObject.GetComponent<AudioSource>().isPlaying)
            {
                gameObject.GetComponent<AudioSource>().Pause();
            }
        }
    }
}