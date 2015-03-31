using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour {

	private bool isMoving;

	public void OnTouchDown(){
		//print ("TOUCHING");
		isMoving = true;
	}

	public void OnTouchUp(){
		//print ("RELEASED");
		isMoving = false;
	}

	public void OnTouchStay(){
		//print ("OVER");
	}

	public void OnTouchMove(Vector2 touchPos){
		//print ("MOVING");
		if (isMoving)
			transform.position = touchPos;
	}

	public void OnTouchExit(){
		//print ("CANCELLED");
	}
}
