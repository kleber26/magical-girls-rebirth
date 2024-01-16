namespace World.View
{
    public class Tile
    {
        public TerrainType Terrain { get; set; }

        public Tile(TerrainType terrain)
        {
            Terrain = terrain;
        }
    }
}