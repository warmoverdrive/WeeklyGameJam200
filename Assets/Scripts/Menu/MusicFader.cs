using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicFader : MonoBehaviour
{
	AudioSource audioSource;
	[SerializeField] float fadeTime = 2f;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();

		audioSource.volume = 0f;
		StartCoroutine(FadeMusicIn());
	}

	public IEnumerator FadeMusicIn()
	{
		float elapsedTime = 0f;

		while (elapsedTime < fadeTime)
		{
			yield return new WaitForEndOfFrame();
			elapsedTime += Time.deltaTime;
			audioSource.volume = Mathf.Clamp01(elapsedTime / fadeTime);
		}
	}

	public IEnumerator FadeMusicOut()
	{
		float elapsedTime = 0f;

		while (elapsedTime < fadeTime)
		{
			yield return new WaitForEndOfFrame();
			elapsedTime += Time.deltaTime;
			audioSource.volume = 1f - Mathf.Clamp01(elapsedTime / fadeTime);
		}
	}
}
