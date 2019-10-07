using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
	public Rigidbody2D rb;
	public List<CollisionState> colStates = new List<CollisionState>();

	public bool kinematic = false;
	void Update()
    {
		kinematic = false;
		foreach (var state in colStates)
		{
			if (state.colliding)
			{
				if (state.movable)
				{
					var box = state.movableObject.GetComponentInParent<Box>();
					if (box)
					{
						kinematic |= box.kinematic;
					}
				}
				else if (state.player)
				{
					var ghost = state.playerObject.GetComponent<Ghost>();
					if (ghost.heldFlag != "")
						kinematic |= true;
				}
				else
					kinematic |= true;
			}
		}

		if (rb.isKinematic != kinematic)
			rb.isKinematic = kinematic;

		if (kinematic)
			rb.mass = int.MaxValue;
		else
			rb.mass = 1;
    }
}
