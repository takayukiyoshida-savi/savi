using UnityEngine;
using MagicSurvivors.Data;

namespace MagicSurvivors.Skills
{
    public class SkillEffect : MonoBehaviour
    {
        private float damage;
        private SkillStats stats;
        private float lifetime;
        private float damageInterval = 0.5f;
        private float damageTimer;
        
        private void Update()
        {
            lifetime -= Time.deltaTime;
            damageTimer -= Time.deltaTime;
            
            if (damageTimer <= 0)
            {
                DealDamageInArea();
                damageTimer = damageInterval;
            }
            
            if (lifetime <= 0)
            {
                Destroy(gameObject);
            }
        }
        
        public void Initialize(float dmg, SkillStats skillStats)
        {
            damage = dmg;
            stats = skillStats;
            lifetime = stats.duration;
            damageTimer = 0f;
            
            if (stats.areaRadius > 0)
            {
                transform.localScale = Vector3.one * stats.areaRadius * 2f;
            }
        }
        
        private void DealDamageInArea()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, stats.areaRadius);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    var enemy = hit.GetComponent<MagicSurvivors.Enemies.Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(damage * damageInterval);
                    }
                }
            }
        }
    }
}
