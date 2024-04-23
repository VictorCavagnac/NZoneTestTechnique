# NZoneTestTechnique

A simple tower defense made in approximately 20-23h~.

Play here : https://victorcavagnac.github.io/NZoneTestTechniqueWebGL/

Right mouse drag ! Camera movement
Left mouse click : Preview tower/Buy tower

## Overview

If you want to play the demo from the start, simply open the `Initialization` scene.
Alternatively, you can open any non-manager scene _(For example the `GameplayLevel` scene, you'll launch the game right away)_ and the `SceneLoader.cs` will load any scene that is needed for the current opened scene.

### Functionalities

#### Placing Towers

The `BuildingManager` script performs most of the work for managing towers :

1. The `UITowerDefense` script loads buttons for all available towers. The `OnTowerRequested` is picked up by the `BuildingManager`.
2. The `BuildingManager` loads a tower in preview mode and waits for another input from the player.
3. The player confirms his request and the script checks if the tower can be placed at the chosen spot _(`GridData`)_, and if the player can afford it. Only then the tower is powered on.

#### Wave flow

A wave will automatically start when all enemies have been defeated or have reached the end of the path.

1. Before anything, `GameManager` will retrieve the enemies spawn point and the path taken by them.
2. `GameManager` will request the first wave at the start of the game when everything is initialized.
3. The `WaveManager` will go through each `SpawnGroup` and waits for each `Enemy` to return. Only then the `WaveManager` will request the next wave.

#### Game/Waves/Towers/Enemies Configuration

Each part can be easily modified through ScriptableObjects found in `ScriptableObjects/Settings`.

1. The `GameSettings` lets you setup the common settings a game could have _(Starting health and money)_. In addition, you can specify which tower is avaialble to the player during the game. Finally, you can configure the waves which will be sent.
2. `WaveSettings` lets you configure `SpawnGroups`. SpawnGroups are just a group of enemies in a wave. A wave could have multiple `SpawnGroups`, beware to setup properly the initial spawn delay.
3. `TowerSettings` and `EnemySettings` are simple ScriptableObjects which contains simple settings for these objects. Don't forget to add an `EnemyPoolSO` for an Enemy.

#### Login and Saving Scores (Removed from the build)

For saving the player's progress, I've used the Unity Authentication and Cloud Save package. Unfortunately, the package doesn't seem to work/throws an error on WebGL, so I've had to remove this part from the build.

### Known Bugs

1. The audio doesn't work on WebGL builds. Since I've made my audio system with AudioMixer which is one of the audio features that are not available specifically on WebGL, you won't be able to hear anything. Don't hesitate to try out the game in the editor!
2. For the login/save system, I've used the Unity Authentication and Cloud Save services. Unfortunately, these seems to throw errors in WebGL builds that I have not been able to solve.
3. There is a slight bug in game : If you try to preview a tower and want to switch preview or cancel your preview, the game will still try to buy the tower you tried to preview.