using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public Vector2 moving = new Vector2();

	public float 
		moveSpeed = 50,
		jumpSpeed = 100,
		jetSpeed = 15f,
		airSpeedMultiplier = 0.3f,
		duration = 5f,
		recoverSpeed = 1f,
		depleteSpeed = 1f,
		rotationSpeed = 200f;

	//sounds
	public AudioClip leftFootSound;
	public AudioClip rightFootSound;
	public AudioClip thudSound;
	public AudioClip rocketSound;

	public bool grounded;

	public Vector2 maxVelocity = new Vector2(3,5);
	public Transform groundedPosition;

	private Vector3 moveDirection;
	private Animator anim;
	private Slider fuel;
	private ParticleSystem particles;
	private LevelManager manager;
	private Rigidbody2D rigidBody;
	private PlayerInput input;
	private RotateTowardsPlanet planetRotate;

	public Color dyingColor;

	void Awake() {

		//import
		planetRotate = GameObject.Find("Player").GetComponent<RotateTowardsPlanet> ();
		input = GameObject.Find ("LevelManager").GetComponent<PlayerInput> ();
		anim = GetComponent<Animator>();
		fuel = GameObject.Find("Fuel").GetComponent<Slider>();
		particles = GetComponentInChildren<ParticleSystem>();
		manager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		rigidBody = GetComponent<Rigidbody2D>();

		//particles are off by default
		particles.enableEmission = false;
	}

	void Update() {

		//reset
		moving.x = moving.y = 0;
		
		//if within range of a planet, move normally
		if(!planetRotate.tooFar)
		{
			//horizontal
			if(input.left)
			{
				moving.x = -1;
			}
			else if(input.right)
			{
				moving.x = 1;
			}
		}
		
		//else if in outter space...
		else
		{
			if(input.left)
			{
				print("rotating");
				transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
			}
			
			else if(input.right)
				transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
		}
		
		//vertical
		if(input.up)
		{
			moving.y = 1;
		}
		else if(input.down)
		{
			moving.y = -1;
		}

		//movement
		float forceX = 0f;
		float forceY = 0f;

		float absVelX = Mathf.Abs(rigidBody.velocity.x);
		float absVelY = Mathf.Abs(rigidBody.velocity.y);

		//if the ground object enters any objects with a 'ground' layer name, set grounded to true
		grounded = Physics2D.Linecast(transform.position, groundedPosition.position, 1 << LayerMask.NameToLayer("Ground") );

		//if moving...
		if(moving.x != 0)
		{
			if(absVelX < maxVelocity.x)
			{
				forceX = grounded ? moveSpeed * moving.x : (moveSpeed * moving.x * airSpeedMultiplier);

				//flipping- if forcex is greater than 0, set to either 1 or -1
				transform.localScale = new Vector3(forceX > 0 ? 1 : -1, 1, 1);
			}

			//walking state
			anim.SetInteger("AnimState", 1);
		}

		//if not moving
		else
		{
			//idle state
			anim.SetInteger("AnimState", 0);
		}

		//jetpacking
		if(fuel.value != 0)
		{
			if(moving.y > 0)
			{
				PlayRocketSound();

				if(absVelY < maxVelocity.y)
					forceY = jetSpeed * moving.y;

				//use fuel
				fuel.value -= depleteSpeed;

				particles.enableEmission = true;

				anim.SetInteger("AnimState", 2);
			}
			else
				particles.enableEmission = false;
		}
		else
			particles.enableEmission = false;

		//go blue if lost in space
		if(grounded)
			RecoverFuel();
		else
		{
			print("go blue");
			Invoke("ChangeColor", 1f);
		}

		rigidBody.AddRelativeForce(new Vector2(forceX, forceY));
	}

	private void RecoverFuel()
	{
		fuel.value += recoverSpeed;
	}

	private void ChangeColor()
	{
		float t = 0;

		GetComponent<Renderer>().material.color = Color.Lerp(Color.white, dyingColor, t);

		if(t < 1)
			t += Time.deltaTime/duration;
		else if(t > duration)
			print("dead");
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
				if(thudSound)
					AudioSource.PlayClipAtPoint(thudSound, transform.position);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Finish")
			manager.Depart();
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
	
}
