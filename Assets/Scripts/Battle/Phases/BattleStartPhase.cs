using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battle.View;
using Characters;
using Characters.View;
using ScriptableObjects.Skills;
using Skills;
using UnityEngine;

namespace Battle.Phases
{
    public class BattleStartPhase : MonoBehaviour
    {
        private CharactersController charactersController;

        List<(Animator, SkillExecution)> skillsInAnimation;
        List<Animator> charactersGettingHit;
        public void Initialize(CharactersController charactersController)
        {
            this.charactersController = charactersController;

            skillsInAnimation = new List<(Animator, SkillExecution)>();
            charactersGettingHit = new List<Animator>();
        }

        public List<(Animator, SkillExecution)> TriggerCharacterAnimation(List<SkillExecution> skillExecutions)
        {
            skillsInAnimation.Clear();
            charactersGettingHit.Clear();
            foreach (SkillExecution skillExecution in skillExecutions)
            {
                Character castingCharacter = skillExecution.CastingCharacter;
                Animator characterAnimator = charactersController.GetCharacterAnimator(castingCharacter);

                string skillAnimations = skillExecution.Skill.characterAnimationTrigger;
                skillsInAnimation.Add((characterAnimator, skillExecution));
                StartCoroutine(CharactersPosition.SetSkillAnimation(characterAnimator, skillAnimations));

                if (skillExecution.CharactersInRange.Count == 0)
                {
                    continue;
                }

                SkillObject skill = skillExecution.Skill;
                Transform castingCharacterTransform = charactersController.GetCharacterGameObject(skillExecution.CastingCharacter).transform;
                if (skill.characterAnimationTrigger.Contains("magic_power01"))
                {
                    List<Character> charactersInRange = skillExecution.CharactersInRange.Where(target => target.Position != castingCharacter.Position).ToList();
                    if (charactersInRange.Count == 0)
                    {
                        continue;
                    }

                    // shot projectile skill
                    foreach (var targetPosition in charactersInRange)
                    {
                        Vector3 targetVector = new Vector3(targetPosition.Position.Item1, 1, targetPosition.Position.Item2);
                        Vector3 targetDirection = targetVector - castingCharacterTransform.position;
                        castingCharacterTransform.rotation = Quaternion.LookRotation(targetDirection);

                        if (characterAnimator.gameObject.activeSelf)
                        {
                            characterAnimator.gameObject.GetComponent<SpawnProjectiles>().SpawnProjectile(targetVector);
                        }
                    }
                }
                else if (skill.range == 0 && skill.areaSize == 0)
                {
                    // Use skill on ownself
                    GameObject vfx = Instantiate (skill.skillAnimation, castingCharacterTransform.position + skill.skillAnimationOffset, Quaternion.identity);
                    vfx.transform.SetParent(transform);
                    vfx.SetActive(true);
                    Destroy(vfx, 3f);
                }
                else
                {
                    // use skill on others
                    foreach (Character characterInRange in skillExecution.CharactersInRange)
                    {
                        Vector3 targetVector = new Vector3(characterInRange.Position.Item1, 1, characterInRange.Position.Item2);
                        GameObject vfx = Instantiate (skill.skillAnimation, targetVector, Quaternion.identity);
                        vfx.transform.SetParent(transform);
                        vfx.SetActive(true);
                        Destroy(vfx, 3f);
                    }
                }
            }

            return skillsInAnimation;
        }

        public bool CharactersFinishedAttacking()
        {
            foreach ((Animator, SkillExecution) animatorExecution in skillsInAnimation)
            {
                if (animatorExecution.Item1.GetCurrentAnimatorClipInfo(0).Length == 0)
                {
                    continue;
                }

                AnimatorClipInfo animatorClipInfo = animatorExecution.Item1.GetCurrentAnimatorClipInfo(0)[0];
                if (!animatorClipInfo.clip.name.Contains("idle"))
                {
                    return false;
                }

            }
            return true;
        }

        public bool CharactersFinishedGettingHit()
        {
            foreach (Animator animatorExecution in charactersGettingHit)
            {
                if (animatorExecution.GetCurrentAnimatorClipInfo(0).Length == 0)
                {
                    continue;
                }

                AnimatorClipInfo animatorClipInfo = animatorExecution.GetCurrentAnimatorClipInfo(0)[0];
                if (!animatorClipInfo.clip.name.Contains("idle") && !animatorClipInfo.clip.name.Contains("die"))
                {
                    return false;
                }

            }
            return true;
        }

        public void InflictDamageAfterSkillExecution()
        {
            foreach ((Animator, SkillExecution) animatorExecution in skillsInAnimation)
            {
                if (animatorExecution.Item1.GetCurrentAnimatorClipInfo(0).Length == 0)
                {
                    continue;
                }
                StartCoroutine(InflictDamageOnCharacter(animatorExecution.Item1, animatorExecution.Item2));
            }
        }

        public void CheckCharactersDeath(List<Character> charactersInSkillRange)
        {
            foreach (Character character in charactersInSkillRange)
            {
                Animator characterAnimator = charactersController.GetCharacterAnimator(character);
                string animationName = character.CurrentLife >= 0 ? "hit02" : "die";
                StartCoroutine(CharactersPosition.SetSkillAnimation(characterAnimator, animationName));
                charactersGettingHit.Add((characterAnimator));
            }
            charactersController.KillCharactersWithoutHealthPoints();
        }

        private IEnumerator InflictDamageOnCharacter(Animator animator, SkillExecution skillExecution)
        {
            AnimatorClipInfo animatorClipInfo = animator.GetCurrentAnimatorClipInfo(0)[0];
            while (!animatorClipInfo.clip.name.Contains("idle") || animatorClipInfo.clip.name.Contains("die"))
            {
                animatorClipInfo = animator.GetCurrentAnimatorClipInfo(0)[0];
                yield return null;
            }

            DamageCalculatorManager.CalculateBattleResult(skillExecution);
            CheckCharactersDeath(skillExecution.CharactersInRange);
        }
    }
}
