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
	Text scoreText, gameOverText;
	[SerializeField]
	ScreenFader screenFader;
	[SerializeField]
	MusicFader musicFader;
	AudioSource audioSource;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (gameOverUI.activeSelf) return;

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			ToggleMenu();
		}
	}

	public void ToggleGameOver(int score, bool hasWon)
	{
		gameUI.SetActive(false);
		menuUI.SetActive(false);
		gameOverUI.SetActive(true);
		gameOverText.text = hasWon ? "Season Complete!" : "Bankrupt!";
		scoreText.text = $"Score: {score}";
	}

	public void ToggleMenu()
	{
		ClickSound();
		gameUI.SetActive(!gameUI.activeSelf);
		menuUI.SetActive(!menuUI.activeSelf);
	}

	public void RestartGame()
	{
		ClickSound();
		StartCoroutine(ChangeScene(SceneManager.GetActiveScene().buildIndex));
	}

	public void ReturnToMainMenu()
	{
		ClickSound();
		StartCoroutine(ChangeScene(0));
	}

	private IEnumerator ChangeScene(int index)
	{
		StartCoroutine(musicFader.FadeMusicOut());
		yield return StartCoroutine(screenFader.FadeScreenOut());
		SceneManager.LoadScene(index);
	}
	void ClickSound() => audioSource.PlayOneShot(audioSource.clip);
}
