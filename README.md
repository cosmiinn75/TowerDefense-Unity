
Check out the game here : https://cosmiinn75.itch.io/kingdom-defense-tactical-evolution

🛡️ Kingdom Defense: Tactical Evolution
A strategic Tower Defense game built in Unity 6, featuring modular enemy scaling and a scriptable wave management system.
Note: Paid graphics/audio assets have been excluded via .gitignore to comply with store licenses.
✨ Features
Scriptable Enemy Architecture: Utilizes Scriptable Objects (EnemyData) to define health, speed, and visual identity, allowing for rapid iteration of new enemy types.

Smart Wave Manager: A robust Coroutine-based system that handles wave progression, enemy spacing, and dynamically scales difficulty.

Intelligent Pathfinding: Enemies navigate complex waypoint hierarchies with smooth transitions and distance-based threshold checks.

Adaptive Buff System: Real-time stat modification logic that applies specific buffs (size/speed) to certain enemy classes (like the "Tank") based on the current wave index.

🛠️ Tech Stack & Skills
Game Engine: Unity 6 (6000.x)

Programming Language: C#

Key Mechanisms:

Data-Driven Design: Decoupling logic from data via Scriptable Objects.

Singleton Pattern: Centralized management for game-wide systems.

Asynchronous Logic: Extensive use of IEnumerator and Coroutines for non-blocking timing.

Lifecycle Management: Advanced use of OnDestroy and isLoaded checks to prevent memory leaks and null exceptions.

📂 Project Structure
/Assets/Scripts: Central logic.

/Assets/Scripts/EnemyType: Data containers for various enemy archetypes.

🚀 Devlog: Wave Logic & Data-Driven Foundations - 28.04.2026
🟢 Progress Summary:
Today I established the core gameplay loop. I successfully implemented a modular spawning system that doesn't just create enemies, but manages their lifecycle and difficulty scaling.

🛠️ Key Technical Solved / Implemented:

Modular Spawner Architecture: Refactored the spawning logic to track activeEnemies. The system now intelligently waits for the battlefield to be cleared before initiating the next wave's countdown.

Instance-Based Stat Copying: Solved the "Scriptable Object Data Corruption" bug. Instead of modifying the SO file, EnemyStats now creates a local runtime copy of base stats, ensuring every enemy can be buffed individually without affecting others.

Hierarchy Cleaning: Implemented a robust OnDestroy listener to ensure the SpawnManager always has an accurate count of active threats, even if an enemy is destroyed prematurely.

📅 Next Steps:

Implement the Currency System (Gold) to link kills to player rewards.

Create the first Tower Spot and construction logic.

Add UI overlays for Wave and Gold tracking.

🚀 Devlog: Level Design & Defensive Foundations - 29.04.2026

🟢 Progress Summary:
Today was about bringing the world to life and defining the stakes. I transitioned from abstract logic to a tangible game environment by integrating high-quality assets and establishing the first "Fail State" for the player. The game now has a start, a path, and a consequence for poor defense.

🛠️ Key Technical Solved / Implemented:

Asset Integration & Script Bridging: Successfully merged third-party monster logic with my custom architecture. I solved the "Waypoint Dependency" bug on the King by adding a tag , allowing the King to utilize the asset's animation and health bar systems without needing to move.

Tilemap Level Design: Crafted the first official map using a layered Tilemap system. This defines the navigation constraints for enemies and prepares the groundwork for turret placement spots.

Sequenced Enemy Spawning: Implemented a precise wave ordering system. Instead of random spawns, enemies now appear in specific, pre-defined sequences per wave, allowing for choreographed difficulty spikes and tactical pacing.

"King's Fate" Lose System: Established a health-based lose condition. Each enemy that successfully breaches the gate triggers a 20% health reduction for the King.

Total Base Health−(Enemies Reached×20)=0⟹Game Over
With five enemies reaching the base, the Time.timeScale is set to 0, successfully halting the game and triggering the defeat logic.

📅 Next Steps:

