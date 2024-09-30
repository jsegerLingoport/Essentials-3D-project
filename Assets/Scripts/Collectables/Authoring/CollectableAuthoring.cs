using Unity.Entities;
using UnityEngine;

namespace Collectables.Authoring
{
    public class CollectableAuthoring : MonoBehaviour
    {
        [SerializeField]private GameObject destroyEffect;

        public class CollectableBaker : Baker<CollectableAuthoring>
        {
            public override void Bake(CollectableAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity,
                    new Collectable { DestroyEffect = GetEntity(authoring.destroyEffect, TransformUsageFlags.Dynamic) });
            }
        }
    }
}