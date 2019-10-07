using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionState : MonoBehaviour
{
	public bool colliding = false;
	public bool movable = false;
	public bool player = false;
	public GameObject colliderObject;
	public GameObject movableObject;
	public GameObject playerObject;

	private List<Collider2D> collisions = new List<Collider2D>();


	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.isTrigger)
			return;

		if (collision.gameObject.layer == 10)
			return;

		if (collisions.Contains(collision))
			return;

		colliding = true;
		if (collision.gameObject.tag == "Movable")
		{
			movable = true;
			movableObject = collision.gameObject;
		}
		if (collision.gameObject.tag == "Player")
		{
			player = true;
			playerObject = collision.gameObject;
		}

		colliderObject = collision.gameObject;
		collisions.Add(collision);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.isTrigger)
			return;

		collisions.Remove(collision);
		var obj = collision.gameObject;
		if (obj == colliderObject)
			colliderObject = null;
		if (obj == movableObject)
		{
			movableObject = null;
			movable = false;
		}
		if (obj == playerObject)
		{
			playerObject = null;
			player = false;
		}

		colliding = collisions.Count > 1;			
	}
}
