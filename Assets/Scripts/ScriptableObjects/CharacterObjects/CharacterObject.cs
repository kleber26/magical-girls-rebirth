using System.Collections.Generic;
using ScriptableObjects.Skills;
using UnityEngine;
using World.View;

namespace ScriptableObjects.CharacterObjects
{
    [CreateAssetMenu(fileName = "ScriptableObjects", menuName = "ScriptableObjects/CharacterObject", order = 1)]
    public class CharacterObject : ScriptableObject
    {
        public string name;
        public int life;
        public int stamina;
        public int atk;
        public int def;
        public int atkBonus;
        public int defBonus;
        public Elements elements;
        public GameObject prefab;
        public Sprite buttonSprite;
        public List<SkillObject> skills;
    }
}
