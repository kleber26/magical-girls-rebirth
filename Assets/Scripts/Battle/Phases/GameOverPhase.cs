using Characters;
using Characters.View;
using Players;
using ScenesManager.View.Enum;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Battle.Phases
{
    public class GameOverPhase : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverPhaseGameObject;
        [SerializeField] private Button gameOverButton;
        [SerializeField] private Sprite winnerSprite;
        [SerializeField] private Sprite loserSprite;

        private CharactersController charactersController;
        private PlayerController playerController;
        private Character gameWinner;

        public void Initialize(CharactersController charactersController, PlayerController playerController)
        {
            this.charactersController = charactersController;
            this.playerController = playerController;
        }

        public void ReturnHome()
        {
            SceneManager.LoadScene((int) Scenes.Main);
        }

        public void ShowGameOverScreen()
        {
            gameOverPhaseGameObject.SetActive(true);
        }

        public void HideGameOverScreen()
        {
            gameOverPhaseGameObject.SetActive(false);
        }

        public bool IsGameOver()
        {
            return IsPlayerDead() || AreAllPlayersDead();
        }

        public void SetupGameOverPopup()
        {
            gameOverButton.image.sprite = DidPlayerWin() ? winnerSprite : loserSprite;
        }

        // TODO: debug method, remove later
        public void KillPlayer()
        {
            charactersController.PlayerCharacter(playerController.MainPlayer().id).CurrentLife = 0;
        }

        public void KillAllPlayers()
        {
            foreach (var player in playerController.Players)
            {
                charactersController.PlayerCharacter(player.id).CurrentLife = 0;
            }
            charactersController.PlayerCharacter(playerController.MainPlayer().id).CurrentLife = 1;
        }

        private bool DidPlayerWin()
        {
            return !IsPlayerDead();
        }

        private bool IsPlayerDead()
        {
            return charactersController.PlayerCharacter(playerController.MainPlayer().id).CurrentLife <= 0;
        }

        private bool AreAllPlayersDead()
        {
            int aliveCharacters = charactersController.GetAliveCharacters().Count;
            return aliveCharacters == 0 || aliveCharacters == 1;
        }
    }
}
