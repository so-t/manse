using UnityEngine;

namespace PlayerControls.Controller
{
    public class InspectionControls : MonoBehaviour
    {
        private const float Speed = 30;

        public void Destroy() => Destroy(this);

        private void FixedUpdate()
        {
            if (Input.GetAxisRaw("Horizontal") != 0.0f || Input.GetAxisRaw("Vertical") != 0.0f)
            {
             
                var worldRotation = new Vector3(
                    Input.GetAxisRaw("Vertical"), 
                    Input.GetAxisRaw("Horizontal"), 
                    0);
                var localRotation = transform.InverseTransformVector(worldRotation);
                
                transform.Rotate(localRotation * (Speed * Time.fixedDeltaTime));
            }
            else if (Input.GetMouseButton(0))
            {
                var worldRotation = new Vector3(
                    Input.GetAxis("Mouse Y"), 
                    -Input.GetAxis("Mouse X"), 
                    0);
                var localRotation = transform.InverseTransformVector(worldRotation);
                
                transform.Rotate(localRotation * (2 * Speed * Time.fixedDeltaTime));
            }
            else if (Input.GetKeyDown("escape"))
            {
                Destroy(this);
            }
        }
    }
}