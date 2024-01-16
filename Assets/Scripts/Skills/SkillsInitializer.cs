using System.Collections.Generic;
using System.Linq;
using ScriptableObjects.Skills;
using UnityEngine;
using UnityEngine.UI;

namespace Skills
{
    public class SkillsInitializer : MonoBehaviour
    {
        [SerializeField] private GameObject skillCardPrefab;
        [SerializeField] private GameObject skillCardsLayoutGroup;
        private List<GameObject> skillCards;

        public SkillObject SelectedSkill { get; set; }

        public delegate void OnSkillSelect(SkillObject skillObject);
        public static OnSkillSelect onSkillSelect;

        public void CreateSkillCards(List<SkillObject> playerSkills)
        {
            skillCards = new List<GameObject>();
            SelectedSkill = playerSkills.First();

            foreach (SkillObject skillObject in playerSkills)
            {
                Button skillButton = InstantiateSkillButton();
                UpdateButtonText(skillButton, skillObject.name, skillObject.skillSprite);
                skillButton.onClick.AddListener(() => SelectSkill(skillObject));
            }

            HideSkillCards();
        }

        public void ShowSkillCards()
        {
            SetCardsVisibility(true);
        }

        public void HideSkillCards()
        {
            SetCardsVisibility(false);
        }

        private void SetCardsVisibility(bool visible)
        {
            foreach (GameObject skillCard in skillCards)
            {
                skillCard.SetActive(visible);
            }
        }

        private Button InstantiateSkillButton()
        {
            GameObject skillButtonGo = Instantiate(skillCardPrefab, Vector3.zero, Quaternion.identity);
            skillButtonGo.transform.SetParent(skillCardsLayoutGroup.transform, false);
            skillCards.Add(skillButtonGo);

            return skillButtonGo.GetComponentInChildren<Button>();
        }

        private void UpdateButtonText(Button button, string skillName, Sprite skillSprite)
        {
            Text buttonText = button.transform.Find("Text").GetComponent<Text>();
            buttonText.text = skillName;
            button.image.sprite = skillSprite;
        }

        private void SelectSkill(SkillObject skillObject)
        {
            SelectedSkill = skillObject;
            onSkillSelect?.Invoke(skillObject);
        }
    }
}
