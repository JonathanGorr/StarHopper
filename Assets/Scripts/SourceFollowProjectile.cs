using UnityEngine;
using System.Collections;

public class SourceFollowProjectile : MonoBehaviour {

	private Transform projectile;
	private ProjectileDragging projectileDragging;

	// Use this for initialization
	void Awake () {
		projectile = transform.parent.Find("PhysicsOBJ");
		projectileDragging = projectile.GetComponent<ProjectileDragging>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		//if not dragging, follow
		if(projectileDragging.dragging == false)
		{
			transform.position = projectile.transform.position;
		}
	}
}
