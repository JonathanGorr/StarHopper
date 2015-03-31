using UnityEngine;
using System.Collections;

public class SpriteRotation : MonoBehaviour {

	public Transform planet;

	void FixedUpdate()
	{
		/*
		Vector2 dir = planet - transform.position;
		Quaternion angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		*/
	}
}
