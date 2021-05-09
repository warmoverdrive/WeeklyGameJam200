using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Improvement : MonoBehaviour, IPointerClickHandler
{
	public ImprovementsSO improvementType;
	[SerializeField]
	MeshRenderer baseMesh;
	[SerializeField] bool isWatered = false;
	SphereCollider pointerTarget;
	ToolTip tooltip;
	TextMesh cooldownText;

	bool tooltipShown = false;
	int growCountdown = 0;

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
		pointerTarget = GetComponent<SphereCollider>();
		pointerTarget.enabled = false;
		cooldownText = GetComponentInChildren<TextMesh>();
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

		// Debug for visualization purposes
		var model = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		model.transform.position = transform.position;
		model.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		model.GetComponent<MeshRenderer>().material.color = type.color;
		//

		improvementType = type;
		pointerTarget.enabled = true;
		if (type.canGrow)
		{
			growCountdown = type.growTime;
			cooldownText.text = growCountdown.ToString();
		}
	}

	void UpdateCooldown()
	{
		if (improvementType && improvementType.canGrow)  // add is watered after testing
		{
			growCountdown -= 1;
			cooldownText.text = growCountdown.ToString();
		}
		isWatered = false;
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
			Debug.Log($"Clicked {this.name}");
			if (growCountdown < 1)
			{
				Debug.Log($"Can Harvest {this.name}");
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
}
