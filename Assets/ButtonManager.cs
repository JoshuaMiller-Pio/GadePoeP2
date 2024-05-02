using System;

using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public static event Action SpawnHA;
    public static event Action SpawnHM;
    public static event Action SpawnHMi;
    
    public static event Action SpawnMA;
    public static event Action SpawnMM;
    public static event Action SpawnMMi;
    
    public static event Action onMovePressed;

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

    public void EndTurn()
    {
        if (TurnManager.TurnPlayer == TurnManager.TurnOrder.Player1)
        {
            TurnManager.TurnPlayer = TurnManager.TurnOrder.Player2;
        }
        else
        {
            TurnManager.TurnPlayer = TurnManager.TurnOrder.Player1;
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

    public void moveCharacter()
    {
        onMovePressed?.Invoke();
    }
    private void Start()
    {
        TurnManager.RoundEnd += switchPlayer;
    }
}
