using System.Collections.Generic;
using UnityEngine;
using World.View;

namespace World
{
    public static class MapParser
    {
        public static Tile[][] Parse(Texture2D mapImg, Dictionary<Color32, TerrainType> mapTerrains)
        {
            int height = mapImg.height;
            int width = mapImg.width;
            Color32[] pixels = mapImg.GetPixels32();
            
            Tile[][] map = new Tile[height][];
            for (int i = 0; i < height; i++)
            {
                map[i] = new Tile[width];
            }

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Color32 pixel = pixels[i * width + j];
                    TerrainType terrain = mapTerrains[pixel];
                    map[i][j] = new Tile(terrain);
                }
            }
            
            return map;
        }
    }
}