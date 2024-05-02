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
    
    
    public void SpawnHArcher()
    {
        if (player1turn)
        {
            SpawnHA?.Invoke();
        }
        else
        {
            SpawnMA?.Invoke();
        }
    }
    
    public void SpawnHMeele()
    {
        if (player1turn)
        {
            SpawnHM?.Invoke();

        }
        else
        {
            SpawnMM?.Invoke();

        }

    }
    
    public void SpawnHMiner()
    {
        if (player1turn)
        {        
            SpawnHMi?.Invoke();
        }
        else
        {
            SpawnMMi?.Invoke();
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
