using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeField]
	GameObject menuUI, creditsUI, helpUI;
	[SerializeField]
	ScreenFader screenFader;
	[SerializeField]
	MusicFader musicFader;
	AudioSource audioSource;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void Start() => ShowMainMenu();

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			ShowMainMenu();
	}

	public void ShowMainMenu()
	{
		ClickSound();
		menuUI.SetActive(true);
		creditsUI.SetActive(false);
		helpUI.SetActive(false);
	}

	public void ShowCredits()
	{
		ClickSound();
		menuUI.SetActive(false);
		creditsUI.SetActive(true);
		helpUI.SetActive(false);
	}

	public void ShowHelp()
	{
		ClickSound();
		menuUI.SetActive(false);
		creditsUI.SetActive(false);
		helpUI.SetActive(true);
	}

	public void QuitGame() => StartCoroutine(TransitionFromMenu(isQuitting: true));

	public void StartGame() => StartCoroutine(TransitionFromMenu(isQuitting: false));

	private IEnumerator TransitionFromMenu(bool isQuitting)
	{
		ClickSound();
		StartCoroutine(musicFader.FadeMusicOut());
		yield return StartCoroutine(screenFader.FadeScreenOut());

		if (isQuitting)
			Application.Quit();
		else
			SceneManager.LoadScene(1);
	}

	void ClickSound() => audioSource.PlayOneShot(audioSource.clip);
}
