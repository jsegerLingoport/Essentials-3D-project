using UnityEngine;

namespace Game.Player
{
    public abstract class PlayerControllerBase<TCastRiggedBody,TMovement> : MonoBehaviour 
    {
        [SerializeField] protected float speed = 5.0f;
        [SerializeField] protected float rotationSpeed = 120.0f;
        [SerializeField] protected TCastRiggedBody riggedBody;

        protected TMovement Movement; 
        protected float HorizontalInput => Input.GetAxisRaw("Horizontal");
        protected float VerticalInput => Input.GetAxisRaw("Vertical");


        protected abstract void UpdateInput();

        protected void Update()
        {
            UpdateInput();
        }

        protected abstract void MovementUpdate();
        protected abstract void RotationUpdate();

        protected void OnHitCollectable(Component other)
        {
        
        }

        public void FixedUpdate()
        {
            MovementUpdate();
            RotationUpdate();
        }

        public void Refresh()
        {
            riggedBody = GetComponent<TCastRiggedBody>();
        }
    }
}