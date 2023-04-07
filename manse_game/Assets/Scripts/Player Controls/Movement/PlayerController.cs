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
}
