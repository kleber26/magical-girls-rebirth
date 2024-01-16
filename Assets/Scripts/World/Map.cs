using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using World.View;

namespace World
{
    public class Map
    {
        private Tile[][] tiles;
        public int Height { get; }
        public int Width { get; }

        public Tile this[int i, int j]
        {
            get => tiles[i][j];
        }

        public Map(Texture2D mapImg, Dictionary<Color32, TerrainType> terrainColors)
        {
            Height = mapImg.height;
            Width = mapImg.width;
            tiles = MapParser.Parse(mapImg, terrainColors);
        }

        public List<CoordinateChanges> ShiftLineRight(int line)
        {
            return MapOperations.ShiftLineRight(tiles, line);
        }

        public List<CoordinateChanges> ShiftLineLeft(int line)
        {
            return MapOperations.ShiftLineLeft(tiles, line);
        }

        public List<CoordinateChanges> ShiftColumnDown(int column)
        {
            return MapOperations.ShiftColumnDown(tiles, column);
        }

        public List<CoordinateChanges> ShiftColumnUp(int column)
        {
            return MapOperations.ShiftColumnUp(tiles, column);
        }

        public List<CoordinateChanges> Carousel((int, int) startPosition, int carouselSize)
        {
            return MapOperations.Carousel(tiles, startPosition, carouselSize);
        }

        public void ChangeTilesTerrain(List<(int, int)> positions, TerrainType terrain)
        {
            foreach ((int, int) position in positions)
            {
                tiles[position.Item1][position.Item2].Terrain = terrain;
            }
        }

        public List<(int, int)> GetBorderTiles()
        {
            List<(int, int)> nonVoidTiles = GetNonVoidTilePositions();

            int maxI = nonVoidTiles.Max(t => t.Item1);
            int minI = nonVoidTiles.Min(t => t.Item1);

            int maxJ = nonVoidTiles.Max(t => t.Item2);
            int minJ = nonVoidTiles.Min(t => t.Item2);

            return nonVoidTiles.Where(t => t.Item1 == maxI || t.Item1 == minI || t.Item2 == maxJ || t.Item2 == minJ).ToList();
        }

        public List<(int, int)> GetNonVoidTilePositions()
        {
            List<(int, int)> tilesPositions = new List<(int, int)>();

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (tiles[i][j].Terrain != TerrainType.Void)
                    {
                        tilesPositions.Add((i, j));
                    }
                }
            }

            return tilesPositions;
        }
    }
}
