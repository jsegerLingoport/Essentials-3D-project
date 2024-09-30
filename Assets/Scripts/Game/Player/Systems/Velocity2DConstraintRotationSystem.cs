using Game.Player.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;

namespace Game.Player.Systems
{
    public partial struct Velocity2DConstraintRotationSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Object2D>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (velocity,constraintZRotation) in SystemAPI.Query<RefRW<PhysicsVelocity>,RefRO<ConstraintZRotation>>().WithAll<Object2D>())
            {
                velocity.ValueRW.Angular = constraintZRotation.ValueRO.Value;
            }
        }
        
    }
}