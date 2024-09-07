using Unity.Entities;
using Unity.Mathematics;

namespace JSE.Components
{
    public struct PlateManagerData : IComponentData
    {
        public float3 StartPosition;
        public quaternion StartRotation;
    }
    
}