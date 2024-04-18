using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// This class is a base class which contains what is common to all game scenes (Levels, Menus, Managers)
/// </summary>
public class GameSceneSO : DescriptionBaseSO
{
	public GameSceneType sceneType;
	public AssetReference sceneReference; // Used at runtime to load the scene from the right AssetBundle

	/// <summary>
	/// Used by the SceneSelector tool to discern what type of scene it needs to load
	/// </summary>
	public enum GameSceneType
	{
		// Playable scenes
		Level, // Loaded with PersistentManagers and GameplayManagers
		Menu, // Loaded with PersistentManagers

		// Special scenes
		Initialisation,
		PersistentManagers,
		GameplayManagers
	}
}