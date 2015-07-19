using UnityEngine;
using System.Collections;

public class PlayerPreferences : MonoBehaviour {
	
	private LevelManager manager;

	private bool
		gun;

	private int 
		health,
		maxHealth;

	void Awake()
	{
		//components and stuff
		manager = GetComponent<LevelManager> ();

		//coins
		manager.coins = PlayerPrefs.GetInt("Coins");

		//if a game has been created, load health
		if(PlayerPrefs.GetInt("GameCreated") == 1)
		{
			//_health.health = PlayerPrefs.GetInt("Health");
		}

		//else if a new game has just been created, set health to max
		else
		{
			//_health.health = _health.maxHealth;
			GameCreated();
		}
	}

	public void SaveStats(float x, float y, int blood, int health)
	{
		PlayerPrefs.SetInt ("Blood", blood);
		PlayerPrefs.SetInt ("Health", health);
		PlayerPrefs.Save();
	}

	public void GameCreated()
	{
		PlayerPrefs.SetInt("GameCreated", 1);
		PlayerPrefs.Save();
	}

	public void ItemsGet()
	{
		PlayerPrefs.SetInt("ItemsGet", 1);
		PlayerPrefs.Save();
	}

	public void EraseAll()
	{
		PlayerPrefs.DeleteAll();
	}
}