Turret Economy: Implement the Shop UI and menus for purchasing, upgrading, and selling towers.

Financial Incentive: Link the CurrencyManager to enemy deaths so players earn gold for every threat neutralized.

UI/UX Overlay: Design and polish the HUD to track current Wave, Gold, and King's Health in real-time.

🚀 Devlog: UI & Economic Foundations - 30.04.2026

🟢 Progress Summary: Today, I laid the foundation for the game's economy and refined the user interface (UI/UX) interactions. I transitioned from basic structural logic to a fully integrated resource management system, ensuring a seamless dynamic between placing, upgrading, and selling towers.

🛠️ Key Technical Solved / Implemented:

Clean Economy & Refund System: Implemented a sell mechanic that returns 80% of the invested gold, automatically rounded to the nearest tens, for a clean and structured economy.

Dynamic Upgrade & Build Menus: Fixed communication issues between SlotManager and the UpgradeSellMenu, ensuring real-time UI price updates without positional errors.

Visual Optimization & State Management: Resolved bugs related to the visibility of construction ghosts and base slots by cleanly decoupling placement states, keeping hover animations intact.

Currency Manager Synchronization: Optimized Spend and Add Gold methods to prevent edge-case locks when purchasing new defenses.

📅 Next Steps:

Wave Balancing: Synchronizing turret costs with enemy payout and wave progression.

Audio Feedback: Adding audio cues for placing, upgrading, and selling towers.

UI Polishing: Implementing animations (tweening) for pop-up UI menus.


🚀 Devlog: Wave Logic & Economic Integration - 01.05.2026

🟢 Progress Summary:
Today, I refined the core game balance and fixed several key architecture dependencies. I shifted focus to completing the tutorial pacing, ensuring that the economy, enemy stat scaling, and defeat conditions work in perfect harmony.

🛠️ Key Technical Solved / Implemented:

Wave Balancing & Standardization: Transitioned the tutorial level to a streamlined 5-wave structure, introducing the Goblin, Spider, Bandit Scout, and Troll.

Economical Fixes & Event Cleanup: Removed the duplicate gold granting issue by updating the enemy death sequence and removing OnDestroy(), ensuring in-game funds are distributed exactly once per elimination.

Instant Lose Condition: Linked the King's remaining defenses to the final wave, instantly triggering the defeat logic whenever a critical unit reaches the destination.

User Interface Interactions: Introduced clear notifications for insufficient funds, providing better feedback during the build and upgrade phases.

📅 Next Steps:

Implement the ramification trees for magic towers (fire, poison, etc.).

🛡️ Kingdom Defense: Tactical Evolution A strategic Tower Defense game built in Unity 6, featuring modular enemy scaling and a scriptable wave management system.

✨ Features Scriptable Enemy Architecture: Utilizes Scriptable Objects (EnemyData) to define health, speed, and visual identity, allowing for rapid iteration of new enemy types.

Smart Wave Manager: A robust Coroutine-based system that handles wave progression, enemy spacing, and dynamically scales difficulty.

Intelligent Pathfinding: Enemies navigate complex waypoint hierarchies with smooth transitions and distance-based threshold checks.

Adaptive Buff System: Real-time stat modification logic that applies specific buffs (size/speed) to certain enemy classes (like the "Tank") based on the current wave index.

🛠️ Tech Stack & Skills Game Engine: Unity 6 (6000.x)

Programming Language: C#

Key Mechanisms:

Data-Driven Design: Decoupling logic from data via Scriptable Objects.

Singleton Pattern: Centralized management for game-wide systems.

Asynchronous Logic: Extensive use of IEnumerator and Coroutines for non-blocking timing.

Lifecycle Management: Advanced use of OnDestroy and isLoaded checks to prevent memory leaks and null exceptions.

📂 Project Structure /Assets/Scripts: Central logic.

/Assets/Scripts/EnemyType: Data containers for various enemy archetypes.

