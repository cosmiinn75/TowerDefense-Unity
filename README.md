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
