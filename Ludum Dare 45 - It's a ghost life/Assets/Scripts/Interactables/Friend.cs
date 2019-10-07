using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : Interactable
{
	public GameObject hatPrefab;
	public GameObject endChoice;

	public bool givenFish = false;


	public override void Interact()
	{
		if (!givenFish)
			GiveFish();
		else
			endChoice.SetActive(true);
	}

	void GiveFish()
	{
		if (ghost.heldFlag != "Fsh")
			return;

		givenFish = true;

		var held = ghost.DropHeld();
		foreach (var item in held)
		{
			Destroy(item);
		}

		Instantiate(hatPrefab);
	}
}
