using System.Collections.Generic;
using Characters.View;
using ScriptableObjects.Skills;

namespace Skills
{
    public static class DamageCalculation
    {
        private static int atkBuffThreshold = 30;
        private static int defBuffThreshold = 15;

        public static void CalculateSkillEffects(Character castingCharacter, SkillObject skill, List<Character> charactersInRange)
        {
            charactersInRange.Remove(castingCharacter);
            foreach(SkillAttributes skillAttribute in skill.skillAttributes)
            {
                CalculateSkillEffect(skillAttribute, castingCharacter, charactersInRange);
            }
        }

        private static void CalculateSkillEffect(SkillAttributes skillAttribute, Character castingCharacter,
            List<Character> charactersInRange)
        {
            switch(skillAttribute.skillEffect)
            {
                case SkillEffect.Damage:
                {
                    int atk = skillAttribute.effectValue + castingCharacter.Atk;
                    int totalAtk = (int)(atk + (float) atk / 100 * castingCharacter.AtkBonus);
                    foreach (Character affectedCharacter in charactersInRange)
                    {
                        InflictDamage(affectedCharacter, totalAtk);
                    }
                    break;
                }

                case SkillEffect.PiercingDamage:
                {
                    int atk = skillAttribute.effectValue + castingCharacter.Atk;
                    int totalAtk = (int) (atk + (float) atk / 100 * castingCharacter.AtkBonus);
                    foreach (Character affectedCharacter in charactersInRange)
                    {
                        InflictPiercingDamage(affectedCharacter, totalAtk);
                    }
                    break;
                }

                case SkillEffect.Heal:
                {
                    Heal(castingCharacter, skillAttribute.effectValue);
                    break;
                }

                case SkillEffect.AtkBuff:
                {
                    if (castingCharacter.AtkBonus + skillAttribute.effectValue < atkBuffThreshold)
                    {
                        castingCharacter.AtkBonus += skillAttribute.effectValue;
                    }
                    break;
                }

                case SkillEffect.DefBuff:
                {
                    if (castingCharacter.DefBonus + skillAttribute.effectValue < defBuffThreshold)
                    {
                        castingCharacter.DefBonus += skillAttribute.effectValue;
                    }

                    break;
                }
            }
        }

        private static void InflictDamage(Character character, int damage)
        {
            int characterDef = character.Def + character.DefBonus;
            int totalDamage = (int) (damage - (float) damage / 100 * characterDef);
            character.CurrentLife -= totalDamage;
        }

        private static void InflictPiercingDamage(Character character, int damage)
        {
            character.CurrentLife -= damage;
        }

        private static void Heal(Character character, int healAmount)
        {
            if (character.CurrentLife <= 0)
            {
                return;
            }

            if (character.CurrentLife + healAmount > character.BaseLife)
            {
                character.CurrentLife = character.BaseLife;
            }
            else
            {
                character.CurrentLife += healAmount;
            }
        }
    }
}
