using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MagicSurvivors.Data;
using MagicSurvivors.Core;

namespace MagicSurvivors.UI
{
    public class HomeScreenUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject characterSelectionPanel;
        [SerializeField] private List<CharacterCard> characterCards = new List<CharacterCard>();
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button metaProgressionButton;
        
        [Header("Character Database")]
        [SerializeField] private List<CharacterData> allCharacters = new List<CharacterData>();
        
        private CharacterClass selectedCharacter = CharacterClass.FireMage;
        
        private void Start()
        {
            SetupCharacterCards();
            
            if (startGameButton != null)
            {
                startGameButton.onClick.AddListener(OnStartGameClicked);
            }
            
            if (metaProgressionButton != null)
            {
                metaProgressionButton.onClick.AddListener(OnMetaProgressionClicked);
            }
        }
        
        private void SetupCharacterCards()
        {
            for (int i = 0; i < characterCards.Count && i < allCharacters.Count; i++)
            {
                characterCards[i].SetCharacterData(allCharacters[i]);
                characterCards[i].OnCardSelected = OnCharacterSelected;
            }
        }
        
        private void OnCharacterSelected(CharacterData characterData)
        {
            selectedCharacter = characterData.characterClass;
            Debug.Log($"HomeScreenUI: Selected character {characterData.characterName}");
        }
        
        private void OnStartGameClicked()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartGame(selectedCharacter);
            }
            
            SceneManager.LoadScene("GameScene");
        }
        
        private void OnMetaProgressionClicked()
        {
            Debug.Log("HomeScreenUI: Opening meta progression menu");
        }
    }
    
    [System.Serializable]
    public class CharacterCard : MonoBehaviour
    {
        [SerializeField] private Image characterImage;
        [SerializeField] private Text characterName;
        [SerializeField] private Text characterDescription;
        [SerializeField] private Button selectButton;
        
        private CharacterData currentCharacterData;
        public System.Action<CharacterData> OnCardSelected;
        
        private void Awake()
        {
            if (selectButton != null)
            {
                selectButton.onClick.AddListener(OnSelectClicked);
            }
        }
        
        public void SetCharacterData(CharacterData data)
        {
            currentCharacterData = data;
            
            if (characterImage != null && data.characterSprite != null)
            {
                characterImage.sprite = data.characterSprite;
            }
            
            if (characterName != null)
            {
                characterName.text = data.characterName;
            }
            
            if (characterDescription != null)
            {
                characterDescription.text = data.description;
            }
        }
        
        private void OnSelectClicked()
        {
            OnCardSelected?.Invoke(currentCharacterData);
        }
    }
}
