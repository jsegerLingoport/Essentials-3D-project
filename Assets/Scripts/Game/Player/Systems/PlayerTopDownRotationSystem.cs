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
    public partial struct PlayerTopDownRotationSystem : ISystem
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
            var jobHandle = new PlayerTopDownRotationJob
            {
                DeltaTime = deltaTime
            }.ScheduleParallel(state.Dependency);

            // Set the job handle as the system's dependency
            state.Dependency = jobHandle;
        }
        
        [BurstCompile]
        public partial struct PlayerTopDownRotationJob : IJobEntity
        {
            public float DeltaTime;

            public void Execute(ref PhysicsVelocity velocity, ref LocalTransform transform,
                ref MovementData movementData, in PlayerInputData inputData)
            {
                ApplyRotation(ref transform, inputData.Movement, DeltaTime, movementData.RotationSpeed);
            }

            private static void ApplyRotation(ref LocalTransform transform, float2 movement, float deltaTime,
                float rotationSpeed)
            {
                if (math.lengthsq(movement) == 0f)
                    return;

                var targetAngle = math.atan2(movement.y, movement.x);
                var targetAngleDegrees = math.degrees(targetAngle);
                var currentAngleDegrees = GetZRotationFromQuaternion(transform.Rotation);

                var angleDifference = targetAngleDegrees - currentAngleDegrees;
                if (angleDifference > 180f) angleDifference -= 360f;
                if (angleDifference < -180f) angleDifference += 360f;

                var newAngleDegrees = currentAngleDegrees + angleDifference * deltaTime * rotationSpeed;
                transform.Rotation = quaternion.EulerXYZ(0, 0, math.radians(newAngleDegrees));
            }

            private static float GetZRotationFromQuaternion(quaternion rotation)
            {
                var euler = math.Euler(rotation);
                return math.degrees(euler.z);
            }
        }
    }
}