using UnityEngine;

namespace MagicSurvivors.Data
{
    [System.Serializable]
    public class SkillStats
    {
        public float baseDamage = 10f;
        public float cooldown = 1f;
        public float projectileSpeed = 10f;
        public float range = 15f;
        public int projectileCount = 1;
        public float areaRadius = 0f;
        public int pierceCount = 0;
        public float duration = 5f;
    }
    
    [CreateAssetMenu(fileName = "SkillData", menuName = "MagicSurvivors/SkillData")]
    public class SkillData : ScriptableObject
    {
        public SkillType skillType;
        public string skillName;
        public ElementType element;
        public Sprite skillIcon;
        
        [TextArea(2, 4)]
        public string description;
        
        public SkillStats baseStats;
        public GameObject projectilePrefab;
        public GameObject effectPrefab;
        
        public int maxLevel = 5;
        public float damagePerLevel = 2f;
        public float cooldownReductionPerLevel = 0.05f;
    }
}
