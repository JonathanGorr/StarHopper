using UnityEngine;
using System.Collections;

public class StarX : MonoBehaviour {

	public GameObject prefab;

	void OnTouchDown(){
		NewStar ();
	}

	void NewStar(){
		Vector3 randomPosition = new Vector3 (Random.Range(0,Screen.width), Random.Range(0, Screen.height), 0f);
		randomPosition = Camera.main.ScreenToWorldPoint (randomPosition);
		randomPosition.z = 0;

		Instantiate (prefab, randomPosition, Quaternion.identity);
		Destroy (gameObject);
	}
}
