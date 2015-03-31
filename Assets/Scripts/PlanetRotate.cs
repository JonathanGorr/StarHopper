using UnityEngine;
using System.Collections;

public class PlanetRotate : MonoBehaviour {

	public float rotationSpeed = 5f;
	private Transform myTransform;

	void Awake()
	{
		myTransform = transform;
	}

	// Update is called once per frame
	void FixedUpdate () {
		myTransform.Rotate(0f,0f,rotationSpeed);
	}
}
