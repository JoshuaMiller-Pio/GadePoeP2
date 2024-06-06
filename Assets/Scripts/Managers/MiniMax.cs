using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMax : MonoBehaviour
{
    public Gamemanager _Gamemanager;
    public CityManager player1, player2;
    public AiGameState _GameState;
    public float currentGameState;
    public 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GameStart()
    {
        player1 = GameObject.FindGameObjectWithTag("HumanB").GetComponent<CityManager>();
        player2 = GameObject.FindGameObjectWithTag("MonsterB").GetComponent<CityManager>();
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
