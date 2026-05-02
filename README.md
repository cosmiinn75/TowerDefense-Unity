🛡️ Kingdom Defense: Tactical Evolution
A strategic Tower Defense game built in Unity 6, featuring modular enemy scaling and a scriptable wave management system.

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

Wave-Based Scaling: Implemented a mathematical multiplier for health and speed.

healthMultiplier = 1f + (currentWave - 1) * 0.1f;

This ensures a 10% difficulty increase per wave, keeping the gameplay challenging.

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

Add specific resistances like armor and magic immunity.

Finalize the HUD and test the complete loop.
