using UnityEngine;

namespace MagicSurvivors.Data
{
    public static class GameConstants
    {
        public const float GAME_DURATION = 1200f;
        public const float STAGE_1_DURATION = 600f;
        public const float MINIBOSS_1_TIME = 300f;
        public const float MINIBOSS_2_TIME = 600f;
        public const float MINIBOSS_3_TIME = 900f;
        public const float FINAL_BOSS_TIME = 1200f;
        
        public const int MAX_SKILL_SLOTS = 6;
        public const int LEVEL_UP_CARD_COUNT = 3;
        public const int MAX_SKILL_LEVEL = 5;
        
        public const float XP_PICKUP_RANGE_BASE = 3f;
        public const float XP_PICKUP_RANGE_MAX = 5f;
        
        public const float PLAYER_BASE_MOVE_SPEED = 4.5f;
        public const float PLAYER_BASE_ATTACK = 10f;
        
        public const int SYNERGY_REQUIRED_LEVEL = 3;
        
        public const int META_UPGRADE_MAX_LEVEL = 10;
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
