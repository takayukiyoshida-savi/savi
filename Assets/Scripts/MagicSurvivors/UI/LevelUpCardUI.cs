using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MagicSurvivors.Data;
using MagicSurvivors.Skills;
using MagicSurvivors.XP;

namespace MagicSurvivors.UI
{
    public class LevelUpCardUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject cardPanel;
        [SerializeField] private List<GameObject> cardObjects = new List<GameObject>();
        [SerializeField] private List<Button> cardButtons = new List<Button>();
        [SerializeField] private List<TextMeshProUGUI> cardTitles = new List<TextMeshProUGUI>();
        [SerializeField] private List<TextMeshProUGUI> cardDescriptions = new List<TextMeshProUGUI>();
        [SerializeField] private List<Image> cardIcons = new List<Image>();
        
        private SkillManager skillManager;
        private XPManager xpManager;
        private List<SkillType> currentOptions = new List<SkillType>();
        private bool isShowing = false;
        
        private void Start()
        {
            skillManager = FindObjectOfType<SkillManager>();
            xpManager = FindObjectOfType<XPManager>();
            
            if (xpManager != null)
            {
                xpManager.OnLevelUp += OnPlayerLevelUp;
            }
            
            if (cardPanel != null)
            {
                cardPanel.SetActive(false);
            }
            
            for (int i = 0; i < cardButtons.Count; i++)
            {
                int index = i;
                if (cardButtons[i] != null)
                {
                    cardButtons[i].onClick.AddListener(() => OnCardSelected(index));
                }
            }
        }
        
        private void OnDestroy()
        {
            if (xpManager != null)
            {
                xpManager.OnLevelUp -= OnPlayerLevelUp;
            }
        }
        
        private void OnPlayerLevelUp(int level)
        {
            ShowLevelUpCards();
        }
        
        private void ShowLevelUpCards()
        {
            if (isShowing) return;
            
            isShowing = true;
            Time.timeScale = 0f;
            
            currentOptions = GenerateSkillOptions();
            
            for (int i = 0; i < cardObjects.Count && i < currentOptions.Count; i++)
            {
                if (cardObjects[i] != null)
                {
                    cardObjects[i].SetActive(true);
                    UpdateCardDisplay(i, currentOptions[i]);
                }
            }
            
            for (int i = currentOptions.Count; i < cardObjects.Count; i++)
            {
                if (cardObjects[i] != null)
                {
                    cardObjects[i].SetActive(false);
                }
            }
            
            if (cardPanel != null)
            {
                cardPanel.SetActive(true);
            }
        }
        
        private List<SkillType> GenerateSkillOptions()
        {
            List<SkillType> options = new List<SkillType>();
            List<SkillDefinition> allSkills = SkillDatabase.GetAllSkills();
            List<SkillDefinition> availableSkills = new List<SkillDefinition>();
            
            foreach (SkillDefinition skill in allSkills)
            {
                if (!skillManager.HasSkill(skill.skillType))
                {
                    availableSkills.Add(skill);
                }
                else
                {
                    availableSkills.Add(skill);
                }
            }
            
            int cardCount = Mathf.Min(GameConstants.LEVEL_UP_CARD_COUNT, availableSkills.Count);
            
            for (int i = 0; i < cardCount; i++)
            {
                if (availableSkills.Count > 0)
                {
                    int randomIndex = Random.Range(0, availableSkills.Count);
                    options.Add(availableSkills[randomIndex].skillType);
                    availableSkills.RemoveAt(randomIndex);
                }
            }
            
            return options;
        }
        
        private void UpdateCardDisplay(int cardIndex, SkillType skillType)
        {
            SkillDefinition skill = SkillDatabase.GetSkill(skillType);
            if (skill == null) return;
            
            bool hasSkill = skillManager.HasSkill(skillType);
            
            if (cardTitles[cardIndex] != null)
            {
                if (hasSkill)
                {
                    cardTitles[cardIndex].text = $"{skill.skillName} Lv Up";
                }
                else
                {
                    cardTitles[cardIndex].text = skill.skillName;
                }
            }
            
            if (cardDescriptions[cardIndex] != null)
            {
                string description = skill.description;
                if (hasSkill)
                {
                    description += $"\n\nDamage: +{skill.damagePerLevel}";
                    description += $"\nCooldown: -{(skill.cooldownReductionPerLevel * 100):F0}%";
                }
                else
                {
                    description += $"\n\nBase Damage: {skill.baseDamage}";
                    description += $"\nCooldown: {skill.cooldown}s";
                }
                cardDescriptions[cardIndex].text = description;
            }
        }
        
        private void OnCardSelected(int cardIndex)
        {
            if (cardIndex < 0 || cardIndex >= currentOptions.Count) return;
            
            SkillType selectedSkill = currentOptions[cardIndex];
            
            if (skillManager != null)
            {
                skillManager.AcquireSkill(selectedSkill);
            }
            
            HideCards();
        }
        
        private void HideCards()
        {
            if (cardPanel != null)
            {
                cardPanel.SetActive(false);
            }
            
            Time.timeScale = 1f;
            isShowing = false;
            currentOptions.Clear();
        }
    }
}
