using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArmState
{
	Idle = 0,
	Hold,
	Push
}

public class Ghost : MonoBehaviour
{
	[Header("Parts")]
	public Rigidbody2D rb;
	public Transform holdingAnchor;
	public SpriteRenderer ghost;
	public SpriteRenderer legs;
	public SpriteRenderer arms;
	[Tooltip("same order as armState enum")]
	public List<Sprite> armSprites = new List<Sprite>();
	public Animator legAnimator;
	public GameObject rod;
	public Transform rodEnd;
	public GameObject hat;

	[Header("CollisionStates")]
	public CollisionState upState;
	public CollisionState downState;
	public CollisionState leftState;
	public CollisionState rightState;

	[Header("Drops")]
	public GameObject armPrefab;
	public GameObject legPrefab;

	[Header("Forces")]
	public float moveForce = 0.1f;
	public float jumpHeight = 5;
	public float timeToJumpHeight;
	public float gravity = 1;
	//public Vector2 posMax;
	//public Vector2 negMax;
	private Vector3 velocity = Vector3.zero;
	private float jumpGravity;
	private float jumpVelocity;

	[Header("Flags")]
	public string heldFlag = "";
	public ArmState armState = ArmState.Idle;
	public bool hasArms = false;
	public bool hasLegs = false;
	public bool fishing = false;

	private Interactable interactable;

    // Start is called before the first frame update
    void Start()
    {
		if (!rb)
			rb = GetComponent<Rigidbody2D>();

		// Calculate the gravity and initial jump velocity values 
		jumpGravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpHeight, 2);
		jumpVelocity = Mathf.Abs(jumpGravity) * timeToJumpHeight;

		gravity = jumpGravity;

		legAnimator.SetFloat("speedMulti", 0);
	}

    // Update is called once per frame
    void Update()
    {
		if (!hasArms && heldFlag != "")
			DropHeld();

		if (Input.GetKeyDown(KeyCode.E) && interactable)
			interactable.Interact();

		if (fishing)
			return;

		if (Input.GetKeyDown(KeyCode.Q) && heldFlag != "")
			DropHeld();

		if (Input.GetKeyDown(KeyCode.Z))
			Drop();
    }

	private void FixedUpdate()
	{
		ChangeArmState(ArmState.Idle);

		if (fishing)
			return;

		if (heldFlag != "")
			ChangeArmState(ArmState.Hold);

		var move = Vector3.right * Input.GetAxis("Horizontal") * Time.fixedDeltaTime * moveForce;

		if (move.x != 0 && downState.colliding)
			legAnimator.SetFloat("speedMulti", 1);
		else
			legAnimator.SetFloat("speedMulti", 0);

		var translate = move;

		if (Input.GetAxis("Horizontal") > 0.1f)
			MovingLeft(false);
		if (Input.GetAxis("Horizontal") < -0.1f)
			MovingLeft(true);

		// Step update
		var stepMovement = (velocity + Vector3.up * gravity * Time.fixedDeltaTime * 0.5f) * Time.fixedDeltaTime;
		translate += stepMovement;

		velocity.y += gravity * Time.fixedDeltaTime;

		// UP DOWN COLLISION
		if (downState.colliding)
		{
			if (velocity.y < 0)
				velocity.y = 0;
			if (translate.y < 0)
				translate.y = 0;
		}
		if (upState.colliding)
		{
			if (velocity.y > 0)
				velocity.y = 0;
			if (translate.y > 0)
				translate.y = 0;
		}

		// LEFT RIGHT Collision
		if (leftState.colliding && translate.x < 0)
		{
			if (leftState.movable && hasArms && hasLegs)
			{
				var box = leftState.movableObject.GetComponentInParent<Box>();
				if (box && !box.kinematic)
					ChangeArmState(ArmState.Push);
				else
					translate.x = 0;
			}
			else
				translate.x = 0;
		}
		if (rightState.colliding && translate.x > 0)
		{
			if (rightState.movable && hasArms && hasLegs)
			{
				var box = rightState.movableObject.GetComponentInParent<Box>();
				if (box && !box.kinematic)
					ChangeArmState(ArmState.Push);
				else
					translate.x = 0;
			}
			else
				translate.x = 0;
		}

		transform.Translate(translate);


		// When jump button pressed,
		if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) && downState.colliding && hasLegs)
			velocity.y = jumpVelocity;
	}

	public void MovingLeft(bool movingLeft)
	{
		ghost.flipX = movingLeft;
		legs.flipX = movingLeft;
		arms.flipX = movingLeft;
	}

	public void ChangeArmState(ArmState newArmState)
	{
		armState = newArmState;
		arms.sprite = armSprites[(int)armState];
	}

	public void Hold(Transform hold)
	{
		DropHeld();

		// hold it
		hold.SetParent(holdingAnchor, false);
		hold.position = holdingAnchor.position;
		var holdColliders = hold.GetComponentsInChildren<Collider2D>();
		foreach (var collider in holdColliders)
		{
			collider.enabled = false;
		}

		var rbs = hold.GetComponentsInChildren<Rigidbody2D>();
		foreach (var item in rbs)
		{
			item.isKinematic = true;
		}
	}

	public List<GameObject> DropHeld()
	{
		List<GameObject> objects = new List<GameObject>();

		// if there is a child
		if (holdingAnchor.childCount > 0)
		{
			// drop it
			var colliders = holdingAnchor.GetComponentsInChildren<Collider2D>();
			foreach (var collider in colliders)
			{
				collider.enabled = true;
				objects.Add(collider.gameObject);
			}

			var rbs = holdingAnchor.GetComponentsInChildren<Rigidbody2D>();
			foreach (var item in rbs)
			{
				item.isKinematic = false;
			}

			holdingAnchor.DetachChildren();
			heldFlag = "";
		}

		return objects;
	}

	public void Drop()
	{
		if (hasArms)
		{
			hasArms = false;
			Instantiate(armPrefab, holdingAnchor.position, holdingAnchor.rotation);
			ChangeArmState(ArmState.Idle);
			arms.gameObject.SetActive(false);
		}

		if (hasLegs)
		{
			hasLegs = false;
			Instantiate(legPrefab, holdingAnchor.position, holdingAnchor.rotation);
			legs.gameObject.SetActive(false);
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		var interact = collision.GetComponent<Interactable>();
		if (interact)
			interactable = interact;
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		interactable = null;
	}

	public void Fishing(bool fshing)
	{
		fishing = fshing;
		rod.SetActive(fshing);
		holdingAnchor.gameObject.SetActive(!fshing);
		MovingLeft(false);
	}
}
