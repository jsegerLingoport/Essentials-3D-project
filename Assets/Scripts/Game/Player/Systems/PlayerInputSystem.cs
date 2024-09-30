using Game.Player.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Player.Systems
{
    [BurstCompile]
    public partial class PlayerInputSystem : SystemBase
    {
        private PlayerInputs _playerInput;

        protected override void OnCreate()
        {
            // Initialize PlayerInputs (Input System)
            _playerInput = new PlayerInputs();
            _playerInput.Enable();
            
            base.OnCreate();
        }

    

        protected override void OnDestroy()
        {
            // Disable input actions when the system is destroyed
            _playerInput.Disable();
            base.OnDestroy();
        }

        [BurstCompile]
        public partial struct PlayerInputJob : IJobEntity
        {
            public float2 MovementInput;
            public bool JumpInput;
            public bool SprintInput;

            public void Execute(ref PlayerInputData playerInputData)
            {
                // Store inputs in the PlayerInputData component
                playerInputData.Movement = MovementInput;
                playerInputData.Jump = JumpInput;
                playerInputData.Sprint = SprintInput;
            }
        }

        protected override void OnUpdate()
        {
            // Read inputs on the main thread
            float2 movementInput = _playerInput.Player.Move.ReadValue<Vector2>();
            var jumpInput = _playerInput.Player.Jump.IsPressed();
            var sprintInput = _playerInput.Player.Sprint.IsPressed();

            // Schedule the job to update PlayerInputData component
            var inputJob = new PlayerInputJob
            {
                MovementInput = movementInput,
                JumpInput = jumpInput,
                SprintInput = sprintInput
            };

            Dependency = inputJob.ScheduleParallel(Dependency);
        }
    }
}