using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Script for controlling when and where to shoot a new ball projectile.
/// </summary>
public class ProjectileController : MonoBehaviour
{
	public GameObject projectile;	// The projectile prefab to be spawn.
	public AudioClip ballShoot;		// The sound to be played when shooting the projectile.
	public float strength;			// The amount of force applied to the projectile.

	AudioSource ballShootSource;

	void Start()
	{
		// Create audio sources for sound playback.
		ballShootSource = AudioHelper.CreateAudioSource(gameObject, ballShoot);
	}

	void Update()
	{
		// *** Add your source code here ***
	}
}