🚀 Devlog: Wave Logic & Data-Driven Foundations - 28.04.2026 🟢 Progress Summary: Today I established the core gameplay loop. I successfully implemented a modular spawning system that doesn't just create enemies, but manages their lifecycle and difficulty scaling.

🛠️ Key Technical Solved / Implemented:

Modular Spawner Architecture: Refactored the spawning logic to track activeEnemies. The system now intelligently waits for the battlefield to be cleared before initiating the next wave's countdown.

Instance-Based Stat Copying: Solved the "Scriptable Object Data Corruption" bug. Instead of modifying the SO file, EnemyStats now creates a local runtime copy of base stats, ensuring every enemy can be buffed individually without affecting others.

Wave-Based Scaling: Implemented a mathematical multiplier for health and speed.

healthMultiplier = 1f + (currentWave - 1) * 0.1f;

This ensures a 10% difficulty increase per wave, keeping the gameplay challenging.

Hierarchy Cleaning: Implemented a robust OnDestroy listener to ensure the SpawnManager always has an accurate count of active threats, even if an enemy is destroyed prematurely.

📅 Next Steps:

Implement the Currency System (Gold) to link kills to player rewards.

Create the first Tower Spot and construction logic.

Add UI overlays for Wave and Gold tracking.

🚀 Devlog: Level Design & Defensive Foundations - 29.04.2026

🟢 Progress Summary: Today was about bringing the world to life and defining the stakes. I transitioned from abstract logic to a tangible game environment by integrating high-quality assets and establishing the first "Fail State" for the player. The game now has a start, a path, and a consequence for poor defense.

🛠️ Key Technical Solved / Implemented:

Asset Integration & Script Bridging: Successfully merged third-party monster logic with my custom architecture. I solved the "Waypoint Dependency" bug on the King by adding a tag , allowing the King to utilize the asset's animation and health bar systems without needing to move.

Tilemap Level Design: Crafted the first official map using a layered Tilemap system. This defines the navigation constraints for enemies and prepares the groundwork for turret placement spots.

Sequenced Enemy Spawning: Implemented a precise wave ordering system. Instead of random spawns, enemies now appear in specific, pre-defined sequences per wave, allowing for choreographed difficulty spikes and tactical pacing.

"King's Fate" Lose System: Established a health-based lose condition. Each enemy that successfully breaches the gate triggers a 20% health reduction for the King.

Total Base Health−(Enemies Reached×20)=0⟹Game Over With five enemies reaching the base, the Time.timeScale is set to 0, successfully halting the game and triggering the defeat logic.

📅 Next Steps:

Turret Economy: Implement the Shop UI and menus for purchasing, upgrading, and selling towers.

Financial Incentive: Link the CurrencyManager to enemy deaths so players earn gold for every threat neutralized.

UI/UX Overlay: Design and polish the HUD to track current Wave, Gold, and King's Health in real-time.

🚀 Devlog: UI & Economic Foundations - 30.04.2026

🟢 Progress Summary: Today, I laid the foundation for the game's economy and refined the user interface (UI/UX) interactions. I transitioned from basic structural logic to a fully integrated resource management system, ensuring a seamless dynamic between placing, upgrading, and selling towers.

🛠️ Key Technical Solved / Implemented:

Clean Economy & Refund System: Implemented a sell mechanic that returns 80% of the invested gold, automatically rounded to the nearest tens, for a clean and structured economy.

Dynamic Upgrade & Build Menus: Fixed communication issues between SlotManager and the UpgradeSellMenu, ensuring real-time UI price updates without positional errors.

Visual Optimization & State Management: Resolved bugs related to the visibility of construction ghosts and base slots by cleanly decoupling placement states, keeping hover animations intact.

Currency Manager Synchronization: Optimized Spend and Add Gold methods to prevent edge-case locks when purchasing new defenses.

📅 Next Steps:

Wave Balancing: Synchronizing turret costs with enemy payout and wave progression.

Audio Feedback: Adding audio cues for placing, upgrading, and selling towers.

