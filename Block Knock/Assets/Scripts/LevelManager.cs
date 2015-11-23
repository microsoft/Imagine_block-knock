using UnityEngine;
using System.Collections;

/// <summary>
/// The level manager is responsible for spawning blocks for the different levels. It can
/// also determine the player rank based on the current throw count. The level manager is a
/// singleton and can be accessed in any script using the LevelManager.Instance syntax.
/// </summary>
public class LevelManager : MonoBehaviour
{
	// The static singleton instance of the level manager.
	public static LevelManager Instance;

	[System.Serializable]
	public class Level
	{
		public int goldRequirement;		// Minimum number of throws for Gold rank.
		public int silverRequirement;	// Minimum number of throws for Silver rank.
		public GameObject prefab;		// Prefab for this level.
	};

	public Level[] levels;				// Array of level data.
	public Transform spawnTransform;	// The spawn location for the blocks.

	GameObject currentLevel;	// Reference to the current level game object.

	void Awake()
	{
		// Register this script as the singleton instance.
		Instance = this;
	}

	/// <summary>
	/// Spawns the prefab for the blocks of a given level.
	/// </summary>
	/// <returns>The number of good blocks for the given level.</returns>
	/// <param name="level">The level to be spawned.</param>
	public int StartLevel(int level)
	{
		// Clear objects from previous level first.
		if (currentLevel)
		{
			Destroy(currentLevel);
		}

		// Spawn the new level. We are subtracting 1 since levels start at 1 but
		// array index starts at 0.
		int index = Mathf.Clamp(level - 1, 0, levels.Length - 1);
		currentLevel = Instantiate(levels[index].prefab, spawnTransform.position, spawnTransform.rotation) as GameObject;

		// Count the number of good blocks for this level.
		int goodBlockCount = 0;
		foreach (Transform child in currentLevel.transform)
		{
			if (child.CompareTag("GoodBlock"))
			{
				++goodBlockCount;
			}
		}
		return goodBlockCount;
	}

	/// <summary>
	/// Gets the rank for the given level based on the throw count.
	/// </summary>
	/// <returns>The player rank.</returns>
	/// <param name="level">The level to be evaluated.</param>
	/// <param name="throws">The number of throws attempted.</param>
	public Rank GetRank(int level, int throws)
	{
		// Set default rank to Bronze.
		Rank rank = Rank.Bronze;

		// Check throw limit for Gold rank.
		if (throws <= levels[level - 1].goldRequirement)
		{
			rank = Rank.Gold;
		}
		// Check throw limit for Silver rank.
		else if (throws <= levels[level - 1].silverRequirement)
		{
			rank = Rank.Silver;
		}
		return rank;
	}
}