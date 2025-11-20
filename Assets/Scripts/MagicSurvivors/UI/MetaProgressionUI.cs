using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MagicSurvivors.MetaProgression;
using MagicSurvivors.Data;

namespace MagicSurvivors.UI
{
    public class MetaProgressionUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject upgradePanel;
        [SerializeField] private Text goldText;
        [SerializeField] private Transform upgradeCardContainer;
        [SerializeField] private GameObject upgradeCardPrefab;
        
        private List<UpgradeCard> upgradeCards = new List<UpgradeCard>();
        
        private void Start()
        {
            if (MetaProgressionManager.Instance != null)
            {
                MetaProgressionManager.Instance.OnGoldChanged += UpdateGoldDisplay;
                MetaProgressionManager.Instance.OnUpgradePurchased += OnUpgradePurchased;
                UpdateGoldDisplay(MetaProgressionManager.Instance.TotalGold);
            }
            
            PopulateUpgradeCards();
        }
        
        private void OnDestroy()
        {
            if (MetaProgressionManager.Instance != null)
            {
                MetaProgressionManager.Instance.OnGoldChanged -= UpdateGoldDisplay;
                MetaProgressionManager.Instance.OnUpgradePurchased -= OnUpgradePurchased;
            }
        }
        
        private void PopulateUpgradeCards()
        {
            if (MetaProgressionManager.Instance == null) return;
            
            List<MetaUpgrade> upgrades = MetaProgressionManager.Instance.GetAllUpgrades();
            
            foreach (MetaUpgrade upgrade in upgrades)
            {
                if (upgradeCardPrefab != null && upgradeCardContainer != null)
                {
                    GameObject cardObj = Instantiate(upgradeCardPrefab, upgradeCardContainer);
                    UpgradeCard card = cardObj.GetComponent<UpgradeCard>();
                    
                    if (card != null)
                    {
                        card.SetUpgradeData(upgrade);
                        card.OnPurchaseClicked = OnPurchaseUpgrade;
                        upgradeCards.Add(card);
                    }
                }
            }
        }
        
        private void OnPurchaseUpgrade(MetaUpgradeType upgradeType)
        {
            if (MetaProgressionManager.Instance != null)
            {
                MetaProgressionManager.Instance.PurchaseUpgrade(upgradeType);
            }
        }
        
        private void OnUpgradePurchased(MetaUpgradeType upgradeType, int level)
        {
            RefreshUpgradeCards();
        }
        
        private void RefreshUpgradeCards()
        {
            foreach (UpgradeCard card in upgradeCards)
            {
                card.RefreshDisplay();
            }
        }
        
        private void UpdateGoldDisplay(int gold)
        {
            if (goldText != null)
            {
                goldText.text = $"Gold: {gold}";
            }
        }
    }
    
    [System.Serializable]
    public class UpgradeCard : MonoBehaviour
    {
        [SerializeField] private Text upgradeName;
        [SerializeField] private Text upgradeDescription;
        [SerializeField] private Text currentLevelText;
        [SerializeField] private Text costText;
        [SerializeField] private Button purchaseButton;
        
        private MetaUpgrade currentUpgrade;
        public System.Action<MetaUpgradeType> OnPurchaseClicked;
        
        private void Awake()
        {
            if (purchaseButton != null)
            {
                purchaseButton.onClick.AddListener(OnPurchaseButtonClicked);
            }
        }
        
        public void SetUpgradeData(MetaUpgrade upgrade)
        {
            currentUpgrade = upgrade;
            RefreshDisplay();
        }
        
        public void RefreshDisplay()
        {
            if (currentUpgrade == null) return;
            
            if (upgradeName != null)
            {
                upgradeName.text = currentUpgrade.upgradeName;
            }
            
            if (upgradeDescription != null)
            {
                upgradeDescription.text = currentUpgrade.description;
            }
            
            if (currentLevelText != null)
            {
                currentLevelText.text = $"Level: {currentUpgrade.currentLevel} / {currentUpgrade.maxLevel}";
            }
            
            if (costText != null)
            {
                if (currentUpgrade.currentLevel < currentUpgrade.maxLevel)
                {
                    costText.text = $"Cost: {currentUpgrade.GetCostForNextLevel()}";
                }
                else
                {
                    costText.text = "MAX";
                }
            }
            
            if (purchaseButton != null)
            {
                purchaseButton.interactable = currentUpgrade.currentLevel < currentUpgrade.maxLevel;
            }
        }
        
        private void OnPurchaseButtonClicked()
        {
            OnPurchaseClicked?.Invoke(currentUpgrade.upgradeType);
        }
    }
}
