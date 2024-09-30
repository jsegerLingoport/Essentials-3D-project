using Attributes;
using UnityEngine;
using Utilities.Events;

namespace Game
{

    public enum GameState
    {
        CountDown,
        Start,
        GameOver,
        Restart,
    }

    public struct GameStateChangeEvent
    {
        public GameState NewState;
        public GameState Previous;
    }
    
    public class GameManager : MonoBehaviour,IEventAggregatorHandel
    {
        public EventAggregator EventAggregator => eventAggregator;
        
        
        public void TriggerGameStart()
        {
           
        }
        public void TriggerGameOver()
        {
            
        }
        public void TriggerRestartGame()
        {
            
        }

        private void TriggerGameState()
        {
            if (this is IEventAggregatorHandel eventAggregatorHandel)
            {
                eventAggregatorHandel.Publish(new GameStateChangeEvent
                {
                    NewState = GameState.Start
                });
            }
        }
        [SerializeField,FindAssetOfType(typeof(GameEventAggregator))]
        private GameEventAggregator eventAggregator;
       
    }
}
