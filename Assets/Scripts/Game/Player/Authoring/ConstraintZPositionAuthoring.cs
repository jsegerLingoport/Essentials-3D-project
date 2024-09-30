using Game.Player.Components;
using Unity.Entities;
using UnityEngine;

namespace Game.Player.Authoring
{
    public class ConstraintZPositionAuthoring : MonoBehaviour
    {
        [SerializeField]private float constraintZPosition;

        public class ConstraintZPositionBaker : Baker<ConstraintZPositionAuthoring>
        {
            public override void Bake(ConstraintZPositionAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ConstraintZPosition { Value = authoring.constraintZPosition });
            }
        }
    }
}