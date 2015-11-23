using UnityEngine;
using System.Collections;

/// <summary>
/// Script to control sound playback for the ball projectile when it comes in contact with
/// other game objects in the level.
/// </summary>
public class BallSound : MonoBehaviour
{
	public AudioClip ballHitBlock;	// Sound when the ball hits any block.
	public AudioClip ballHitTable;	// Sound when the ball hits the table.
	public AudioClip ballRoll;		// Sound when the ball rolls on the table.
	
	AudioSource ballHitBlockSource;
	AudioSource ballHitTableSource;
	AudioSource ballRollSource;

	void Start()
	{
		// Create audio sources for sound playback.
		ballHitBlockSource = AudioHelper.CreateAudioSource(gameObject, ballHitBlock);
		ballHitTableSource = AudioHelper.CreateAudioSource(gameObject, ballHitTable);
		ballRollSource = AudioHelper.CreateAudioSource(gameObject, ballRoll);
	}

	void OnCollisionEnter(Collision collision)
	{
		// Check if the ball hits a good block or a bad block.
		if (collision.gameObject.CompareTag("GoodBlock") || collision.gameObject.CompareTag("BadBlock"))
		{
			ballHitBlockSource.Play();
		}
		// Check if the ball hits the table.
		else if (collision.gameObject.CompareTag("Table"))
		{
			ballHitTableSource.Play();
			ballRollSource.Play();
		}
	}

	void OnCollisionExit(Collision collision)
	{
		// If the ball rolls off the table, stop the rolling sound.
		if (collision.gameObject.CompareTag("Table"))
		{
			ballRollSource.Stop();
		}
	}
}
