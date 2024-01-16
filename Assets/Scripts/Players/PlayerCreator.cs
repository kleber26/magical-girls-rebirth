using System.Collections.Generic;
using Players.View;
using ScriptableObjects.CharacterObjects;
using UnityEngine;

namespace Players
{
    // TODO: Players should only be created when game starts for the first time.
    // TODO: We must check if the player is saved in the database before creating.
    public class PlayerCreator : MonoBehaviour
    {
        [SerializeField] private int botsQuantity = 20;

        [SerializeField] private CharacterObject[] gameCharacters;

        public List<Player> AddPlayers()
        {
            List<Player> players = new List<Player>();
            for (int i = 0; i <= botsQuantity; i++)
            {
                int id = players.Count + 1;
                players.Add(CreatePlayer(id));
            }

            return players;
        }

        private Player CreatePlayer(int id)
        {
            Player player = new Player(id);
            player.equippedCharacter = gameCharacters[Random.Range(0, gameCharacters.Length)];

            return player;
        }
    }
}
