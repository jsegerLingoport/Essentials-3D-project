using Unity.Entities;
using Unity.Mathematics;

namespace Game.Player.Components
{
    public struct PlayerInputData : IComponentData
    {
        public float2 Movement;
        public bool Jump;
        public bool Sprint;
    }
}