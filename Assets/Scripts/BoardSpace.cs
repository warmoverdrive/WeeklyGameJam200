using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class BoardSpace : MonoBehaviour, IPointerClickHandler
{
	public PieceSO pieceType;
	ToolTip tooltip;
	public List<BoardSpace> neighbors;
	public GameObject model { get; private set; }
	[SerializeField] ParticleSystem reclaimParticles;
	[SerializeField] AudioClip reclaimSound;
	AudioSource audioSource;

	int[] rotations = { 0, 90, 180, 270 };

	bool tooltipShown = false;

	public static event Action<BoardSpace> OnBoardClicked;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		StartCoroutine(PieceInit());
	}

	private IEnumerator PieceInit()
	{
		while (tooltip == null)
		{
			tooltip = FindObjectOfType<ToolTip>();
			yield return new WaitForEndOfFrame();
		}
	}

	public void SetPieceType(PieceSO type)
	{
		if (model) Destroy(model);
		gameObject.name = $"{type.pieceName} {transform.position.x}, {transform.position.z}";
		pieceType = type;
		model = Instantiate(type.model, transform);
		model.transform.localPosition = Vector3.zero;
		model.transform.Rotate(new Vector3(0, rotations[UnityEngine.Random.Range(0, rotations.Length)], 0));
	}

	public IEnumerator Reclaimation(PieceSO type)
	{
		reclaimParticles.Play();
		audioSource.PlayOneShot(reclaimSound);
		yield return new WaitForSeconds(reclaimParticles.main.duration / 2);
		SetPieceType(type);
		yield return new WaitForSeconds(reclaimParticles.main.duration / 2);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right)
		{
			if (tooltipShown)
			{
				ClickSound();
				HideTooltip();
			}
			else
			{
				ClickSound();
				ShowTooltip();
			}
		}
		else if (eventData.button == PointerEventData.InputButton.Left && GameManager.state == States.PlayerTurn)
		{
			OnBoardClicked?.Invoke(this);
		}
	}

	private void OnMouseExit()
	{
		if (tooltipShown)
			HideTooltip();
	}

	private void ShowTooltip()
	{
		tooltipShown = true;
		tooltip.ActivateTooltip(pieceType.name, pieceType.rules);
	}
	private void HideTooltip()
	{
		tooltipShown = false;
		tooltip.DeactivateTooltip();
	}

	void ClickSound() => audioSource.PlayOneShot(audioSource.clip);
}
