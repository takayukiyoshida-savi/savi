using System.Collections.Generic;
using UnityEngine;

namespace MagicSurvivors.Data
{
    [System.Serializable]
    public class EvolutionSkillDefinition
    {
        public SkillEvolutionType evolutionType;
        public string skillName;
        public string skillNameJapanese;
        public string description;
        public SkillType baseSkill;
        public ElementType element;
        public float damageMultiplier;
        public float cooldownMultiplier;
        public string evolutionEffect;
    }

    public static class EvolutionSkillDatabase
    {
        private static Dictionary<SkillEvolutionType, EvolutionSkillDefinition> evolutions;

        public static void Initialize()
        {
            evolutions = new Dictionary<SkillEvolutionType, EvolutionSkillDefinition>
            {
                {
                    SkillEvolutionType.FireLance,
                    new EvolutionSkillDefinition
                    {
                        evolutionType = SkillEvolutionType.FireLance,
                        skillName = "Fire Lance",
                        skillNameJapanese = "ファイアランス",
                        description = "Evolved Firebolt - Pierces through enemies",
                        baseSkill = SkillType.Firebolt,
                        element = ElementType.Fire,
                        damageMultiplier = 1.5f,
                        cooldownMultiplier = 0.8f,
                        evolutionEffect = "Pierces 3 enemies"
                    }
                },
                {
                    SkillEvolutionType.IceNeedle,
                    new EvolutionSkillDefinition
                    {
                        evolutionType = SkillEvolutionType.IceNeedle,
                        skillName = "Ice Needle",
                        skillNameJapanese = "アイスニードル",
                        description = "Evolved Ice Shot - Fires multiple needles",
                        baseSkill = SkillType.IceShot,
                        element = ElementType.Ice,
                        damageMultiplier = 1.3f,
                        cooldownMultiplier = 0.9f,
                        evolutionEffect = "Fires 3 needles in a spread"
                    }
                },
                {
                    SkillEvolutionType.TwinArrow,
                    new EvolutionSkillDefinition
                    {
                        evolutionType = SkillEvolutionType.TwinArrow,
                        skillName = "Twin Arrow",
                        skillNameJapanese = "ツインアロー",
                        description = "Evolved Wind Arrow - Fires two arrows",
                        baseSkill = SkillType.WindArrow,
                        element = ElementType.Wind,
                        damageMultiplier = 1.4f,
                        cooldownMultiplier = 0.85f,
                        evolutionEffect = "Fires 2 arrows simultaneously"
                    }
                },
                {
                    SkillEvolutionType.SuperconductorCore,
                    new EvolutionSkillDefinition
                    {
                        evolutionType = SkillEvolutionType.SuperconductorCore,
                        skillName = "Superconductor Core",
                        skillNameJapanese = "超伝導コア",
                        description = "Evolved Lightning Chain - Chains indefinitely",
                        baseSkill = SkillType.LightningChain,
                        element = ElementType.Thunder,
                        damageMultiplier = 1.6f,
                        cooldownMultiplier = 0.75f,
                        evolutionEffect = "Chains to all nearby enemies"
                    }
                },
                {
                    SkillEvolutionType.PrimeFire,
                    new EvolutionSkillDefinition
                    {
                        evolutionType = SkillEvolutionType.PrimeFire,
                        skillName = "Prime Fire",
                        skillNameJapanese = "プライムファイア",
                        description = "Ultimate Fire enhancement",
                        baseSkill = SkillType.Firebolt,
                        element = ElementType.Fire,
                        damageMultiplier = 2.0f,
                        cooldownMultiplier = 0.7f,
                        evolutionEffect = "All Fire skills deal massive damage"
                    }
                },
                {
                    SkillEvolutionType.PrimeIce,
                    new EvolutionSkillDefinition
                    {
                        evolutionType = SkillEvolutionType.PrimeIce,
                        skillName = "Prime Ice",
                        skillNameJapanese = "プライムアイス",
                        description = "Ultimate Ice enhancement",
                        baseSkill = SkillType.IceShot,
                        element = ElementType.Ice,
                        damageMultiplier = 2.0f,
                        cooldownMultiplier = 0.7f,
                        evolutionEffect = "All Ice skills freeze enemies"
                    }
                },
                {
                    SkillEvolutionType.PrimeThunder,
                    new EvolutionSkillDefinition
                    {
                        evolutionType = SkillEvolutionType.PrimeThunder,
                        skillName = "Prime Thunder",
                        skillNameJapanese = "プライムサンダー",
                        description = "Ultimate Thunder enhancement",
                        baseSkill = SkillType.LightningChain,
                        element = ElementType.Thunder,
                        damageMultiplier = 2.0f,
                        cooldownMultiplier = 0.7f,
                        evolutionEffect = "All Thunder skills chain infinitely"
                    }
                },
                {
                    SkillEvolutionType.PrimeWind,
                    new EvolutionSkillDefinition
                    {
                        evolutionType = SkillEvolutionType.PrimeWind,
                        skillName = "Prime Wind",
                        skillNameJapanese = "プライムウィンド",
                        description = "Ultimate Wind enhancement",
                        baseSkill = SkillType.WindArrow,
                        element = ElementType.Wind,
                        damageMultiplier = 2.0f,
                        cooldownMultiplier = 0.7f,
                        evolutionEffect = "All Wind skills pierce infinitely"
                    }
                },
                {
                    SkillEvolutionType.PrimeLight,
                    new EvolutionSkillDefinition
                    {
                        evolutionType = SkillEvolutionType.PrimeLight,
                        skillName = "Prime Light",
                        skillNameJapanese = "プライムライト",
                        description = "Ultimate Light enhancement",
                        baseSkill = SkillType.HolyShot,
                        element = ElementType.Light,
                        damageMultiplier = 2.0f,
                        cooldownMultiplier = 0.7f,
                        evolutionEffect = "All Light skills heal the player"
                    }
                },
                {
                    SkillEvolutionType.PrimeDark,
                    new EvolutionSkillDefinition
                    {
                        evolutionType = SkillEvolutionType.PrimeDark,
                        skillName = "Prime Dark",
                        skillNameJapanese = "プライムダーク",
                        description = "Ultimate Dark enhancement",
                        baseSkill = SkillType.DarkBolt,
                        element = ElementType.Dark,
                        damageMultiplier = 2.0f,
                        cooldownMultiplier = 0.7f,
                        evolutionEffect = "All Dark skills drain life"
                    }
                }
            };
        }

