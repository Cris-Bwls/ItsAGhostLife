using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arms : Interactable
{
	public override void Interact()
	{
		if (ghost.hasArms)
			return;

		ghost.hasArms = true;
		ghost.arms.gameObject.SetActive(true);

		Destroy(gameObject);
	}
}
