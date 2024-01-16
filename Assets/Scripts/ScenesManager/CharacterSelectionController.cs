using Players;

namespace ScenesManager
{
    public class CharacterSelectionController
    {
        private CharacterSelector characterSelector;

        public CharacterSelectionController(CharacterSelector characterSelector)
        {
            this.characterSelector = characterSelector;
        }

        public void SetupCharacterSelectionScreen(PlayerController playerController)
        {
            characterSelector.SetupCharacterSelectionScreen(playerController);
        }
    }
}
