using UnityEngine;
using System.Collections;

public class Planet 
{
	public string name;
	public Transform prefab;
	public int number;

	public Planet(string newName, Transform newPrefab, int newNumber)
	{
		name = newName;
		prefab = newPrefab;
		number = newNumber;
	}
}
