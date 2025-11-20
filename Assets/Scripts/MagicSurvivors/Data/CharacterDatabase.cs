using System.Collections.Generic;
using UnityEngine;

namespace MagicSurvivors.Data
{
    [System.Serializable]
    public class CharacterDefinition
    {
        public CharacterClass characterClass;
        public string characterName;
        public string characterNameJapanese;
        public string description;
        public float maxHP;
        public float attackPower;
        public float moveSpeed;
        public float pickupRange;
        public SkillType startingSkill;
        public ElementType primaryElement;
        public string passiveDescription;
    }

    public static class CharacterDatabase
    {
        private static Dictionary<CharacterClass, CharacterDefinition> characters;

        public static void Initialize()
        {
            characters = new Dictionary<CharacterClass, CharacterDefinition>
            {
                {
                    CharacterClass.FireMage,
                    new CharacterDefinition
                    {
                        characterClass = CharacterClass.FireMage,
                        characterName = "Fire Mage",
                        characterNameJapanese = "炎術師",
                        description = "A mage who wields the power of fire. High damage output.",
                        maxHP = 100f,
                        attackPower = 10f,
                        moveSpeed = 4.5f,
                        pickupRange = 3f,
                        startingSkill = SkillType.Firebolt,
                        primaryElement = ElementType.Fire,
                        passiveDescription = "Fire damage +10%"
                    }
                },
                {
                    CharacterClass.IceMage,
                    new CharacterDefinition
                    {
                        characterClass = CharacterClass.IceMage,
                        characterName = "Frost Witch",
                        characterNameJapanese = "氷術師",
                        description = "A witch who controls ice. Slows enemies.",
                        maxHP = 105f,
                        attackPower = 10f,
                        moveSpeed = 4.5f,
                        pickupRange = 3f,
                        startingSkill = SkillType.IceShot,
                        primaryElement = ElementType.Ice,
                        passiveDescription = "Ice skills slow enemies by 20%"
                    }
                },
                {
                    CharacterClass.LightMage,
                    new CharacterDefinition
                    {
                        characterClass = CharacterClass.LightMage,
                        characterName = "Holy Adept",
                        characterNameJapanese = "光術師",
                        description = "An adept of holy magic. Balanced stats with healing.",
                        maxHP = 110f,
                        attackPower = 10f,
                        moveSpeed = 4.5f,
                        pickupRange = 3f,
                        startingSkill = SkillType.HolyShot,
                        primaryElement = ElementType.Light,
                        passiveDescription = "Regenerate 1 HP per second"
                    }
                }
            };
        }

        public static CharacterDefinition GetCharacter(CharacterClass characterClass)
        {
            if (characters == null)
            {
                Initialize();
            }
            
            if (characters.TryGetValue(characterClass, out CharacterDefinition character))
            {
                return character;
            }
            
            return null;
        }

        public static List<CharacterDefinition> GetAllCharacters()
        {
            if (characters == null)
            {
                Initialize();
            }
            
            return new List<CharacterDefinition>(characters.Values);
        }
    }
}
