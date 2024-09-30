using Collectables.Authoring;
using Game.Player.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

namespace Collectables.Systems
{
    public struct CollectedCollectableEvent : IComponentData
    {
    }

    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSimulationGroup))] // We are updating after the physics simulation
    public partial struct CollectableManagerSystem : ISystem
    {
        private ComponentLookup<LocalTransform> _localTransformLookup;
        private ComponentLookup<Collectable> _collectableLookup;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SimulationSingleton>();
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            _localTransformLookup = state.GetComponentLookup<LocalTransform>(true);
            _collectableLookup = state.GetComponentLookup<Collectable>(true);
        }

        [BurstCompile]
        public partial struct CollectableManagerJob : ITriggerEventsJob
        {
            public EntityCommandBuffer.ParallelWriter CommandBuffer;
            [ReadOnly] public ComponentLookup<LocalTransform> LocalTransformLookup;
            [ReadOnly] public ComponentLookup<Collectable> CollectableLookup;
            [ReadOnly] public ComponentLookup<PlayerTag> PlayerTagLookup;

            public void Execute(TriggerEvent triggerEvent)
            {
                var entityA = triggerEvent.EntityA;
                var entityB = triggerEvent.EntityB;

                // Check if one of the entities is the player and the other is the collectable
                if (PlayerTagLookup.HasComponent(entityA) && CollectableLookup.HasComponent(entityB))
                {
                    // Handle collection for entityB
                    SpawnAndDestroyCollectible(entityB, 0);
                }
                else if (PlayerTagLookup.HasComponent(entityB) && CollectableLookup.HasComponent(entityA))
                {
                    // Handle collection for entityA
                    SpawnAndDestroyCollectible(entityA, 1);
                }
            }

            private void SpawnAndDestroyCollectible(Entity collectableEntity, int jobIndex)
            {
                var collectable = CollectableLookup[collectableEntity];
                var collectableTransform = LocalTransformLookup[collectableEntity];
                float3 collectablePosition = collectableTransform.Position;

                // Spawn destroy effect if available
                if (collectable.DestroyEffect != Entity.Null)
                {
                    var destroyEffectEntity = CommandBuffer.Instantiate(jobIndex, collectable.DestroyEffect);
                    CommandBuffer.SetComponent(jobIndex, destroyEffectEntity, new LocalTransform
                    {
                        Position = collectablePosition,
                        Rotation = quaternion.identity,
                        Scale = 1.0f
                    });
                }

                SpawnCollectibleEventEntity(collectableEntity, collectablePosition,collectable,jobIndex);
                // Destroy the collectable entity
                CommandBuffer.DestroyEntity(jobIndex, collectableEntity);
            }

            private void SpawnCollectibleEventEntity(Entity collectableEntity,float3 position,Collectable collectable, int jobIndex)
            {
                // Spawn destroy effect if available
                if (collectable.DestroyEffect == Entity.Null) return;
                var collectableCollectedEntity = CommandBuffer.CreateEntity(jobIndex);
                CommandBuffer.AddComponent<CollectedCollectableEvent>(jobIndex, collectableCollectedEntity);
                CommandBuffer.SetComponent(jobIndex, collectableCollectedEntity, new LocalTransform
                {
                    Position = position,
                    Rotation = quaternion.identity,
                    Scale = 1.0f
                });
            }
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // Update lookups to get the latest component data
            _localTransformLookup.Update(ref state);
            _collectableLookup.Update(ref state);

            // Get the EntityCommandBuffer for deferred entity commands
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();

            // Schedule the trigger events job
            var job = new CollectableManagerJob
            {
                CommandBuffer = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                LocalTransformLookup = _localTransformLookup,
                CollectableLookup = _collectableLookup,
                PlayerTagLookup = state.GetComponentLookup<PlayerTag>(true) // Add player tag lookup
            };

            state.Dependency = job.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
        }
    }
}