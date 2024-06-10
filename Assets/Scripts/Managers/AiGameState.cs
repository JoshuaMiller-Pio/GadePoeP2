using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiGameState : MonoBehaviour
{
    /*
     * AIGameState is used to evaluate the state of the game at the end of a turn
     * When called the calculateGameState method is passed a reference to 2 city manager scripts (this can be done on either the actual games city manager r the simulated city managers usd for the minimax
     * There are several metrics used to asses the state of the game (Both players army sizes, how many mines they control, city health etc)
     * A score of 1, 0 or -1 is applied to the turn players board state depending on the comparison of the above mentioned variables
     * All of the above scores are added together to represent the overall gamestate faced by the player character.
     */
    public float turnPlayerArmyPop { get; set; }
    public float enemyPlayerArmyPop { get; set; }
    public float turnPlayerWorkerPop { get; set; }
    public float enemyPlayerWorkerPop { get; set; }
    public float turnPlayerHealth { get; set; }
    
    public float armyHealthState { get; set; }
    public float enemyHealth { get; set; }
    public float armyPopState { get; set; }
    public float workerPopState { get; set; }
    public float HealthState { get; set; }
    public float mineState { get; set; }
    public float turnPlayerArmyHealth { get; set; }
    public float enemyArmyHealth { get; set; }
    public float turnPlayerMines { get; set; }
    public float enemyPlayerMines { get; set; }
    public float gameState { get; set; }
    private TurnManager _turnManager;
    private CityManager turnPlayerCityManager, enemyPlayerCityManager;
    // Start is called before the first frame update
    void Start()
    {
        _turnManager = GameObject.FindGameObjectWithTag("TurnManager").GetComponent<TurnManager>();
    }

    public float CalculateGameState(TurnManager.TurnOrder turnPlayer, CityManager player, CityManager ai)
    {
        if (turnPlayer == TurnManager.TurnOrder.Player1)
        {
            turnPlayerCityManager = player;
            enemyPlayerCityManager = ai;
        }
        else
        {
            enemyPlayerCityManager = player;
            turnPlayerCityManager = ai;
        }

        turnPlayerArmyPop= turnPlayerCityManager._aPop;
        enemyPlayerArmyPop = enemyPlayerCityManager._aPop;
        turnPlayerWorkerPop = turnPlayerCityManager._bPop;
        enemyPlayerWorkerPop = enemyPlayerCityManager._bPop;
        turnPlayerHealth = turnPlayerCityManager.CityHealth;
        enemyHealth = enemyPlayerCityManager.CityHealth;
        turnPlayerArmyHealth = turnPlayerCityManager.UpdateArmyHealth();
        enemyArmyHealth = enemyPlayerCityManager.UpdateArmyHealth();
        turnPlayerMines = turnPlayerCityManager.UpdatesControlledMines();
        enemyPlayerMines = enemyPlayerCityManager.UpdatesControlledMines();
        CalculateArmyPopState();
        CalculateWorkerPopState();
        CalculateHealthState();
        CalculatePlayerArmyHealth();
        CalculateControlledMinesState();
        

        gameState = HealthState + mineState + armyPopState + workerPopState + armyHealthState;

        if (gameState > 1)
        {
            gameState = 1;
        }

        if (gameState < -1)
        {
            gameState = -1;
        }
        
        return gameState;
    }

    public void CalculateArmyPopState()
    {
        if (turnPlayerArmyPop < enemyPlayerArmyPop)
        {
            armyPopState = -1;
        }
        if (turnPlayerArmyPop == enemyPlayerArmyPop)
        {
            armyPopState = 0;
        }
        if (turnPlayerArmyPop > enemyPlayerArmyPop)
        {
            armyPopState = 1;
        }
    }
    
    public void CalculateWorkerPopState()
    {
        if (turnPlayerWorkerPop < enemyPlayerWorkerPop)
        {
            workerPopState = -1;
        }
        if (turnPlayerWorkerPop == enemyPlayerWorkerPop)
        {
            workerPopState = 0;
        }
        if (turnPlayerWorkerPop > enemyPlayerWorkerPop)
        {
            workerPopState = 1;
        }
    }

    public void CalculateHealthState()
    {
        if (turnPlayerHealth < enemyHealth)
        {
            HealthState = -1;
        }
        if (turnPlayerHealth == enemyHealth)
        {
            HealthState = 0;
        }
        if (turnPlayerHealth > enemyHealth)
        {
            HealthState = 1;
        }
    }

    public void CalculatePlayerArmyHealth()
    {
        if (turnPlayerArmyHealth < enemyArmyHealth)
        {
            armyHealthState = -1;
        }
        
        if (turnPlayerArmyHealth == enemyArmyHealth)
        {
            armyHealthState = 0;
        }
        
        if (turnPlayerArmyHealth > enemyArmyHealth)
        {
            armyHealthState = 1;
        }
    }

    public void CalculateControlledMinesState()
    {
        if (turnPlayerMines < enemyPlayerMines)
        {
            mineState = -1;
        }
        
        if (turnPlayerMines == enemyPlayerMines)
        {
            mineState = 0;
        }
        
        if (turnPlayerMines > enemyPlayerMines)
        {
            mineState = 1;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
