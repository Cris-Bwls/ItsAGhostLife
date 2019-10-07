using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMenu : MonoBehaviour
{
	public GameObject pauseMenu;
	bool paused = false;

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
			paused = !paused;

		if (pauseMenu.activeSelf != paused)
			pauseMenu.SetActive(paused);
    }

	public void Close()
	{
		paused = false;
		pauseMenu.SetActive(false);
	}
}
