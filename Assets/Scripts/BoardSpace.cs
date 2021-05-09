using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoardSpace : MonoBehaviour, IPointerClickHandler
{
    public PieceSO pieceType;
    [SerializeField]
    MeshRenderer baseMesh;
    ToolTip tooltip;

    bool tooltipShown = false;

    private void Start()
    {
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
        gameObject.name = $"{type.pieceName} {transform.position.x}, {transform.position.z}";
        baseMesh.material.color = type.color;
        pieceType = type;
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
            Debug.Log($"Clicked {this.name}");
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
}
