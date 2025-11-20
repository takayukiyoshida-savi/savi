using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MagicSurvivors.Data;
using MagicSurvivors.Skills;
using MagicSurvivors.XP;
using MagicSurvivors.Core;

namespace MagicSurvivors.UI
{
    public class LevelUpUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject levelUpPanel;
        [SerializeField] private List<SkillCard> skillCards = new List<SkillCard>();
        
        [Header("Skill Database")]
        [SerializeField] private List<SkillData> allSkills = new List<SkillData>();
        
        private SkillManager skillManager;
        private XPManager xpManager;
        
        private void Start()
        {
            skillManager = FindObjectOfType<SkillManager>();
            xpManager = FindObjectOfType<XPManager>();
            
            if (xpManager != null)
            {
                xpManager.OnLevelUp += ShowLevelUpOptions;
            }
            
            if (levelUpPanel != null)
            {
                levelUpPanel.SetActive(false);
            }
        }
        
        private void OnDestroy()
        {
            if (xpManager != null)
            {
                xpManager.OnLevelUp -= ShowLevelUpOptions;
            }
        }
        
        private void ShowLevelUpOptions(int level)
        {
            if (levelUpPanel != null)
            {
                levelUpPanel.SetActive(true);
                GameManager.Instance?.PauseGame();
                
                List<SkillData> options = GenerateSkillOptions();
                DisplaySkillOptions(options);
            }
        }
        
        private List<SkillData> GenerateSkillOptions()
        {
            List<SkillData> options = new List<SkillData>();
            List<SkillData> availableSkills = new List<SkillData>(allSkills);
            
            for (int i = 0; i < GameConstants.LEVEL_UP_CARD_COUNT && availableSkills.Count > 0; i++)
            {
                int randomIndex = Random.Range(0, availableSkills.Count);
                options.Add(availableSkills[randomIndex]);
                availableSkills.RemoveAt(randomIndex);
            }
            
            return options;
        }
        
        private void DisplaySkillOptions(List<SkillData> options)
        {
            for (int i = 0; i < skillCards.Count && i < options.Count; i++)
            {
                skillCards[i].SetSkillData(options[i]);
                skillCards[i].OnCardSelected = OnSkillSelected;
                skillCards[i].gameObject.SetActive(true);
            }
            
            for (int i = options.Count; i < skillCards.Count; i++)
            {
                skillCards[i].gameObject.SetActive(false);
            }
        }
        
        private void OnSkillSelected(SkillData skillData)
        {
            if (skillManager != null)
            {
                skillManager.AcquireSkill(skillData.skillType);
            }
            
            HideLevelUpPanel();
        }
        
        private void HideLevelUpPanel()
        {
            if (levelUpPanel != null)
            {
                levelUpPanel.SetActive(false);
                GameManager.Instance?.ResumeGame();
            }
        }
    }
    
    [System.Serializable]
    public class SkillCard : MonoBehaviour
    {
        [SerializeField] private Image skillIcon;
        [SerializeField] private Text skillName;
        [SerializeField] private Text skillDescription;
        [SerializeField] private Button selectButton;
        
        private SkillData currentSkillData;
        public System.Action<SkillData> OnCardSelected;
        
        private void Awake()
        {
            if (selectButton != null)
            {
                selectButton.onClick.AddListener(OnSelectClicked);
            }
        }
        
        public void SetSkillData(SkillData data)
        {
            currentSkillData = data;
            
            if (skillIcon != null && data.skillIcon != null)
            {
                skillIcon.sprite = data.skillIcon;
            }
            
            if (skillName != null)
            {
                skillName.text = data.skillName;
            }
            
            if (skillDescription != null)
            {
                skillDescription.text = data.description;
            }
        }
        
        private void OnSelectClicked()
        {
            OnCardSelected?.Invoke(currentSkillData);
        }
    }
}
