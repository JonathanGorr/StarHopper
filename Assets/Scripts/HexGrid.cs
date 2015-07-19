using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexGrid : MonoBehaviour {
	
	public Transform
		start,
		end,
		merchant,
		poison,
		repulse,
		money;

	//Make a list
	private List<Planet> planets = new List<Planet>();
	
	public int spawnX = 5;
	public int spawnY = 5;

	public int
		poisonCount = 3,
		repulsiveCount = 2,
		moneyCount = 3;

	public float radius = 0.5f, delay = 1f;
	public bool useAsInnerCircleRadius;

	private bool
		startSpawned,
		merchantSpawned,
		endSpawned,
		canSpawn;
	
	private float offsetX, offsetY;

	private int planet;
	
	void Awake()
	{
		//add to list
		planets.Add ( new Planet("Start", start, 1));
		planets.Add ( new Planet("End", end, 1));
		planets.Add ( new Planet("Merchant", merchant, 1));
		planets.Add ( new Planet("Poison", poison, poisonCount));
		planets.Add ( new Planet("Repulse", repulse, repulsiveCount));
		planets.Add ( new Planet("Money", money, moneyCount));
			
		//Hex Grid spawning
		float unitLength = ( useAsInnerCircleRadius )? (radius / (Mathf.Sqrt(3)/2)) : radius;
		
		offsetX = unitLength * Mathf.Sqrt(3);
		offsetY = unitLength * 1.5f;
		
		for( int i = 0; i < spawnX; i++ )
		{
			for( int j = 0; j < spawnY; j++ )
			{
				Vector2 hexpos = HexOffset( i, j );
				Vector3 pos = new Vector3( hexpos.x, hexpos.y, 0 );
				
				if(planets.Count > 0)
					Spawn(pos);
			}
		}
	}

	void Spawn(Vector3 position)
	{
		int planet = Random.Range (0, planets.Count);
		
		//if the planet hasn't exceeded its spawn limit, spawn and count down...
		if (CheckNumber(planet))
		{
			Instantiate (planets [planet].prefab, position, Quaternion.identity);
			planets [planet].number --;
		}
		else
			planets.Remove(planets[planet]);
	}
	
	bool CheckNumber(int index)
	{
		bool truth = planets[index].number > 0 ? true : false;
		return truth;
	}
	
	int RandomNumber()
	{
		int number = Random.Range (0, planets.Count);
		return number;
	}

	Vector2 HexOffset( int x, int y ) {
		Vector2 position = Vector2.zero;
		
		if( y % 2 == 0 ) {
			position.x = x * offsetX;
			position.y = y * offsetY;
		}
		else {
			position.x = ( x + 0.5f ) * offsetX;
			position.y = y * offsetY;
		}
		
		return position;
	}
}
