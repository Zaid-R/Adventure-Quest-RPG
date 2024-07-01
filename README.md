# Adventure Quest RPG
Welcome to Adventure Quest RPG! In this game, you'll embark on an epic journey, battle formidable monsters, and explore dangerous dungeons. Can you emerge victorious and prove your heroism?

## Features
- **Engaging Battles**: Fight against monsters with varying difficulty levels.
- **Character Classes**: Play as a brave Player and face off against different types of Monsters.
- **Turn-Based Combat**: Strategically take turns to attack and defend.
- **Leveling System**: Gain experience and upgrade your stats by defeating monsters.

## Installation
1. **Clone the repository:**
```sh
git clone https://github.com/yourusername/adventure-quest-rpg.git
cd adventure-quest-rpg
```
2. **Build the project:**
```sh
dotnet build
```
3. **Run the application:**
```sh
dotnet run --project AdventureQuestRPG
```

## How to Play
1. **Enter your name**: Start the game by entering your character's name.
2. **Select difficulty**: Choose the difficulty level for the battle (Easy, Medium, Hard).
3. **Battle Monsters**: Engage in turn-based combat until either you or the monster wins.
4. **Repeat or Exit**: Continue playing or exit by pressing the Escape key.

## Project Structure
- **AdventureQuestRPG**: Contains the game logic and classes for Player, Monster, and BattleSystem.
- **AdventureQuestRPGTests**: Includes unit tests to ensure the game's functionality.

## Key Classes
- **Player**: Represents the player-controlled character with attributes like Name, Health, AttackPower, and Defense.
- **Monster**: Represents the enemy monsters with similar attributes and varying levels of difficulty.
- **BattleSystem**: Handles the attack logic and manages the battle flow.

## Unit Tests
Ensure the game's reliability with the included XUnit tests:
- **Test_Enemy_Health**: Validates that the enemy's health is correctly reduced when attacked.
- **Test_Player_Health**: Ensures the player's health is accurately decreased when attacked by a monster.
- **Test_Health_Validation**: Confirms health values do not go negative after a battle.<br/><br/>
Run the tests using:
```sh
dotnet test
```

