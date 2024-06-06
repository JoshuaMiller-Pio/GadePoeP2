using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnOrder TurnPlayer;
    public static event Action RoundEnd;
    
    public enum TurnOrder
    {
        Player1,
        Player2,
        AI
    };
    
    // Start is called before the first frame update
    void Start()
    {
        TurnPlayer = TurnOrder.Player1;
        ResetGame();
    }

    
    public void ResetGame()
    {
        TurnPlayer = TurnOrder.Player1;
        
    }
    
    public void EndTurn()
    {
        RoundEnd?.Invoke();
        Gamemanager.Instance.selectedunit = null;
        Gamemanager.Instance.selectedEnemy = null;
        if (TurnPlayer == TurnOrder.Player1 )
        { 
            if ( !Gamemanager.Instance.AIPlayer)
            {
                TurnPlayer = TurnOrder.Player2;
                
            }
            else
            {
                TurnPlayer = TurnOrder.AI;
               
                    AIFunction.Instance.AIUtilityFunction();

                
            }
            

        }
        else
        {
            TurnPlayer = TurnOrder.Player1;
        }
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Gamemanager.Instance.currentAP <= 0)
        {
            EndTurn();
        }
    }
}
