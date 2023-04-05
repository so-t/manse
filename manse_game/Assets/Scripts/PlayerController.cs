using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 velocity;
    
    public float Speed = 10f;
    public float TurnSpeed = 125f;
    
    void Update()
    {
        // Handle Movement
        if (Input.GetAxis("Vertical") != 0.0f)
        {
            velocity = new Vector3(0.0f, 0.0f, Speed * Input.GetAxisRaw("Vertical"));
            transform.Translate(velocity * Time.deltaTime);
        }

        // Handle Turning
        if (Input.GetAxis("Horizontal") != 0.0f)
        {
            velocity = new Vector3(0.0f, TurnSpeed * Input.GetAxisRaw("Horizontal"), 0.0f);
            transform.Rotate(velocity * Time.deltaTime);
        }
    }
}
