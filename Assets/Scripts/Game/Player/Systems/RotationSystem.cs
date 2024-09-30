using Game.Player.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Player.Systems
{
    [BurstCompile]
    public partial struct RotationSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            // Ensure the system only updates if there are entities with RotationData
            state.RequireForUpdate<RotationData>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;

            // Schedule the job to rotate the entities
            var jobHandle = new RotationJob
            {
                DeltaTime = deltaTime
            }.ScheduleParallel(state.Dependency);

            // Set the job handle as the system's dependency
            state.Dependency = jobHandle;
        }

        [BurstCompile]
        public partial struct RotationJob : IJobEntity
        {
            public float DeltaTime;

            public void Execute(ref RotationData rotationData, ref LocalTransform localTransform)
            {
                // Calculate the new rotation based on the rotation speed and axis
                var rotation = quaternion.AxisAngle(rotationData.Axis, rotationData.Speed * DeltaTime);

                // Apply the new rotation to the current local transform's rotation
                localTransform.Rotation = math.mul(localTransform.Rotation, rotation);
            }
        }
    }
}