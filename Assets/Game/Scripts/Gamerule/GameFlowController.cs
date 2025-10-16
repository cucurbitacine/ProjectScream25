using System;
using System.Collections;
using Game.Scripts.Player;
using UnityEngine;

namespace Game.Scripts.Gamerule
{
    public class GameFlowController : MonoBehaviour
    {
        [field: SerializeField] public GameFlowState GameState { get; private set; }
        [SerializeField] private GameResult gameResult;
        
        [Space]
        [SerializeField] private GameController game;
        [SerializeField] private TriggerZone safeZone;
        
        private PlayerController Player => game.Player;
        
        public GameResult GameResults => gameResult;
        public event Action<GameFlowState> GameFlowChanged;
        
        private void NextState(GameFlowState nextState)
        {
            var prevState = GameState;
            GameState = nextState;
            
            GameFlowChanged?.Invoke(GameState);
            
            switch (GameState)
            {
                case GameFlowState.Unknown: return;
                case GameFlowState.Prepare:
                    EnterPrepare(prevState);
                    break;
                case GameFlowState.Playing:
                    EnterPlaying(prevState);
                    break;
                case GameFlowState.Ended:
                    EnterEnded(prevState);
                    break;
                case GameFlowState.Quiting:
                    EnterQuiting(prevState);
                    break;
                default:
                    return;
            }
        }

        private void EnterPrepare(GameFlowState from)
        {
            NextState(GameFlowState.Playing);
        }

        private void EnterPlaying(GameFlowState from)
        {
            safeZone.Triggered += OnSafeZoneTriggered;
            Player.Damageable.Health.Died += OnPlayerDied;
        }

        private void EnterEnded(GameFlowState from)
        {
            if (from == GameFlowState.Ended) return;
            
            StartCoroutine(DelayBeforeQuit(2f));
            
            return;

            IEnumerator DelayBeforeQuit(float time)
            {
                yield return new WaitForSeconds(time);
                
                NextState(GameFlowState.Quiting);
            }
        }

        private void EnterQuiting(GameFlowState from)
        {
            game.StopGame(gameResult);
        }
        
        private void OnGameStarted(bool isPlaying)
        {
            if (isPlaying && GameState == GameFlowState.Unknown)
            {
                NextState(GameFlowState.Prepare);
            }
        }
        
        private void OnSafeZoneTriggered(Collider2D cld)
        {
            if (cld.attachedRigidbody && cld.attachedRigidbody.TryGetComponent(out PlayerController triggeredPlayer))
            {
                if (triggeredPlayer == Player)
                {
                    GameWin();
                }
            }
        }

        private void OnPlayerDied(bool dead)
        {
            GameLose();
        }
        
        private void GameWin()
        {
            safeZone.Triggered -= OnSafeZoneTriggered;
            Player.Damageable.Health.Died -= OnPlayerDied;
            
            gameResult.status = GameResultStatus.Survivied;
            
            NextState(GameFlowState.Ended);
        }

        private void GameLose()
        {
            safeZone.Triggered -= OnSafeZoneTriggered;
            Player.Damageable.Health.Died -= OnPlayerDied;
            
            gameResult.status = GameResultStatus.Died;
            
            NextState(GameFlowState.Ended);
        }
        
        private void Start()
        {
            if (game.IsPlaying)
            {
                NextState(GameFlowState.Prepare);
            }
            
            game.GameStarted += OnGameStarted;
        }

        private void OnDestroy()
        {
            game.GameStarted -= OnGameStarted;
        }
    }

    public enum GameFlowState
    {
        Unknown,
        Prepare,
        Playing,
        Ended,
        Quiting,
    }
}