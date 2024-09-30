using Game.Player.Components;
using Unity.Entities;
using UnityEngine;

namespace Game.Player.Authoring
{
    public class Object2DAuthoring : MonoBehaviour
    {
        public class Object2DBaker : Baker<Object2DAuthoring>
        {
            public override void Bake(Object2DAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<Object2D>(entity);
            }
        }
    }
}