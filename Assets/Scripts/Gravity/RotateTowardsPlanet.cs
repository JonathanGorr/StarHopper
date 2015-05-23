using UnityEngine;
using System.Collections;

public class RotateTowardsPlanet : MonoBehaviour {
		
	public float
		rotationSpeed = 100f,
		tooFarDistance = 20f;

	public Color 
		planetLinesColor,
		closestPlanetColor;

	public bool
		planetLines,
		closestPlanetLine;

	[HideInInspector] public bool tooFar;

	private GameObject closest;
	private Rigidbody2D rigidBody;
	
	GameObject[] planets;
	
	void Start () {

		//get all the planets
		planets = GameObject.FindGameObjectsWithTag("Planet");
	}

	void FixedUpdate () {

		//closest planet distance
		float closestDist = Mathf.Infinity;

		//find the closest planet
		foreach(GameObject planet in planets) {

			//get their distance
			float dist = Vector3.Distance(planet.transform.position, transform.position);

			//if current planet distance is the least...look at it
			if (dist <= closestDist)
			{
				closestDist = dist;
				closest = planet;
			}
		}

		//if there is no planet within reach, transition to free-rotate mode
		if(closestDist > tooFarDistance)
		{
			tooFar = true;
		}
		else
		{
			//rotate towards nearest planet
			Vector3 vectorToTarget = closest.transform.position - transform.position;
			float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) + 90;
			Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
			transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationSpeed);
			tooFar = false;
		}
	}

	void OnDrawGizmos()
	{
		if(closestPlanetLine)
		{
			Gizmos.color = closestPlanetColor;
			if(closest) Gizmos.DrawLine(transform.position, closest.transform.position);
		}

		if(planetLines)
		{
			foreach(GameObject planet in planets)
			{
				Gizmos.color = planetLinesColor;
				Gizmos.DrawLine(planet.transform.position, transform.position);
			}
		}
	}
}
