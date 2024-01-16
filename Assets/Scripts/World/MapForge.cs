using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using World.View;

namespace World
{

    public class MapForge : MonoBehaviour
    {
        private GameObject[][] tiles;
        private Dictionary<TerrainType, GameObject[]> terrainGODict;
        private System.Random random;

        public void InstantiateMap(Map map, Dictionary<TerrainType, GameObject[]> terrainGOs)
        {
            terrainGODict = terrainGOs;
            random = new System.Random();

            tiles = new GameObject[map.Height][];
            for (int i = 0; i < map.Height; i++)
            {
                tiles[i] = new GameObject[map.Width];
            }

            for (int i = 0; i < map.Height; i++)
            {
                for (int j = 0; j < map.Width; j++)
                {
                    Tile tile = map[i, j];
                    if (tile.Terrain != TerrainType.Void)
                    {
                        int randomNumber = random.Next(0, terrainGOs[tile.Terrain].Length);
                        GameObject terrainGO = terrainGOs[tile.Terrain][randomNumber];

                        int randomDegreee = random.Next(4);
                        Quaternion rotation = Quaternion.Euler(new Vector3(0, 90 * randomDegreee, 0));
                        tiles[i][j] = Instantiate(terrainGO, new Vector3(i, 0, j), rotation);
                        tiles[i][j].name = $"{i},{j}";
                    }
                }
            }
        }

        public void UpdateMap(List<CoordinateChanges> changes)
        {
            List<(int, int)> oldPositions = changes.Select(c => c.OldPosition).ToList();
            List<(int, int)> newPositions = changes.Select(c => c.NewPosition).ToList();
            List<GameObject> gameObjectsChanged = oldPositions.Select(op => tiles[op.Item1][op.Item2]).ToList();

            for (int i = 0; i < newPositions.Count; i++)
            {
                int destinationX = newPositions[i].Item1;
                int destinationZ = newPositions[i].Item2;
                tiles[destinationX][destinationZ] = gameObjectsChanged[i];
            }

            StartCoroutine(MoveTiles(gameObjectsChanged, newPositions));
        }

        // TODO: Create the animation for changing Tile Terrain
        public void UpdateTilesTerrain(List<(int, int)> positions, TerrainType terrain)
        {
            foreach ((int, int) position in positions)
            {
                int i = position.Item1;
                int j = position.Item2;

                Destroy(tiles[i][j]);

                int randomNumber = random.Next(terrainGODict[terrain].Length - 1);
                GameObject terrainGO = terrainGODict[terrain][randomNumber];
                tiles[i][j] = Instantiate(terrainGO, new Vector3(i, 0, j), Quaternion.identity);
                tiles[i][j].name = $"{i},{j}";
            }
        }

        public void DropTile(List<(int, int)> positions)
        {
            foreach ((int, int) position in positions)
            {
                int i = position.Item1;
                int j = position.Item2;
                tiles[i][j].AddComponent<Rigidbody>();
                StartCoroutine(DropTilesCorrout(tiles[i][j]));
            }
        }

        private IEnumerator DropTilesCorrout(GameObject go)
        {
            yield return new WaitForSeconds(2f);

            Destroy(go, 2f);
        }

        public List<GameObject> TilesGameObjects(List<(int, int)> tilesList)
        {
            List<GameObject> gameObjects = new List<GameObject>();
            foreach ((int, int) position in tilesList)
            {
                gameObjects.Add(tiles[position.Item1][position.Item2]);
            }

            return gameObjects;
        }

        private IEnumerator MoveTiles(List<GameObject> gos, List<(int, int)> targetPositions)
        {
            float speed = 20.0f;
            float step =  speed * Time.deltaTime;
            float height = 1f;

            for (int i = 0; i < gos.Count; i++)
            {
                GameObject go = gos[i];

                if (go == null)
                {
                    continue;
                }

                Vector3 targetLocation = new Vector3(go.transform.position.x, height, go.transform.position.z);

                while (Vector3.Distance(go.transform.position, targetLocation)> 0.001f)
                {
                    if (go == null) { break; }
                    go.transform.position = Vector3.MoveTowards(go.transform.position, targetLocation, step);
                    yield return new WaitForEndOfFrame();
                    if (go == null) { break; }
                }
            }

            for (int i = 0; i < gos.Count; i++)
            {
                GameObject go = gos[i];

                if (go == null)
                {
                    continue;
                }

                (int, int) position = targetPositions[i];

                Vector3 targetLocation = new Vector3(position.Item1, height, position.Item2);

                while (Vector3.Distance(go.transform.position, targetLocation)> 0.001f)
                {
                    go.transform.position = Vector3.MoveTowards(go.transform.position, targetLocation, step);
                    yield return new WaitForEndOfFrame();
                }
            }

            for (int i = 0; i < gos.Count; i++)
            {
                GameObject go = gos[i];

                if (go == null)
                {
                    continue;
                }

                (int, int) position = targetPositions[i];
                Vector3 targetLocation = new Vector3(position.Item1, 0, position.Item2);

                while (Vector3.Distance(go.transform.position, targetLocation)> 0.001f)
                {
                    go.transform.position = Vector3.MoveTowards(go.transform.position, targetLocation, step);
                    yield return new WaitForEndOfFrame();
                }

                go.transform.position = targetLocation;
            }
        }
    }
}
