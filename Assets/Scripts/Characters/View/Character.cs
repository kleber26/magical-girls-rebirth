using System.Collections.Generic;
using ScriptableObjects.CharacterObjects;
using Players.View;
using ScriptableObjects.Skills;
using UnityEngine;

namespace Characters.View
{
    public class Character
    {
        public string Name { get; }
        public int BaseLife { get; }
        public int CurrentLife { get; set; }
        public int Atk { get; }
        public int Def { get; }
        public int AtkBonus { get; set; }
        public int DefBonus{ get; set; }
        public int Stamina { get; }
        public List<SkillObject> Skills { get; }
        public GameObject Prefab { get; }
        public Player Player { get; }
        public (int, int) Position { get; set; }

        public Character(CharacterObject characterObject, Player player, (int, int) position)
        {
            Prefab = characterObject.prefab;
            BaseLife = characterObject.life;
            CurrentLife = characterObject.life;
            Atk = characterObject.atk;
            Def = characterObject.def;
            Stamina = characterObject.stamina;
            Skills = characterObject.skills;
            Player = player;
            Position = position;
            DefBonus = characterObject.defBonus;
            AtkBonus = characterObject.atkBonus;
            Name = characterObject.name;
        }
    }
}
