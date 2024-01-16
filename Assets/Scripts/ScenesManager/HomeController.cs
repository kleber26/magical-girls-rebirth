using Players;

namespace ScenesManager
{
    public class HomeController
    {
        private PlayerSelector playerSelector;

        public HomeController(PlayerSelector playerSelector)
        {
            this.playerSelector = playerSelector;
        }

        public void SetupHomeScreen(PlayerController playerController)
        {
            playerSelector.SetupHomeScreen(playerController);
        }

        public void InstantiateSelectedCharacter(PlayerController playerController)
        {
            playerSelector.InstantiateSelectedCharacter(playerController);
        }
    }
}
