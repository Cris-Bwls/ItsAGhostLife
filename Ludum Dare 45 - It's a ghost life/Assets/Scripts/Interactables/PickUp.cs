using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Interactable
{
	public string flag;

	public override void Interact()
	{
		if (!ghost.hasArms)
			return;

		ghost.Hold(transform);
		ghost.heldFlag = flag;
	}
}
