using Unity.Entities;
using UnityEngine;

namespace Collectables.Authoring
{
    public struct Collectable : IComponentData
    {
        public Entity DestroyEffect;
    }
    
}