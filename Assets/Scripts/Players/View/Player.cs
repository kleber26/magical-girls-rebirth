using ScriptableObjects.CharacterObjects;

namespace Players.View
{
    public class Player
    {
        public int id;
        public CharacterObject equippedCharacter;

        public Player(int id)
        {
            this.id = id;
        }
    }
}