using JSE.Components;
using Unity.Entities;
using UnityEngine;

namespace JSE.Authoring
{
    public class PlateManagerAuthoring : MonoBehaviour
    {
        [SerializeField] private Transform plate;
        private class PlateAuthoringBaker : Baker<PlateManagerAuthoring>
        {
            public override void Bake(PlateManagerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                var plate = authoring.plate;
                AddComponent(entity,new PlateManagerData
                {
                    StartPosition = plate.position,
                    StartRotation = plate.rotation
                });
            }
        }
    }
}