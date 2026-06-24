# 🛡️ Kingdom Defense: Tactical Evolution

**Kingdom Defense: Tactical Evolution** is a strategic Tower Defense game built in **Unity 6** and **C#**.

The game features a full 10-level campaign, tower placement, tower upgrades, enemy waves, elemental effects, enemy resistances, unlockable tower slots, a World Map progression system, and local development integration with a Spring Boot backend for account-based progression.

🎮 **Play the game here:**  
https://cosmiinn75.itch.io/kingdom-defense-tactical-evolution

🔗 **Backend API repository:**  
https://github.com/cosmiinn75/tower-defense-progress-api

> **Important Note:** The public WebGL version published on itch.io does **not** include online authentication or backend account progression.  
> The login/register system and account-based progress synchronization are implemented for local development/testing with a Spring Boot backend running locally.

> **Asset Note:** Paid graphics and audio assets are excluded from this repository via `.gitignore` to comply with asset store licenses.

---

## ✨ Features

- 10 playable levels across multiple biomes
- World Map level selection with locked/unlocked progression
- Tower placement, upgrading, selling, and refund system
- Multiple tower types: Cannon, Crossbow, and Magic Tower
- Elemental magic system with Poison, Ice, and Lightning effects
- Enemy resistances: Armor, Magic, Slow, Poison, and Stun resistance
- Coroutine-based wave spawning system
- Economy system with gold rewards, upgrade costs, and sell refunds
- Unlockable tower slots for additional strategic decisions
- Tutorial, pause menu, settings menu, defeat screen, and level completed screen
- WebGL build published on itch.io
- Local development integration with a Spring Boot backend API
- Account-based progress system for local testing
- Auto-login and sign out flow for backend-connected local builds
- Reset progress system through backend API
- Local audio settings saved between sessions

---

## 🛠️ Tech Stack

- **Engine:** Unity 6
- **Language:** C#
- **UI:** Unity UI, TextMeshPro
- **Networking:** UnityWebRequest
- **Backend Integration:** Spring Boot REST API
- **Authentication:** JWT
- **Database:** MySQL on the backend side
- **Deployment:** WebGL / itch.io

---

## 🧱 Architecture Concepts Used

- Scriptable Objects
- Coroutines
- Singleton Managers
- Data-driven enemy configuration
- Runtime stat modification
- Scene management
- UI state management
- Backend API communication
- JWT-based local account session
- Local settings persistence with PlayerPrefs

---

## 🔐 Account & Backend Progress System

The project includes local development integration with a separate Spring Boot backend API.

Backend API repository:  
https://github.com/cosmiinn75/tower-defense-progress-api

The backend handles:

- user registration
- user login
- JWT authentication
- password hashing
- per-account level progression
- stars saved per account
- reset progress functionality

The Unity client communicates with the backend using `UnityWebRequest`.

After login or register, the Unity client receives a JWT token and stores it locally.  
The token is then sent with protected requests using the `Authorization` header:

```http
Authorization: Bearer <token>
```

The backend validates the token, identifies the current user, and loads or updates only that user's progress.

```text
Unity Client
     ↓
Spring Boot REST API
     ↓
JWT Authentication
     ↓
MySQL Database
```

> The public WebGL version on itch.io does not currently use this backend system.  
> This backend integration is currently intended for local development and testing.

---

## 🌍 World Map Progression

The game includes a World Map that tracks campaign progression.

Completed levels unlock the next stage, while locked levels remain unavailable and guide the player through the campaign.

In the local backend-connected version, the World Map no longer relies only on local `PlayerPrefs` progression. Instead, it can load progress from the backend for the currently logged-in account.

The backend provides:

- highest unlocked level
- stars earned for each level
- per-account progress isolation

Example progress structure:

```json
{
  "maxLevelUnlocked": 2,
  "levels": [
    {
      "levelNumber": 1,
      "stars": 3
    },
    {
      "levelNumber": 2,
      "stars": 0
    }
  ]
}
```

The Unity client uses this data to:

- unlock available levels
- keep locked levels inaccessible
- show stars for completed levels
- focus the World Map on the latest unlocked level

---

## ⭐ Level Completion & Star Saving

When a level is completed, the game calculates the number of stars earned based on the King's Tower remaining health.

Star logic:

```text
80%+ health remaining  -> 3 stars
40%+ health remaining  -> 2 stars
below 40% health       -> 1 star
```

In the backend-connected local version, after winning a level, Unity sends the result to the backend:

```http
PUT /api/player/levels/{levelNumber}
```

Example request body:

```json
{
  "stars": 3
}
```

The backend then:

- saves the best star result
- never decreases already-earned stars
- unlocks the next level if needed
- returns the updated player progress

---

## 🔄 Reset Progress

The project includes a reset progress option.

In the backend-connected local version, resetting progress is done through the backend, not through local `PlayerPrefs`.

The reset action sets:

```text
maxLevelUnlocked = 1
all level stars = 0
```

Only the currently logged-in account is reset.

---

## 🔊 Local Settings

Audio settings are stored locally using `PlayerPrefs`.

These settings remain saved between sessions and are not tied to the backend account.

Examples of local settings:

- master volume
- music volume
- sound effects volume

Sign out only removes login data, not audio settings.

---

## 🧠 Technical Highlights

---

### Data-Driven Enemy System

Enemy data is configured through Scriptable Objects, allowing values such as health, speed, rewards, resistances, and visual identity to be adjusted without rewriting gameplay logic.

