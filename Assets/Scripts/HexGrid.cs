using UnityEngine;
using System.Collections;

public class HexGrid : MonoBehaviour {
	
	public Transform 
		start,
		end,
		merchant,
		poison,
		repulse;
	
	public int spawnX = 5;
	public int spawnY = 5;

	public float radius = 0.5f;
	public bool useAsInnerCircleRadius;

	private bool startSpawned, endSpawned;
	
	private float offsetX, offsetY;
	
	void Awake()
	{
		float unitLength = ( useAsInnerCircleRadius )? (radius / (Mathf.Sqrt(3)/2)) : radius;
		
		offsetX = unitLength * Mathf.Sqrt(3);
		offsetY = unitLength * 1.5f;

		for( int i = 0; i < spawnX; i++ )
		{
			for( int j = 0; j < spawnY; j++ )
			{
				Vector2 hexpos = HexOffset( i, j );
				Vector3 pos = new Vector3( hexpos.x, hexpos.y, 0 );

				switch(RandomNumber())
				{
					case 1:
						if(!startSpawned){
							Instantiate( start, pos, Quaternion.identity);
							startSpawned = true;
						}
						break;

					case 2:
						if(!endSpawned){
							Instantiate( end, pos, Quaternion.identity);
							endSpawned = true;
						}
						break;

					case 3:
						Instantiate( merchant, pos, Quaternion.identity);
						break;
					case 4:
						Instantiate( poison, pos, Quaternion.identity);
						break;
					case 5:
						Instantiate( repulse, pos, Quaternion.identity);
						break;
				}
			}
		}
	}

	int RandomNumber()
	{
		return Random.Range(0, 6);
	}
	
	Vector2 HexOffset( int x, int y ) {
		Vector2 position = Vector2.zero;
		
		if( y % 2 == 0 ) {
			position.x = x * offsetX;
			position.y = y * offsetY;
		}
		else {
			position.x = ( x + 0.5f ) * offsetX;
			position.y = y * offsetY;
		}
		
		return position;
	}
}
