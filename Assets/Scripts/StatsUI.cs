using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField]
    Text turnCount, bankCount;

    private void Awake()
    {
        GameManager.OnBankUpdate += UpdateBank;
        GameManager.OnTurnCountUpdate += UpdateTurnCount;
    }

    private void OnDestroy()
    {
        GameManager.OnBankUpdate -= UpdateBank;
        GameManager.OnTurnCountUpdate -= UpdateTurnCount;
    }

    void UpdateBank(int newBank) =>
        bankCount.text = $"${newBank}";
    void UpdateTurnCount(int newTurnCount) =>
        turnCount.text = $"Turn {newTurnCount}";
}
