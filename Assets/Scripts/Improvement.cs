using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Improvement : MonoBehaviour, IPointerClickHandler
{
	public ImprovementsSO improvementType;
	[SerializeField] bool isWatered = false;
	SphereCollider pointerTarget;
	ToolTip tooltip;
	TextMesh cooldownText;
	BoardSpace parentSpace;
	[SerializeField]
	SpriteRenderer waterIcon;

	GameObject model;

	bool tooltipShown = false;
	int growCountdown = 0;

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
				HideTooltip();
			else
				ShowTooltip();
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

	public void WaterImprovement()
	{
		if (improvementType.needsWater)
		{
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
		OnHarvest?.Invoke(improvementType.harvestIncome);
		if (improvementType.tileChangeOnHarvest) // if there is a piece to change to after harvest
			parentSpace.SetPieceType(improvementType.tileChangeOnHarvest);
		improvementType = null;
		UnwaterImprovement();
		pointerTarget.enabled = false;
		cooldownText.text = "";
		Destroy(model);
	}
}
