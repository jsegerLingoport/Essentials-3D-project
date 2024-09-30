using Unity.Entities;

namespace Game.Player.Components
{
    public struct PlayerTag : IComponentData
    {
        
    }
    public struct TopDownPlayer2DTag : IComponentData
    {
       
    }
    
    public struct MovementData : IComponentData
    {
        public float MovementSpeed;
        public float RotationSpeed;
        public bool isMovingHorizontally;

    }

    public struct Player3DTag : IComponentData
    {
        
    }
}