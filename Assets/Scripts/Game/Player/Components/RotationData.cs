using Unity.Entities;
using Unity.Mathematics;

namespace Game.Player.Components
{
    public struct RotationData : IComponentData
    {
        public float3 Axis;
        public float Speed;
    }
    
}