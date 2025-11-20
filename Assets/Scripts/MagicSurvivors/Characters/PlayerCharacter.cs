using UnityEngine;
using MagicSurvivors.Data;

namespace MagicSurvivors.Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerCharacter : MonoBehaviour
    {
        [Header("Character Data")]
        [SerializeField] private CharacterData characterData;
        
        private Rigidbody2D rb2d;
        private CharacterStats currentStats;
        private float currentHP;
        private Vector2 moveInput;
        
        public CharacterStats CurrentStats => currentStats;
        public float CurrentHP => currentHP;
        public float MaxHP => currentStats.maxHP;
        public CharacterData Data => characterData;
        
        public delegate void HealthChangeEvent(float current, float max);
        public event HealthChangeEvent OnHealthChanged;
        public event System.Action OnDeath;
        
        private void Awake()
        {
            rb2d = GetComponent<Rigidbody2D>();
            InitializeStats();
        }
        
        private void Start()
        {
            gameObject.tag = "Player";
        }
        
        private void Update()
        {
            HandleInput();
        }
        
        private void FixedUpdate()
        {
            HandleMovement();
        }
        
        public void InitializeWithData(CharacterData data)
        {
            characterData = data;
            InitializeStats();
        }
        
        private void InitializeStats()
        {
            if (characterData != null)
            {
                currentStats = new CharacterStats
                {
                    maxHP = characterData.baseStats.maxHP,
                    attackPower = characterData.baseStats.attackPower,
                    moveSpeed = characterData.baseStats.moveSpeed,
                    cooldownReduction = characterData.baseStats.cooldownReduction,
                    xpMultiplier = characterData.baseStats.xpMultiplier,
                    goldMultiplier = characterData.baseStats.goldMultiplier,
                    pickupRange = characterData.baseStats.pickupRange
                };
                
                currentHP = currentStats.maxHP;
                OnHealthChanged?.Invoke(currentHP, currentStats.maxHP);
            }
        }
        
        private void HandleInput()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            moveInput = new Vector2(horizontal, vertical).normalized;
        }
        
        private void HandleMovement()
        {
            Vector2 moveForce = moveInput * currentStats.moveSpeed;
            rb2d.AddForce(moveForce, ForceMode2D.Force);
            
            float maxSpeed = currentStats.moveSpeed * 2f;
            if (rb2d.velocity.magnitude > maxSpeed)
            {
                rb2d.velocity = rb2d.velocity.normalized * maxSpeed;
            }
            
            if (moveInput.magnitude == 0)
            {
                rb2d.velocity = Vector2.Lerp(rb2d.velocity, Vector2.zero, Time.fixedDeltaTime * 5f);
            }
        }
        
        public void TakeDamage(float damage)
        {
            currentHP -= damage;
            currentHP = Mathf.Max(0, currentHP);
            OnHealthChanged?.Invoke(currentHP, currentStats.maxHP);
            
            if (currentHP <= 0)
            {
                Die();
            }
        }
        
        public void Heal(float amount)
        {
            currentHP += amount;
            currentHP = Mathf.Min(currentHP, currentStats.maxHP);
            OnHealthChanged?.Invoke(currentHP, currentStats.maxHP);
        }
        
        public void ApplyStatModifier(string statName, float value)
        {
            switch (statName)
            {
                case "maxHP":
                    currentStats.maxHP += value;
                    currentHP += value;
                    OnHealthChanged?.Invoke(currentHP, currentStats.maxHP);
                    break;
                case "attackPower":
                    currentStats.attackPower += value;
                    break;
                case "moveSpeed":
                    currentStats.moveSpeed += value;
                    break;
                case "cooldownReduction":
                    currentStats.cooldownReduction += value;
                    break;
                case "xpMultiplier":
                    currentStats.xpMultiplier += value;
                    break;
                case "goldMultiplier":
                    currentStats.goldMultiplier += value;
                    break;
                case "pickupRange":
                    currentStats.pickupRange += value;
                    break;
            }
        }
        
        private void Die()
        {
            OnDeath?.Invoke();
            Debug.Log("PlayerCharacter: Player died");
        }
        
        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}