UI Polishing: Implementing animations (tweening) for pop-up UI menus.

🚀 Devlog: Wave Logic & Economic Integration - 01.05.2026

🟢 Progress Summary: Today, I refined the core game balance and fixed several key architecture dependencies. I shifted focus to completing the tutorial pacing, ensuring that the economy, enemy stat scaling, and defeat conditions work in perfect harmony.

🛠️ Key Technical Solved / Implemented:

Wave Balancing & Standardization: Transitioned the tutorial level to a streamlined 5-wave structure, introducing the Goblin, Spider, Bandit Scout, and Troll.

Economical Fixes & Event Cleanup: Removed the duplicate gold granting issue by updating the enemy death sequence and removing OnDestroy(), ensuring in-game funds are distributed exactly once per elimination.

Instant Lose Condition: Linked the King's remaining defenses to the final wave, instantly triggering the defeat logic whenever a critical unit reaches the destination.

User Interface Interactions: Introduced clear notifications for insufficient funds, providing better feedback during the build and upgrade phases.

📅 Next Steps:

Implement the ramification trees for magic towers (fire, poison, etc.).

Add specific resistances like armor and magic immunity.

Finalize the HUD and test the complete loop.

Add specific resistances like armor and magic immunity.

Finalize the HUD and test the complete loop.

🚀 Devlog: Elemental Magic & Balance Foundations - 02.05.2026

🟢 Progress Summary: Today, I connected the visual and functional aspects of our elemental mechanics. I integrated the elemental magic system with the core combat loop, ensuring that elements like poison and ice apply actual gameplay effects (damage over time and movement speed reduction) while preserving a clear and readable UI.

🛠️ Key Technical Solved / Implemented:

Elemental Magic Integration: Linked the MagicTower script to the projectile system. The elemental states now dynamically alter the _speed and apply periodic damage to the target.

Smart Buff/Debuff Management: Optimized coroutines to handle stats dynamically. The system tracks the initial stats via runtime instances, making sure that slow and poison effects are correctly applied and reset when the duration expires.

HUD and Economic Balancing: Synchronized the cost of elemental upgrades with the new wave payout. Implemented visual color feedback for electrocuted units, making stuns recognizable even on normal-colored enemies.

Bug Fixes: Solved the target-locking issue with larger enemies near sensors by increasing checkpoint detection tolerance, allowing units like the Spider to move smoothly through tight corners.

📅 Next Steps:

Resistance Mechanics: Introduce armor ratings and magic immunity to diversify enemies.

🚀 Devlog: Combat Enhancements & System Integration - 04.05.2026
🟢 Progress Summary: Today, I focused on deep system integration and bug fixing to ensure the combat mechanics, dynamic stat scaling, and HUD elements work in perfect harmony. I transitioned to isolating system materials and properties to fix visual bugs, while fine-tuning our elemental resistances and balance framework.

🛠️ Key Technical Solved / Implemented:
Visual UI Bug Fix (Monster.cs): Solved a critical visual bug where the hit and elemental material states were overwriting the resist icons and HUD elements. Added an explicit isResist tag and HUD transform check to keep icons intact during animations.

Dynamic Resistance Integration: Fully connected elemental resistances (Poison, Ice, Stun) to specific enemy classes, scaling down status effects depending on the target's resistances.

Value Calculations & Sell Logic: Synchronized the economy loop to calculate proper sell refunds (80% rounded to the nearest tens) when a player removes a defense, accounting for dynamic upgrades and changing elements.

Architecture Stabilization: Removed duplicate calls in the death and cleanup loop to prevent memory leaks and null references while checking unit states.

📅 Next Steps:
Audio Cues: Integrate sound effects for placing, upgrading, and selling towers, as well as distinct cues for elemental impacts.


Here is the devlog for 06.05.2026 translated and styled in the same technical format:

