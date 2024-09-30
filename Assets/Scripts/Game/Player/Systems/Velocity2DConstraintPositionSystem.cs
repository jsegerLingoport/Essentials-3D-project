using Game.Player.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Player.Systems
{
    public partial struct Velocity2DConstraintPositionSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Object2D>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (transform,constraintZPosition) in SystemAPI.Query<RefRW<LocalTransform>,RefRO<ConstraintZPosition>>().WithAll<Object2D>())
            {
                transform.ValueRW.Position = new float3(transform.ValueRW.Position.xy, constraintZPosition.ValueRO.Value);
            }
        }
        
    }
}