using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Improvement : MonoBehaviour, IPointerClickHandler
{
	public ImprovementsSO improvementType;
	[SerializeField] bool isWatered = false;
	[SerializeField] AudioClip harvestSound, selectSound, waterSound;
	SphereCollider pointerTarget;
	ToolTip tooltip;
	TextMesh cooldownText;
	BoardSpace parentSpace;
	[SerializeField]
	SpriteRenderer waterIcon;
	AudioSource audioSource;
	[SerializeField] ParticleSystem waterParticleSystem;

	GameObject model;

	bool tooltipShown = false;
	int growCountdown = 0;

	public bool IsWatered() => isWatered;

	public static event Action<int> OnHarvest;

	private void Awake()
	{
		GameManager.OnPlayerTurn += UpdateCooldown;
	}
	private void OnDestroy()
	{
		GameManager.OnPlayerTurn -= UpdateCooldown;
	}

	private void Start()
	{
		StartCoroutine(PieceInit());
		parentSpace = GetComponentInParent<BoardSpace>();
		pointerTarget = GetComponent<SphereCollider>();
		audioSource = GetComponent<AudioSource>();
		pointerTarget.enabled = false;
		cooldownText = GetComponentInChildren<TextMesh>();
		UnwaterImprovement();
	}

	private IEnumerator PieceInit()
	{
		while (tooltip == null)
		{
			tooltip = FindObjectOfType<ToolTip>();
			yield return new WaitForEndOfFrame();
		}
	}

	public void SetImprovementType(ImprovementsSO type)
	{
		gameObject.name = $"{type.improvementName} {transform.position.x}, {transform.position.z}";

		improvementType = type;
		pointerTarget.enabled = true;
		if (type.canGrow)
		{
			growCountdown = type.growTime;
			cooldownText.text = growCountdown.ToString();
		}

		SetImprovementModel();
	}

	private void SetImprovementModel()
	{
		if (model) Destroy(model);
		model = Instantiate(improvementType.modelsByGrowth[growCountdown]);
		model.transform.SetParent(this.transform);
		model.transform.rotation = parentSpace.model.transform.rotation;
		model.transform.localPosition = Vector3.zero;
	}

	void UpdateCooldown()
	{
		if (improvementType && improvementType.canGrow)
		{
			if (isWatered || !improvementType.needsWater)
				growCountdown -= 1;
			if (growCountdown > 0)
				cooldownText.text = growCountdown.ToString();
			else
			{
				cooldownText.text = "!";
				growCountdown = 0;
			}
			SetImprovementModel();
			UnwaterImprovement();
		}
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
			if (growCountdown < 1)
			{
				HarvestImprovement();
			}
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
		tooltip.ActivateTooltip(improvementType.improvementName, improvementType.improvementRules);
	}
	private void HideTooltip()
	{
		tooltipShown = false;
		tooltip.DeactivateTooltip();
	}

	public IEnumerator WaterImprovement()
	{
		if (improvementType.needsWater)
		{
			waterParticleSystem.Play();
			audioSource.PlayOneShot(waterSound);
			yield return new WaitForSeconds(waterParticleSystem.main.duration);
			isWatered = true;
			waterIcon.enabled = true;
		}
	}

	private void UnwaterImprovement()
	{
		isWatered = false;
		waterIcon.enabled = false;
	}

	private void HarvestImprovement()
	{
		audioSource.PlayOneShot(harvestSound);
		OnHarvest?.Invoke(improvementType.harvestIncome);
		if (improvementType.tileChangeOnHarvest) // if there is a piece to change to after harvest
			parentSpace.SetPieceType(improvementType.tileChangeOnHarvest);
		improvementType = null;
		UnwaterImprovement();
		pointerTarget.enabled = false;
		cooldownText.text = "";
		Destroy(model);
	}

	void ClickSound() => audioSource.PlayOneShot(audioSource.clip);
}
