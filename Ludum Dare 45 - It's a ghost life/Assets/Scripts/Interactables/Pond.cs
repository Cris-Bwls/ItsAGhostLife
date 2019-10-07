using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pond : Interactable
{
	public GameObject fshPrefab;
	public GameObject fshZoneText;
	public GameObject bobber;
	public GameObject excMark;

	public Transform spawnPoint;
	public Transform bobberEnd;

	public LineRenderer lineRenderer;

	public float waitTime;

	private int fshAttempt = 0;
	private bool fshing = false;
	private bool caught = false;

	public override void Interact()
	{
		if (ghost.heldFlag != "Rod")
			return;

		if (fshing)
		{
			if (caught)
				EndFishing();
		}
		else
			StartFishing();		
	}

	private void FixedUpdate()
	{
		lineRenderer.enabled = fshing;
		if (fshing)
		{
			lineRenderer.SetPosition(0, ghost.rodEnd.position);
			lineRenderer.SetPosition(1, bobberEnd.position);
		}
	}

	public void StartFishing()
	{
		fshing = true;

		bobber.SetActive(true);
		StartCoroutine(Fishing());

		ghost.Fishing(true);
	}

	IEnumerator Fishing()
	{
		yield return new WaitForSeconds(waitTime);

		caught = true;
		excMark.SetActive(true);
	}

	public void EndFishing()
	{
		fshing = false;

		ghost.Fishing(false);

		excMark.SetActive(false);
		bobber.SetActive(false);
		fshAttempt++;

		if (fshAttempt == 1)
			Instantiate(fshPrefab, spawnPoint);
		else if (fshAttempt > 1)
		{
			var held = ghost.DropHeld();
			foreach (var item in held)
			{
				Destroy(item);
			}

			fshZoneText.SetActive(true);
		}
	}
}
