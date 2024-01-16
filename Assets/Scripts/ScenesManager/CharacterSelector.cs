using System;
using Players;
using ScriptableObjects.CharacterObjects;
using UnityEngine;
using UnityEngine.UI;

namespace ScenesManager
{
    public class CharacterSelector : MonoBehaviour
    {
        public event EventHandler<OnCharacterSelectedEventArgs> OnCharacterSelected;
        public class OnCharacterSelectedEventArgs : EventArgs
        {
            public CharacterObject seletectedCharacter;
        }

        [SerializeField] private GameObject characterButtonPrefab;
        [SerializeField] private GameObject characterButtonLayoutGroup;
        [SerializeField] private CharacterObject[] characterObjects;

        private PlayerController playerController;

        public void SelectCharacter(CharacterObject characterObject)
        {
            OnCharacterSelected?.Invoke(this, new OnCharacterSelectedEventArgs {
                seletectedCharacter = characterObject
            });
        }

        public void SetupCharacterSelectionScreen(PlayerController playerController)
        {
            this.playerController = playerController;
            SetupCharacterButtons(playerController.MainPlayer().equippedCharacter);
        }

        private void SetupCharacterButtons(CharacterObject selectedCharacter)
        {
            foreach (CharacterObject characterObject in characterObjects)
            {
                CreateButton(characterObject);
            }
        }
        private void CreateButton(CharacterObject characterObject)
        {
            GameObject buttonObject = Instantiate(characterButtonPrefab, Vector3.zero, Quaternion.identity);
            Button button = buttonObject.GetComponentInChildren<Button>();

            buttonObject.transform.SetParent(characterButtonLayoutGroup.transform, false);
            button.image.sprite = characterObject.buttonSprite;

            Text buttonText = button.transform.Find("Text").GetComponent<Text>();
            buttonText.text = characterObject.name;

            button.onClick.AddListener(() => SelectCharacter(characterObject));
        }
    }
}
