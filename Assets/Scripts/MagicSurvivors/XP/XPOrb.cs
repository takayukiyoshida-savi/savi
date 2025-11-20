using UnityEngine;
using MagicSurvivors.Data;

namespace MagicSurvivors.XP
{
    public class XPOrb : MonoBehaviour
    {
        [SerializeField] private int xpValue = 10;
        [SerializeField] private float attractSpeed = 5f;
        [SerializeField] private float attractRange = 5f;
        
        private Transform playerTransform;
        private bool isBeingAttracted = false;
        
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
            if (playerTransform == null) return;
            
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            
            var player = playerTransform.GetComponent<Characters.PlayerCharacter>();
            float pickupRange = player != null ? player.CurrentStats.pickupRange : GameConstants.XP_PICKUP_RANGE_BASE;
            
            if (distance <= pickupRange)
            {
                isBeingAttracted = true;
            }
            
            if (isBeingAttracted)
            {
                Vector3 direction = (playerTransform.position - transform.position).normalized;
                transform.position += direction * attractSpeed * Time.deltaTime;
                
                if (distance < 0.5f)
                {
                    CollectXP();
                }
            }
        }
        
        public void Initialize(int value)
        {
            xpValue = value;
        }
        
        private void CollectXP()
        {
            XPManager xpManager = FindObjectOfType<XPManager>();
            if (xpManager != null)
            {
                xpManager.AddXP(xpValue);
            }
            
            Destroy(gameObject);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                CollectXP();
            }
        }
    }
}
