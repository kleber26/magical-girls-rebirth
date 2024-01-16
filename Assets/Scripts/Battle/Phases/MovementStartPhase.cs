using System.Collections.Generic;
using Characters;
using Characters.View;
using Highlight;
using PathFinder;
using Players;
using Range;
using UnityEngine;
using World;

namespace Battle.Phases
{
    public class MovementStartPhase : MonoBehaviour
    {
        private MapController mapController;
        private CharactersController charactersController;
        private PlayerController playerController;

        private bool movementPhase;
        private GameObject selectedTileGo;

        private RandomoBotController randomoBotController;
        public void Initialize(MapController mapController, CharactersController charactersController, PlayerController playerController)
        {
            this.mapController = mapController;
            this.charactersController = charactersController;
            this.playerController = playerController;
            this.playerController = playerController;

            randomoBotController = new RandomoBotController(charactersController, mapController);
            movementPhase = false;
        }

        public void MoveCharacters((int, int) playerInputPosition)
        {
            int id = playerController.MainPlayer().id;
            MoveCharacterToPosition(id, playerInputPosition);
        }

        public void MoveCharacterToTile(int playerId, GameObject selectedTileGo)
        {
            if (selectedTileGo is null) return;

            Vector3 tileTransform = selectedTileGo.transform.gameObject.transform.position;
            (int, int) goPosition = ((int) tileTransform.x, (int) tileTransform.z);
            MoveCharacterToPosition(playerId, goPosition);
        }

        public void MoveCharacterToPosition(int playerId, (int, int) goPosition)
        {
            Character character = charactersController.PlayerCharacter(playerId);

            PathFinding pathFinding = new PathFinding();
            List<(int, int)> path = pathFinding.Find(character.Position, goPosition, mapController, charactersController);

            charactersController.MoveCharacter(character, path);
        }

        public bool CharactersFinishedMoving()
        {
            foreach (Character character in GetPlayersCharacters())
            {
                if (CharacterMoving(character)) { return false; }
            }

            return true;
        }

        public bool CharacterMoving(Character character)
        {
            GameObject characterGo = charactersController.GetCharacterGameObject(character);
            (int, int) gameObjectPosition = charactersController.GameObjectCurrentPosition(characterGo);

            return character.Position != gameObjectPosition;
        }

        public void MoveBots()
        {
            foreach (var botId in playerController.PlayerIds())
            {
                if (botId == playerController.MainPlayer().id) { continue; }

                if (charactersController.PlayerCharacter(botId).CurrentLife <= 0)
                {
                    continue;
                }

                MoveCharacterToPosition(botId, randomoBotController.GetBotRandomMovePosition(botId));
            }
        }

        private List<Character> GetPlayersCharacters()
        {
            List<Character> characters = new List<Character>();
            foreach (int playerId in playerController.PlayerIds())
            {
                if (charactersController.PlayerCharacter(playerId).CurrentLife <= 0)
                {
                    continue;
                }

                characters.Add(charactersController.PlayerCharacter(playerId));
            }

            return characters;
        }
    }
}
