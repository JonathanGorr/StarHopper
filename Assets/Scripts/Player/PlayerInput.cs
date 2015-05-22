using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour {

	private UIButtonHold
		leftButton,
		rightButton,
		upButton;

	[HideInInspector] public bool
		left,
		right,
		up,
		down,
		pause,
		restart;

	public float rotationSpeed;

	void Awake()
	{
		//buttons
		leftButton = GameObject.Find("Left").GetComponent<UIButtonHold>();
		rightButton = GameObject.Find("Right").GetComponent<UIButtonHold>();
		upButton = GameObject.Find("Up").GetComponent<UIButtonHold>();
	}

	void Update () {

		//input
		left = Input.GetKey(KeyCode.A) || leftButton.buttonHeld;
		right = Input.GetKey (KeyCode.D) || rightButton.buttonHeld;
		up = Input.GetKey (KeyCode.W) || upButton.buttonHeld;
		down = Input.GetKey(KeyCode.S);
		pause = Input.GetKeyDown (KeyCode.Escape);
		restart = Input.GetKeyDown (KeyCode.R);
	}
}
