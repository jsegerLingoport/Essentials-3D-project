using System;
using Game.Player.Components;
using Unity.Entities;
using UnityEngine;

namespace Game.Player.Authoring
{

    public enum PlayerType
    {
        Player3D,
        Player2DTopDown
    }
    
    public class PlayerTagAuthoring : MonoBehaviour
    {
        [SerializeField] private PlayerType playerType;
        public class PlayerTagBaker : Baker<PlayerTagAuthoring>
        {
            public override void Bake(PlayerTagAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<PlayerTag>(entity);

                switch (authoring.playerType)
                {
                    case PlayerType.Player3D:
                        AddComponent<Player3DTag>(entity);
                        break;
                    case PlayerType.Player2DTopDown:
                        AddComponent<TopDownPlayer2DTag>(entity);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                AddComponent<PlayerInputData>(entity);
            }
        }
    }
}