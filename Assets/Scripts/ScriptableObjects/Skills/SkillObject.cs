using System.Collections.Generic;
using Skills;
using Skills.View;
using UnityEngine;

namespace ScriptableObjects.Skills
{
    [CreateAssetMenu(fileName = "ScriptableObjects", menuName = "ScriptableObjects/SkillObject", order = 1)]
    public class SkillObject : ScriptableObject
    {
        public string name;
        public string description;
        public SkillArea skillArea;
        public int areaSize;
        public int range;
        public string characterAnimationTrigger;
        public Sprite skillSprite;

        public GameObject skillAnimation;
        public Vector3 skillAnimationOffset;
        public List<SkillAttributes> skillAttributes;
    }
}