🚀 Devlog: Level 1 Completion & Level 2 Optimization - 06.05.2026
🟢 Progress Summary: Today marked an important milestone: we completed and stabilized Level 1, ensuring the core pacing is working exactly as intended. We also initiated the development of Level 2 and conducted an extensive balance pass on all game statistics (towers and enemies), ensuring that the economy remains strict and prevents players from purchasing advanced magic or elemental options too early.

🛠️ Key Technical Solved / Implemented:

Dynamic Wave Management: Rewrote the level initialization logic to support varying numbers of waves based on the current level (5 waves for Level 1, 6 waves for Level 2) without needing separate, hardcoded scripts for each map.

King's Fate Wipeout System: Optimized the cleanup sequence upon the King's death. Converted the active enemy tracking into a safe list iteration using enemiesLeft.Clear() to prevent NullReferenceException errors between stages.

Economic and Tower Balancing: Analyzed the data from the balance sheet and re-adjusted the cost-to-performance ratio, restricting access to elemental features and the Level 3 Magic Tower to ensure early-game progression relies heavily on basic, physical defenses.

Performance & Time Management: Fixed the main update loop transition from Wave 5 to Wave 6 on Level 2, ensuring the final boss wave spawns properly.

📅 Next Steps:

Develop Levels 3 and 4 based on the established wave scaling foundations.

Start working on the World Map / Level Select scene to enable a seamless multi-stage playthrough.

Implement UI polish for level transition screens.

Finishing First Level: Complete balancing, ensure the tutorial loop works perfectly from the first to the final wave, and trigger the game loop conclusion properly.


Audio-Visual Polishing: Add sound effects for elemental impacts and UI elements.

Iată propunerea pentru devlog-ul de astăzi, 08.05.2026, păstrând stilul tehnic, structura pe secțiuni și terminologia folosită anterior în proiectul tău:

🚀 Devlog: Bug Squashing & Level 3 Strategic Foundations - 08.05.2026
🟢 Progress Summary:
Today was dedicated to system stability and expanding the content for the mid-game phase. I successfully resolved a critical NullReference bug that occurred during the endgame state and finalized the technical implementation of Level 3. The focus shifted towards a "Quality over Quantity" design philosophy for the new level, emphasizing strategic tower placement due to limited slot availability.

🛠️ Key Technical Solved / Implemented:

Endgame NullReference Fix: Solved a persistent crash in Tower.cs where towers would attempt to access a target's tag and state after the King's death. Implemented a robust null-safety check before CompareTag calls and integrated a Time.timeScale short-circuit to prevent unnecessary update cycles after a Game Over.

Level 3 Logic & Wave Sequencing: Completed the LoadLevel3 configuration. Implemented a 5-wave structure that introduces the Bandit Leader (Boss) with 2000 HP and Armor. The wave pacing was adjusted to account for the map's unique "choke-point" layout.

Armor-Archer Synergy Balancing: Fine-tuned the combat loop to reinforce the new elemental/physical priority. Validated that Archer Towers now act as the primary counter to Armored units (like Wargs and the Bandit Leader), while Cannons deal reduced damage to these specific types.

Level 2 Stabilization: Successfully completed all playtests for Level 2, ensuring the 6-wave transition and the final boss cleanup logic are 100% stable.

📂 Current Project State:

Level 1: Completed & Balanced.

Level 2: Completed & Balanced.

Level 3: Code Logic implemented; Wave data verified via Spreadsheet.

📅 Next Steps:

Environment Art: Complete the background and visual assets for Level 3 to match the tactical layout.
Next Levels: Continue working on the next 7 remaining levels before developing the world map.

🚀 Devlog: Level 4 Completion & UI State Optimization - 09.05.2026

🟢 Progress Summary:
Today marked a major milestone in content expansion as I successfully completed the implementation and balancing for Level 3 and Level 4. Beyond map expansion, I focused on refining the User Experience (UX) by optimizing the construction menus for max-tier towers, ensuring a cleaner interface and preventing logic redundancies.

🛠️ Key Technical Solved / Implemented:

