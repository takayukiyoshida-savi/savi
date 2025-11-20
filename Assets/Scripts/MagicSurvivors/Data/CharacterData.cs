using UnityEngine;

namespace MagicSurvivors.Data
{
    [System.Serializable]
    public class CharacterStats
    {
        public float maxHP = 100f;
        public float attackPower = 10f;
        public float moveSpeed = 5f;
        public float cooldownReduction = 0f;
        public float xpMultiplier = 1f;
        public float goldMultiplier = 1f;
        public float pickupRange = 2f;
    }
    
    [CreateAssetMenu(fileName = "CharacterData", menuName = "MagicSurvivors/CharacterData")]
    public class CharacterData : ScriptableObject
    {
        public CharacterClass characterClass;
        public string characterName;
        public string description;
        public Sprite characterSprite;
        
        public CharacterStats baseStats;
        public SkillType startingSkill;
        public ElementType primaryElement;
        
        [TextArea(3, 5)]
        public string passiveDescription;
    }
}
