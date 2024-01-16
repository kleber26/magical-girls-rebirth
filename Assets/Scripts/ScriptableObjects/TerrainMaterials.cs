using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ScriptableObjects", menuName = "ScriptableObjects/TerrainMaterials", order = 1)]
    public class TerrainMaterials : ScriptableObject
    {
        public Material earthMaterial;
        public Material grassMaterial;
        public Material voidMaterial;
        public Material waterMaterial;
        public Material lavaMaterial;
    }
}
