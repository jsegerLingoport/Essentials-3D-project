using Unity.Entities;
using UnityEngine;

namespace Collectables.Authoring
{
    public class CollectableMangerAuthoring : MonoBehaviour
    {
        public class CollectableManagerBaker : Baker<CollectableMangerAuthoring>
        {
            public override void Bake(CollectableMangerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<CollectableManager>(entity);
            }
        }
    }
}