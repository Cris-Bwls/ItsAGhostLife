using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : Interactable
{
	public override void Interact()
	{
		ghost.hat.SetActive(true);
		Destroy(gameObject);
	}
}
