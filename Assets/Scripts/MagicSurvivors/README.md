# Magic Survivors - Game System Documentation

## Overview
Magic Survivors is a 20-minute roguelite auto-battler featuring 3 playable mage characters, 12 unique skills, 10 synergy combinations, and a progressive difficulty system.

## Architecture

### Core Systems
- **GameManager**: Manages game state, 20-minute timer, stage transitions, and boss spawn events
- **PlayerCharacter**: Handles player movement, stats, health, and stat modifications
- **SkillManager**: Auto-attack system managing all 12 skills with cooldowns and targeting
- **SynergyManager**: Detects and activates synergies when specific element combinations are acquired
- **EnemySpawner**: Spawns enemies with B-curve density progression (100% → 250%)
- **XPManager**: Handles experience collection, leveling, and level-up events
- **MetaProgressionManager**: Persistent upgrades across runs (8 categories)
- **StageManager**: Manages stage transitions and mini-dungeon generation

### Data Structures
All game data is defined using ScriptableObjects for easy configuration:
- **CharacterData**: Character stats, starting skills, and passive abilities
- **SkillData**: Skill damage, cooldowns, projectile behavior, and level scaling
- **EnemyData**: Enemy stats, behavior, and rewards
- **SynergyData**: Element requirements and damage multipliers

## Game Flow

### 20-Minute Run Structure
1. **0:00** - Game starts in Forest stage
2. **~5:00** - Mini-Boss 1 (Flame Ogre) spawns
3. **10:00** - Stage transition to Cave + Mini-Boss 2 (Wind Harpy)
4. **~15:00** - Mini-Boss 3 (Ice Golem) spawns
5. **20:00** - Final Boss (Ancient Dragon) spawns
6. **Victory/Defeat** - Result screen with rewards

### Spawn Density Progression
Uses B-curve formula: `density = lerp(1.0, 2.5, t²)` where t = currentTime / totalTime
- 0-5 min: 100% density
- 5-10 min: ~125% density
- 10-15 min: ~175% density
- 15-20 min: 250% density

## Characters (3 Total)

### Fire Mage (炎術師)
- **Primary Element**: Fire
- **Starting Skill**: Firebolt
- **Base Stats**: High attack, medium HP
- **Playstyle**: Aggressive damage dealer

### Ice Mage (氷術師)
- **Primary Element**: Ice
- **Starting Skill**: Ice Shot
- **Base Stats**: Medium attack, high HP
- **Playstyle**: Crowd control and area denial

### Light Mage (光術師)
- **Primary Element**: Light
- **Starting Skill**: Holy Shot
- **Base Stats**: Balanced stats
- **Playstyle**: Support and sustained damage

## Skills (12 Total)

### Fire Skills
1. **Firebolt**: Single-target fire projectile
2. **Flame Nova**: Area-of-effect fire explosion

### Ice Skills
3. **Ice Shot**: Piercing ice projectile
4. **Frost Area**: Freezing ground effect

### Thunder Skills
5. **Lightning Chain**: Chains between multiple enemies
6. **Thunder Spear**: High-damage lightning projectile

### Wind Skills
7. **Wind Arrow**: Fast wind projectile
8. **Tornado**: Moving area damage

### Light Skills
9. **Holy Shot**: Homing light projectile
10. **Holy Shield**: Protective aura

### Dark Skills
11. **Dark Bolt**: Life-stealing projectile
12. **Life Drain**: Continuous health drain

## Synergies (10 Total)

1. **Fire Tornado** (Fire × Wind): Enhanced damage and movement
2. **Steam Explosion** (Fire × Ice): Area burst damage
3. **Explosive Thunder** (Fire × Thunder): Chain explosions
4. **Nuclear Explosion** (Light × Dark): Massive area damage
5. **Superconductor** (Ice × Thunder): Enhanced chain effects
6. **Storm** (Wind × Thunder): Lightning storm
7. **Blizzard** (Ice × Wind): Freezing tornado
8. **Solar Flare** (Fire × Light): Burning light waves
9. **Frozen Curse** (Ice × Dark): Slowing curse
10. **Divine Wind** (Wind × Light): Healing wind

## Enemies (8 Types)

### Basic Enemies (3)
- **Goblin**: Melee attacker, low HP
- **Slime**: Slow tank, high HP
- **Skeleton Archer**: Ranged attacker, medium HP

### Elite Enemies (2)
- **Elite Goblin**: Enhanced melee, 2x stats
- **Elite Skeleton**: Enhanced ranged, 2x stats

### Mini-Bosses (3)
- **Flame Ogre**: Fire-based boss, spawns at 5 min
- **Wind Harpy**: Wind-based boss, spawns at 10 min
- **Ice Golem**: Ice-based boss, spawns at 15 min

### Final Boss (1)
- **Ancient Dragon**: Multi-element boss, spawns at 20 min

## Skill Evolutions (10 Total)

Unlocked after defeating mini-bosses:
1. **Fire Lance**: Firebolt evolution
2. **Ice Needle**: Ice Shot evolution
3. **Twin Arrow**: Wind Arrow evolution
4. **Superconductor Core**: Thunder evolution
5. **Prime Fire**: Ultimate fire skill
6. **Prime Ice**: Ultimate ice skill
7. **Prime Thunder**: Ultimate thunder skill
8. **Prime Wind**: Ultimate wind skill
9. **Prime Light**: Ultimate light skill
10. **Prime Dark**: Ultimate dark skill

