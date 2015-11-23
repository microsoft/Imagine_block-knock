using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// The user interface (UI) manager is responsible for controlling which screen to display
/// as well as updating the current game heads up display (HUD). The UI manager is a singleton
/// and can be accessed in any script using the UIManager.Instance syntax.
/// </summary>
public class UIManager : MonoBehaviour
{
	// The static singleton instance of the UI manager.
	public static UIManager Instance { get; private set; }

	public Text levelText;			// HUD text for the current level.
	public Text throwText;			// HUD text for the throw counter.
	public Text rankText;			// HUD text for the player rank.
	public GameObject[] screens;	// GameObject array for all the screens.
	public GameObject hud;			// GameObject for the HUD.
	public AudioClip buttonClick;	// Sound when a button is clicked.

	AudioSource buttonClickSource;

	void Awake()
	{
		// Register this script as the singleton instance.
		Instance = this;
	}

	void Start()
	{
		// Create audio sources for sound playback.
		buttonClickSource = AudioHelper.CreateAudioSource(gameObject, buttonClick);
	}

	/// <summary>
	/// Shows the screen with the given name and hide everything else.
	/// </summary>
	/// <param name="name">Name of the screen to be shown.</param>
	public void ShowScreen(string name)
	{
		// Loop through all the screens in the array.
		foreach (GameObject screen in screens)
		{
			// Activate the screen with the matching name, and deactivate
			// any screen that doesn't match.
			screen.SetActive(screen.name == name);
		}
	}

	/// <summary>
	/// Shows/hides the HUD.
	/// </summary>
	/// <param name="show">Do we show the HUD?</param>
	public void ShowHUD(bool show)
	{
		hud.SetActive(show);
	}

	/// <summary>
	/// Updates the heads up display.
	/// </summary>
	/// <param name="level">The current level.</param>
	/// <param name="throws">The number of throws so far.</param>
	/// <param name="rank">The player's current rank.</param>
	public void UpdateHUD(int level, int throws, Rank rank)
	{
		levelText.text = string.Format(LocalizationManager.Instance.GetString("HUD Level"), level.ToString());
		throwText.text = string.Format(LocalizationManager.Instance.GetString("HUD Throws"), throws.ToString());
		rankText.text = string.Format(LocalizationManager.Instance.GetString("HUD Rank"), GetRankString(rank));
	}

	string GetRankString(Rank rank)
	{
		return LocalizationManager.Instance.GetString(string.Format("Rank {0}", rank.ToString()));
	}

	/// <summary>
	/// Call this function to play the button click sound.
	/// </summary>
	public void OnButton()
	{
		buttonClickSource.Play();
	}

	public void OnLanguageChanged()
	{
		foreach(GameObject o in screens)
		{
			var staticText = o.GetComponent<StaticTextManager>();
			if (staticText)
			{
				staticText.OnLanguageChanged();
			}
		}
	}

	public bool IsMouseOverUI()
	{
		return (EventSystem.current.IsPointerOverGameObject()
			&& EventSystem.current.currentSelectedGameObject != null);
	}
}