using UnityEngine;
using System.Collections;

public class SingleTouch : MonoBehaviour {

	public LayerMask touchLayer;
	
	void Update () {
		if(Input.touchCount > 0)
		{
			CheckTouch(Input.GetTouch(0));
		}
	}

	void CheckTouch(Touch touch)
	{
		Vector3 screenPos = Camera.main.ScreenToWorldPoint (touch.position);
		Vector2 touchPos = new Vector2 (screenPos.x, screenPos.y);
		Collider2D other = Physics2D.OverlapPoint (touchPos, touchLayer);

		if(other)//= if != null
		{
			if(touch.phase == TouchPhase.Began)
			{
				other.SendMessage("OnTouchDown", 0, SendMessageOptions.DontRequireReceiver);//<--suppresses warning/error
			}

			if(touch.phase == TouchPhase.Ended)
			{
				other.SendMessage("OnTouchUp", 0, SendMessageOptions.DontRequireReceiver);//<--suppresses warning/error
			}

			if(touch.phase == TouchPhase.Stationary)
			{
				other.SendMessage("OnTouchStay", 0, SendMessageOptions.DontRequireReceiver);//<--suppresses warning/error
			}

			if(touch.phase == TouchPhase.Moved)
			{
				other.SendMessage("OnTouchMove", touchPos, SendMessageOptions.DontRequireReceiver);//<--suppresses warning/error
			}

			if(touch.phase == TouchPhase.Canceled)
			{
				other.SendMessage("OnTouchExit", 0, SendMessageOptions.DontRequireReceiver);//<--suppresses warning/error
			}
		}

	}

}
