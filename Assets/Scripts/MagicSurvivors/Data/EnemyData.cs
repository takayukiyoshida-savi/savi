using UnityEngine;

namespace MagicSurvivors.Data
{
    [System.Serializable]
    public class EnemyStats
    {
        public float maxHP = 50f;
        public float attackDamage = 10f;
        public float moveSpeed = 2f;
        public float detectionRange = 15f;
        public float attackRange = 1f;
        public float attackCooldown = 1f;
        public int xpDrop = 10;
        public int goldDrop = 5;
    }
    
    [CreateAssetMenu(fileName = "EnemyData", menuName = "MagicSurvivors/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        public EnemyType enemyType;
        public string enemyName;
        public Sprite enemySprite;
        
        public EnemyStats baseStats;
        public bool isBoss = false;
        public bool isElite = false;
        
        [TextArea(2, 3)]
        public string description;
    }
}
