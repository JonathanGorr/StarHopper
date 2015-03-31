using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UIButtonHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[HideInInspector]
	public bool buttonHeld;
	
	public void OnPointerDown(PointerEventData eventData)
	{
		buttonHeld = true;
	}
	
	public void OnPointerUp(PointerEventData eventData)
	{
		buttonHeld = false;
	}
}
