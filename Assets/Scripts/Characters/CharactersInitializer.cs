using System.Collections.Generic;
using Characters.View;
using Players.View;
using ScriptableObjects.CharacterObjects;
using Randomizer;

namespace Characters
{
    public class CharactersInitializer
    {
       public List<Character> InitializeGameCharacters(List<Player> players, List<(int, int)> availablePositions)
       {
           int playerCount = players.Count;
           List<Character> initializedCharacters = new List<Character>();

           List<(int, int)> positions = ListsRandomizer.GetRandomElementsFromList(availablePositions, playerCount);
           
           for (int i = 0; i < playerCount; i++)
           {
               initializedCharacters.Add(GetPlayerCharacter(players[i], positions[i]));
           }

           return initializedCharacters;
       }

       private Character GetPlayerCharacter(Player player, (int, int) position)
       {
           CharacterObject playerCharacter = player.equippedCharacter;
           Character character = new Character(playerCharacter, player, position);

           return character;
       }
    }
}