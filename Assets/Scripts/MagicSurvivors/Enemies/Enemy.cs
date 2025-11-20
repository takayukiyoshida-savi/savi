using UnityEngine;
using MagicSurvivors.Data;

namespace MagicSurvivors.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : MonoBehaviour
    {
        [Header("Enemy Data")]
        [SerializeField] private EnemyData enemyData;
        
        private Rigidbody2D rb2d;
        private Transform playerTransform;
        private EnemyStats currentStats;
        private float currentHP;
        private float attackCooldownTimer;
        
        public EnemyType EnemyType => enemyData.enemyType;
        public bool IsBoss => enemyData.isBoss;
        public bool IsElite => enemyData.isElite;
        
        public delegate void EnemyEvent(Enemy enemy);
        public event EnemyEvent OnEnemyDeath;
        
        private void Awake()
        {
            rb2d = GetComponent<Rigidbody2D>();
            InitializeStats();
        }
        
        private void Start()
        {
            gameObject.tag = "Enemy";
            FindPlayer();
        }
        
        private void Update()
        {
            if (attackCooldownTimer > 0)
            {
                attackCooldownTimer -= Time.deltaTime;
            }
        }
        
        private void FixedUpdate()
        {
            MoveTowardsPlayer();
        }
        
        public void InitializeWithData(EnemyData data)
        {
            enemyData = data;
            InitializeStats();
        }
        
        private void InitializeStats()
        {
            if (enemyData != null)
            {
                currentStats = new EnemyStats
                {
                    maxHP = enemyData.baseStats.maxHP,
                    attackDamage = enemyData.baseStats.attackDamage,
                    moveSpeed = enemyData.baseStats.moveSpeed,
                    detectionRange = enemyData.baseStats.detectionRange,
                    attackRange = enemyData.baseStats.attackRange,
                    attackCooldown = enemyData.baseStats.attackCooldown,
                    xpDrop = enemyData.baseStats.xpDrop,
                    goldDrop = enemyData.baseStats.goldDrop
                };
                
                currentHP = currentStats.maxHP;
            }
        }
        
        private void FindPlayer()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }
        
        private void MoveTowardsPlayer()
        {
            if (playerTransform == null) return;
            
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            
            if (distanceToPlayer <= currentStats.detectionRange)
            {
                Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
                Vector2 moveForce = directionToPlayer * currentStats.moveSpeed;
                rb2d.AddForce(moveForce, ForceMode2D.Force);
                
                float maxSpeed = currentStats.moveSpeed * 2f;
                if (rb2d.velocity.magnitude > maxSpeed)
                {
                    rb2d.velocity = rb2d.velocity.normalized * maxSpeed;
                }
                
                if (distanceToPlayer <= currentStats.attackRange && attackCooldownTimer <= 0)
                {
                    AttackPlayer();
                }
            }
            else
            {
                rb2d.velocity = Vector2.Lerp(rb2d.velocity, Vector2.zero, Time.fixedDeltaTime * 2f);
            }
        }
        
        private void AttackPlayer()
        {
            if (playerTransform == null) return;
            
            var player = playerTransform.GetComponent<Characters.PlayerCharacter>();
            if (player != null)
            {
                player.TakeDamage(currentStats.attackDamage);
                attackCooldownTimer = currentStats.attackCooldown;
            }
        }
        
        public void TakeDamage(float damage)
        {
            currentHP -= damage;
            
            if (currentHP <= 0)
            {
                Die();
            }
        }
        
        private void Die()
        {
            DropXP();
            OnEnemyDeath?.Invoke(this);
            Destroy(gameObject);
        }
        
        private void DropXP()
        {
            GameObject xpOrbPrefab = Resources.Load<GameObject>("Prefabs/XPOrb");
            if (xpOrbPrefab != null)
            {
                GameObject xpOrb = Instantiate(xpOrbPrefab, transform.position, Quaternion.identity);
                var xpComponent = xpOrb.GetComponent<XP.XPOrb>();
                if (xpComponent != null)
                {
                    xpComponent.Initialize(currentStats.xpDrop);
                }
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                AttackPlayer();
            }
        }
    }
}
