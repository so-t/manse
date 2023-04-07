using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHeadBob : MonoBehaviour
{
    public float walkingBobbingSpeed = 14f;
    public float bobbingAmount = 0.05f;

    private float defaultPosY = 0;
    private float timer = 0;

    // Start is called before the first frame update
    void Awake()
    {
        defaultPosY = transform.localPosition.y;
    }

    public void HeadBob()
    {
        if(Input.GetAxis("Vertical") != 0.0f)
        {
            //Player is moving
            timer += Time.deltaTime * walkingBobbingSpeed;
            transform.localPosition = new Vector3(transform.localPosition.x, defaultPosY + Mathf.Sin(timer) * 
                                            bobbingAmount, transform.localPosition.z);
        }
        else
        {
            //Idle
            timer = 0;
            transform.localPosition = new Vector3(transform.localPosition.x, 
                                                    Mathf.Lerp(transform.localPosition.y, defaultPosY, 
                                                                Time.deltaTime * walkingBobbingSpeed), 
                                                    transform.localPosition.z);
        }
    }
}
