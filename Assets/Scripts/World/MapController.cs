using System.Collections.Generic;
using Randomizer;
using UnityEngine;
using World.View;

namespace World
{
    public class MapController
    {
        private Map map;
        private MapForge mapForge;

        public MapController(Texture2D mapImg, Dictionary<Color32, TerrainType> terrainColors,
            Dictionary<TerrainType, GameObject[]> terrainGOs, MapForge mapForge)
        {
            this.mapForge = mapForge;

            map = new Map(mapImg, terrainColors);
            mapForge.InstantiateMap(map, terrainGOs);
        }

        public void ShiftLineRight(int line)
        {
            List<CoordinateChanges> changes = map.ShiftLineRight(line);
            mapForge.UpdateMap(changes);
        }

        public void ShiftLineLeft(int line)
        {
            List<CoordinateChanges> changes = map.ShiftLineLeft(line);
            mapForge.UpdateMap(changes);
        }

        public void ShiftColumnDown(int column)
        {
            List<CoordinateChanges> changes = map.ShiftColumnDown(column);
            mapForge.UpdateMap(changes);
        }

        public void ShiftColumnUp(int column)
        {
            List<CoordinateChanges> changes = map.ShiftColumnUp(column);
            mapForge.UpdateMap(changes);
        }

        public void Carousel((int, int) startPosition, int carouselSize)
        {
            List<CoordinateChanges> changes = map.Carousel(startPosition, carouselSize);
            mapForge.UpdateMap(changes);
        }

        public void LavaFloorEvent(int numberOfChanges)
        {
            ChangeTilesTerrain(numberOfChanges, TerrainType.Lava);
        }

        public void FloodGateEvent(int numberOfChanges)
        {
            ChangeTilesTerrain(numberOfChanges, TerrainType.Water);
        }

        public TerrainType GetTileTerrainType((int, int) position)
        {
            return map[position.Item1, position.Item2].Terrain;
        }

        public int MapWidth()
        {
            return map.Width;
        }

        public int MapHeight()
        {
            return map.Height;
        }

        public bool IsTileVoid((int, int) position)
        {
            return map[position.Item1, position.Item2].Terrain == TerrainType.Void;
        }

        public List<(int, int)> GetNonVoidTilePositions()
        {
            return map.GetNonVoidTilePositions();
        }

        public List<(int, int)> GetBorderTiles()
        {
            return map.GetBorderTiles();
        }

        public void DropTiles(List<(int, int)> tiles)
        {
            map.ChangeTilesTerrain(tiles, TerrainType.Void);
            mapForge.DropTile(tiles);
        }

        public List<GameObject> GetTilesGameObjects(List<(int, int)> tilesPositions)
        {
            return mapForge.TilesGameObjects(tilesPositions);
        }

        public Vector3 MiddleMapPosition()
        {
            int width = MapWidth() / 2;
            int height = MapHeight() / 2;
            return new Vector3(width, 0, 1);
        }

        private void ChangeTilesTerrain(int numberOfChanges, TerrainType terrain)
        {
            List<(int, int)> availableTiles = GetNonVoidTilePositions();
            List<(int, int)> selectedTiles = ListsRandomizer.GetRandomElementsFromList(availableTiles, numberOfChanges);

            map.ChangeTilesTerrain(selectedTiles, terrain);
            mapForge.UpdateTilesTerrain(selectedTiles, terrain);
        }
    }
}
