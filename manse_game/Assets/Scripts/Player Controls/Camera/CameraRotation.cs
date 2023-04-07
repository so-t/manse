using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    private Vector2 rotation = Vector2.zero;

    [Range(0f, 90f)] 
    public float RotationLimit = 88f;

    public GameObject Player;
    
	public float UserSpeed = 2f;
    public float EventSpeed = 0.5f;

    public bool HasTarget;
    private Vector3 targetPosition;
    private Vector3 prevLookTarget;

    public void returnToLookTarget()
    {
        // Currently Returning to the forward before LookAt was prompted jerks the camera very fast. 
        // Need to slow this down.
        this.HasTarget = true;
        targetPosition = prevLookTarget;
    }

    public void LookAt(Transform target)
    {
        this.HasTarget = true;
        this.targetPosition = target.position;
        this.prevLookTarget = transform.position + transform.forward;
    }
    
    private void SmoothLookAt(Vector3 target)
    {
        Vector3 forward = transform.forward;
        Quaternion toRotation = Quaternion.LookRotation(target - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, EventSpeed * Time.time);
        if(forward == transform.forward) this.HasTarget = false;
    }

    void Update()
    {
        if (Player.GetComponent<PlayerController>().State.GetType() != typeof(Interacting))
        {
            if (Input.GetAxis("Camera Horizontal") != 0.0f || Input.GetAxis("Camera Vertical") != 0.0f)
            {
                rotation.x += Input.GetAxis("Camera Horizontal") * UserSpeed;
                rotation.y += Input.GetAxis("Camera Vertical") * UserSpeed;
                rotation.x = Mathf.Clamp(rotation.x, -RotationLimit, RotationLimit);
                rotation.y = Mathf.Clamp(rotation.y, -RotationLimit, RotationLimit);
            }
            else
            {
                rotation.x += Mathf.Sign(rotation.x + -Mathf.Sign(rotation.x) * EventSpeed) == Mathf.Sign(rotation.x) ? 
                                -Mathf.Sign(rotation.x) * EventSpeed : 0.0f;
                rotation.y += Mathf.Sign(rotation.y + -Mathf.Sign(rotation.y) * EventSpeed) == Mathf.Sign(rotation.y) ? 
                                -Mathf.Sign(rotation.y) * EventSpeed : 0.0f;
            }
            var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
            var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);
            
            transform.localRotation = xQuat * yQuat;
        }
    }

    void FixedUpdate()
    {
        if (HasTarget) SmoothLookAt(targetPosition);
    }
}
