using JSE.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace JSE.Systems
{
    public partial struct PlateManagerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<RestartPlateRequest>();
            state.RequireForUpdate<PlateManagerData>();
            state.RequireForUpdate<PlateTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var plateManagerData = SystemAPI.GetSingleton<PlateManagerData>();
            var plateEntity = SystemAPI.GetSingletonEntity<PlateTag>();
            
            foreach (var  (request,entity) in SystemAPI.Query<RefRO<RestartPlateRequest>>().WithEntityAccess())
            {
                var restartPosition = LocalTransform.FromPositionRotation(plateManagerData.StartPosition,plateManagerData.StartRotation);
                ecb.SetComponent(plateEntity,restartPosition);
                
                var physicsVelocity = state.EntityManager.GetComponentData<PhysicsVelocity>(plateEntity);
                physicsVelocity.Linear = float3.zero;
                physicsVelocity.Angular = float3.zero;
                ecb.SetComponent(plateEntity, physicsVelocity);
                ecb.DestroyEntity(entity);
            }
            ecb.Playback(state.EntityManager);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
        
        
        public void ResetPhysics(Entity entity, EntityManager entityManager)
        {
            // Check if the entity has a PhysicsVelocity component and reset it
            if (entityManager.HasComponent<PhysicsVelocity>(entity))
            {
              
            }
            
        }
    }
}