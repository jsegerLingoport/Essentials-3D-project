using Unity.Entities;

namespace Game.Player.Components
{
    public struct DestroyAfterTime : IComponentData
    {
        public float TimeToLive; // Time remaining before the entity should be destroyed
    }
}