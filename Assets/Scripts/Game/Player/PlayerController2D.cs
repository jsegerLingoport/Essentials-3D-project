using UnityEngine;

namespace Game.Player
{
    public class PlayerController2D : PlayerControllerBase<Rigidbody2D,Vector2>
    {

        private bool _isMovingHorizontally = true; // Flag to track if the player is moving horizontally

        protected override void UpdateInput()
        {
            // Check if diagonal movement is allowed
            if (canMoveDiagonally)
            {
                // Set movement direction based on input
                Movement = new Vector2(HorizontalInput, VerticalInput);
                // Rotate the player based on movement direction
                RotatePlayer(HorizontalInput, VerticalInput);
            }
            else
            {
                // Determine the priority of movement based on input
                if (HorizontalInput != 0)
                {
                    _isMovingHorizontally = true;
                }
                else if (VerticalInput != 0)
                {
                    _isMovingHorizontally = false;
                }

                // Set movement direction and rotate the player
                if (_isMovingHorizontally)
                {
                    Movement = new Vector2(HorizontalInput, 0);
                    RotatePlayer(HorizontalInput, 0);
                }
                else
                {
                    Movement = new Vector2(0, VerticalInput);
                    RotatePlayer(0, VerticalInput);
                }
            }
        }


        protected override void MovementUpdate()
        {
            riggedBody.velocity = Movement * speed;
        }

        protected override void RotationUpdate()
        {
            if (canMoveDiagonally)
            {
                RotatePlayer(HorizontalInput, VerticalInput);
            }
            else
            {

                if (_isMovingHorizontally)
                {

                    RotatePlayer(HorizontalInput, 0);
                }
                else
                {

                    RotatePlayer(0, VerticalInput);
                }
            }
        }
        
        private void RotatePlayer(float x, float y)
        {
            // If there is no input, do not rotate the player
            if (x == 0 && y == 0) return;

            // Calculate the target rotation angle based on input direction
            float targetAngle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;

            // Get the current rotation angle
            float currentAngle = transform.eulerAngles.z;

            // Smoothly rotate towards the target angle using Lerp
            float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);

            // Apply the new rotation to the player
            transform.rotation = Quaternion.Euler(0, 0, newAngle);
        }

        private void OnTriggerEnter2D(Collider2D other) => OnHitCollectable(other);
        [SerializeField] private bool canMoveDiagonally = true; // Controls whether the player can move diagonally
    }
}
