using System.Collections.Generic;
using Characters.View;
using ScriptableObjects.Skills;

namespace Battle.View
{
    public class SkillExecution
    {
        public Character CastingCharacter { get; }
        public SkillObject Skill { get; }
        public List<Character> CharactersInRange { get; }

        public SkillExecution(Character castingCharacter, SkillObject skill, List<Character> charactersInRange)
        {
            CastingCharacter = castingCharacter;
            Skill = skill;
            CharactersInRange = charactersInRange;
        }
    }
}
