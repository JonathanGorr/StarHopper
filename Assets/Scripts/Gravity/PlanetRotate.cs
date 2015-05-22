using UnityEngine;
using System.Collections;

public class PlanetRotate : MonoBehaviour {

	public float rotationSpeed = 5f;
	private Transform myTransform;
	private bool left;

	void Awake()
	{
		myTransform = transform;

		//pick random direction of rotation
		left = (Random.value < 0.5) ? true : false;
	}

	// Update is called once per frame
	void FixedUpdate () {

		if(left)
			myTransform.Rotate(0f,0f,rotationSpeed);
		else
			myTransform.Rotate(0f,0f, -rotationSpeed);
	}
}