## Meta Progression (8 Categories)

Persistent upgrades purchased with gold:
1. **Max HP**: +10 HP per level
2. **Attack Power**: +5% damage per level
3. **Cooldown Reduction**: -2% cooldown per level
4. **Movement Speed**: +0.5 speed per level
5. **XP Gain**: +5% XP per level
6. **Gold Gain**: +5% gold per level
7. **Elemental Damage**: +3% elemental damage per level
8. **Pickup Range**: +0.5 range per level

Each category has 10 levels, cost increases by 1.5x per level.

## UI Systems

### Home Screen
- Character selection with card UI
- Meta progression access
- Start game button

### In-Game HUD
- HP bar (top-left)
- XP bar (bottom)
- Timer (top-center)
- Level display
- Minimap (top-right)

### Level-Up UI
- 3-card selection system
- Pauses game
- Shows skill name, icon, description

### Mini-Boss Reward UI
- Special evolution card display
- Triggered after mini-boss defeat
- Enhanced visual effects

### Result Screen
- Time survived
- Enemies defeated
- Level reached
- Gold earned
- Retry/Return to home buttons

## Stage System

### Stage 1: Forest (0-10 minutes)
- Bright fantasy theme
- Base enemy density
- 2 mini-bosses

### Stage 2: Cave (10-20 minutes)
- Dark dungeon theme
- Increased enemy density
- 1 mini-boss + final boss

### Mini-Dungeon Templates (5 Types)
1. **Plaza**: Large open area
2. **Corridor + Plaza**: Linear to open
3. **Cross Intersection**: 4-way crossing
4. **Simple Maze**: Branching paths
5. **Object-Heavy**: Obstacle-filled rooms

## Implementation Notes

### Auto-Attack System
- Skills fire automatically based on cooldowns
- Targets nearest enemy by default
- No manual input required
- Projectiles track or move in direction

### XP Collection (Method C)
- XP orbs auto-attract within pickup range
- Pickup range affected by meta progression
- Instant collection on contact

### Damage Calculation
```
finalDamage = baseDamage * attackPowerMultiplier * synergyMultiplier * (1 + level * damagePerLevel)
```

### Cooldown Calculation
```
finalCooldown = baseCooldown * (1 - cooldownReduction) * (1 - level * cooldownReductionPerLevel)
```

## Setup Instructions

### Creating ScriptableObject Data
1. Right-click in Project window
2. Create → MagicSurvivors → [DataType]
3. Configure values in Inspector
4. Assign to appropriate manager

### Scene Setup
1. Create GameManager GameObject
2. Add PlayerCharacter to player GameObject (tag: "Player")
3. Add SkillManager, SynergyManager, XPManager to player
4. Create EnemySpawner GameObject
5. Create StageManager GameObject
6. Set up UI Canvas with HUD, LevelUpUI, etc.

### Prefab Requirements
- Enemy prefab with Enemy component
- XP orb prefab with XPOrb component
- Projectile prefabs for each skill
- Effect prefabs for area skills
- Stage prefabs (Forest, Cave)
- Mini-dungeon templates

## Balance Configuration

### Recommended Starting Values
- Player HP: 100
- Player Attack: 10
- Player Speed: 5
- Base XP Required: 100
- XP Scaling: 1.2x per level
- Enemy Spawn Interval: 2 seconds
- Max Enemy Count: 50

### Skill Balance
- Projectile skills: 10-15 base damage, 1-2s cooldown
- Area skills: 5-10 base damage, 3-5s cooldown
- Damage per level: +2 damage
- Cooldown reduction per level: -5%

### Enemy Balance
- Basic: 50 HP, 10 damage
- Elite: 150 HP, 20 damage
- Mini-Boss: 500 HP, 30 damage
- Final Boss: 2000 HP, 50 damage

## Extension Points

### Adding New Characters
1. Create CharacterData ScriptableObject
2. Define stats and starting skill
3. Add to HomeScreenUI character list

### Adding New Skills
1. Create SkillData ScriptableObject
2. Create projectile/effect prefab
3. Add to SkillManager database
4. Define SkillType enum entry

### Adding New Synergies
1. Create SynergyData ScriptableObject
2. Define element requirements
3. Add to SynergyManager database
4. Define SynergyType enum entry

### Adding New Enemies
1. Create EnemyData ScriptableObject
2. Define stats and behavior
3. Add to EnemySpawner database
4. Define EnemyType enum entry

## Performance Considerations

- Object pooling recommended for projectiles and enemies
- Limit max enemy count to prevent performance issues
- Use sprite atlases for UI elements
- Optimize collision detection with appropriate layers
- Consider using DOTween for UI animations

## Testing Checklist

- [ ] All 3 characters are selectable and playable
- [ ] All 12 skills fire automatically
- [ ] All 10 synergies activate correctly
- [ ] All 8 enemy types spawn and behave correctly
- [ ] Mini-bosses spawn at correct times
- [ ] Stage transition occurs at 10 minutes
- [ ] Final boss spawns at 20 minutes
- [ ] XP collection works within pickup range
- [ ] Level-up UI pauses game and shows 3 cards
- [ ] Meta progression persists between runs
- [ ] Result screen displays correct statistics
- [ ] Spawn density increases over time
