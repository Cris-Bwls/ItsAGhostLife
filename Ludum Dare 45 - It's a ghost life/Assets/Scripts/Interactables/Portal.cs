using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : Interactable
{
	public GameObject endChoice;

	public SpriteRenderer wave;
	public float speed;

	private void Update()
	{
		wave.material.mainTextureOffset += Vector2.down * speed * Time.deltaTime;
	}

	public override void Interact()
	{
		endChoice.SetActive(true);
	}
}
