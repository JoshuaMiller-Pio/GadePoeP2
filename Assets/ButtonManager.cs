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
    public static event Action onAttackPressed;
    public static event Action onHReinforcedPressed;
    public static event Action onMReinforcedPressed;
    public NewAIFunction _newAIFunction;
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


    void switchPlayer()
    {
        if (player1turn)
        {
            player1turn = false;
            if (Gamemanager.Instance.AIPlayer)
            {
                _newAIFunction = Gamemanager.Instance.GetComponent<NewAIFunction>();
                _newAIFunction._BoardState.TurnStartUpdateBoardState();
                //run minimax
            }
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
    public void AttackCharacter()
    {
        onAttackPressed?.Invoke();
    }
    private void Start()
    {
        TurnManager.RoundEnd += switchPlayer;
    }
    public void reinforce()
    {
        if (TurnManager.TurnPlayer == TurnManager.TurnOrder.Player1)
        {
            onHReinforcedPressed?.Invoke();
        }
        else
        {
            
            onMReinforcedPressed?.Invoke();
        }
    }
}
