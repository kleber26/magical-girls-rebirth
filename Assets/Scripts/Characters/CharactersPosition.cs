using System;
using System.Collections;
using System.Collections.Generic;
using Characters.View;
using UnityEngine;
using UnityEngine.UI;

namespace Characters
{
    public class CharactersPosition : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 4f;

        private GameObject[][] characters;
        static readonly int Run = Animator.StringToHash("run");

        public void InitializeCharacterMap(int height, int width)
        {
            characters = new GameObject[height][];
            for (int i = 0; i < height; i++)
            {
                characters[i] = new GameObject[width];
            }
        }

        public void MoveCharacter((int, int) characterPosition, List<(int, int)> path, Animator animator)
        {
            Transform characterTransform = characters[characterPosition.Item1][characterPosition.Item2].transform;
            StartCoroutine(MoveCharacterCoroutine(characterTransform, path, animator));
        }

        public void DeployCharacters(List<Character> charactersList)
        {
            foreach (Character character in charactersList)
            {
                AddCharacterOnMap(character);
            }
        }

        public void UpdateCharacterPosition((int, int) startPos, (int, int) targetPos)
        {
            var characterGo = characters[startPos.Item1][startPos.Item2];
            characters[startPos.Item1][startPos.Item2] = null;
            characters[targetPos.Item1][targetPos.Item2] = characterGo;
        }

        public GameObject GetCharacterGameObject((int, int) characterPosition)
        {
            return characters[characterPosition.Item1][characterPosition.Item2];
        }

        public void UpdateCharacterHp(GameObject canvasCharGo, string hp)
        {
            Text playerHealth = canvasCharGo.GetComponent<Text>();
            if (playerHealth.text != hp)
            {
                playerHealth.text = hp;
            }
        }

        public GameObject GetCharacterGameObjectWithTag(Transform parent, string tag)
        {
            foreach (Transform child in parent.transform.GetComponentsInChildren<Transform>())
            {
                if (child.CompareTag(tag))
                {
                    return child.gameObject;
                }
            }

            throw new NullReferenceException($"Unable to get tag #{tag} from given GameObject");
        }

        // tá feio mas n to nem ai
        public void RemovePlayerFlag(GameObject characterGo){
            foreach (Transform child in characterGo.transform)
            {
                if (child.name.Contains("CharacterCanvas"))
                {
                    foreach (Transform childs_child in child)
                    {
                        if (childs_child.CompareTag("CanvasPlayerFlag"))
                        {
                            childs_child.gameObject.SetActive(false); //hadouken krl
                        }
                    }
                }
            }
        }

        public static IEnumerator SetSkillAnimation(Animator characterAnimator, string skillAnimation)
        {
            characterAnimator.SetBool(skillAnimation, true);
            yield return new WaitForSeconds(1f);
            characterAnimator.SetBool(skillAnimation, false);
        }

        private IEnumerator MoveCharacterCoroutine(Transform characterTransform, List<(int, int)> path, Animator animator)
        {
            foreach ((int, int) position in path)
            {
                animator.SetBool(Run, true);
                Vector3 targetPosition = new Vector3(position.Item1, 0.55f, position.Item2);
                LookAtDirection(characterTransform, targetPosition);

                while (characterTransform.position != targetPosition)
                {
                    characterTransform.position = Vector3.MoveTowards(characterTransform.position, targetPosition, moveSpeed * Time.deltaTime);
                    yield return null;
                }
            }
            animator.SetBool("run", false);
        }

        private void LookAtDirection(Transform characterTransform, Vector3 targetPosition)
        {
            Vector3 targetDirection = targetPosition - characterTransform.position;
            targetDirection.y = 0;
            characterTransform.rotation = Quaternion.LookRotation(targetDirection);
        }

        private void AddCharacterOnMap(Character character)
        {
            GameObject characterPrefab = character.Prefab;

            int xPosition = character.Position.Item1;
            int zPosition = character.Position.Item2;

            Vector3 positionVector = new Vector3(xPosition, 0.55f, zPosition);
            GameObject characterGO = Instantiate(characterPrefab, positionVector, characterPrefab.transform.rotation);

            characters[xPosition][zPosition] = characterGO;
        }

        public void KillCharacter(GameObject characterGo)
        {
            StartCoroutine(KillCharacterCorroutine(characterGo));
        }

        public void MudaAiOTileDoMalucoNaForcaBrutaMesmo(GameObject charGo, (int, int) newPos)
        {
            var transformPosition = charGo.transform.position;
            transformPosition.x = newPos.Item1;
            transformPosition.z = newPos.Item2;
        }

        private IEnumerator KillCharacterCorroutine(GameObject characterGo)
        {
            yield return new WaitForSeconds(4f);
            if (characterGo != null && characterGo.activeSelf)
            {
               characterGo.SetActive(false);
            }
        }
    }
}
