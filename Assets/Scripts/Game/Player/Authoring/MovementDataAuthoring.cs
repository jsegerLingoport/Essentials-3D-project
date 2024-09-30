using Game.Player.Components;
using Unity.Entities;
using UnityEngine;

namespace Game.Player.Authoring
{
    public class MovementDataAuthoring : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 1f;
        [SerializeField] private float rotationSpeed = 1f;

        public class MovementDataBaker : Baker<MovementDataAuthoring>
        {
            public override void Bake(MovementDataAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity,
                    new MovementData
                    {
                        MovementSpeed = authoring.movementSpeed,
                        RotationSpeed = authoring.rotationSpeed,
                        isMovingHorizontally = true
                    });
            }
        }
    }
}