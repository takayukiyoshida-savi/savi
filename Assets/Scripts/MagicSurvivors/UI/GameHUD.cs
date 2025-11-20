using UnityEngine;
using UnityEngine.UI;
using MagicSurvivors.Characters;
using MagicSurvivors.XP;
using MagicSurvivors.Core;
using MagicSurvivors.Synergies;
using MagicSurvivors.Data;
using System.Collections.Generic;

namespace MagicSurvivors.UI
{
    public class GameHUD : MonoBehaviour
    {
        [Header("HP Bar")]
        [SerializeField] private Slider hpBar;
        [SerializeField] private Text hpText;
        
        [Header("XP Bar")]
        [SerializeField] private Slider xpBar;
        [SerializeField] private Text levelText;
        
        [Header("Timer")]
        [SerializeField] private Text timerText;
        
        [Header("Synergy Icons")]
        [SerializeField] private Transform synergyIconContainer;
        [SerializeField] private GameObject synergyIconPrefab;
        
        [Header("Minimap")]
        [SerializeField] private RawImage minimapImage;
        
        private PlayerCharacter player;
        private XPManager xpManager;
        private SynergyManager synergyManager;
        private Dictionary<SynergyType, GameObject> synergyIcons = new Dictionary<SynergyType, GameObject>();
        
        private void Start()
        {
            FindReferences();
            SubscribeToEvents();
        }
        
        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }
        
        private void Update()
        {
            UpdateTimer();
        }
        
        private void FindReferences()
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.GetComponent<PlayerCharacter>();
            }
            
            xpManager = FindObjectOfType<XPManager>();
            synergyManager = FindObjectOfType<SynergyManager>();
        }
        
        private void SubscribeToEvents()
        {
            if (player != null)
            {
                player.OnHealthChanged += UpdateHealthBar;
                UpdateHealthBar(player.CurrentHP, player.MaxHP);
            }
            
            if (xpManager != null)
            {
                xpManager.OnXPChanged += UpdateXPBar;
                xpManager.OnLevelUp += UpdateLevel;
                UpdateXPBar(xpManager.CurrentXP, xpManager.XPRequiredForNextLevel);
                UpdateLevel(xpManager.CurrentLevel);
            }
            
            if (synergyManager != null)
            {
                synergyManager.OnSynergyActivated += OnSynergyActivated;
            }
        }
        
        private void UnsubscribeFromEvents()
        {
            if (player != null)
            {
                player.OnHealthChanged -= UpdateHealthBar;
            }
            
            if (xpManager != null)
            {
                xpManager.OnXPChanged -= UpdateXPBar;
                xpManager.OnLevelUp -= UpdateLevel;
            }
            
            if (synergyManager != null)
            {
                synergyManager.OnSynergyActivated -= OnSynergyActivated;
            }
        }
        
        private void UpdateHealthBar(float current, float max)
        {
            if (hpBar != null)
            {
                hpBar.value = current / max;
            }
            
            if (hpText != null)
            {
                hpText.text = $"{Mathf.RoundToInt(current)} / {Mathf.RoundToInt(max)}";
            }
        }
        
        private void UpdateXPBar(int current, int required)
        {
            if (xpBar != null)
            {
                xpBar.value = (float)current / required;
            }
        }
        
        private void UpdateLevel(int level)
        {
            if (levelText != null)
            {
                levelText.text = $"Level {level}";
            }
        }
        
        private void UpdateTimer()
        {
            if (GameManager.Instance != null && timerText != null)
            {
                float time = GameManager.Instance.CurrentGameTime;
                int minutes = Mathf.FloorToInt(time / 60f);
                int seconds = Mathf.FloorToInt(time % 60f);
                timerText.text = $"{minutes:00}:{seconds:00}";
            }
        }
        
        private void OnSynergyActivated(SynergyType synergyType)
        {
            if (synergyIconContainer == null || synergyIconPrefab == null) return;
            if (synergyIcons.ContainsKey(synergyType)) return;
            
            GameObject iconObj = Instantiate(synergyIconPrefab, synergyIconContainer);
            synergyIcons[synergyType] = iconObj;
            
            SynergyDefinition synergy = SynergyDatabase.GetSynergy(synergyType);
            if (synergy != null)
            {
                Text iconText = iconObj.GetComponentInChildren<Text>();
                if (iconText != null)
                {
                    iconText.text = synergy.synergyName;
                }
            }
        }
    }
}
