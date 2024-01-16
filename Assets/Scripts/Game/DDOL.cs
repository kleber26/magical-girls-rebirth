using Players;
using ScenesManager.View.Enum;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class DDOL : MonoBehaviour
    {
        private PlayerController playerController;
        public PlayerController PlayerController => playerController;

        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
            playerController = new PlayerController(gameObject.GetComponent<PlayerCreator>());
            SceneManager.LoadScene((int) Scenes.Main);
        }
    }
}
