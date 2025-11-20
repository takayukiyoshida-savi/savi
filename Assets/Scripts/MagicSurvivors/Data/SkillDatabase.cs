using System.Collections.Generic;
using UnityEngine;

namespace MagicSurvivors.Data
{
    [System.Serializable]
    public class SkillDefinition
    {
        public SkillType skillType;
        public string skillName;
        public string skillNameJapanese;
        public ElementType element;
        public string description;
        public float baseDamage;
        public float cooldown;
        public float projectileSpeed;
        public float range;
        public int projectileCount;
        public float areaRadius;
        public int pierceCount;
        public float duration;
        public int maxLevel;
        public float damagePerLevel;
        public float cooldownReductionPerLevel;
    }

    public static class SkillDatabase
    {
        private static Dictionary<SkillType, SkillDefinition> skills;

        public static void Initialize()
        {
            skills = new Dictionary<SkillType, SkillDefinition>
            {
                {
                    SkillType.Firebolt,
                    new SkillDefinition
                    {
                        skillType = SkillType.Firebolt,
                        skillName = "Firebolt",
                        skillNameJapanese = "ファイアボルト",
                        element = ElementType.Fire,
                        description = "Shoots a fireball at the nearest enemy",
                        baseDamage = 15f,
                        cooldown = 1.5f,
                        projectileSpeed = 12f,
                        range = 15f,
                        projectileCount = 1,
                        areaRadius = 0f,
                        pierceCount = 0,
                        duration = 0f,
                        maxLevel = 5,
                        damagePerLevel = 3f,
                        cooldownReductionPerLevel = 0.05f
                    }
                },
                {
                    SkillType.FlameNova,
                    new SkillDefinition
                    {
                        skillType = SkillType.FlameNova,
                        skillName = "Flame Nova",
                        skillNameJapanese = "フレイムノヴァ",
                        element = ElementType.Fire,
                        description = "Creates a ring of fire around the player",
                        baseDamage = 25f,
                        cooldown = 3f,
                        projectileSpeed = 0f,
                        range = 5f,
                        projectileCount = 8,
                        areaRadius = 5f,
                        pierceCount = 0,
                        duration = 0.5f,
                        maxLevel = 5,
                        damagePerLevel = 5f,
                        cooldownReductionPerLevel = 0.1f
                    }
                },
                {
                    SkillType.IceShot,
                    new SkillDefinition
                    {
                        skillType = SkillType.IceShot,
                        skillName = "Ice Shot",
                        skillNameJapanese = "アイスショット",
                        element = ElementType.Ice,
                        description = "Fires ice projectiles that slow enemies",
                        baseDamage = 12f,
                        cooldown = 1.2f,
                        projectileSpeed = 10f,
                        range = 12f,
                        projectileCount = 1,
                        areaRadius = 0f,
                        pierceCount = 1,
                        duration = 0f,
                        maxLevel = 5,
                        damagePerLevel = 2.5f,
                        cooldownReductionPerLevel = 0.05f
                    }
                },
                {
                    SkillType.FrostArea,
                    new SkillDefinition
                    {
                        skillType = SkillType.FrostArea,
                        skillName = "Frost Area",
                        skillNameJapanese = "フロストエリア",
                        element = ElementType.Ice,
                        description = "Creates a freezing zone that damages enemies",
                        baseDamage = 8f,
                        cooldown = 2f,
                        projectileSpeed = 0f,
                        range = 6f,
                        projectileCount = 1,
                        areaRadius = 6f,
                        pierceCount = 0,
                        duration = 3f,
                        maxLevel = 5,
                        damagePerLevel = 2f,
                        cooldownReductionPerLevel = 0.08f
                    }
                },
                {
                    SkillType.LightningChain,
                    new SkillDefinition
                    {
                        skillType = SkillType.LightningChain,
                        skillName = "Lightning Chain",
                        skillNameJapanese = "ライトニングチェイン",
                        element = ElementType.Thunder,
                        description = "Lightning that chains between enemies",
                        baseDamage = 20f,
                        cooldown = 2.5f,
                        projectileSpeed = 20f,
                        range = 10f,
                        projectileCount = 1,
                        areaRadius = 0f,
                        pierceCount = 3,
                        duration = 0f,
                        maxLevel = 5,
                        damagePerLevel = 4f,
                        cooldownReductionPerLevel = 0.1f
                    }
                },
                {
                    SkillType.ThunderSpear,
                    new SkillDefinition
                    {
                        skillType = SkillType.ThunderSpear,
                        skillName = "Thunder Spear",
                        skillNameJapanese = "サンダースピア",
                        element = ElementType.Thunder,
                        description = "Powerful lightning spear that pierces enemies",
                        baseDamage = 35f,
                        cooldown = 4f,
                        projectileSpeed = 15f,
                        range = 20f,
                        projectileCount = 1,
                        areaRadius = 0f,
                        pierceCount = 5,
                        duration = 0f,
                        maxLevel = 5,
                        damagePerLevel = 7f,
                        cooldownReductionPerLevel = 0.15f
                    }
                },
                {
                    SkillType.WindArrow,
                    new SkillDefinition
                    {
                        skillType = SkillType.WindArrow,
                        skillName = "Wind Arrow",
                        skillNameJapanese = "ウィンドアロー",
                        element = ElementType.Wind,
                        description = "Fast wind arrows that pierce enemies",
                        baseDamage = 10f,
                        cooldown = 0.8f,
                        projectileSpeed = 18f,
                        range = 15f,
                        projectileCount = 1,
                        areaRadius = 0f,
                        pierceCount = 2,
                        duration = 0f,
                        maxLevel = 5,
                        damagePerLevel = 2f,
                        cooldownReductionPerLevel = 0.04f
                    }
                },
                {
                    SkillType.Tornado,
                    new SkillDefinition
                    {
                        skillType = SkillType.Tornado,
                        skillName = "Tornado",
                        skillNameJapanese = "トルネード",
                        element = ElementType.Wind,
                        description = "Creates a tornado that pulls in enemies",
                        baseDamage = 18f,
                        cooldown = 3.5f,
                        projectileSpeed = 5f,
                        range = 8f,
                        projectileCount = 1,
                        areaRadius = 4f,
                        pierceCount = 0,
                        duration = 4f,
                        maxLevel = 5,
                        damagePerLevel = 4f,
                        cooldownReductionPerLevel = 0.12f
                    }
                },
                {
                    SkillType.HolyShot,
                    new SkillDefinition
                    {
                        skillType = SkillType.HolyShot,
                        skillName = "Holy Shot",
                        skillNameJapanese = "ホーリーショット",
                        element = ElementType.Light,
                        description = "Holy projectile that seeks enemies",
                        baseDamage = 14f,
                        cooldown = 1.3f,
                        projectileSpeed = 11f,
                        range = 14f,
                        projectileCount = 1,
                        areaRadius = 0f,
                        pierceCount = 0,
                        duration = 0f,
                        maxLevel = 5,
                        damagePerLevel = 3f,
                        cooldownReductionPerLevel = 0.05f
                    }
                },
                {
                    SkillType.HolyShield,
                    new SkillDefinition
                    {
                        skillType = SkillType.HolyShield,
                        skillName = "Holy Shield",
                        skillNameJapanese = "ホーリーシールド",
                        element = ElementType.Light,
                        description = "Creates a protective shield that damages enemies",
                        baseDamage = 22f,
                        cooldown = 2.8f,
                        projectileSpeed = 0f,
                        range = 3f,
                        projectileCount = 1,
                        areaRadius = 3f,
                        pierceCount = 0,
                        duration = 5f,
                        maxLevel = 5,
                        damagePerLevel = 4.5f,
                        cooldownReductionPerLevel = 0.1f
                    }
                },
                {
                    SkillType.DarkBolt,
                    new SkillDefinition
                    {
                        skillType = SkillType.DarkBolt,
                        skillName = "Dark Bolt",
                        skillNameJapanese = "ダークボルト",
                        element = ElementType.Dark,
                        description = "Dark projectile that deals high damage",
                        baseDamage = 18f,
                        cooldown = 1.8f,
                        projectileSpeed = 13f,
                        range = 16f,
                        projectileCount = 1,
                        areaRadius = 0f,
                        pierceCount = 0,
                        duration = 0f,
                        maxLevel = 5,
                        damagePerLevel = 3.5f,
                        cooldownReductionPerLevel = 0.07f
                    }
                },
                {
                    SkillType.LifeDrain,
                    new SkillDefinition
                    {
                        skillType = SkillType.LifeDrain,
                        skillName = "Life Drain",
                        skillNameJapanese = "ライフドレイン",
                        element = ElementType.Dark,
                        description = "Drains life from enemies in an area",
                        baseDamage = 12f,
                        cooldown = 2.2f,
                        projectileSpeed = 0f,
                        range = 7f,
                        projectileCount = 1,
                        areaRadius = 7f,
                        pierceCount = 0,
                        duration = 2f,
                        maxLevel = 5,
                        damagePerLevel = 2.5f,
                        cooldownReductionPerLevel = 0.08f
                    }
                }
            };
        }

        public static SkillDefinition GetSkill(SkillType skillType)
        {
            if (skills == null)
            {
                Initialize();
            }
            
            if (skills.TryGetValue(skillType, out SkillDefinition skill))
            {
                return skill;
            }
            
            return null;
        }

        public static List<SkillDefinition> GetAllSkills()
        {
            if (skills == null)
            {
                Initialize();
            }
            
            return new List<SkillDefinition>(skills.Values);
        }

        public static List<SkillDefinition> GetSkillsByElement(ElementType element)
        {
            if (skills == null)
            {
                Initialize();
            }
            
            List<SkillDefinition> result = new List<SkillDefinition>();
            foreach (var skill in skills.Values)
            {
                if (skill.element == element)
                {
                    result.Add(skill);
                }
            }
            return result;
        }
    }
}
