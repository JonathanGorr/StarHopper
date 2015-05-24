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
		restart,
		fire;

	public float rotationSpeed;

	private LevelManager manager;

	void Awake()
	{
		//components
		manager = GetComponent<LevelManager> ();

		//buttons
		leftButton = GameObject.Find("Left").GetComponent<UIButtonHold>();
		rightButton = GameObject.Find("Right").GetComponent<UIButtonHold>();
		upButton = GameObject.Find("Up").GetComponent<UIButtonHold>();
	}

	void Update () {

		if (!manager.paused) 
		{
			//input
			left = Input.GetKey (KeyCode.A) || leftButton.buttonHeld;
			right = Input.GetKey (KeyCode.D) || rightButton.buttonHeld;
			up = Input.GetKey (KeyCode.W) || upButton.buttonHeld;
			down = Input.GetKey (KeyCode.S);
			restart = Input.GetKeyDown (KeyCode.R);
			fire = Input.GetMouseButtonDown (0);
		}
		pause = Input.GetKeyDown (KeyCode.Escape);
	}
}
