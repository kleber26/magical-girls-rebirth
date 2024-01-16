using Game;
using Players;
using ScenesManager.View.Enum;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScenesManager
{
    public class ScreenManager : MonoBehaviour
    {
        [SerializeField] private GameObject homeScene;
        [SerializeField] private GameObject characterSelectionScene;
        [SerializeField] private PlayerSelector playerSelector;
        [SerializeField] private CharacterSelector characterSelector;
        [SerializeField] private HomeUi homeUi;

        private PlayerController playerController;
        private HomeController homeController;
        private CharacterSelectionController characterSelectionController;

        void Start()
        {
            playerController = FindObjectOfType<DDOL>().PlayerController;
            homeController = new HomeController(playerSelector);
            characterSelectionController = new CharacterSelectionController(characterSelector);
            homeController.SetupHomeScreen(playerController);
            characterSelectionController.SetupCharacterSelectionScreen(playerController);
            characterSelector.OnCharacterSelected += CharacterSelected;
            // ShowHomeScreen();
            homeUi.Initialize(playerController);
        }

        public void OpenCharacterSelectionScene()
        {
            homeScene.SetActive(false);
            characterSelectionScene.SetActive(true);
        }

        // public void OpenHomeScene()
        // {
        //     ShowHomeScreen();
        // }

        public void OpenGameplayScene()
        {
            SceneManager.LoadScene((int) Scenes.Gameplay);
        }

        private void CharacterSelected(object sender, CharacterSelector.OnCharacterSelectedEventArgs args)
        {
            playerController.MainPlayer().equippedCharacter = args.seletectedCharacter;
            homeController.InstantiateSelectedCharacter(playerController);
            homeUi.SetUiStatistics();
            // ShowHomeScreen();
        }

        // private void ShowHomeScreen()
        // {
        //     homeScene.SetActive(true);
        //     characterSelectionScene.SetActive(false);
        // }
    }
}
