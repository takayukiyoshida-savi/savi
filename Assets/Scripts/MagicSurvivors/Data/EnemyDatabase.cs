using System.Collections.Generic;
using UnityEngine;

namespace MagicSurvivors.Data
{
    [System.Serializable]
    public class EnemyDefinition
    {
        public EnemyType enemyType;
        public string enemyName;
        public string enemyNameJapanese;
        public string description;
        public float maxHP;
        public float attackDamage;
        public float moveSpeed;
        public float detectionRange;
        public float attackRange;
        public float attackCooldown;
        public int xpDrop;
        public int goldDrop;
        public bool isBoss;
        public bool isElite;
        public bool isMiniBoss;
    }

    public static class EnemyDatabase
    {
        private static Dictionary<EnemyType, EnemyDefinition> enemies;

        public static void Initialize()
        {
            enemies = new Dictionary<EnemyType, EnemyDefinition>
            {
                {
                    EnemyType.Goblin,
                    new EnemyDefinition
                    {
                        enemyType = EnemyType.Goblin,
                        enemyName = "Goblin",
                        enemyNameJapanese = "ゴブリン",
                        description = "Weak melee enemy",
                        maxHP = 30f,
                        attackDamage = 5f,
                        moveSpeed = 2f,
                        detectionRange = 15f,
                        attackRange = 1f,
                        attackCooldown = 1f,
                        xpDrop = 5,
                        goldDrop = 2,
                        isBoss = false,
                        isElite = false,
                        isMiniBoss = false
                    }
                },
                {
                    EnemyType.Slime,
                    new EnemyDefinition
                    {
                        enemyType = EnemyType.Slime,
                        enemyName = "Slime",
                        enemyNameJapanese = "スライム",
                        description = "Slow but tanky enemy",
                        maxHP = 50f,
                        attackDamage = 3f,
                        moveSpeed = 1.5f,
                        detectionRange = 12f,
                        attackRange = 1f,
                        attackCooldown = 1.5f,
                        xpDrop = 4,
                        goldDrop = 1,
                        isBoss = false,
                        isElite = false,
                        isMiniBoss = false
                    }
                },
                {
                    EnemyType.SkeletonArcher,
                    new EnemyDefinition
                    {
                        enemyType = EnemyType.SkeletonArcher,
                        enemyName = "Skeleton Archer",
                        enemyNameJapanese = "スケルトンアーチャー",
                        description = "Ranged enemy that keeps distance",
                        maxHP = 25f,
                        attackDamage = 8f,
                        moveSpeed = 2.5f,
                        detectionRange = 20f,
                        attackRange = 10f,
                        attackCooldown = 2f,
                        xpDrop = 6,
                        goldDrop = 3,
                        isBoss = false,
                        isElite = false,
                        isMiniBoss = false
                    }
                },
                {
                    EnemyType.EliteGoblin,
                    new EnemyDefinition
                    {
                        enemyType = EnemyType.EliteGoblin,
                        enemyName = "Elite Goblin",
                        enemyNameJapanese = "エリートゴブリン",
                        description = "Stronger goblin variant",
                        maxHP = 80f,
                        attackDamage = 12f,
                        moveSpeed = 2.5f,
                        detectionRange = 18f,
                        attackRange = 1.5f,
                        attackCooldown = 0.8f,
                        xpDrop = 15,
                        goldDrop = 8,
                        isBoss = false,
                        isElite = true,
                        isMiniBoss = false
                    }
                },
                {
                    EnemyType.EliteSkeleton,
                    new EnemyDefinition
                    {
                        enemyType = EnemyType.EliteSkeleton,
                        enemyName = "Elite Skeleton",
                        enemyNameJapanese = "エリートスケルトン",
                        description = "Powerful skeleton warrior",
                        maxHP = 100f,
                        attackDamage = 15f,
                        moveSpeed = 3f,
                        detectionRange = 20f,
                        attackRange = 2f,
                        attackCooldown = 1.2f,
                        xpDrop = 20,
                        goldDrop = 10,
                        isBoss = false,
                        isElite = true,
                        isMiniBoss = false
                    }
                },
                {
                    EnemyType.FlameOgre,
                    new EnemyDefinition
                    {
                        enemyType = EnemyType.FlameOgre,
                        enemyName = "Flame Ogre",
                        enemyNameJapanese = "フレイムオーガ",
                        description = "Mini-boss at 5 minutes",
                        maxHP = 500f,
                        attackDamage = 25f,
                        moveSpeed = 2f,
                        detectionRange = 25f,
                        attackRange = 3f,
                        attackCooldown = 2f,
                        xpDrop = 100,
                        goldDrop = 50,
                        isBoss = false,
                        isElite = false,
                        isMiniBoss = true
                    }
                },
                {
                    EnemyType.WindHarpy,
                    new EnemyDefinition
                    {
                        enemyType = EnemyType.WindHarpy,
                        enemyName = "Wind Harpy",
                        enemyNameJapanese = "ウィンドハーピー",
                        description = "Mini-boss at 10 minutes",
                        maxHP = 800f,
                        attackDamage = 30f,
                        moveSpeed = 3.5f,
                        detectionRange = 30f,
                        attackRange = 8f,
                        attackCooldown = 1.5f,
                        xpDrop = 150,
                        goldDrop = 75,
                        isBoss = false,
                        isElite = false,
                        isMiniBoss = true
                    }
                },
                {
                    EnemyType.IceGolem,
                    new EnemyDefinition
                    {
                        enemyType = EnemyType.IceGolem,
                        enemyName = "Ice Golem",
                        enemyNameJapanese = "アイスゴーレム",
                        description = "Mini-boss at 15 minutes",
                        maxHP = 1200f,
                        attackDamage = 35f,
                        moveSpeed = 1.5f,
                        detectionRange = 25f,
                        attackRange = 4f,
                        attackCooldown = 2.5f,
                        xpDrop = 200,
                        goldDrop = 100,
                        isBoss = false,
                        isElite = false,
                        isMiniBoss = true
                    }
                },
                {
                    EnemyType.AncientDragon,
                    new EnemyDefinition
                    {
                        enemyType = EnemyType.AncientDragon,
                        enemyName = "Ancient Dragon",
                        enemyNameJapanese = "エンシェントドラゴン",
                        description = "Final boss at 20 minutes",
                        maxHP = 3000f,
                        attackDamage = 50f,
                        moveSpeed = 2.5f,
                        detectionRange = 40f,
                        attackRange = 6f,
                        attackCooldown = 3f,
                        xpDrop = 500,
                        goldDrop = 300,
                        isBoss = true,
                        isElite = false,
                        isMiniBoss = false
                    }
                }
            };
        }

