using System;
using UnityEngine;

namespace World.View
{
    [Serializable]
    public class TerrainPrefab
    {
        public TerrainType terrain;
        public GameObject[] terrainGOs;
    }
}
