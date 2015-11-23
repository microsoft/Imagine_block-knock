using UnityEngine;
using System.Collections;

/// <summary>
/// Script to control sound playback for the blocks when it comes in contact with
/// other game objects in the level.
/// </summary>
public class BlockSound : MonoBehaviour
{
	public AudioClip blockHitBlock;	// Sound when the block hits other blocks.
	public AudioClip blockHitTable;	// Sound when the block hits the table.
	
	AudioSource blockHitBlockSource;
	AudioSource blockHitTableSource;

	void Start()
	{
		// Create audio sources for sound playback.
		blockHitBlockSource = AudioHelper.CreateAudioSource(gameObject, blockHitBlock);
		blockHitTableSource = AudioHelper.CreateAudioSource(gameObject, blockHitTable);
	}
	
	void OnCollisionEnter(Collision collision)
	{
		// Only allow block contact sounds during gameplay. This prevents the loud "clunk"
		// when the blocks are initially spawned into the level.
		if (GameplayManager.Instance.CanThrow())
		{
			// Check if the block hits a good block or a bad block.
			if (collision.gameObject.CompareTag("GoodBlock") || collision.gameObject.CompareTag("BadBlock"))
			{
				blockHitBlockSource.Play();
			}
			// Check if the ball hits the table.
			else if (collision.gameObject.CompareTag("Table"))
			{
				blockHitTableSource.Play();
			}
		}
	}
}
