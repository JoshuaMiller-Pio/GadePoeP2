using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiGameState : MonoBehaviour
{
    public float turnPlayerArmyPop { get; set; }
    public float enemyPlayerArmyPop { get; set; }
    public float turnPlayerWorkerPop { get; set; }
    public float enemyPlayerWorkerPop { get; set; }
    public float turnPlayerHealth { get; set; }
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

    public float CalculateGameState(TurnManager.TurnOrder turnPlayer)
    {
        if (turnPlayer == TurnManager.TurnOrder.Player1)
        {
            turnPlayerCityManager = GameObject.FindGameObjectWithTag("HumanB").GetComponent<CityManager>();
            enemyPlayerCityManager = GameObject.FindGameObjectWithTag("MonsterB").GetComponent<CityManager>();
        }
        else
        {
            enemyPlayerCityManager = GameObject.FindGameObjectWithTag("HumanB").GetComponent<CityManager>();
            turnPlayerCityManager = GameObject.FindGameObjectWithTag("MonsterB").GetComponent<CityManager>();
        }

        turnPlayerArmyPop= turnPlayerCityManager._aPop;
        enemyPlayerArmyPop = enemyPlayerCityManager._aPop;
        turnPlayerWorkerPop = turnPlayerCityManager._bPop;
        enemyPlayerWorkerPop = enemyPlayerCityManager._bPop;
        turnPlayerHealth = turnPlayerCityManager.CityHealth;
        enemyHealth = enemyPlayerCityManager.CityHealth;


        CalculateArmyPopState();
        CalculateWorkerPopState();
        CalculateHealthState();


        gameState = HealthState + mineState + armyPopState + workerPopState;

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
    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
