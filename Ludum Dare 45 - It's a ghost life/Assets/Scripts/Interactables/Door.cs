using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
	public Animator animator;
	new public BoxCollider2D collider;
	public SpriteRenderer spriteRenderer;
	public Sprite doorOpen;

	bool open = false;

	public void Update()
	{
		collider.enabled = ((ghost.hasArms || ghost.hasLegs) && !open);
	}

	public override void Interact()
	{
		if (open)
			return;

		if (ghost.heldFlag != "Key")
			return;

		var held = ghost.DropHeld();
		foreach (var item in held)
		{
			Destroy(item);
		}

		animator.SetBool("open", true);
		collider.enabled = false;
		open = true;
	}
}
