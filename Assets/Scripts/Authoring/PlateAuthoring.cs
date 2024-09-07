using JSE.Components;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

namespace JSE.Authoring
{
    public class PlateAuthoring : MonoBehaviour
    {
        private class PlateAuthoringBaker : Baker<PlateAuthoring>
        {
            public override void Bake(PlateAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<PlateTag>(entity);
            }
        }
    }
}