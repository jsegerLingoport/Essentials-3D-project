using Game.Player.Components;
using Unity.Burst;
using Unity.Entities;

namespace Game.Player.Systems
{
    [BurstCompile]
    public partial struct DestroyAfterTimeSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            // Require the system to run only if there are entities with DestroyAfterTime component
            state.RequireForUpdate<DestroyAfterTime>();
        }

        public void OnUpdate(ref SystemState state)
        {
            // Create an EntityCommandBuffer to queue structural changes (like entity destruction)
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
        
            float deltaTime = SystemAPI.Time.DeltaTime;

            // Schedule the job to update entities with the DestroyAfterTime component
            var jobHandle = new DestroyAfterTimeJob
            {
                DeltaTime = deltaTime,
                CommandBuffer = ecb
            }.ScheduleParallel(state.Dependency);

            // Set the job as a dependency
            state.Dependency = jobHandle;
        }

        [BurstCompile]
        public partial struct DestroyAfterTimeJob : IJobEntity
        {
            public float DeltaTime;
            public EntityCommandBuffer.ParallelWriter CommandBuffer;

            public void Execute(Entity entity, [EntityIndexInQuery] int entityIndex, ref DestroyAfterTime destroyAfterTime)
            {
                // Decrease the remaining time
                destroyAfterTime.TimeToLive -= DeltaTime;

                // If time is up, destroy the entity
                if (destroyAfterTime.TimeToLive <= 0f)
                {
                    CommandBuffer.DestroyEntity(entityIndex, entity);
                }
            }
        }
    }
}