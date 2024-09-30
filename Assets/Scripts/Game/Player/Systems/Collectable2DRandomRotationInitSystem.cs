using Game.Player.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Player.Systems
{
    [BurstCompile]
    public partial struct Collectable2DRandomRotationInitSystem : ISystem
    {
        private Random _random;

        public void OnCreate(ref SystemState state)
        {
            // Initialize the random state with a seed
            _random = new Random((uint)UnityEngine.Random.Range(1, 10000));

            // Ensure the system only runs if there are new collectables
            state.RequireForUpdate<Random2DRotation>();
        }

        public void OnUpdate(ref SystemState state)
        {
            // Create an EntityCommandBuffer to defer structural changes
            var ecb = new EntityCommandBuffer(Allocator.TempJob);

            // Loop through all entities with CollectableTag that do not yet have RotationData
            foreach (var  (localTransform,entity) in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<Random2DRotation>().WithNone<RotationData,Random3DRotation>().WithEntityAccess())
            {
                // Generate a random axis for rotation using Unity.Mathematics.Random
                float3 randomAxis = math.normalize(new float3(
                    0,
                    0,
                    _random.NextFloat(-1f, 1f)
                ));

                // Generate a random rotation speed using Unity.Mathematics.Random
                float randomSpeed = _random.NextFloat(1f, 5f);

                // Use the ECB to add RotationData after iteration is complete
                ecb.AddComponent(entity, new RotationData
                {
                    Axis = randomAxis,
                    Speed = randomSpeed
                });
            }

            // Play back the ECB to apply the deferred structural changes
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}