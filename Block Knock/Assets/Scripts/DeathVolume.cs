using UnityEngine;
using System.Collections;

/// <summary>
/// Script to destroy any game object caught by the trigger volume.
/// </summary>
public class DeathVolume : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		// Destroy anything caught by this volume.
		Destroy(other.gameObject);
	}
}
