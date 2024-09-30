using UnityEngine;

// Controls player movement and rotation.
namespace Game.Player
{
    public class PlayerController : PlayerControllerBase<Rigidbody,Vector3>
    {
        [SerializeField] private float jumpForce = 5.0f;
        private bool JumpInput => Input.GetButtonDown("Jump");

        protected override void UpdateInput()
        {
            if (JumpInput) {
                riggedBody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            }
        }

        protected override void MovementUpdate()
        {
             Movement = transform.forward * (VerticalInput * speed * Time.fixedDeltaTime);
            riggedBody.MovePosition(riggedBody.position + Movement);
        }

        protected override void RotationUpdate()
        {
            // Rotate player based on horizontal input.
            float turn = HorizontalInput * rotationSpeed * Time.fixedDeltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
            riggedBody.MoveRotation(riggedBody.rotation * turnRotation);
        }

        private void OnTriggerEnter(Collider other) => OnHitCollectable(other);

    }
}