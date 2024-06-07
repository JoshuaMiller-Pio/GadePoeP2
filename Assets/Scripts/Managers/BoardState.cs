using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class BoardState : MonoBehaviour
{
    public GridManager _GridManager;
    public GameObject[] presortedTiles;

    public GameObject[,] tiles = new GameObject[20,20];

    public TurnManager simTurnManager;
    private TurnManager.TurnOrder turnPlayer;

    public CityManager playerManager, AIManager, turnStartPlayerManager, turnStartAIManager;

    public List<GameObject> turnStartPlayerArmy, turnStartAIArmy, turnStartPlayerWorkers, turnStartAIWorkers,
        playerArmy, aiArmy, playerWorkers, aiWorkers;
    // Start is called before the first frame update
    void Start()
    {
        turnPlayer = TurnManager.TurnOrder.AI;
        if (presortedTiles == null)
        {
            presortedTiles = GameObject.FindGameObjectsWithTag("Tile");
            int sentinal = 0, lavas = 0, golds =0;
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    tiles[i,j] = presortedTiles[sentinal];
                    sentinal++;
                 
                }
            
            }
        }
    }

    public void TurnStartUpdateBoardState()
    {
        turnStartAIManager = GameObject.FindGameObjectWithTag("MonsterB").GetComponent<CityManager>();
        turnStartPlayerManager = GameObject.FindGameObjectWithTag("HumanB").GetComponent<CityManager>();
        playerManager._cityHealth = turnStartPlayerManager._cityHealth;
        playerManager._gpt = turnStartPlayerManager._gpt;
        AIManager._cityHealth = turnStartAIManager._cityHealth;
        AIManager._gpt = turnStartAIManager._gpt;
        turnStartPlayerArmy = turnStartPlayerManager.summonedArmy;
        turnStartAIArmy = turnStartAIManager.summonedArmy;
        turnStartPlayerWorkers = turnStartPlayerManager.summonedWorkers;
        turnStartAIWorkers = turnStartAIManager.summonedWorkers;
        
        for (int j = 0; j < turnStartPlayerArmy.Count; j++)
        {
            GameObject newArmy = new GameObject();
            newArmy.AddComponent<Character>();
            Character newArmyCharacter = newArmy.GetComponent<Character>();
            Character actualArmyCharacter = turnStartPlayerArmy[j].GetComponent<Character>();
            newArmyCharacter.characterScript = actualArmyCharacter.characterScript;
            newArmyCharacter.Occupiedtile = actualArmyCharacter.Occupiedtile;
            newArmyCharacter.damage = actualArmyCharacter.damage;
            newArmyCharacter.availableMoves = actualArmyCharacter.availableMoves;
            newArmyCharacter.canMove = actualArmyCharacter.canMove;
            newArmyCharacter.currentHealth = actualArmyCharacter.currentHealth;
            newArmyCharacter.moveableTiles = actualArmyCharacter.moveableTiles;
            playerArmy.Add(newArmy);
        }
        
        for (int j = 0; j < turnStartAIArmy.Count; j++)
        {
            GameObject newArmy = new GameObject();
            newArmy.AddComponent<Character>();
            Character newArmyCharacter = newArmy.GetComponent<Character>();
            Character actualArmyCharacter = turnStartAIArmy[j].GetComponent<Character>();
            newArmyCharacter.characterScript = actualArmyCharacter.characterScript;
            newArmyCharacter.Occupiedtile = actualArmyCharacter.Occupiedtile;
            newArmyCharacter.damage = actualArmyCharacter.damage;
            newArmyCharacter.availableMoves = actualArmyCharacter.availableMoves;
            newArmyCharacter.canMove = actualArmyCharacter.canMove;
            newArmyCharacter.currentHealth = actualArmyCharacter.currentHealth;
            newArmyCharacter.moveableTiles = actualArmyCharacter.moveableTiles;
            aiArmy.Add(newArmy);
        }
        
        for (int j = 0; j < turnStartPlayerWorkers.Count; j++)
        {
            GameObject newWorker = new GameObject();
            newWorker.AddComponent<Character>();
            Character newWorkerCharacter = newWorker.GetComponent<Character>();
            Character actualWorkerCharacter = turnStartPlayerArmy[j].GetComponent<Character>();
            newWorkerCharacter.characterScript = actualWorkerCharacter.characterScript;
            newWorkerCharacter.Occupiedtile = actualWorkerCharacter.Occupiedtile;
            newWorkerCharacter.damage = actualWorkerCharacter.damage;
            newWorkerCharacter.availableMoves = actualWorkerCharacter.availableMoves;
            newWorkerCharacter.canMove = actualWorkerCharacter.canMove;
            newWorkerCharacter.currentHealth = actualWorkerCharacter.currentHealth;
            newWorkerCharacter.moveableTiles = actualWorkerCharacter.moveableTiles;
            playerWorkers.Add(newWorker);
        }
        
        for (int j = 0; j < turnStartAIWorkers.Count; j++)
        {
            GameObject newWorker = new GameObject();
            newWorker.AddComponent<Character>();
            Character newWorkerCharacter = newWorker.GetComponent<Character>();
            Character actualWorkerCharacter = turnStartPlayerArmy[j].GetComponent<Character>();
            newWorkerCharacter.characterScript = actualWorkerCharacter.characterScript;
            newWorkerCharacter.Occupiedtile = actualWorkerCharacter.Occupiedtile;
            newWorkerCharacter.damage = actualWorkerCharacter.damage;
            newWorkerCharacter.availableMoves = actualWorkerCharacter.availableMoves;
            newWorkerCharacter.canMove = actualWorkerCharacter.canMove;
            newWorkerCharacter.currentHealth = actualWorkerCharacter.currentHealth;
            newWorkerCharacter.moveableTiles = actualWorkerCharacter.moveableTiles;
            aiWorkers.Add(newWorker);
        }
        
        
    }

    public void UodateBoardState()
    {
        
    }
    public void UpdateBoardMove(GameObject character, GameObject tile)
    {
        Tile SelectedTile = tile.GetComponent<Tile>();
        Character characterControl = character.GetComponent<Character>();
        characterControl.availableMoves--;
        Tile tileScript = characterControl.Occupiedtile;
        tileScript._occupied = false;
        SelectedTile._occupied = true;
        
      //  CurrentTile = SelectedTile.gameObject;
    }

    public void UpdateBoardAttack(GameObject attacker, GameObject defender)
    {
        defender.GetComponent<Character>().TakeDamage(attacker.GetComponent<CharacterScriptable>().damage);
        //Add destroy if needed
    }

    public void UpdateBoardHeal(TurnManager.TurnOrder turnPlayer)
    {
        if (turnPlayer == TurnManager.TurnOrder.AI)
        {
            AIManager._cityHealth += 10;
        }
        else
        {
            playerManager._cityHealth += 10;
        }
    }

    public void UpdateBoardSummon(GameObject chosenPiece, GameObject targetLocation, TurnManager.TurnOrder turnPlayer)
    {
        if (turnPlayer == TurnManager.TurnOrder.AI)
        {
           GameObject newCharacter = Instantiate(chosenPiece, targetLocation.transform.position, Quaternion.identity);
           Character newCharacterControl = newCharacter.GetComponent<Character>();
           if (newCharacterControl.characterScript.CharacterType == CharacterScriptable.characterType.Miner)
           {
               aiWorkers.Add(newCharacter);
           }
           else
           {
               aiArmy.Add(newCharacter);
           }
        }
        else
        {
            GameObject newCharacter = Instantiate(chosenPiece, targetLocation.transform.position, Quaternion.identity);
            Character newCharacterControl = newCharacter.GetComponent<Character>();
            if (newCharacterControl.characterScript.CharacterType == CharacterScriptable.characterType.Miner)
            {
                playerWorkers.Add(newCharacter);
            }
            else
            {
                playerArmy.Add(newCharacter);
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
