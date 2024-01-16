using UnityEngine;
using World.View;

namespace ScriptableObjects.MapEventObjects
{
    [CreateAssetMenu(fileName = "ScriptableObjects", menuName = "ScriptableObjects/MapEventObjects", order = 1)]
    public class MapEventObject : ScriptableObject
    {
        public MapEvent mapEvent;
        public Sprite mapEventSprite;
    }
}
