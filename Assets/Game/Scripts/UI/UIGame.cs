using Game.Scripts.Gamerule;
using Game.Scripts.Input;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class UIGame : MonoBehaviour
    {
        [SerializeField] private GameController game;
        [SerializeField] private GameFlowController gameFlow;
        
        [Space]
        [SerializeField] private UIInput uiInput;
        
        [Space]
        [SerializeField] private UIPlayer playerUI;
        [SerializeField] private GameObject pauseUI;
        [SerializeField] private GameObject survivedUI;
        [SerializeField] private GameObject diedUI;
        
        private void OnCanceled(bool cancel)
        {
            if (!cancel) return;
            
            if (game.Paused)
            {
                game.Pause(false);
                pauseUI.SetActive(false);
            }
            else
            {
                if (gameFlow.GameState == GameFlowState.Playing)
                {
                    game.Pause(true);
                    pauseUI.SetActive(true);
                }
            }
        }
        
        private void OnGameFlowChanged(GameFlowState gameFlowState)
        {
            if (gameFlowState == GameFlowState.Prepare)
            {
                playerUI.gameObject.SetActive(false);
            }
            
            if (gameFlowState == GameFlowState.Playing)
            {
                playerUI.gameObject.SetActive(true);
                playerUI.Initialize(game.Player);
            }
            
            if (gameFlowState == GameFlowState.Ended)
            {
                playerUI.gameObject.SetActive(false);
                playerUI.Deinitialize();

                if (gameFlow.GameResults.status == GameResultStatus.Survivied)
                {
                    survivedUI.gameObject.SetActive(true);
                }
                else if (gameFlow.GameResults.status == GameResultStatus.Died)
                {
                    diedUI.gameObject.SetActive(true);
                }
            }
        }
        
        private void Initialize()
        {
            uiInput.Canceled += OnCanceled;
            gameFlow.GameFlowChanged += OnGameFlowChanged;
        }

        private void Deinitialize()
        {
            uiInput.Canceled -= OnCanceled;
            gameFlow.GameFlowChanged -= OnGameFlowChanged;
        }
        
        private void Start()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            Deinitialize();
        }
    }
}