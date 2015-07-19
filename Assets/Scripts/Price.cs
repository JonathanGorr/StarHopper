using UnityEngine;
using System.Collections;

public class Price : MonoBehaviour {

	private GameObject canvas;

	void Awake()
	{
		canvas = transform.Find ("Cost").transform.gameObject;
		canvas.SetActive (false);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
			canvas.SetActive (true);
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Player")
			canvas.SetActive (false);
	}
}
