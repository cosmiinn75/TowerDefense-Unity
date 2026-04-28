🛡️ Kingdom Defense: Tactical Evolution
A strategic Tower Defense game inspired by Kingdom Rush, built in Unity 6, featuring modular enemy scaling and a scriptable wave management system.

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
/Assets/Scripts/Managers: Central logic for spawning and game state.

/Assets/Scripts/Enemies: Component-based scripts for stats and movement.

/Assets/ScriptableObjects: Data containers for various enemy archetypes.

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
