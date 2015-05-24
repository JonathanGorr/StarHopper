using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	//variable floats
	public float
		moveSpeed = 50,
		jetSpeed = 15f,
		airSpeedMultiplier = 0.3f,
		duration = 5f,
		recoverSpeed = 1f,
		fuelDepleteSpeed = 1f,
		noAirDepleteSpeed = .2f,
		gasGiantDepleteSpeed = .5f,
		rotationSpeed = 2,
		laserSpeed = 100f;

	private float t;

	//sounds
	public AudioClip 
		leftFootSound,
		rightFootSound,
		thudSound,
		rocketSound,
		fireSound;

	//bools
	private bool grounded;

	//vectors
	public Vector2 maxVelocity = new Vector2(3,5);
	public Transform groundedPosition;
	private Vector3 moveDirection;
	private Vector2 moving = new Vector2();

	//components
	private Animator anim;
	private ParticleSystem particles;
	private LevelManager manager;
	private Rigidbody2D rigidBody;
	private PlayerInput input;
	private RotateTowardsPlanet planetRotate;

	//gameobjects
	private GameObject playerCanvas;

	//prefabs
	public GameObject laser;

	//bars
	private Slider fuel, health;

	//colors
	public Color dyingColor;

	void Awake() {

		//import
		playerCanvas = GameObject.Find ("PlayerCanvas");
		planetRotate = GameObject.Find("Player").GetComponent<RotateTowardsPlanet> ();
		input = GameObject.Find ("LevelManager").GetComponent<PlayerInput> ();
		anim = GetComponent<Animator>();
		fuel = GameObject.Find("Fuel").GetComponent<Slider>();
		health = GameObject.Find("Health").GetComponent<Slider>();
		particles = GetComponentInChildren<ParticleSystem>();
		manager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		rigidBody = GetComponent<Rigidbody2D>();

		//particles are off by default
		particles.enableEmission = false;
	}

	void Start()
	{
		transform.position = GameObject.Find ("Start(Clone)").transform.position + new Vector3(0, .5f, 0);
	}

	void Update() {

		//movement----------------------------------------------------------------------
		float forceX = 0f;
		float forceY = 0f;
		
		float absVelX = Mathf.Abs(rigidBody.velocity.x);
		float absVelY = Mathf.Abs(rigidBody.velocity.y);

		//change color if in space
		ChangeColor();

		//reset
		moving.x = moving.y = 0;
		
		//if within range of a planet, move normally
		if(!planetRotate.tooFar)
		{
			//horizontal
			if(input.left)
				moving.x = -1;
			else if(input.right)
				moving.x = 1;
		}
		
		//else if in outter space...
		else
		{
			if(input.left) 
				transform.Rotate(Vector3.forward * rotationSpeed * 100 * Time.deltaTime);
			else if(input.right) 
				transform.Rotate(Vector3.back * rotationSpeed * 100 * Time.deltaTime);
		}
		
		//up and down
		if(input.up) 
			moving.y = 1;
		else if(input.down)
			moving.y = -1;

		//if the ground object enters any objects with a 'ground' layer name, set grounded to true
		grounded = Physics2D.Linecast(transform.position, groundedPosition.position, 1 << LayerMask.NameToLayer("Ground") );

		//calculate force
		if(moving.x != 0)
		{
			if(absVelX < maxVelocity.x)
			{
				forceX = grounded ? moveSpeed * moving.x : (moveSpeed * moving.x * airSpeedMultiplier);

				//flipping- if forcex is greater than 0, set to either 1 or -1
				transform.localScale = new Vector3(forceX > 0 ? 1 : -1, 1, 1);
			}

			if(grounded)
			{
				//walking state
				anim.SetInteger("AnimState", 1);
			}
		}
		//if not moving, idle anim
		else
			anim.SetInteger("AnimState", 0);

		if(rigidBody.velocity.magnitude > maxVelocity.x)
		{
			rigidBody.velocity = rigidBody.velocity.normalized * maxVelocity.x;
		}

		//jetpacking---------------------------------------------------------------
		if(fuel.value != 0)
		{
			if(moving.y > 0)
			{
				PlayRocketSound();

				if(absVelY < maxVelocity.y)
					forceY = jetSpeed * moving.y;

				//use fuel
				fuel.value -= fuelDepleteSpeed;

				particles.enableEmission = true;

				anim.SetInteger("AnimState", 2);
			}
			else
				particles.enableEmission = false;
		}
		else
			particles.enableEmission = false;

		//go blue if lost in space----------------------------------------
		if(grounded) RecoverFuel();

		rigidBody.AddRelativeForce(new Vector2(forceX, forceY));

		if (health.value <= 0)
			Kill ();

		if(input.fire)
			Fire();
	}

	void Fire()
	{
		Vector3 pos = Input.mousePosition;
		pos.z = transform.position.z - Camera.main.transform.position.z;
		pos = Camera.main.ScreenToWorldPoint(pos);

		AudioSource.PlayClipAtPoint(fireSound, transform.position);
		
		Quaternion q = Quaternion.FromToRotation(Vector3.right, pos - transform.position);
		GameObject go = Instantiate(laser, transform.position, q) as GameObject;
		go.GetComponent<Rigidbody2D> ().AddForce(go.transform.right * laserSpeed);
	}

	void RecoverFuel()
	{
		fuel.value += recoverSpeed;
	}

	void ChangeColor()
	{
		GetComponent<SpriteRenderer>().material.color = Color.Lerp(Color.white, dyingColor, t);

		float min = 0;
		float max = 1;

		//clamp
		if (t > max) 
			t = max;
		else if (t < min) 
			t = min;

		if(grounded)
		{
			if(t > min)
				t -= Time.deltaTime/duration * 4; //recover oxygen faster when on planet
		}
		else if(!grounded)
		{
			if(t < max)
				t += Time.deltaTime/duration;
		}

		if (t >= max) 
		{
			if(playerCanvas) playerCanvas.GetComponent<Animator> ().SetInteger ("AnimState", 1);
			health.value -= noAirDepleteSpeed;
		}
		else
			if(playerCanvas) playerCanvas.GetComponent<Animator>().SetInteger ("AnimState", 0);
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
			transform.parent = null;

			if(rigidBody)
			{
				float absVelX = Mathf.Abs(rigidBody.velocity.x);
				float absVelY = Mathf.Abs(rigidBody.velocity.y);

				if(absVelX <= .1f || absVelY <= .1f)
					if(thudSound)
						AudioSource.PlayClipAtPoint(thudSound, transform.position);
			}
		}

		//else grounded
		else
		{
			//if on planet, become child; inherit rotation
			if (other.gameObject.tag == "Planet") 
				transform.parent = other.transform;
		}
	}
	
	void OnCollisionExit2D(Collision2D other)
	{
		if(!grounded)
			if(other.gameObject.tag == "Planet")
				transform.parent = null;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Finish")
			manager.Depart();
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.tag == "Damager") 
		{
			print("Damaging");

			if(health) health.value -= gasGiantDepleteSpeed;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject.tag == "Finish")
			manager.Suppress();
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position, groundedPosition.position);
	}

	void Kill()
	{
		Destroy (playerCanvas);
		//Destroy (gameObject);
		manager.Kill ();
	}
	
}
