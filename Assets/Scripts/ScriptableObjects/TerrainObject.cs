using UnityEngine;
using World.View;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ScriptableObjects", menuName = "ScriptableObjects/TerrainObject", order = 1)]
    public class TerrainObject : ScriptableObject
    {
        public TerrainPrefab[] terrainPrefabs;
    }
}