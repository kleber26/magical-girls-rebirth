using UnityEngine;

namespace Characters
{
    public class CharactersCanvasUpdater : MonoBehaviour
    {
        private CharactersController charactersController;

        public void Initialize(CharactersController charactersController)
        {
            this.charactersController = charactersController;
        }

        void Update()
        {
            if (charactersController is null)
            {
                return;
            }

            charactersController.UpdateCharactersHp();
        }
    }
}
