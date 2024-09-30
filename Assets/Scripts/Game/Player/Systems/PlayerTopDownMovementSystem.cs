using Game.Player.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Game.Player.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(PlayerInputSystem))]
    public partial struct PlayerTopDownMovementSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            // Ensure the system processes entities with the TopDownPlayer2DTag
            state.RequireForUpdate<TopDownPlayer2DTag>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;

            // Schedule the job with additional movement validation and clamping logic
            var jobHandle = new PlayerTopDownMovementJob
            {
                DeltaTime = deltaTime
            }.ScheduleParallel(state.Dependency);

            // Set the job handle as the system's dependency
            state.Dependency = jobHandle;
        }

        
          [BurstCompile]
        public partial struct PlayerTopDownMovementJob : IJobEntity
        {
            public float DeltaTime;

            public void Execute(ref PhysicsVelocity velocity, ref LocalTransform transform,
                ref MovementData movementData, in PlayerInputData inputData)
            {
                // Extract movement input from the player input data
                float2 movement = inputData.Movement;
                
                // Apply movement based on the input and movement speed
                MovePlayer(ref velocity, movement, ref movementData, DeltaTime);
            }

            // Reuse the movement and rotation logic from the shared job
            private static void MovePlayer(ref PhysicsVelocity velocity, float2 movement, ref MovementData movementData,
                float deltaTime)
            {
                // If the player is moving, apply the movement smoothly
                if (math.lengthsq(movement) > 0f)
                {
                    var movementFactor = movementData.MovementSpeed;
                    var smoothMovement = movement * movementFactor;
                    velocity.Linear.xy = math.lerp(velocity.Linear.xy, smoothMovement, deltaTime * 10f);
                }
                else
                {
                    velocity.Linear.xy = math.lerp(velocity.Linear.xy, float2.zero, deltaTime * 10f);
                }
            }
            
        }
        
      
    }
    
}