        public static EvolutionSkillDefinition GetEvolution(SkillEvolutionType evolutionType)
        {
            if (evolutions == null)
            {
                Initialize();
            }
            
            if (evolutions.TryGetValue(evolutionType, out EvolutionSkillDefinition evolution))
            {
                return evolution;
            }
            
            return null;
        }

        public static List<EvolutionSkillDefinition> GetAllEvolutions()
        {
            if (evolutions == null)
            {
                Initialize();
            }
            
            return new List<EvolutionSkillDefinition>(evolutions.Values);
        }

        public static List<EvolutionSkillDefinition> GetEvolutionsForSkill(SkillType skillType)
        {
            if (evolutions == null)
            {
                Initialize();
            }
            
            List<EvolutionSkillDefinition> result = new List<EvolutionSkillDefinition>();
            foreach (var evolution in evolutions.Values)
            {
                if (evolution.baseSkill == skillType)
                {
                    result.Add(evolution);
                }
            }
            return result;
        }

        public static List<EvolutionSkillDefinition> GetEvolutionsByElement(ElementType element)
        {
            if (evolutions == null)
            {
                Initialize();
            }
            
            List<EvolutionSkillDefinition> result = new List<EvolutionSkillDefinition>();
            foreach (var evolution in evolutions.Values)
            {
                if (evolution.element == element)
                {
                    result.Add(evolution);
                }
            }
            return result;
        }
    }
}
