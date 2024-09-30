using Game.Player.Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Player.Authoring
{
    public class ConstraintZRotationAuthoring : MonoBehaviour
    {
        [SerializeField]private float3 constraintZRotation;

        public class ConstraintZRotationBaker : Baker<ConstraintZRotationAuthoring>
        {
            public override void Bake(ConstraintZRotationAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ConstraintZRotation { Value = authoring.constraintZRotation });
            }
        }
    }
}