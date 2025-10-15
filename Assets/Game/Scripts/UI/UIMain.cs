using Game.Scripts.Player;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class UIMain : MonoBehaviour
    {
        [SerializeField] private PlayerController player;
        [Space]
        [SerializeField] private UIPlayer playerUI;
        
        private void Initialize()
        {
            playerUI.Initialize(player);
        }

        private void Deinitialize()
        {
            playerUI.Deinitialize();
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