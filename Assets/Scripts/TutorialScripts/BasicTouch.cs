using UnityEngine;
using System.Collections;

using UnityEngine.UI; // Need this for Text

public class BasicTouch : MonoBehaviour {

	private Text touchCountText, touchPositionText, tapCountText, swipeText; // UI Text
	private string directionText; // Store swipe direction text

	void Start () {
		touchCountText = GameObject.Find ("TouchCountText").GetComponent<Text>();
		touchPositionText = GameObject.Find ("TouchPositionText").GetComponent<Text>();
		tapCountText = GameObject.Find ("TapText").GetComponent<Text>();
		swipeText = GameObject.Find ("SwipeText").GetComponent<Text>();
	}
	
	void Update () {
		print (Input.touchCount);//how many input occuring on screen
		touchCountText.text = "Touches " + Input.touchCount.ToString ("00");

		if(Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			print(touch.position);
			touchPositionText.text = "Position x:" + touch.position.x + "y:" + touch.position.y;
			tapCountText.text = "Tap count: " + touch.tapCount.ToString("000");

			if(Mathf.Abs(touch.deltaPosition.x) > Mathf.Abs(touch.deltaPosition.y))
			{
				if(touch.deltaPosition.x > 0) 
					directionText = "R";
				else 
					directionText = "L";
			}

			else
			{
				if(touch.deltaPosition.y > 0) 
					directionText = "U";
				else 
					directionText = "D";
			}
				swipeText.text = "Swipe: " + directionText;
			}
		}
	}
