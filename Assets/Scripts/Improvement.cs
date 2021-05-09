using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Improvement : MonoBehaviour, IPointerClickHandler
{
    public ImprovementsSO improvementType;
    [SerializeField]
    MeshRenderer baseMesh;
    SphereCollider pointerTarget;
    ToolTip tooltip;

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
    }

    void UpdateCooldown() => growCountdown += improvementType.canGrow ? -1 : 0;

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
