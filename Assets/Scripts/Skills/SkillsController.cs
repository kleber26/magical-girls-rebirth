using System.Collections.Generic;
using ScriptableObjects.Skills;

namespace Skills
{
    public class SkillsController
    {
        private SkillsInitializer skillsInitializer;
        private List<SkillObject> playerSkills;

        public SkillsController(SkillsInitializer skillsInitializer, List<SkillObject> playerSkills)
        {
            // TODO: Must be initialized with the skills that came from home screen
            this.playerSkills = playerSkills;
            this.skillsInitializer = skillsInitializer;
        }

        public void CreateSkillCards()
        {
            skillsInitializer.CreateSkillCards(playerSkills);
        }

        public void ShowSkillCards()
        {
            skillsInitializer.ShowSkillCards();
        }

        public void ClearSkillCards()
        {
            skillsInitializer.HideSkillCards();
        }

        public SkillObject SelectedSkill()
        {
            return skillsInitializer.SelectedSkill;
        }
    }
}
