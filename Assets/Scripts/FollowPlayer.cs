using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	private Transform player;
	public Vector3 offset;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		transform.position = player.position;
	}

	void Update()
	{
		if(player) transform.position = player.position + offset;
	}
}
