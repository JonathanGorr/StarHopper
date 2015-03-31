using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

	private SFX sfx;
	private LevelManager manager;

	void Awake()
	{
		sfx = GameObject.Find("LevelManager").GetComponent<SFX>();
		manager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Player")
		{
			if(gameObject.tag == "Coin")
			{
				manager.AddToScore("Coins");
				sfx.GetComponent<AudioSource>().PlayOneShot(sfx.collect[Random.Range(0, sfx.collect.Length)]);
				Destroy (gameObject);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.tag == "Player")
		{
			if(gameObject.tag == "Part")
			{
				manager.AddToScore("Parts");
				sfx.GetComponent<AudioSource>().PlayOneShot(sfx.collect[Random.Range(0, sfx.collect.Length)]);
				Destroy (gameObject);
			}
		}
	}
}
