using System;
using System.Collections.Generic;
using System.Linq;
using Battle.Phases;
using Battle.View;
using Characters.View;
using Players;
using Players.View;
using ScriptableObjects.Skills;
using UnityEngine;
using World;

namespace Characters
{
    public class CharactersController
    {
        private CharactersPosition charactersPosition;
        private List<Character> Characters { get; }
        private PlayerController playerController;

        public CharactersController(CharactersInitializer charactersInitializer, MapController mapController,
                                    CharactersPosition charactersPosition, List<Player> players, PlayerController playerController)
        {
            this.charactersPosition = charactersPosition;
            this.playerController = playerController;

            Characters = charactersInitializer.InitializeGameCharacters(players, mapController.GetNonVoidTilePositions());
            charactersPosition.InitializeCharacterMap(mapController.MapHeight(), mapController.MapWidth());
            charactersPosition.DeployCharacters(Characters);
        }

        public List<SkillObject> GetCharacterSkills(Character character)
        {
            return character.Skills;
        }

        public Character PlayerCharacter(int playerId)
        {
            foreach (Character character in Characters)
            {
                if (character.Player.id == playerId)
                {
                    return character;
                }
            }

            return null;
        }

        public List<(int, int)> OtherCharactersPositions(int playerId)
        {
            List<(int, int)> positions = new List<(int, int)>();
            foreach (var character in Characters)
            {
                if (character.Player.id == playerId) { continue; }

                positions.Add(character.Position);
            }

            return positions;
        }

        public List<Character> GetCharactersInRange(List<(int, int)> range)
        {
            return Characters.Where(c => range.Contains(c.Position)).ToList();
        }

        public Character GetCharacterWithHighestHealth(List<(int, int)> positions)
        {
            return Characters.Where(character => positions.Contains(character.Position))
                .OrderBy(c => c.CurrentLife).LastOrDefault();
        }

        public Character CharacterWithPosition((int, int) position)
        {
            return Characters.Single(character => character.Position == position);
        }

        public void MoveCharacter(Character character, List<(int, int)> path)
        {
            if (path == null || path.Count == 0) { return; }

            (int, int) targetPosition = path.Last();
            charactersPosition.UpdateCharacterPosition(character.Position, targetPosition);
            character.Position = targetPosition;


            charactersPosition.MoveCharacter(targetPosition, path, GetCharacterAnimator(character));
        }

        public bool IsCharacterOccupyingPosition((int, int) targetPosition)
        {
            foreach (var character in Characters)
            {
                GameObject characterGameObject = GetCharacterGameObject(character);

                if (characterGameObject is null)
                {
                    continue;
                }

                if (character.Position == targetPosition && characterGameObject.activeSelf)
                {
                    return true;
                }
            }

            return false;
        }

        public GameObject GetCharacterGameObject(Character character)
        {
            return charactersPosition.GetCharacterGameObject(character.Position);
        }

        // TODO: Pode ser usado para qualquer game object. Deveria estar em alguma outra classe.
        public (int, int) GameObjectCurrentPosition(GameObject characterGo)
        {
            Vector3 charTransform = characterGo.transform.position;
            return ((int) charTransform.x, (int)charTransform.z);
        }

        public void CheckCharactersOnVoidTiles(MapController mapController, BattleStartPhase battleStartPhase)
        {
            foreach (var character in Characters)
            {
                if (mapController.IsTileVoid(character.Position))
                {
                    GameObject charGo = GetCharacterGameObject(character);
                    character.CurrentLife = 0;
                    charGo.AddComponent<Rigidbody>();
                    battleStartPhase.CheckCharactersDeath(new List<Character> { character });
                }
            }
        }

        public void UpdateCharactersHp()
        {
            foreach (Character character in Characters)
            {
                GameObject charGo = GetCharacterGameObject(character);
                if (charGo is null)
                {
                    continue;
                }
                GameObject canvasCharGo = charactersPosition.GetCharacterGameObjectWithTag(charGo.transform, "CanvasCharHP");
                charactersPosition.UpdateCharacterHp(canvasCharGo, character.CurrentLife.ToString());
            }
        }

        public void RemovelayerFlag()
        {
            foreach (var character in Characters)
            {
                if (character.Player.id == 1)
                {
                    continue;
                }
                charactersPosition.RemovePlayerFlag(GetCharacterGameObject(character));
            }
        }

        public void KillCharactersWithoutHealthPoints()
        {
            foreach (Character character in Characters)
            {
                if (character.CurrentLife <= 0)
                {
                    charactersPosition.KillCharacter(GetCharacterGameObject(character));
                }
            }
        }

        public void RemoveGamePlayers()
        {
            List<Character> killedCharacters = new List<Character>();
            foreach (Character character in Characters)
            {
                if (character.CurrentLife <= 0)
                {
                    killedCharacters.Add(character);
                }
            }

            foreach (var character in killedCharacters)
            {
                playerController.RemovePlayer(character.Player.id);
                Characters.Remove(character);
            }
        }

        public List<Character> GetAliveCharacters()
        {
            List<Character> aliveCharacters = new List<Character>();

            foreach (Character character in Characters)
            {
                if (character.CurrentLife > 0)
                {
                    aliveCharacters.Add(character);
                }
            }

            return aliveCharacters;
        }

        public Animator GetCharacterAnimator(Character character)
        {
            GameObject characterGo = GetCharacterGameObject(character);
            return characterGo.GetComponent<Animator>();
        }
    }
}
