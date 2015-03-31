using UnityEngine;
using System.Collections;

public class Acceleration : MonoBehaviour {

	private float updateInterval = 1.0f / 60.0f;
	private float lowPassWidth = .5f;
	private float lowPassFilterFactor;
	private Vector3 lowPassValue = Vector3.zero;

	void Awake()
	{
		lowPassValue = Input.acceleration;
		lowPassFilterFactor = updateInterval / lowPassWidth;
	}

	void FixedUpdate()
	{
		print (Input.acceleration);

		Vector3 worldBounds = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height, 0));
		Vector3 newPosition = transform.position + Filtered();//the result of filtered method which is lowpassvalue

		transform.position = new Vector2 (Mathf.Clamp (newPosition.x, -worldBounds.x, worldBounds.x), Mathf.Clamp (newPosition.y, -worldBounds.y, worldBounds.y));
	}

	Vector3 Filtered()
	{
		lowPassValue = Vector3.Lerp (lowPassValue, Input.acceleration, lowPassFilterFactor);
		return lowPassValue;
	}
}