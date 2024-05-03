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
        Player2
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
        Debug.Log("roundEnded");
        if (TurnPlayer == TurnOrder.Player1)
        {
            TurnPlayer = TurnOrder.Player2;
        }
        else
        {
            TurnPlayer = TurnOrder.Player1;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
