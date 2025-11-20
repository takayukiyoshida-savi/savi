using System.Collections.Generic;
using UnityEngine;
using MagicSurvivors.Data;

namespace MagicSurvivors.MetaProgression
{
    [System.Serializable]
    public class MetaUpgrade
    {
        public MetaUpgradeType upgradeType;
        public string upgradeName;
        public string description;
        public int currentLevel = 0;
        public int maxLevel = 10;
        public int baseCost = 100;
        public float costMultiplier = 1.5f;
        public float valuePerLevel = 5f;
        
        public int GetCostForNextLevel()
        {
            return Mathf.RoundToInt(baseCost * Mathf.Pow(costMultiplier, currentLevel));
        }
        
        public float GetCurrentValue()
        {
            return valuePerLevel * currentLevel;
        }
    }
    
    public class MetaProgressionManager : MonoBehaviour
    {
        public static MetaProgressionManager Instance { get; private set; }
        
        [Header("Meta Upgrades")]
        [SerializeField] private List<MetaUpgrade> metaUpgrades = new List<MetaUpgrade>();
        
        [Header("Currency")]
        [SerializeField] private int totalGold = 0;
        
        public int TotalGold => totalGold;
        
        public delegate void UpgradeEvent(MetaUpgradeType upgradeType, int level);
        public delegate void GoldEvent(int amount);
        public event UpgradeEvent OnUpgradePurchased;
        public event GoldEvent OnGoldChanged;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeUpgrades();
                LoadProgress();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void InitializeUpgrades()
        {
            if (metaUpgrades.Count == 0)
            {
                metaUpgrades.Add(new MetaUpgrade
                {
                    upgradeType = MetaUpgradeType.MaxHP,
                    upgradeName = "Max HP",
                    description = "Increase maximum health",
                    valuePerLevel = 10f
                });
                
                metaUpgrades.Add(new MetaUpgrade
                {
                    upgradeType = MetaUpgradeType.AttackPower,
                    upgradeName = "Attack Power",
                    description = "Increase damage dealt",
                    valuePerLevel = 5f
                });
                
                metaUpgrades.Add(new MetaUpgrade
                {
                    upgradeType = MetaUpgradeType.CooldownReduction,
                    upgradeName = "Cooldown Reduction",
                    description = "Reduce skill cooldowns",
                    valuePerLevel = 2f
                });
                
                metaUpgrades.Add(new MetaUpgrade
                {
                    upgradeType = MetaUpgradeType.MovementSpeed,
                    upgradeName = "Movement Speed",
                    description = "Increase movement speed",
                    valuePerLevel = 0.5f
                });
                
                metaUpgrades.Add(new MetaUpgrade
                {
                    upgradeType = MetaUpgradeType.XPGain,
                    upgradeName = "XP Gain",
                    description = "Increase XP gained",
                    valuePerLevel = 5f
                });
                
                metaUpgrades.Add(new MetaUpgrade
                {
                    upgradeType = MetaUpgradeType.GoldGain,
                    upgradeName = "Gold Gain",
                    description = "Increase gold gained",
                    valuePerLevel = 5f
                });
                
                metaUpgrades.Add(new MetaUpgrade
                {
                    upgradeType = MetaUpgradeType.ElementalDamage,
                    upgradeName = "Elemental Damage",
                    description = "Increase elemental damage",
                    valuePerLevel = 3f
                });
                
                metaUpgrades.Add(new MetaUpgrade
                {
                    upgradeType = MetaUpgradeType.PickupRange,
                    upgradeName = "Pickup Range",
                    description = "Increase XP pickup range",
                    valuePerLevel = 0.5f
                });
            }
        }
        
        public bool PurchaseUpgrade(MetaUpgradeType upgradeType)
        {
            MetaUpgrade upgrade = metaUpgrades.Find(u => u.upgradeType == upgradeType);
            
            if (upgrade != null && upgrade.currentLevel < upgrade.maxLevel)
            {
                int cost = upgrade.GetCostForNextLevel();
                
                if (totalGold >= cost)
                {
                    totalGold -= cost;
                    upgrade.currentLevel++;
                    
                    OnUpgradePurchased?.Invoke(upgradeType, upgrade.currentLevel);
                    OnGoldChanged?.Invoke(totalGold);
                    
                    SaveProgress();
                    Debug.Log($"MetaProgressionManager: Purchased {upgrade.upgradeName} level {upgrade.currentLevel}");
                    return true;
                }
            }
            
            return false;
        }
        
        public void AddGold(int amount)
        {
            totalGold += amount;
            OnGoldChanged?.Invoke(totalGold);
            SaveProgress();
        }
        
        public MetaUpgrade GetUpgrade(MetaUpgradeType upgradeType)
        {
            return metaUpgrades.Find(u => u.upgradeType == upgradeType);
        }
        
        public List<MetaUpgrade> GetAllUpgrades()
        {
            return new List<MetaUpgrade>(metaUpgrades);
        }
        
        private void SaveProgress()
        {
            PlayerPrefs.SetInt("TotalGold", totalGold);
            
            foreach (MetaUpgrade upgrade in metaUpgrades)
            {
                PlayerPrefs.SetInt($"MetaUpgrade_{upgrade.upgradeType}", upgrade.currentLevel);
            }
            
            PlayerPrefs.Save();
        }
        
        private void LoadProgress()
        {
            totalGold = PlayerPrefs.GetInt("TotalGold", 0);
            
            foreach (MetaUpgrade upgrade in metaUpgrades)
            {
                upgrade.currentLevel = PlayerPrefs.GetInt($"MetaUpgrade_{upgrade.upgradeType}", 0);
            }
        }
    }
}
