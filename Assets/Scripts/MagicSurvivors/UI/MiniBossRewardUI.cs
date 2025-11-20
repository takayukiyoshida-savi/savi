using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MagicSurvivors.Data;
using MagicSurvivors.Core;

namespace MagicSurvivors.UI
{
    [System.Serializable]
    public class SkillEvolutionData
    {
        public SkillEvolutionType evolutionType;
        public string evolutionName;
        public string description;
        public Sprite icon;
        public SkillType requiredSkill;
    }
    
    public class MiniBossRewardUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject rewardPanel;
        [SerializeField] private List<EvolutionCard> evolutionCards = new List<EvolutionCard>();
        
        [Header("Evolution Database")]
        [SerializeField] private List<SkillEvolutionData> allEvolutions = new List<SkillEvolutionData>();
        
        private void Start()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnMiniBoss1Time += ShowMiniBossReward;
                GameManager.Instance.OnMiniBoss2Time += ShowMiniBossReward;
                GameManager.Instance.OnMiniBoss3Time += ShowMiniBossReward;
            }
            
            if (rewardPanel != null)
            {
                rewardPanel.SetActive(false);
            }
        }
        
        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnMiniBoss1Time -= ShowMiniBossReward;
                GameManager.Instance.OnMiniBoss2Time -= ShowMiniBossReward;
                GameManager.Instance.OnMiniBoss3Time -= ShowMiniBossReward;
            }
        }
        
        private void ShowMiniBossReward(float time)
        {
            if (rewardPanel != null)
            {
                rewardPanel.SetActive(true);
                GameManager.Instance?.PauseGame();
                
                List<SkillEvolutionData> options = GenerateEvolutionOptions();
                DisplayEvolutionOptions(options);
            }
        }
        
        private List<SkillEvolutionData> GenerateEvolutionOptions()
        {
            List<SkillEvolutionData> options = new List<SkillEvolutionData>();
            List<SkillEvolutionData> availableEvolutions = new List<SkillEvolutionData>(allEvolutions);
            
            int optionCount = Mathf.Min(3, availableEvolutions.Count);
            for (int i = 0; i < optionCount; i++)
            {
                int randomIndex = Random.Range(0, availableEvolutions.Count);
                options.Add(availableEvolutions[randomIndex]);
                availableEvolutions.RemoveAt(randomIndex);
            }
            
            return options;
        }
        
        private void DisplayEvolutionOptions(List<SkillEvolutionData> options)
        {
            for (int i = 0; i < evolutionCards.Count && i < options.Count; i++)
            {
                evolutionCards[i].SetEvolutionData(options[i]);
                evolutionCards[i].OnCardSelected = OnEvolutionSelected;
                evolutionCards[i].gameObject.SetActive(true);
            }
            
            for (int i = options.Count; i < evolutionCards.Count; i++)
            {
                evolutionCards[i].gameObject.SetActive(false);
            }
        }
        
        private void OnEvolutionSelected(SkillEvolutionData evolutionData)
        {
            Debug.Log($"MiniBossRewardUI: Selected evolution {evolutionData.evolutionName}");
            HideRewardPanel();
        }
        
        private void HideRewardPanel()
        {
            if (rewardPanel != null)
            {
                rewardPanel.SetActive(false);
                GameManager.Instance?.ResumeGame();
            }
        }
    }
    
    [System.Serializable]
    public class EvolutionCard : MonoBehaviour
    {
        [SerializeField] private Image evolutionIcon;
        [SerializeField] private Text evolutionName;
        [SerializeField] private Text evolutionDescription;
        [SerializeField] private Button selectButton;
        
        private SkillEvolutionData currentEvolutionData;
        public System.Action<SkillEvolutionData> OnCardSelected;
        
        private void Awake()
        {
            if (selectButton != null)
            {
                selectButton.onClick.AddListener(OnSelectClicked);
            }
        }
        
        public void SetEvolutionData(SkillEvolutionData data)
        {
            currentEvolutionData = data;
            
            if (evolutionIcon != null && data.icon != null)
            {
                evolutionIcon.sprite = data.icon;
            }
            
            if (evolutionName != null)
            {
                evolutionName.text = data.evolutionName;
            }
            
            if (evolutionDescription != null)
            {
                evolutionDescription.text = data.description;
            }
        }
        
        private void OnSelectClicked()
        {
            OnCardSelected?.Invoke(currentEvolutionData);
        }
    }
}
