using UnityEngine;

namespace MagicSurvivors.Data
{
    public static class GameConstants
    {
        public const float GAME_DURATION = 1200f; // 20 minutes in seconds
        public const float STAGE_1_DURATION = 600f; // 10 minutes
        public const float MINIBOSS_1_TIME = 300f; // 5 minutes
        public const float MINIBOSS_2_TIME = 600f; // 10 minutes
        public const float MINIBOSS_3_TIME = 900f; // 15 minutes
        public const float FINAL_BOSS_TIME = 1200f; // 20 minutes
        
        public const int MAX_SKILL_SLOTS = 6;
        public const int LEVEL_UP_CARD_COUNT = 3;
        
        public const float XP_PICKUP_RANGE_BASE = 2f;
    }
    
    public enum ElementType
    {
        Fire,
        Ice,
        Thunder,
        Wind,
        Light,
        Dark
    }
    
    public enum CharacterClass
    {
        FireMage,
        IceMage,
        LightMage
    }
    
    public enum EnemyType
    {
        Goblin,
        Slime,
        SkeletonArcher,
        EliteGoblin,
        EliteSkeleton,
        FlameOgre,
        WindHarpy,
        IceGolem,
        AncientDragon
    }
    
    public enum SkillType
    {
        Firebolt,
        FlameNova,
        IceShot,
        FrostArea,
        LightningChain,
        ThunderSpear,
        WindArrow,
        Tornado,
        HolyShot,
        HolyShield,
        DarkBolt,
        LifeDrain
    }
    
    public enum SynergyType
    {
        FireTornado,        // Fire x Wind
        SteamExplosion,     // Fire x Ice
        ExplosiveThunder,   // Fire x Thunder
        NuclearExplosion,   // Light x Dark
        Superconductor,     // Ice x Thunder
        Storm,              // Wind x Thunder
        Blizzard,           // Ice x Wind
        SolarFlare,         // Fire x Light
        FrozenCurse,        // Ice x Dark
        DivineWind          // Wind x Light
    }
    
    public enum SkillEvolutionType
    {
        FireLance,
        IceNeedle,
        TwinArrow,
        SuperconductorCore,
        PrimeFire,
        PrimeIce,
        PrimeThunder,
        PrimeWind,
        PrimeLight,
        PrimeDark
    }
    
    public enum MetaUpgradeType
    {
        MaxHP,
        AttackPower,
        CooldownReduction,
        MovementSpeed,
        XPGain,
        GoldGain,
        ElementalDamage,
        PickupRange
    }
    
    public enum StageType
    {
        Forest,
        Cave
    }
}
