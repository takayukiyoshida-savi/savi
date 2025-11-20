using System.Collections.Generic;
using UnityEngine;
using MagicSurvivors.Data;

namespace MagicSurvivors.Skills
{
    public class SkillManager : MonoBehaviour
    {
        [Header("Skill Database")]
        [SerializeField] private List<SkillData> allSkills = new List<SkillData>();
        
        private Dictionary<SkillType, SkillInstance> activeSkills = new Dictionary<SkillType, SkillInstance>();
        private Transform playerTransform;
        private float attackPowerMultiplier = 1f;
        private float cooldownReductionMultiplier = 1f;
        
        public delegate void SkillEvent(SkillType skillType);
        public event SkillEvent OnSkillAcquired;
        public event SkillEvent OnSkillLevelUp;
        
        private void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }
        
        private void Update()
        {
            if (playerTransform != null)
            {
                UpdateSkills();
            }
        }
        
        public bool HasSkill(SkillType skillType)
        {
            return activeSkills.ContainsKey(skillType);
        }
        
        public void AcquireSkill(SkillType skillType)
        {
            if (activeSkills.ContainsKey(skillType))
            {
                LevelUpSkill(skillType);
                return;
            }
            
            SkillData skillData = allSkills.Find(s => s.skillType == skillType);
            if (skillData != null)
            {
                SkillInstance instance = new SkillInstance(skillData);
                activeSkills.Add(skillType, instance);
                OnSkillAcquired?.Invoke(skillType);
                Debug.Log($"SkillManager: Acquired skill {skillType}");
            }
        }
        
        public void LevelUpSkill(SkillType skillType)
        {
            if (activeSkills.ContainsKey(skillType))
            {
                activeSkills[skillType].LevelUp();
                OnSkillLevelUp?.Invoke(skillType);
                Debug.Log($"SkillManager: Leveled up skill {skillType} to level {activeSkills[skillType].CurrentLevel}");
            }
        }
        
        public List<ElementType> GetActiveElements()
        {
            List<ElementType> elements = new List<ElementType>();
            foreach (var skill in activeSkills.Values)
            {
                if (!elements.Contains(skill.Data.element))
                {
                    elements.Add(skill.Data.element);
                }
            }
            return elements;
        }
        
        public int GetTotalLevelForElement(ElementType element)
        {
            int totalLevel = 0;
            foreach (var skill in activeSkills.Values)
            {
                if (skill.Data.element == element)
                {
                    totalLevel += skill.CurrentLevel;
                }
            }
            return totalLevel;
        }
        
        public void SetAttackPowerMultiplier(float multiplier)
        {
            attackPowerMultiplier = multiplier;
        }
        
        public void SetCooldownReductionMultiplier(float multiplier)
        {
            cooldownReductionMultiplier = multiplier;
        }
        
        private void UpdateSkills()
        {
            foreach (var skill in activeSkills.Values)
            {
                skill.Update(Time.deltaTime);
                
                if (skill.CanCast())
                {
                    CastSkill(skill);
                }
            }
        }
        
        private void CastSkill(SkillInstance skill)
        {
            GameObject target = FindNearestEnemy();
            
            if (skill.Data.projectilePrefab != null)
            {
                SpawnProjectile(skill, target);
            }
            else if (skill.Data.effectPrefab != null)
            {
                SpawnEffect(skill);
            }
            
            skill.Cast();
        }
        
        private void SpawnProjectile(SkillInstance skill, GameObject target)
        {
            if (playerTransform == null) return;
            
            Vector3 direction = Vector3.right;
            if (target != null)
            {
                direction = (target.transform.position - playerTransform.position).normalized;
            }
            
            for (int i = 0; i < skill.CurrentStats.projectileCount; i++)
            {
                float angle = 0f;
                if (skill.CurrentStats.projectileCount > 1)
                {
                    angle = (i - (skill.CurrentStats.projectileCount - 1) / 2f) * 15f;
                }
                
                Vector3 rotatedDirection = Quaternion.Euler(0, 0, angle) * direction;
                GameObject projectile = Instantiate(skill.Data.projectilePrefab, playerTransform.position, Quaternion.identity);
                
                Projectile projComponent = projectile.GetComponent<Projectile>();
                if (projComponent != null)
                {
                    projComponent.Initialize(skill.GetDamage(attackPowerMultiplier), rotatedDirection, skill.CurrentStats);
                }
            }
        }
        
        private void SpawnEffect(SkillInstance skill)
        {
            if (playerTransform == null) return;
            
            GameObject effect = Instantiate(skill.Data.effectPrefab, playerTransform.position, Quaternion.identity);
            
            SkillEffect effectComponent = effect.GetComponent<SkillEffect>();
            if (effectComponent != null)
            {
                effectComponent.Initialize(skill.GetDamage(attackPowerMultiplier), skill.CurrentStats);
            }
        }
        
        private GameObject FindNearestEnemy()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject nearest = null;
            float minDistance = float.MaxValue;
            
            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(playerTransform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = enemy;
                }
            }
            
            return nearest;
        }
    }
    
    [System.Serializable]
    public class SkillInstance
    {
        private SkillData data;
        private int currentLevel;
        private float cooldownTimer;
        private SkillStats currentStats;
        
        public SkillData Data => data;
        public int CurrentLevel => currentLevel;
        public SkillStats CurrentStats => currentStats;
        
        public SkillInstance(SkillData skillData)
        {
            data = skillData;
            currentLevel = 1;
            cooldownTimer = 0f;
            currentStats = new SkillStats
            {
                baseDamage = skillData.baseStats.baseDamage,
                cooldown = skillData.baseStats.cooldown,
                projectileSpeed = skillData.baseStats.projectileSpeed,
                range = skillData.baseStats.range,
                projectileCount = skillData.baseStats.projectileCount,
                areaRadius = skillData.baseStats.areaRadius,
                pierceCount = skillData.baseStats.pierceCount,
                duration = skillData.baseStats.duration
            };
        }
        
        public void Update(float deltaTime)
        {
            if (cooldownTimer > 0)
            {
                cooldownTimer -= deltaTime;
            }
        }
        
        public bool CanCast()
        {
            return cooldownTimer <= 0;
        }
        
        public void Cast()
        {
            cooldownTimer = currentStats.cooldown;
        }
        
        public void LevelUp()
        {
            if (currentLevel < data.maxLevel)
            {
                currentLevel++;
                currentStats.baseDamage += data.damagePerLevel;
                currentStats.cooldown *= (1f - data.cooldownReductionPerLevel);
            }
        }
        
        public float GetDamage(float multiplier)
        {
            return currentStats.baseDamage * multiplier;
        }
    }
}
