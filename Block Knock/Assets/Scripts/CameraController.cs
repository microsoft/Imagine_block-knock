using UnityEngine;
using System.Collections;

/// <summary>
/// Script to control the camera rotation when the player drags with the right
/// mouse button. The camera will rotate around a target's position based on the
/// transform component parameter.
/// </summary>
public class CameraController : MonoBehaviour
{
	public Transform targetTransform;	// The transform component of the camera's view target.
	public float sensitivity;			// Controls how much to rotate based on the mouse movement.

	void Update()
	{
		// *** Add your source code here ***
	}
}