Level 3 & 4 Finalization: Integrated the wave sequences for both levels. Level 4 has been specifically tuned to reward Physical Tower (Archer/Cannon) Level 3 upgrades while making advanced magic elements economically difficult to obtain, forcing a more tactical use of basic slots.

Max-Level UI State Logic: Refactored the UpgradeSellMenu logic. When a tower reaches Level 3 (Max), the script now dynamically hides the "Upgrade" button and its associated costs, displaying only the Sell option. This ensures the player cannot attempt invalid upgrades and declutters the UI.

Minor Bug Squashing:

Monster Death Routine: Fixed a race condition in the Die() method where the script would occasionally throw a null reference when trying to stop coroutines on a disabled object.

📂 Current Project State:

Levels 1-4: Fully playable and balanced.

Tower Systems: Level 3 progression complete with context-sensitive menus.

Enemy Variety: Index 0-17 units are now fully integrated into the wave spawner logic.

📅 Next Steps:

Desert Biome (Levels 5-7): Transitioning to the new environment, featuring high Magic Resistance enemies and heat-themed maps.

Dark Forest Biome (Levels 8-10): Designing the final stretch of levels leading up to the Level 10 showdown with the King.

World Map System: Developing the level selection scene to bridge the completed stages.


🚀 Devlog: Desert Biome Conclusion & Magical Forest Foundations - 13.05.2026

🟢 Progress Summary:
Today officially marks the end of the Desert chapter. I have successfully finalized and stabilized Levels 6 and 7. The core focus was calibrating difficulty for long, winding paths where I maximized the use of Mech Spiders and Armored units. With the desert biome closed, I have begun transitioning into the final zone of the game: the Dark Forest / Magical Theme, preparing the infrastructure for new magic-specific gameplay mechanics.

🛠️ Key Technical Solved / Implemented:

Desert Biome Completion (Levels 6 & 7):

Level 6 Stabilization: Implemented a 7-wave structure focused on a balanced mix of speed (Wargs) and durability (TrollNoRes). Starting gold was set to $750 to allow for tactical flexibility.

Level 7 "Long Path" Logic: Configured waves to take advantage of the long layout with the oasis. I increased Mech Spider density (over 50 units total) to create a "mechanical invasion" atmosphere, forcing the player to manage sustained pressure.

Specific Level 7 Balancing (Armor Focus):

As a strategic choice for Level 7, I removed all enemies with Magic Resistance. This was done to explicitly highlight the efficiency of Level 3 Magic Towers against heavy armor units (Trolls, Bandit Leaders). This validated the damage priority system and gave the player a satisfying "power trip" before the next biome's difficulty spike.

Biome Transition Logic: Prepared the scene loading system for the visual environment shift. I have begun defining parameters for Dark Forest enemies, where I plan to reintroduce high magic resistances to shift the tactical focus back toward physical defenses.

Gold Curve Optimization: Synchronized gold rewards across the last two desert levels to ensure the player finishes the biome with a clear understanding of Level 3 upgrade values before entering the high-stakes final three levels.

📂 Current Project State:

Levels 1-7: Fully completed, tested, and balanced (Tutorial, Meadow, Desert).

Enemy Database: Boss-type units (Bandit Leader, Cyclops) are fully integrated into the wave spawner logic.

Visuals: Desert Theme finalized (Oasis, cacti, skeletons).

📅 Next Steps:

Dark Forest Biome (Levels 8-10): Designing magic-themed maps and introducing forest-specific visual obstacles.

Final Boss Integration: Preparing special logic for Level 10, where the King will serve as the ultimate victory/defeat condition.

World Map / Level Select: Developing the level selection scene to bridge all 10 stages into a cohesive campaign.

🚀 Devlog: Level 8 Completion & Unlockable Slots Feature - 18.05.2026

🟢 Progress Summary:
Today marks a major tactical milestone as I officially entered the Dark Forest biome, successfully completing and balancing Level 8. To address late-game economic surpluses and prevent map overcrowding, I introduced a major new gameplay mechanic: Unlockable Slots. The player must now strategically manage their gold to clear environmental obstacles before gaining access to premium tower placements, shifting the late-game focus from mindless "tower spamming" to calculated spatial progression.

