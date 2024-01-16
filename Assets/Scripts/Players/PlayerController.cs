using System.Collections.Generic;
using System.Linq;
using Characters.View;
using Players.View;

namespace Players
{
    public class PlayerController
    {
        public List<Player> Players { get; }
        private PlayerCreator playerCreator;

        public PlayerController(PlayerCreator playerCreator)
        {
            Players = new List<Player>();
            this.playerCreator = playerCreator;
            AddGamePlayers();
        }

        public void AddGamePlayers()
        {
            Players.Clear();
            Players.AddRange(playerCreator.AddPlayers());
        }

        public List<int> PlayerIds()
        {
            List<int> ids = new List<int>();
            foreach (var player in Players)
            {
                ids.Add(player.id);
            }

            return ids;
        }

        public Player MainPlayer()
        {
            return Players.First();
        }

        public void RemovePlayer(int id)
        {
            Player player = null;
            foreach (var p in Players)
            {
                if (p.id == id)
                {
                    player = p;
                }
            }
            Players.Remove(player);
        }
    }
}
