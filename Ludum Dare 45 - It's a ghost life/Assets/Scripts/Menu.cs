using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
	private void OnEnable()
	{
		Time.timeScale = 0;
	}

	private void OnDisable()
	{
		Time.timeScale = 1;
	}

	public void Close()
	{
		gameObject.SetActive(false);
	}

	public void EndGame()
	{
		Application.Quit();
	}

	public void Restart()
	{
		SceneManager.LoadScene(0);
	}
}
