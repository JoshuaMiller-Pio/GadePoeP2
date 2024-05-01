using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public Gamemanager _gameManager;
    public TurnOrder turnPlayer;
    public enum TurnOrder
    {
        Player1,
        Player2
    };
    
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = Gamemanager.Instance;
        ResetGame();
    }

    public void ResetGame()
    {
        turnPlayer = TurnOrder.Player1;
    }
    
    public void EndTurn(TurnOrder currentTurnPlayer)
    {
        if (currentTurnPlayer == TurnOrder.Player1)
        {
            turnPlayer = TurnOrder.Player2;
        }
        else
        {
            turnPlayer = TurnOrder.Player1;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
