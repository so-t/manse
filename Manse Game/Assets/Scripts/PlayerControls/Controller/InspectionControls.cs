using UnityEngine;

namespace PlayerControls.Controller
{
    public class InspectionControls : MonoBehaviour
    {
        public int Speed = 200;
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = gameObject.GetComponent<Rigidbody>();
        }

        public void Destroy() => Destroy(this);

        private void FixedUpdate()
        {
            if (Input.GetAxisRaw("Horizontal") != 0.0f || Input.GetAxisRaw("Vertical") != 0.0f)
            {
                var v = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
                var delta = Quaternion.Euler(v * (Speed * Time.fixedDeltaTime));
                _rb.MoveRotation(_rb.rotation * delta);
            }
            if (Input.GetKeyDown("escape"))
            {
                Destroy(this);
            }
        }
    }
}