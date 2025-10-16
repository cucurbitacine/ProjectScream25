using System;
using Game.Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Gamerule
{
    public class GameController : MonoBehaviour
    {
        [field: SerializeField] public bool IsPlaying { get; private set; }
        [field: SerializeField] public bool Paused { get; private set; }
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

            SceneManager.LoadScene(0);
        }

        public void Pause(bool pause)
        {
            if (Paused == pause) return;
            Paused = pause;
            
            if (Paused)
            {
                Time.timeScale = 0.001f;
            }
            else
            {
                Time.timeScale = 1f;
            }
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
