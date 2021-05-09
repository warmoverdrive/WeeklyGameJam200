using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum States { PlayerTurn, EndTurn };

public class GameManager : MonoBehaviour
{

    public static event Action OnPlayerTurn;
    public static event Action OnEndTurn;
    public static event Action<int> OnBankUpdate;
    public static event Action<int> OnTurnCountUpdate;
    public static States state;

    int turnCount = 1;
    int bank = 10;
    [SerializeField] int turnDelay = 2;


    private void Start()
    {
        // trigger BoardManager building board, returning a list of tiles
        InitializeUI();
        StartPlayerTurn();
    }


    void InitializeUI()
    {
        OnBankUpdate.Invoke(bank);
        OnTurnCountUpdate.Invoke(turnCount);
    }

    void StartPlayerTurn()
    {
        // Set cash (passive income?)
        // Deal Hand
        // Set State to PlayerTurn
        OnPlayerTurn.Invoke();
        state = States.PlayerTurn;
    }

    public void EndTurn()
    {
        // Check hazards
        // Calculate tiles
        // Add card to deck
        // turn count ++
        // Start Player TurnCount
        Debug.Log("Ending turn");
        state = States.EndTurn;
        OnEndTurn.Invoke();
        StartCoroutine(EndOfTurnDelay());
    }

    IEnumerator EndOfTurnDelay()
    {
        yield return new WaitForSeconds(turnDelay);
        Debug.Log("Starting new turn");
        turnCount++;
        OnTurnCountUpdate.Invoke(turnCount);
        StartPlayerTurn();
    }
}
