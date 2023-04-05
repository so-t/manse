using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    private Vector2 rotation = Vector2.zero;

    [Range(0f, 90f)] 
    public float RotationLimit = 88f;
	public float Sensitivity = 2f;
    public float RecenterSpeed = 0.5f;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetAxis("Camera Horizontal") != 0.0f || Input.GetAxis("Camera Vertical") != 0.0f)
        {
            rotation.x += Input.GetAxis("Camera Horizontal") * Sensitivity;
            rotation.y += Input.GetAxis("Camera Vertical") * Sensitivity;
            rotation.x = Mathf.Clamp(rotation.x, -RotationLimit, RotationLimit);
            rotation.y = Mathf.Clamp(rotation.y, -RotationLimit, RotationLimit);
        }
        else
        {
            rotation.x += Mathf.Sign(rotation.x + -Mathf.Sign(rotation.x) * RecenterSpeed) == Mathf.Sign(rotation.x) ? 
                            -Mathf.Sign(rotation.x) * RecenterSpeed : 0.0f;
            rotation.y += Mathf.Sign(rotation.y + -Mathf.Sign(rotation.y) * RecenterSpeed) == Mathf.Sign(rotation.y) ? 
                            -Mathf.Sign(rotation.y) * RecenterSpeed : 0.0f;
        }
		var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
		var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);
        
        transform.localRotation = xQuat * yQuat;
    }
}
