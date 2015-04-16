using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	[HideInInspector]
	public Vector2 moving = new Vector2();

	private UIButtonHold
		left,
		right,
		up;

	private bool
		leftHeld,
		rightHeld,
		upHeld;

	void Awake()
	{
		left = GameObject.Find("Left").GetComponent<UIButtonHold>();
		right = GameObject.Find("Right").GetComponent<UIButtonHold>();
		up = GameObject.Find("Up").GetComponent<UIButtonHold>();
	}

	void Update () {

		//reset
		moving.x = moving.y = 0;

		//horizontal
		if(Input.GetKey(KeyCode.D) || right.buttonHeld)
		{
			moving.x = 1;
		}
		else if(Input.GetKey(KeyCode.A) || left.buttonHeld)
		{
			moving.x = -1;
		}

		//vertical
		if(Input.GetKey(KeyCode.W) || up.buttonHeld)
		{
			moving.y = 1;
		}
		else if(Input.GetKey(KeyCode.S))
		{
			moving.y = -1;
		}
	}
}
