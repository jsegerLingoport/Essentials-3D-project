using Game.Player.Components;
using Unity.Entities;
using UnityEngine;

namespace Game.Player.Authoring
{
    public class DestroyAfterTimeAuthoring : MonoBehaviour
    {
        [SerializeField]private float timeToLive;

        public class DestroyAfterTimeBaker : Baker<DestroyAfterTimeAuthoring>
        {
            public override void Bake(DestroyAfterTimeAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new DestroyAfterTime { TimeToLive = authoring.timeToLive });
            }
        }
    }
}