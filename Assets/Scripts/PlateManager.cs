using JSE.Components;
using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;

namespace JSE
{
    public class PlateManager : MonoBehaviour
    {
        
        [Button]
        public void StartRoll()
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var newEntity = entityManager.CreateEntity(typeof(RestartPlateRequest));
            entityManager.SetName(newEntity,"RestartPlateRequest");
        }
    }
}
