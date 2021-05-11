using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeField]
	GameObject menuUI, creditsUI, helpUI;

	private void Start() => ShowMainMenu();

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			ShowMainMenu();
	}

	public void ShowMainMenu()
	{
		menuUI.SetActive(true);
		creditsUI.SetActive(false);
		helpUI.SetActive(false);
	}

	public void ShowCredits()
	{
		menuUI.SetActive(false);
		creditsUI.SetActive(true);
		helpUI.SetActive(false);
	}

	public void ShowHelp()
	{
		menuUI.SetActive(false);
		creditsUI.SetActive(false);
		helpUI.SetActive(true);
	}

	public void QuitGame() => Application.Quit();

	public void StartGame() => SceneManager.LoadScene(1);
}
