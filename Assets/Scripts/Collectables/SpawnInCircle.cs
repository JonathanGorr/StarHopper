using UnityEngine;
using System.Collections;

public class SpawnInCircle : MonoBehaviour {
	
	public int numPoints = 20, coinRotOffset = 30;
	private Vector3 centerPos;
	
	public GameObject coinPrefab;
	public GameObject[] propPrefabs, partPrefabs;

	private float radiusX,radiusY;
	public float offset;
	
	public bool 
		isCircular,
		vertical = true,
		spawnCoins,
		spawnProps,
		spawnPart;

	private CircleCollider2D circleCollider;
	
	Vector3 pointPos;

	void Awake () {

		circleCollider = transform.Find("Planet").GetComponent<CircleCollider2D>();

		//radius is circle collider radius * the scale of the sprite
		radiusY = radiusX = circleCollider.radius * transform.localScale.x + offset;

		centerPos = transform.position;

		for(int i = 0; i<numPoints;i++){
			//multiply 'i' by '1.0f' to ensure the result is a fraction
			float pointNum = (i*1.0f)/numPoints;
			
			//angle along the unit circle for placing points
			float angle = pointNum*Mathf.PI*2;
			
			float x = Mathf.Sin (angle)*radiusX;
			float y = Mathf.Cos (angle)*radiusY;
			
			//position for the point prefab
			if(vertical)
				pointPos = new Vector3(x, y)+centerPos;
			else if (!vertical)
				pointPos = new Vector3(x, 0, y)+centerPos;

			if(spawnCoins)
			{
				//place the prefab at given position
				GameObject coin = Instantiate (coinPrefab, pointPos, Quaternion.identity) as GameObject;

				coin.transform.parent = transform.Find("Coins").transform;

				//rotate towards nearest planet
				Vector3 vectorToTarget = coin.transform.parent.transform.position - coin.transform.position; //planet vs prop
				float propAngle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) + 90;
				Quaternion q = Quaternion.AngleAxis(propAngle, Vector3.forward);
				coin.transform.rotation = q;
			}

			if(spawnProps)
			{
				//place a random prop at given position
				GameObject prop = Instantiate (propPrefabs[Random.Range(0, propPrefabs.Length)], pointPos, Quaternion.identity) as GameObject;

				//parent it
				prop.transform.parent = transform.Find("Props").transform;

				//rotate towards nearest planet
				Vector3 vectorToTarget = prop.transform.parent.transform.position - prop.transform.position; //planet vs prop
				float propAngle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) + 90;
				Quaternion q = Quaternion.AngleAxis(propAngle, Vector3.forward);
				prop.transform.rotation = q;
			}
		}

		//only spawn 1 part per planet
		if(spawnPart)
		{
			//place a random prop at given position
			GameObject part = Instantiate (partPrefabs[Random.Range(0, partPrefabs.Length)], pointPos, Quaternion.identity) as GameObject;
			
			//parent it
			part.transform.parent = transform.parent;
			
			//rotate towards nearest planet
			Vector3 vectorToTarget = part.transform.parent.transform.position - part.transform.position; //planet vs prop
			float propAngle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) + 90;
			Quaternion q = Quaternion.AngleAxis(propAngle, Vector3.forward);
			part.transform.rotation = q;
		}
		
		//keeps radius on both axes the same if circular
		if(isCircular){
			radiusY = radiusX;
		}
	}
}