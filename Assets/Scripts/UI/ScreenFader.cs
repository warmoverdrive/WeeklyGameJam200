using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
	[SerializeField] float fadeTime = 2f;
	Image panel;

	private void Start()
	{
		panel = GetComponent<Image>();

		panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, 1);
		StartCoroutine(FadeScreenIn());
	}

	public IEnumerator FadeScreenIn()
	{
		panel.raycastTarget = false;
		float elapsedTime = 0f;
		Color c = panel.color;

		while (elapsedTime < fadeTime)
		{
			yield return new WaitForEndOfFrame();
			elapsedTime += Time.deltaTime;
			c.a = 1f - Mathf.Clamp01(elapsedTime / fadeTime);
			panel.color = c;
		}
	}

	public IEnumerator FadeScreenOut()
	{
		panel.raycastTarget = true;
		float elapsedTime = 0f;
		Color c = panel.color;

		while (elapsedTime < fadeTime)
		{
			yield return new WaitForEndOfFrame();
			elapsedTime += Time.deltaTime;
			c.a = Mathf.Clamp01(elapsedTime / fadeTime);
			panel.color = c;
		}
	}
}
