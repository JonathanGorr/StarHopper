using UnityEngine;

public class ProjectileDragging : MonoBehaviour {

	public float maxStretch = 3.0f;
	public LineRenderer catapultLine;
	public bool dragging;

	private SpringJoint2D spring;
	private bool ClickedOn;
	private Ray rayToMouse;
	private Ray leftCatapultToProjectile;
	private float maxStretchSqr;
	private float boxSize;
	private Transform catapult;
	private Vector2 prevVelocity;

	void Awake()
	{
		//get spring component
		spring = GetComponent<SpringJoint2D>();

		//catapult is the thing to which the spring is attached
		catapult = spring.connectedBody.transform;
	}

	void Start()
	{
		LineRendererSetup();

		//create rays
		rayToMouse = new Ray(catapult.position, Vector3.zero);
		leftCatapultToProjectile = new Ray(catapultLine.transform.position, Vector3.zero);

		//square for efficiency
		maxStretchSqr = maxStretch * maxStretch;

		//get reference to circle collider
		BoxCollider2D box = GetComponent<Collider2D>() as BoxCollider2D;
		//assign radius as collider radius
		boxSize = box.size.x;
	}

	void Update()
	{
		if(ClickedOn)
		{
			dragging = true;
			Dragging();
		}
		else
			dragging = false;

		if(spring != null)
		{
			//if not kinematic and slowing down, destroy spring and set velocity to previous velocity before slowdown
			if(!GetComponent<Rigidbody2D>().isKinematic && prevVelocity.sqrMagnitude > GetComponent<Rigidbody2D>().velocity.sqrMagnitude)
			{
				Destroy(spring);
				GetComponent<Rigidbody2D>().velocity = prevVelocity;
			}

			//if we havent yet launched, set previous velocity to most current
			if(!ClickedOn)
				prevVelocity = GetComponent<Rigidbody2D>().velocity;

			//update the line renderer each frame
			LineRendererUpdate();
		}

		else
		{
			catapultLine.enabled = false;
		}
	}

	void LineRendererSetup()
	{
		//setup their positions
		catapultLine.SetPosition(0, catapultLine.transform.position);

		//set them in sorting layer
		catapultLine.sortingLayerName = "Foreground";

		//set up their sorting orders
		catapultLine.sortingOrder = 3;
	}

	void LineRendererUpdate()
	{
		Vector2 catapultToProjectile = transform.position - catapultLine.transform.position;
		leftCatapultToProjectile.direction = catapultToProjectile;
		Vector3 holdPoint = leftCatapultToProjectile.GetPoint(catapultToProjectile.magnitude + boxSize);

		catapultLine.SetPosition(1, holdPoint);
	}

	//when held
	void OnMouseDown()
	{
		GetComponent<Collider>().enabled = false;
		spring.enabled = false;
		ClickedOn = true;
	}

	//when released
	void OnMouseUp()
	{
		GetComponent<Collider>().enabled = true;
		spring.enabled = true;
		GetComponent<Rigidbody2D>().isKinematic = false;
		ClickedOn = false;
	}

	//when dragging
	void Dragging()
	{
		//where is mouse?
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//distance between mouse and catapult
		Vector2 catapultToMouse = mouseWorldPoint - catapult.position;

		//restrict band
		if(catapultToMouse.sqrMagnitude > maxStretchSqr)
		{
			rayToMouse.direction = catapultToMouse;
			mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
		}

		//flatten, no depth, 2d
		mouseWorldPoint.z = 0f;
		//set object to mouse position
		transform.position = mouseWorldPoint;
	}

	void CreateSpringJoint()
	{
		SpringJoint2D joint = gameObject.AddComponent<SpringJoint2D>();
		GetComponent<Rigidbody2D>().isKinematic = true;
		transform.parent.Find("Source").transform.position = transform.position;
		joint.connectedBody = transform.parent.Find("Source").GetComponent<Rigidbody2D>();
		joint.distance = 1;
		joint.frequency = 5;
		joint.anchor.Set(transform.position.x, transform.position.y);
		joint.connectedAnchor.Set(transform.position.x, transform.position.y);
		spring = joint;
	}
}