🛠️ Key Technical Solved / Implemented:

Unlockable Slots Mechanics (Clear Obstacles): Designed and integrated a "Deforestation Fee" system. Certain high-value building spots are now blocked by thematic obstacles ("Corrupted Trees").

Implemented an isLocked state variable directly into the core TowerSlot architecture to prevent complex and expensive runtime Instantiate/Destroy routines.

The player can click on a locked slot to open a dedicated context-sensitive UI menu showing a shovel icon and a clearing cost ($500 gold). Upon payment, the overlay sprite vanishes via SetActive(false), seamlessly enabling the slot's standard construction behavior.

HandleMenus Refactoring (Spaghetti Code Elimination): Cleaned up the heavily nested if-else blocks inside HandleMenus() by implementing Guard Clauses.

Isolated state evaluation into specialized, single-responsibility methods (HandlePlacedTowerMenus() and ResetMenuTimerIfActive()). The method now checks for the isLocked state at the very top and short-circuits execution with a clean return, making the entire UI state machine robust and infinitely easier to extend.

Level 8 Layout & Balancing: Configured a challenging 5-wave structure tailored to the Dark Forest theme. Adjusted the starting gold curve to account for the clearing costs of premium slots. Towers placed inside the large inner loops of the path now require Level 3 range upgrades to be fully effective, creating a perfect balance between clearing the environment and upgrading physical structures.

📂 Current Project State:

Levels 1-8: Fully playable, stabilized, and balanced.

Slot Architecture: Refactored universal slot system supporting both default open spots and locked environment hazards.

UI Systems: Context-aware Upgrade/Sell, Choose Magic, and the new Shovel-themed Unlock menus are completely synchronized.

📅 Next Steps:

Level 9 & 10 Development: Design the final stretch of maps for the Dark Forest biome, tuning the scaling for dual-element options.

Final Boss Integration: Write the custom logic for Level 10, connecting the King as the ultimate active threat on the battlefield.

World Map Campaign: Connect all 10 finalized levels into a smooth, playable progression scene.


🚀 Devlog: Campaign Completion & World Map Integration - 25.05.2026

🟢 Progress Summary:
Today marks the completion of the core content pipeline. I have finalized Levels 9 and 10, bringing the total campaign to 10 playable stages. Additionally, I successfully integrated the World Map system, bridging all levels into a cohesive progression flow. With the gameplay loop now fully functional from start to finish, the project has shifted into the final polishing and asset-standardization phase.

🛠️ Key Technical Solved / Implemented:

World Map System: Developed a dynamic WorldMapManager that tracks player progress via MainGameManager. Implemented a "Lock/Unlock" logic: levels above the maxLevelReached now display a visual Padlock icon, preventing unauthorized access and guiding the player through the campaign.

10-Level Campaign Finalization: Completed the balancing pass for the final two levels in the Dark Forest biome. Level 10 is now tuned as the ultimate defensive challenge, requiring optimized usage of all tower types and elemental resistances.

Progression Synchronization: Linked the UnlockNextLevel logic to the victory state of each level, ensuring a seamless transition from the gameplay scene back to the World Map with updated progression states.

📂 Current Project State:

Levels 1-10: Fully functional, balanced, and integrated into the World Map.

World Map: Level selection, progression tracking, and locked/unlocked state visualization (Padlock system) are live.

Core Loop: The transition between stages, world map selection, and victory/defeat cycles is stable.

📅 Next Steps (The Polish Phase):

Visual Consistency Pass: Begin the refactor of background art for Levels 1-5. The goal is to migrate them from the legacy "cartoonish" style to match the detailed, painterly aesthetic established in the later biomes (Levels 6-10).

Main Menu: Design and implement the primary entry point for the game.

Final Polish: Systematic review of all UI/UX elements, particle effect timing, and audio feedback to unify the user experience.
