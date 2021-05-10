using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField]
	GameObject gameUI, menuUI, gameOverUI;
	[SerializeField]
	Text scoreText, highScoreText;

	private void Update()
	{
		if (gameOverUI.activeSelf) return;

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			ToggleMenu();
		}
	}

	public void ToggleGameOver(int score)
	{
		gameUI.SetActive(false);
		menuUI.SetActive(false);
		gameOverUI.SetActive(true);
		scoreText.text = $"Score: {score}";
	}

	public void ToggleMenu()
	{
		gameUI.SetActive(!gameUI.activeSelf);
		menuUI.SetActive(!menuUI.activeSelf);
	}

	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void ReturnToMainMenu()
	{
		SceneManager.LoadScene(0);
	}
}
