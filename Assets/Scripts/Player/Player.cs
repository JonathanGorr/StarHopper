using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public float 
		moveSpeed = 50,
		jumpSpeed = 100,
		jetSpeed = 15f,
		airSpeedMultiplier = 0.3f,
		duration = 5f,
		recoverSpeed = 1f,
		depleteSpeed = 1f;

	//sounds
	public AudioClip leftFootSound;
	public AudioClip rightFootSound;
	public AudioClip thudSound;
	public AudioClip rocketSound;

	public bool grounded;

	public Vector2 maxVelocity = new Vector2(3,5);
	public Transform groundedPosition;

	private Vector3 moveDirection;
	private PlayerController controller;
	private Animator anim;
	private Slider slider;
	private ParticleSystem particles;
	private LevelManager manager;
	
	public Color dyingColor;

	//rotation
	public float rotationSpeed = 100f;

	//gizmos
	public Color 
		planetLinesColor,
		closestPlanetColor;
	public bool
		planetLines,
		closestPlanetLine;

	//planets
	private GameObject closest;
	private Rigidbody2D rigidBody;
	GameObject[] planets;

	void Awake() {

		//find all planets
		planets = GameObject.FindGameObjectsWithTag("Planet");

		//import
		controller = GetComponent<PlayerController>();
		anim = GetComponent<Animator>();
		slider = GameObject.Find("Fuel").GetComponent<Slider>();
		particles = GetComponentInChildren<ParticleSystem>();
		manager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		rigidBody = GetComponent<Rigidbody2D>();

		//particles are off by default
		particles.enableEmission = false;
	}

	void Update()
	{
		//FindClosestPlanet ();
	}

	void FixedUpdate() {

		//if standing on a planet, inherit its rotation
		if (grounded) {
			transform.parent = FindClosestPlanet().transform;
		} else
			transform.parent = null;

		//movement
		float forceX = 0f;
		float forceY = 0f;

		float absVelX = Mathf.Abs(rigidBody.velocity.x);
		float absVelY = Mathf.Abs(rigidBody.velocity.y);

		//if the ground object enters any objects with a 'ground' layer name, set grounded to true
		grounded = Physics2D.Linecast(transform.position, groundedPosition.position, 1 << LayerMask.NameToLayer("Ground") );

		if(controller.moving.x != 0)
		{
			if(absVelX < maxVelocity.x)
			{
				forceX = grounded ? moveSpeed * controller.moving.x : (moveSpeed * controller.moving.x * airSpeedMultiplier);

				//flipping- if forcex is greater than 0, set to either 1 or -1
				transform.localScale = new Vector3(forceX > 0 ? 1 : -1, 1, 1);
			}
			//animation state
			anim.SetInteger("AnimState", 1);
		}

		else
		{
			//animation state
			anim.SetInteger("AnimState", 0);
		}

		//jetpacking
		if(slider.value != 0)
		{
			if(controller.moving.y > 0)
			{
				PlayRocketSound();

				if(absVelY < maxVelocity.y)
					forceY = jetSpeed * controller.moving.y;

				//use fuel
				slider.value -= depleteSpeed;

				particles.enableEmission = true;

				anim.SetInteger("AnimState", 2);
			}
			else
			{
				particles.enableEmission = false;
			}
		}
		else
			particles.enableEmission = false;

		//go blue if lost in space
		if(grounded)
		{
			RecoverFuel();
		}
		else
			Invoke("ChangeColor", 1f);

		GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(forceX, forceY));

		//rotate towards nearest planet
		Vector3 vectorToTarget = FindClosestPlanet().transform.position - transform.position;
		float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) + 90;
		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationSpeed);
	}

	GameObject FindClosestPlanet()
	{
		//closest planet
		float closestDist = Mathf.Infinity;
		
		//do to each planet in tag array
		foreach(GameObject planet in planets) {
			
			//what is distance?
			float dist = Vector3.Distance(planet.transform.position, transform.position);
			
			//if current planet distance is the least...look at it
			if (dist <= closestDist)
			{
				closestDist = dist;
				closest = planet;
			}
		}
		return closest;
	}

	private void RecoverFuel()
	{
		slider.value += recoverSpeed;
	}

	private void ChangeColor()
	{
		float t = 0;

		GetComponent<Renderer>().material.color = Color.Lerp(Color.white, dyingColor, t);

		if(t < 1)
		{
			t += Time.deltaTime/duration;
		}
		else if(t > duration)
		{
			print("dead");
		}
	}

	void PlayLeftFootSound()
	{
		if(leftFootSound)
			AudioSource.PlayClipAtPoint(leftFootSound, transform.position);
	}

	void PlayRightFootSound()
	{
		if(rightFootSound)
			AudioSource.PlayClipAtPoint(rightFootSound, transform.position);
	}

	void PlayRocketSound()
	{
		if(!rocketSound || GameObject.Find("RocketSound"))
			return;

		GameObject go = new GameObject("RocketSound");
		AudioSource aSrc = go.AddComponent<AudioSource>();
		aSrc.clip = rocketSound;
		aSrc.volume = 0.7f;
		aSrc.Play();

		Destroy(go, rocketSound.length);
	}

	//thud sound on fall
	void OnCollisionEnter2D(Collision2D other)
	{
		if(!grounded)
		{
			float absVelX = Mathf.Abs(rigidBody.velocity.x);
			float absVelY = Mathf.Abs(rigidBody.velocity.y);

			if(absVelX <= .1f || absVelY <= .1f)
			{
				if(thudSound)
					AudioSource.PlayClipAtPoint(thudSound, transform.position);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Finish")
		{
			manager.Depart();
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject.tag == "Finish")
		{
			manager.Suppress();
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position, groundedPosition.position);
	}
	
}
