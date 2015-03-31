using UnityEngine;
using System.Collections;

//	Attach this script to the Main Camera.
//	This script will set the transform values for the GameObject it is attached to.
public class ProjectileFollow : MonoBehaviour {

	public Transform projectile;        // The transform of the projectile to follow.
	public Transform top;           // The transform representing the left bound of the camera's position.
	public Transform bottom;          // The transform representing the right bound of the camera's position.
	public float offsetY;

	void Update () {
		// Store the position of the camera.
		Vector3 newPosition = transform.position;

		Vector3 offset = new Vector3(0, offsetY, 0);

		// Set the y value of the stored position to that of the bird.
		newPosition.y = projectile.position.y;
		
		// Clamp the y value of the stored position between the left and right bounds.
		newPosition.y = Mathf.Clamp (newPosition.y, bottom.position.y, top.position.y);
		
		// Set the camera's position to this stored position.
		transform.position = newPosition + offset;
	}
}
