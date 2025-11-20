using System.Collections.Generic;
using UnityEngine;

namespace MagicSurvivors.Data
{
    [System.Serializable]
    public class SynergyDefinition
    {
        public SynergyType synergyType;
        public string synergyName;
        public string synergyNameJapanese;
        public string description;
        public ElementType element1;
        public ElementType element2;
        public float damageMultiplier;
        public float effectCooldown;
        public string effectDescription;
    }

    public static class SynergyDatabase
    {
        private static Dictionary<SynergyType, SynergyDefinition> synergies;

        public static void Initialize()
        {
            synergies = new Dictionary<SynergyType, SynergyDefinition>
            {
                {
                    SynergyType.FireTornado,
                    new SynergyDefinition
                    {
                        synergyType = SynergyType.FireTornado,
                        synergyName = "Fire Tornado",
                        synergyNameJapanese = "炎の竜巻",
                        description = "Fire and Wind combine to create burning tornadoes",
                        element1 = ElementType.Fire,
                        element2 = ElementType.Wind,
                        damageMultiplier = 1.5f,
                        effectCooldown = 5f,
                        effectDescription = "Tornado skills leave burning trails"
                    }
                },
                {
                    SynergyType.SteamExplosion,
                    new SynergyDefinition
                    {
                        synergyType = SynergyType.SteamExplosion,
                        synergyName = "Steam Explosion",
                        synergyNameJapanese = "蒸気爆発",
                        description = "Fire and Ice create explosive steam",
                        element1 = ElementType.Fire,
                        element2 = ElementType.Ice,
                        damageMultiplier = 1.6f,
                        effectCooldown = 6f,
                        effectDescription = "Ice skills explode on impact"
                    }
                },
                {
                    SynergyType.ExplosiveThunder,
                    new SynergyDefinition
                    {
                        synergyType = SynergyType.ExplosiveThunder,
                        synergyName = "Explosive Thunder",
                        synergyNameJapanese = "爆雷",
                        description = "Fire and Thunder create explosive lightning",
                        element1 = ElementType.Fire,
                        element2 = ElementType.Thunder,
                        damageMultiplier = 1.7f,
                        effectCooldown = 7f,
                        effectDescription = "Thunder skills cause explosions"
                    }
                },
                {
                    SynergyType.NuclearExplosion,
                    new SynergyDefinition
                    {
                        synergyType = SynergyType.NuclearExplosion,
                        synergyName = "Nuclear Explosion",
                        synergyNameJapanese = "核爆発",
                        description = "Light and Dark create devastating explosions",
                        element1 = ElementType.Light,
                        element2 = ElementType.Dark,
                        damageMultiplier = 2.0f,
                        effectCooldown = 10f,
                        effectDescription = "Massive area damage periodically"
                    }
                },
                {
                    SynergyType.Superconductor,
                    new SynergyDefinition
                    {
                        synergyType = SynergyType.Superconductor,
                        synergyName = "Superconductor",
                        synergyNameJapanese = "超伝導",
                        description = "Ice and Thunder create perfect conductivity",
                        element1 = ElementType.Ice,
                        element2 = ElementType.Thunder,
                        damageMultiplier = 1.8f,
                        effectCooldown = 8f,
                        effectDescription = "Lightning chains infinitely through frozen enemies"
                    }
                },
                {
                    SynergyType.Storm,
                    new SynergyDefinition
                    {
                        synergyType = SynergyType.Storm,
                        synergyName = "Storm",
                        synergyNameJapanese = "嵐",
                        description = "Wind and Thunder create powerful storms",
                        element1 = ElementType.Wind,
                        element2 = ElementType.Thunder,
                        damageMultiplier = 1.6f,
                        effectCooldown = 6f,
                        effectDescription = "Wind skills call down lightning strikes"
                    }
                },
                {
                    SynergyType.Blizzard,
                    new SynergyDefinition
                    {
                        synergyType = SynergyType.Blizzard,
                        synergyName = "Blizzard",
                        synergyNameJapanese = "吹雪",
                        description = "Ice and Wind create freezing blizzards",
                        element1 = ElementType.Ice,
                        element2 = ElementType.Wind,
                        damageMultiplier = 1.5f,
                        effectCooldown = 5f,
                        effectDescription = "Wind skills freeze enemies"
                    }
                },
                {
                    SynergyType.SolarFlare,
                    new SynergyDefinition
                    {
                        synergyType = SynergyType.SolarFlare,
                        synergyName = "Solar Flare",
                        synergyNameJapanese = "太陽フレア",
                        description = "Fire and Light create intense solar energy",
                        element1 = ElementType.Fire,
                        element2 = ElementType.Light,
                        damageMultiplier = 1.7f,
                        effectCooldown = 7f,
                        effectDescription = "Light skills burn enemies over time"
                    }
                },
                {
                    SynergyType.FrozenCurse,
                    new SynergyDefinition
                    {
                        synergyType = SynergyType.FrozenCurse,
                        synergyName = "Frozen Curse",
                        synergyNameJapanese = "凍結の呪い",
                        description = "Ice and Dark create cursed ice",
                        element1 = ElementType.Ice,
                        element2 = ElementType.Dark,
                        damageMultiplier = 1.6f,
                        effectCooldown = 6f,
                        effectDescription = "Dark skills freeze and curse enemies"
                    }
                },
                {
                    SynergyType.DivineWind,
                    new SynergyDefinition
                    {
                        synergyType = SynergyType.DivineWind,
                        synergyName = "Divine Wind",
                        synergyNameJapanese = "神風",
                        description = "Wind and Light create holy gales",
                        element1 = ElementType.Wind,
                        element2 = ElementType.Light,
                        damageMultiplier = 1.5f,
                        effectCooldown = 5f,
                        effectDescription = "Wind skills heal the player"
                    }
                }
            };
        }

        public static SynergyDefinition GetSynergy(SynergyType synergyType)
        {
            if (synergies == null)
            {
                Initialize();
            }
            
            if (synergies.TryGetValue(synergyType, out SynergyDefinition synergy))
            {
                return synergy;
            }
            
            return null;
        }

        public static List<SynergyDefinition> GetAllSynergies()
        {
            if (synergies == null)
            {
                Initialize();
            }
            
            return new List<SynergyDefinition>(synergies.Values);
        }

        public static SynergyType? CheckSynergyActivation(ElementType element1, ElementType element2)
        {
            if (synergies == null)
            {
                Initialize();
            }
            
            foreach (var synergy in synergies.Values)
            {
                if ((synergy.element1 == element1 && synergy.element2 == element2) ||
                    (synergy.element1 == element2 && synergy.element2 == element1))
                {
                    return synergy.synergyType;
                }
            }
            
            return null;
        }
    }
}
