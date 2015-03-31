using UnityEngine;
using System.Collections;

public class ProjectileGravityPull : MonoBehaviour {
		
	public float maxGravDist = 4.0f;
	public float maxGravity = 35.0f;
	public float rotationSpeed = 100f;

	public Color 
		planetLinesColor,
		closestPlanetColor;

	public bool
		planetLines,
		closestPlanetLine;

	private GameObject closest;
	private Rigidbody2D rigidBody;
	
	GameObject[] planets;
	
	void Awake () {
		planets = GameObject.FindGameObjectsWithTag("Planet");
	}

	void FixedUpdate () {

		//closest planet
		float closestDist = Mathf.Infinity;

		//do to each planet in tag array
		foreach(GameObject planet in planets) {

			//what is distance?
			float dist = Vector3.Distance(planet.transform.position, transform.position);
			//how big is the planet?
			float planetSize = planet.transform.localScale.x;

			//if player is within gravitational pull radius, apply force in direction of planet
			if (dist <= maxGravDist) {
				//direction
				Vector3 v = planet.transform.position - transform.position;
				//force
				GetComponent<Rigidbody2D>().AddForce(v.normalized * (1.0f - dist / maxGravDist) * maxGravity);
			}

			//if current planet distance is the least...look at it
			if (dist <= closestDist)
			{
				closestDist = dist;
				closest = planet;
			}
		}
		//rotate towards nearest planet
		Vector3 vectorToTarget = closest.transform.position - transform.position;
		float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) + 90;
		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationSpeed);
	}

	void OnDrawGizmos()
	{
		if(closestPlanetLine)
		{
			Gizmos.color = closestPlanetColor;
			Gizmos.DrawLine(transform.position, closest.transform.position);
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
