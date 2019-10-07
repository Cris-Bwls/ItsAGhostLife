using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
abstract public class Interactable : MonoBehaviour
{
	protected Ghost ghost;

	private void Start()
	{
		ghost = FindObjectOfType<Ghost>();
	}

	abstract public void Interact();
}
