using System.Linq;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using Utilities.Events;

namespace Collectables.Systems
{
    public struct CollectablePickUpEvent
    {
    }

    public partial class CollectedCollectableEventSystem : SystemBase
    {
        private GameEventAggregator _gameEventAggregator;

        protected override void OnCreate()
        {
            base.OnCreate();
            _gameEventAggregator = Resources.FindObjectsOfTypeAll<GameEventAggregator>().FirstOrDefault();
        }


        protected override void OnUpdate()
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            if (_gameEventAggregator == null) return;

            foreach (var (collectableEvent, entity) in SystemAPI.Query<RefRO<CollectedCollectableEvent>>()
                         .WithEntityAccess())
            {
                _gameEventAggregator.Publish(new CollectablePickUpEvent());
                ecb.DestroyEntity(entity);
            }

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
    }
}