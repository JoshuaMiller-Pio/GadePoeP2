using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public static event Action SpawnHA;
    public static event Action SpawnHM;
    public static event Action SpawnHMi;
    
    public static event Action SpawnMA;
    public static event Action SpawnMM;
    public static event Action SpawnMMi;

    private bool player1turn = true;
    
    
    public void SpawnArcher()
    {
        if (TurnManager.TurnPlayer == TurnManager.TurnOrder.Player1)
        {
            SpawnHA?.Invoke();
        }
        else
        {
            SpawnMA?.Invoke();
            Debug.Log("AH");
        }
    }
    
    public void SpawnMeele()
    {
        if (TurnManager.TurnPlayer == TurnManager.TurnOrder.Player1)
        {
            SpawnHM?.Invoke();

        }
        else
        {
            SpawnMM?.Invoke();
            Debug.Log("AH");

        }

    }
    
    public void SpawnMiner()
    {
        if (TurnManager.TurnPlayer == TurnManager.TurnOrder.Player1)
        {        
            SpawnHMi?.Invoke();
        }
        else
        {
            SpawnMMi?.Invoke();
            Debug.Log("AH");

        }

    }

    void switchPlayer()
    {
        if (player1turn)
        {
            player1turn = false;
        }
        else
        {
            player1turn = true;
        }
    }
    
    private void Start()
    {
        TurnManager.RoundEnd += switchPlayer;
    }
}
