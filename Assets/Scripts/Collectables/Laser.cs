using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

	public GameObject explosion;
	public float time;

	void Update()
	{
		time -= Time.deltaTime;

		if(time < 0)
		{
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		Instantiate (explosion, other.contacts[0].point, Quaternion.identity);
		Destroy (gameObject);
	}
}