This made it easier to create and balance multiple enemy types across the campaign.

---

### Coroutine-Based Wave Manager

The wave system uses Coroutines to control:

- enemy spawning
- enemy spacing
- wave progression
- delays between waves
- difficulty pacing

This allowed each level to have custom wave sequences while still using a shared spawning architecture.

---

### Tower Slot System

The tower slot system supports multiple states:

- empty slot
- occupied slot
- locked slot

This allowed the same slot architecture to handle normal building, tower upgrades, selling, and unlockable blocked slots.

---

### Tower Upgrade & Economy System

The player can build, upgrade, and sell towers using gold earned from defeating enemies.

The economy includes:

- tower costs
- upgrade costs
- insufficient gold feedback
- 80% sell refund system
- gold reward balancing across waves

---

### Elemental Magic System

Magic Towers can unlock elemental effects that change how they behave in combat:

- **Poison:** damages enemies over time
- **Ice:** slows enemies
- **Lightning:** briefly stuns enemies

Enemies can also have resistances that reduce or block certain effects, adding more strategic decision-making to tower placement and upgrades.

---

### Enemy Resistance System

Enemies can have different resistances that influence how effective certain towers and effects are against them.

Supported resistance types include:

- Armor resistance
- Magic resistance
- Slow resistance
- Poison resistance
- Stun resistance

This system makes enemy design more flexible and encourages the player to choose towers more strategically.

---

### Backend API Integration

The Unity client includes local development integration with a Spring Boot REST API.

Implemented backend-related flows:

- login
- register
- auto-login
- load player progress
- save level result
- reset progress
- sign out

The Unity client keeps temporary runtime progress inside a `GameSession` class, while the backend remains the real source of truth for account progression in the local backend-connected version.

---

### UI and Game State Management

The project includes multiple UI/game state screens:

- Login Page
- Register/Login flow
- Main Menu
- Options Menu
- Audio Settings Menu
- Tutorial Menu
- Pause Menu
- Defeat Screen
- Level Completed Screen
- World Map level selection

These menus are connected to scene transitions, audio settings, player progression, backend authentication, and gameplay state changes.

---

## 📸 Screenshots

### Main Menu

![Main Menu](Screenshots/main-menu.png)

### World Map

![World Map](Screenshots/world-map.png)

### Gameplay

![Gameplay](Screenshots/gameplay.png)

### Level Completed

![Level Completed](Screenshots/level-completed.png)

---

## 📂 Project Structure

```text
Assets/
├── Scripts/
│   ├── Enemies/
│   ├── Towers/
│   ├── Managers/
│   ├── UI/
│   ├── WaveSystem/
│   └── ScriptableObjects/
├── Prefabs/
├── Scenes/
├── Images/
└── Resources/
```

---

## 🚀 Current Status

The game is currently playable as a WebGL demo with:

- 10 integrated levels
- complete World Map progression
- tower building and upgrading
- elemental magic mechanics
- enemy resistances
- unlockable tower slots
- full victory and defeat flow
- tutorial and settings menus
- WebGL publishing on itch.io

The project also includes a local backend-connected version with:

- login/register UI
- JWT-based authentication
- auto-login
- sign out
- backend account progress synchronization
- reset progress through backend API

> The public itch.io build does not currently include online authentication or backend account progression.

---

## 🌐 Backend Note

The backend integration currently uses a local Spring Boot API during development:

```text
http://localhost:8080
```

Backend API repository:  
https://github.com/cosmiinn75/tower-defense-progress-api

For a public online release with working account login for all players, the backend must be hosted online and the Unity client must use the deployed API URL instead of `localhost`.

The public WebGL version on itch.io currently remains playable without online authentication.

---

## 🧪 Playtesting & Iteration

The game was published on itch.io and improved based on external player feedback.

Improvements made after feedback include:

- clearer tutorial flow
- improved UI consistency
- more readable menus
- better button responsiveness
- earlier access to the Magic Tower elemental system
- improved onboarding for core mechanics
- improved menu structure
- cleaner login and sign out flow for the local backend-connected version

---

## 📚 What I Learned

Through this project, I practiced:

- building a complete gameplay loop from scratch
- organizing Unity scenes, prefabs, and UI systems
- using Scriptable Objects for data-driven design
- managing enemy waves with Coroutines
- implementing tower placement, upgrades, selling, and economy logic
- implementing elemental effects and enemy resistances
- debugging UI, state management, and gameplay issues
- balancing levels, enemy waves, tower costs, and rewards
- publishing a WebGL game on itch.io
- improving a project based on player feedback
- connecting Unity to a Spring Boot backend
- working with JWT authentication from a game client
- saving account-based progress in a database
- separating local settings from backend progress

---

## 🔮 Future Improvements

Planned or possible improvements:

- host the backend online for public account progression
- more enemy variety
- more tower upgrade branches
- more in-game tooltips and descriptions
- improved visual feedback for elemental effects
- better enemy resistance indicators during gameplay
- more polished animations and audio feedback
- additional balancing based on future playtesting
- more levels and biomes
- improved loading screens
- better online error handling for server downtime

---

## 📜 Devlog

A detailed development log is available in [`DEVLOG.md`](DEVLOG.md).

---

## 👤 Developer

Developed by **Anghel Cosmin** as a Unity/C# project focused on gameplay programming, UI systems, game architecture, backend integration, playtesting, and publishing a complete playable WebGL game.

The project combines Unity gameplay development with a Spring Boot backend system for local account-based progression testing.
