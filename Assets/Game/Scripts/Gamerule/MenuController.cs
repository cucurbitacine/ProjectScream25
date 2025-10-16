using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Gamerule
{
    public class MenuController : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene(1);
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}