using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// The gameplay manager is responsible for controlling the overall flow of the game. The
/// game is divided into three main states: Tutorial, InGame, and GameOver. The user interface
/// and input controls are different depending on the current game state. The gameplay
/// manager tracks the player progress and switches between the game states based on
/// the results as well as the user input. The gameplay manager is a singleton and can be
/// accessed in any script using the GameplayManager.Instance syntax.
/// </summary>
public class GameplayManager : MonoBehaviour
{
	// The static singleton instance of the gameplay manager.
	public static GameplayManager Instance { get; private set; }

	// Enumeration for the different game states. The default starting
	// state is the tutorial.
	enum GameState
	{
		Tutorial,	// Show player the game instructions.
		InGame,		// Player can start shooting with the left mouse button.
		GameOver,	// Game ended, player input is blocked.
	};
	GameState state = GameState.Tutorial;
	
	Rank rank = Rank.Gold;		// The rank of the player for the current level.
	int currentLevel = 1;		// The current level the player is playing.
	int throws = 0;				// The number of throws attempted for the current level.
	int goodBlockCount = 0;		// The total number of good blocks for the current level.
	int goodBlockCaught = 0;	// The number of good blocks knocked off the table so far.

	void Awake()
	{
		// Register this script as the singleton instance.
		Instance = this;
	}

	void Start()
	{
		// Refresh the HUD and show the tutorial screen.
		UIManager.Instance.UpdateHUD(currentLevel, throws, rank);
		UIManager.Instance.ShowHUD(false);
		UIManager.Instance.ShowScreen("Tutorial");
		goodBlockCount = LevelManager.Instance.StartLevel(currentLevel);
	}

	/// <summary>
	/// Evaluates the player result and show the proper UI screen.
	/// </summary>
	void CheckResult()
	{
		// Did the player knock all the good blocks off the table?
		if (goodBlockCaught == goodBlockCount)
		{
			// Is this the last level?
			if (currentLevel == LevelManager.Instance.levels.Length)
			{
				// The player has completed the entire game.
				UIManager.Instance.ShowScreen("Game Complete");
			}
			else
			{
				// The player has completed the current level.
				UIManager.Instance.ShowScreen("Level Complete");
			}
		}
		else
		{
			// The player has lost.
			UIManager.Instance.ShowScreen("Game Over");
		}
	}

	/// <summary>
	/// Reloads the current scene.
	/// </summary>
	void ReloadScene()
	{
		Application.LoadLevel(Application.loadedLevel);
	}

	/// <summary>
	/// Call this function to start the gameplay.
	/// </summary>
	public void OnStartGame()
	{
		state = GameState.InGame;
		UIManager.Instance.ShowHUD(true);
		UIManager.Instance.ShowScreen("");
	}

	/// <summary>
	/// Call this function to reload the current level. The player progress will be reset.
	/// </summary>
	public void OnRetryLevel()
	{
		// Clear all the balls shot so the table is clear.
		GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
		foreach (GameObject ball in balls)
		{
			Destroy(ball);
		}

		// Respawn new blocks for the current level.
		goodBlockCount = LevelManager.Instance.StartLevel(currentLevel);
		
		// Start gameplay and refresh the HUD.
		rank = Rank.Gold;
		throws = 0;
		goodBlockCaught = 0;
		UIManager.Instance.UpdateHUD(currentLevel, throws, rank);
		UIManager.Instance.ShowScreen("");
		Invoke("OnStartGame", 0.5f);
	}

	/// <summary>
	/// Call this function to advance to the next level.
	/// </summary>
	public void OnNextLevel()
	{
		// Update the current level number but make sure we don't go over the
		// total number of levels.
		currentLevel = (currentLevel == LevelManager.Instance.levels.Length) ? 0 : currentLevel + 1;

		// Call retry level since the logic is essentially the same.
		OnRetryLevel();
	}

	/// <summary>
	/// Call this function to restart the current level.
	/// </summary>
	public void OnRestart()
	{
		// Reload the current scene.
		Invoke("ReloadScene", 0.5f);
	}

	/// <summary>
	/// Call this function when a good block falls off the table.
	/// </summary>
	public void OnGoodBlockCaught()
	{
		// Is the game over yet? We don't want to count blocks after the game is over.
		if (state == GameState.InGame)
		{
			// Increment the good block counter.
			++goodBlockCaught;

			// Check if we caught all the good blocks
			if (goodBlockCaught == goodBlockCount)
			{
				// Switch to the game over state and evaluate the results.
				state = GameState.GameOver;
				Invoke("CheckResult", 0.5f);
			}
		}
	}

	/// <summary>
	/// Call this function when a bad block falls off the table.
	/// </summary>
	public void OnBadBlockCaught()
	{
		// Is the game over yet? We don't want to end the game multiple times.
		if (state == GameState.InGame)
		{
			// Switch to the game over state and evaluate the results.
			state = GameState.GameOver;
			Invoke("CheckResult", 0.5f);
		}
	}

	/// <summary>
	/// Call this function to notify the manager that a ball is thrown.
	/// </summary>
	public void OnThrow()
	{
		// Increment the throw counter.
		++throws;

		// Evaluate the player's current rank based on the throw count for the current level.
		rank = LevelManager.Instance.GetRank(currentLevel, throws);

		// Refresh the game HUD.
		UIManager.Instance.UpdateHUD(currentLevel, throws, rank);
	}

	/// <summary>
	/// Determines whether the player can start throwing balls.
	/// </summary>
	/// <returns><c>true</c> if the player can throw; otherwise, <c>false</c>.</returns>
	public bool CanThrow()
	{
		// The player can throw balls only during the InGame state.
		return (state == GameState.InGame);
	}

	public void OnLanguageChanged()
	{
		UIManager.Instance.OnLanguageChanged();
		UIManager.Instance.UpdateHUD(currentLevel, throws, rank);
	}
}