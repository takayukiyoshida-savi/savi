using System.Collections.Generic;
using UnityEngine;
using MagicSurvivors.Data;
using MagicSurvivors.Skills;

namespace MagicSurvivors.Synergies
{
    public class SynergyManager : MonoBehaviour
    {
        private List<SynergyType> activeSynergies = new List<SynergyType>();
        private Dictionary<ElementType, int> elementLevels = new Dictionary<ElementType, int>();
        private SkillManager skillManager;
        
        public delegate void SynergyEvent(SynergyType synergyType);
        public event SynergyEvent OnSynergyActivated;
        
        private void Awake()
        {
            SynergyDatabase.Initialize();
        }
        
        private void Start()
        {
            skillManager = GetComponent<SkillManager>();
            if (skillManager != null)
            {
                skillManager.OnSkillAcquired += OnSkillAcquired;
                skillManager.OnSkillLevelUp += OnSkillLevelUp;
            }
        }
        
        private void OnDestroy()
        {
            if (skillManager != null)
            {
                skillManager.OnSkillAcquired -= OnSkillAcquired;
                skillManager.OnSkillLevelUp -= OnSkillLevelUp;
            }
        }
        
        private void OnSkillAcquired(SkillType skillType)
        {
            UpdateElementLevels();
            CheckForNewSynergies();
        }
        
        private void OnSkillLevelUp(SkillType skillType)
        {
            UpdateElementLevels();
            CheckForNewSynergies();
        }
        
        private void UpdateElementLevels()
        {
            elementLevels.Clear();
            
            List<ElementType> activeElements = skillManager.GetActiveElements();
            foreach (ElementType element in activeElements)
            {
                int totalLevel = skillManager.GetTotalLevelForElement(element);
                elementLevels[element] = totalLevel;
            }
        }
        
        private void CheckForNewSynergies()
        {
            List<SynergyDefinition> allSynergies = SynergyDatabase.GetAllSynergies();
            
            foreach (SynergyDefinition synergy in allSynergies)
            {
                if (!activeSynergies.Contains(synergy.synergyType))
                {
                    int level1 = elementLevels.ContainsKey(synergy.element1) ? elementLevels[synergy.element1] : 0;
                    int level2 = elementLevels.ContainsKey(synergy.element2) ? elementLevels[synergy.element2] : 0;
                    
                    if (level1 >= GameConstants.SYNERGY_REQUIRED_LEVEL && 
                        level2 >= GameConstants.SYNERGY_REQUIRED_LEVEL)
                    {
                        ActivateSynergy(synergy);
                    }
                }
            }
        }
        
        private void ActivateSynergy(SynergyDefinition synergy)
        {
            activeSynergies.Add(synergy.synergyType);
            OnSynergyActivated?.Invoke(synergy.synergyType);
            Debug.Log($"SynergyManager: Activated synergy {synergy.synergyName} ({synergy.synergyNameJapanese})");
        }
        
        public bool HasSynergy(SynergyType synergyType)
        {
            return activeSynergies.Contains(synergyType);
        }
        
        public List<SynergyType> GetActiveSynergies()
        {
            return new List<SynergyType>(activeSynergies);
        }
        
        public float GetDamageMultiplier()
        {
            float multiplier = 1f;
            foreach (SynergyType synergyType in activeSynergies)
            {
                SynergyDefinition synergy = SynergyDatabase.GetSynergy(synergyType);
                if (synergy != null)
                {
                    multiplier *= synergy.damageMultiplier;
                }
            }
            return multiplier;
        }
    }
}
