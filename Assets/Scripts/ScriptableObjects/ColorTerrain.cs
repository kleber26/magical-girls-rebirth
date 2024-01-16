using UnityEngine;
using World.View;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ScriptableObjects", menuName = "ScriptableObjects/ColorTerrain", order = 1)]
    public class ColorTerrain : ScriptableObject
    {
        public TerrainColor[] terrainColors;
    }
}