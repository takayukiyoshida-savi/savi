using System.Collections.Generic;
using UnityEngine;
using MagicSurvivors.Data;
using MagicSurvivors.Skills;

namespace MagicSurvivors.Synergies
{
    public class SynergyManager : MonoBehaviour
    {
        [Header("Synergy Database")]
        [SerializeField] private List<SynergyData> allSynergies = new List<SynergyData>();
        
        private List<SynergyType> activeSynergies = new List<SynergyType>();
        private SkillManager skillManager;
        
        public delegate void SynergyEvent(SynergyType synergyType);
        public event SynergyEvent OnSynergyActivated;
        
        private void Start()
        {
            skillManager = GetComponent<SkillManager>();
            if (skillManager != null)
            {
                skillManager.OnSkillAcquired += CheckForNewSynergies;
            }
        }
        
        private void OnDestroy()
        {
            if (skillManager != null)
            {
                skillManager.OnSkillAcquired -= CheckForNewSynergies;
            }
        }
        
        private void CheckForNewSynergies(SkillType skillType)
        {
            List<ElementType> activeElements = skillManager.GetActiveElements();
            
            foreach (SynergyData synergy in allSynergies)
            {
                if (!activeSynergies.Contains(synergy.synergyType))
                {
                    if (activeElements.Contains(synergy.requirement.element1) &&
                        activeElements.Contains(synergy.requirement.element2))
                    {
                        ActivateSynergy(synergy);
                    }
                }
            }
        }
        
        private void ActivateSynergy(SynergyData synergy)
        {
            activeSynergies.Add(synergy.synergyType);
            OnSynergyActivated?.Invoke(synergy.synergyType);
            Debug.Log($"SynergyManager: Activated synergy {synergy.synergyName}");
        }
        
        public bool HasSynergy(SynergyType synergyType)
        {
            return activeSynergies.Contains(synergyType);
        }
        
        public float GetDamageMultiplier()
        {
            float multiplier = 1f;
            foreach (SynergyType synergyType in activeSynergies)
            {
                SynergyData data = allSynergies.Find(s => s.synergyType == synergyType);
                if (data != null)
                {
                    multiplier *= data.damageMultiplier;
                }
            }
            return multiplier;
        }
    }
}
