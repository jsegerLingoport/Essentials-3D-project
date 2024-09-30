using Game.Player.Components;
using Unity.Entities;
using UnityEngine;

namespace Game.Player.Authoring
{
    public class RandomRotationAuthoring : MonoBehaviour
    {
        [SerializeField] private bool rotate2D;
        public class RandomRotationBaker : Baker<RandomRotationAuthoring>
        {
            
            public override void Bake(RandomRotationAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                if (authoring.rotate2D)
                {
                    AddComponent<Random2DRotation>(entity);
                }
                else
                {
                    AddComponent<Random3DRotation>(entity);
                }
            }
        }
    }
}