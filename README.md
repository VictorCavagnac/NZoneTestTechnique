# NZoneTestTechnique

A simple tower defense made in approximately 20-23h~.

Play here : https://victorcavagnac.github.io/NZoneTestTechniqueWebGL/

## Overview

If you want to play the demo from the start, simply open the `Initialization` scene.
Alternatively, you can open any non-manager scene _(For example the `GameplayLevel` scene, you'll launch the game right away)_ and the `SceneLoader.cs` will load any scene that is needed for the current opened scene.

### Functionalities

#### Placing Towers

The `BuildingManager` script performs most of the work for managing towers :

1. The `UITowerDefense` script loads buttons for all available towers. The `OnTowerRequested` is picked up by the `BuildingManager`.
2. The `BuildingManager` loads a tower in preview mode and waits for another input from the player.
3. The player confirms his request and the script checks if the tower can be placed at the chosen spot _(`GridData`)_, and if the player can afford it. Only then the tower is powered on.

#### Game/Waves/Towers/Enemies Configuration

Each part can be easily modified through ScriptableObjects found in `ScriptableObjects/Settings`.

1. The `GameSettings` lets you setup the common settings a game could have _(Starting health and money)_. In addition, you can specify which tower is avaialble to the player during the game. Finally, you can configure the waves which will be sent.
2. `WaveSettings` lets you configure `SpawnGroups`. SpawnGroups are just a group of enemies in a wave. A wave could have multiple `SpawnGroups`, beware to setup properly the initial spawn delay.
3. `TowerSettings` and `EnemySettings` are simple ScriptableObjects which contains simple settings for these objects. Don't forget to add an `EnemyPoolSO` for an Enemy.

#### Login and Saving Scores

### Known Bugs
