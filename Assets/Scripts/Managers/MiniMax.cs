using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMax : MonoBehaviour
{
    public Gamemanager _Gamemanager;
    public CityManager player1, player2;
    public AiGameState _GameState;
    public float currentGameState;

    public BoardState _BoardState;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GameStart()
    {
        player1 = GameObject.FindGameObjectWithTag("HumanB").GetComponent<CityManager>();
        player2 = GameObject.FindGameObjectWithTag("MonsterB").GetComponent<CityManager>();
        
    }

    public void TurnStartCheckBoardState()
    {
        _BoardState.TurnStartUpdateBoardState();
    }
    public int RunMiniMax(float difficulty, bool isMax)
    {
        
    int miniMax = 0; 
    /*
         for (however many times we want minmax to run)
         {
             UpdateBoardState for start of turn
             for (AP)
                 {
                   Run AIFunction utility check
                   apply the chosen function to the board state (sim functions in board state)
                   store the function in a list
                   can even be a string list like "Attack(GameObject attcker, GameObject target)"
                 }
             Run the game state check
             add that gamestate value to a list

             Once the above is done, check for highest game state value
             execute the stored array of functions in the actual game
             clear all lists
         }
        */
    return miniMax;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
