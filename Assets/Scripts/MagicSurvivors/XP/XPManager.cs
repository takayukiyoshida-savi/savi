using UnityEngine;
using MagicSurvivors.Characters;

namespace MagicSurvivors.XP
{
    public class XPManager : MonoBehaviour
    {
        [Header("Level Settings")]
        [SerializeField] private int currentLevel = 1;
        [SerializeField] private int currentXP = 0;
        [SerializeField] private int baseXPRequired = 100;
        [SerializeField] private float xpScalingFactor = 1.2f;
        
        private PlayerCharacter player;
        private int xpRequiredForNextLevel;
        
        public int CurrentLevel => currentLevel;
        public int CurrentXP => currentXP;
        public int XPRequiredForNextLevel => xpRequiredForNextLevel;
        
        public delegate void LevelEvent(int level);
        public delegate void XPEvent(int current, int required);
        public event LevelEvent OnLevelUp;
        public event XPEvent OnXPChanged;
        
        private void Start()
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.GetComponent<PlayerCharacter>();
            }
            
            CalculateXPRequired();
            OnXPChanged?.Invoke(currentXP, xpRequiredForNextLevel);
        }
        
        public void AddXP(int amount)
        {
            if (player != null)
            {
                amount = Mathf.RoundToInt(amount * player.CurrentStats.xpMultiplier);
            }
            
            currentXP += amount;
            OnXPChanged?.Invoke(currentXP, xpRequiredForNextLevel);
            
            while (currentXP >= xpRequiredForNextLevel)
            {
                LevelUp();
            }
        }
        
        private void LevelUp()
        {
            currentXP -= xpRequiredForNextLevel;
            currentLevel++;
            CalculateXPRequired();
            
            OnLevelUp?.Invoke(currentLevel);
            OnXPChanged?.Invoke(currentXP, xpRequiredForNextLevel);
            
            Debug.Log($"XPManager: Level up! Now level {currentLevel}");
        }
        
        private void CalculateXPRequired()
        {
            xpRequiredForNextLevel = Mathf.RoundToInt(baseXPRequired * Mathf.Pow(xpScalingFactor, currentLevel - 1));
        }
    }
}
