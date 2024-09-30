using Unity.Entities;
using Unity.Mathematics;

namespace Game.Player.Components
{
    public struct ConstraintZRotation : IComponentData
    {
        public float3 Value;
    }
}