        public static EnemyDefinition GetEnemy(EnemyType enemyType)
        {
            if (enemies == null)
            {
                Initialize();
            }
            
            if (enemies.TryGetValue(enemyType, out EnemyDefinition enemy))
            {
                return enemy;
            }
            
            return null;
        }

        public static List<EnemyDefinition> GetAllEnemies()
        {
            if (enemies == null)
            {
                Initialize();
            }
            
            return new List<EnemyDefinition>(enemies.Values);
        }

        public static List<EnemyDefinition> GetNormalEnemies()
        {
            if (enemies == null)
            {
                Initialize();
            }
            
            List<EnemyDefinition> result = new List<EnemyDefinition>();
            foreach (var enemy in enemies.Values)
            {
                if (!enemy.isBoss && !enemy.isElite && !enemy.isMiniBoss)
                {
                    result.Add(enemy);
                }
            }
            return result;
        }

        public static List<EnemyDefinition> GetEliteEnemies()
        {
            if (enemies == null)
            {
                Initialize();
            }
            
            List<EnemyDefinition> result = new List<EnemyDefinition>();
            foreach (var enemy in enemies.Values)
            {
                if (enemy.isElite)
                {
                    result.Add(enemy);
                }
            }
            return result;
        }

        public static List<EnemyDefinition> GetMiniBosses()
        {
            if (enemies == null)
            {
                Initialize();
            }
            
            List<EnemyDefinition> result = new List<EnemyDefinition>();
            foreach (var enemy in enemies.Values)
            {
                if (enemy.isMiniBoss)
                {
                    result.Add(enemy);
                }
            }
            return result;
        }
    }
}
