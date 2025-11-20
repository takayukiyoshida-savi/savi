using UnityEngine;

namespace MagicSurvivors.Data
{
    [System.Serializable]
    public class SynergyRequirement
    {
        public ElementType element1;
        public ElementType element2;
    }
    
    [CreateAssetMenu(fileName = "SynergyData", menuName = "MagicSurvivors/SynergyData")]
    public class SynergyData : ScriptableObject
    {
        public SynergyType synergyType;
        public string synergyName;
        public Sprite synergyIcon;
        
        public SynergyRequirement requirement;
        
        [TextArea(2, 4)]
        public string description;
        
        public float damageMultiplier = 1.5f;
        public GameObject effectPrefab;
        public float effectCooldown = 5f;
    }
}
