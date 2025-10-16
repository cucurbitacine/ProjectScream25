using System;
using Game.Scripts.Player;
using UnityEngine;

namespace Game.Scripts.Gamerule
{
    public class GameController : MonoBehaviour
    {
        [field: SerializeField] public bool IsPlaying { get; private set; }
        [field: SerializeField] public PlayerController Player { get; private set; }

        public event Action<bool> GameStarted;
        
        public void StartGame()
        {
            IsPlaying = true;
            
            GameStarted?.Invoke(true);
        }

        public void StopGame(GameResult result)
        {
            IsPlaying = false;
            
            GameStarted?.Invoke(false);
        }
        
        private void Start()
        {
            StartGame();
        }
    }

    [Serializable]
    public struct GameResult
    {
        public GameResultStatus status;
    }

    public enum GameResultStatus
    {
        Unknown,
        Survivied,
        Died,
    }
}
