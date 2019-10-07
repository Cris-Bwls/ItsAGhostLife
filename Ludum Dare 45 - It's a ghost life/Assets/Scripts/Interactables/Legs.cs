using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legs : Interactable
{
	public override void Interact()
	{
		if (ghost.hasLegs)
			return;

		ghost.hasLegs = true;
		ghost.legs.gameObject.SetActive(true);

		Destroy(gameObject);
	}
}
