using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

	private SFX sfx;
	private LevelManager manager;

	public int
		cost = 5;

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

			if(gameObject.tag == "Gun")
			{
				StartCoroutine("GetGun");
			}
		}
	}

	IEnumerator GetGun()
	{
		yield return StartCoroutine(WaitForKeyDown(KeyCode.E));

		if (manager.coins >= cost)
		{
			manager.SubtractCoins (cost);
			sfx.GetComponent<AudioSource> ().PlayOneShot (sfx.collect [Random.Range (0, sfx.collect.Length)]);
			Destroy (gameObject);
		}
		else
		{
			print("Dont have enough money");
			//play error sound//sfx.GetComponent<AudioSource> ().PlayOneShot (sfx.collect [Random.Range (0, sfx.collect.Length)]);
		}
	}
	
	IEnumerator WaitForKeyDown(KeyCode keyCode)
	{
		while (!Input.GetKeyDown(keyCode))
			yield return null;
	}
	
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Planet")
		{
			transform.parent = other.transform;
		}

		else if(other.gameObject.tag == "Player")
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
