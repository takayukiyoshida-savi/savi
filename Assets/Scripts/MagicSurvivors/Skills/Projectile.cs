using UnityEngine;
using MagicSurvivors.Data;

namespace MagicSurvivors.Skills
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        private float damage;
        private Vector3 direction;
        private SkillStats stats;
        private Rigidbody2D rb2d;
        private int remainingPierces;
        private float lifetime;
        
        private void Awake()
        {
            rb2d = GetComponent<Rigidbody2D>();
        }
        
        private void Update()
        {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0)
            {
                Destroy(gameObject);
            }
        }
        
        public void Initialize(float dmg, Vector3 dir, SkillStats skillStats)
        {
            damage = dmg;
            direction = dir.normalized;
            stats = skillStats;
            remainingPierces = stats.pierceCount;
            lifetime = stats.range / stats.projectileSpeed;
            
            rb2d.velocity = direction * stats.projectileSpeed;
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                var enemy = other.GetComponent<MagicSurvivors.Enemies.Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
                
                remainingPierces--;
                if (remainingPierces < 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
