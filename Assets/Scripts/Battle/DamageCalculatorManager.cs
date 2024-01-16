using System.Collections.Generic;
using Battle.View;
using Skills;

namespace Battle
{
    public static class DamageCalculatorManager
    {
        public static void CalculateBattleResult(List<SkillExecution> skillExecutions)
        {
            foreach (SkillExecution skillExecution in skillExecutions)
            {
                DamageCalculation.CalculateSkillEffects(skillExecution.CastingCharacter, skillExecution.Skill,
                    skillExecution.CharactersInRange);
            }
        }

        public static void CalculateBattleResult(SkillExecution skillExecution)
        {
            DamageCalculation.CalculateSkillEffects(skillExecution.CastingCharacter, skillExecution.Skill, skillExecution.CharactersInRange);
        }
    }
}
