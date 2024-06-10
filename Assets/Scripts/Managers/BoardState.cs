using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using Object = System.Object;

public class BoardState : MonoBehaviour
{
    /*
     * Board state is used to create and effect simulated characters and tiles that correlate to the actual character and tiles on the board
     * We do this so that we can alter and effectively play with the characters, city managers and game bor without having to revert these changes each time the minimax runs.
     * This effectively allows us to simulate every action done in a turn and assess the turn players gamestate at the end of the turn without effecting the actual game
     * To do this we create dummy character objects for all the summoned characters and apply their corresponding actul game pieces stats (location, current health, character type etc to them.
     * Then depending on the action decided upon by the NewAIFunction utility calculation we simulate the effect of the chosen move on the simulated board and characters
     * We then create an ExecutableMethod (a separate class found at the bottom of the script) that holds a reference to the method chosen for the move (in the form of an enum) as well as
     * object references to the apropriate variables needed for the move to be done (i.e if the action chosen was to move a piece we need to store a reference to the specific character being moved and which tile it is moving to).
     * These Executable methods are stored in a list that represents 1 full turn
     * The GameState is then calculated on the end result of the simulated turn and its game score is stored in a list
     * After a full turn is simulated the list of moves for that turn is stored in another list that stores the list of every simulated turn
     */
    public GridManager _GridManager;
    private GameObject[] presortedTiles;
    
    public GameObject[,] tiles = new GameObject[20,20];

    public TurnManager simTurnManager;
    private TurnManager.TurnOrder turnPlayer;

    public CityManager playerManager, AIManager, turnStartPlayerManager, turnStartAIManager;
    public Tile tile;
    public List<GameObject> turnStartPlayerArmy, turnStartAIArmy, turnStartPlayerWorkers, turnStartAIWorkers,
        playerArmy, aiArmy, playerWorkers, aiWorkers;
    // Start is called before the first frame update
    
    public List<MethodVariables> methodsForTurn = new List<MethodVariables>();

    public List<List<MethodVariables>> listOfTurns = new List<List<MethodVariables>>();
    void Start()
    {

        presortedTiles = null;
    }

    
    //Creates simulated versions of all tiles and summoned characters and sets all the stats apropriately.
    public void TurnStartUpdateBoardState()
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
                    Tile presortedTile = presortedTiles[sentinal].GetComponent<Tile>();
                    tiles[i, j].AddComponent<Tile>();
                    Tile newTile = tiles[i, j].GetComponent<Tile>();
                    newTile._occupied = presortedTile._occupied;
                    newTile.tileInfo = presortedTile.tileInfo;
                    newTile.x = presortedTile.x;
                    newTile.y = presortedTile.y;
                   // newTile.HasMine = presortedTile.HasMine;
                    
                    sentinal++;
                 
                }
            
            }
        }

       
        turnStartAIManager = GameObject.FindGameObjectWithTag("MonsterB").GetComponent<CityManager>();
        turnStartPlayerManager = GameObject.FindGameObjectWithTag("HumanB").GetComponent<CityManager>();
        playerManager = new CityManager();
        AIManager = new CityManager();
        playerManager._gpt = turnStartPlayerManager._gpt;
        AIManager._cityHealth = turnStartAIManager._cityHealth;
        AIManager._gpt = turnStartAIManager._gpt;
        turnStartPlayerArmy = turnStartPlayerManager.summonedArmy;
        turnStartAIArmy = turnStartAIManager.summonedArmy;
        turnStartPlayerWorkers = turnStartPlayerManager.summonedWorkers;
        turnStartAIWorkers = turnStartAIManager.summonedWorkers;
        tile = tiles[turnStartPlayerManager.tileBelow.GetComponent<Tile>().x,
            turnStartPlayerManager.tileBelow.GetComponent<Tile>().y].GetComponent<Tile>();
        playerManager.tileBelow = tile.gameObject;
        tile._occupied = true;
        playerManager._cityHealth = turnStartPlayerManager._cityHealth;
        tile = tiles[turnStartAIManager.tileBelow.GetComponent<Tile>().x,
            turnStartAIManager.tileBelow.GetComponent<Tile>().y].GetComponent<Tile>();
        AIManager.tileBelow = tile.gameObject;
        tile._occupied = true;
        playerManager.worker = turnStartPlayerManager.worker;
        playerManager.meele = turnStartPlayerManager.meele;
        playerManager.ranger = turnStartPlayerManager.ranger;
        AIManager.worker = turnStartAIManager.worker;
        AIManager.meele = turnStartAIManager.meele;
        AIManager.ranger = turnStartAIManager.ranger;
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
    
    //Simulates moving a character to a new position on the board and creates the apropriate MethodVariables object and adds it to the list for the turn
    public void UpdateBoardMove(GameObject character, GameObject tile)
    {
        Tile SelectedTile = tile.GetComponent<Tile>();
        Character characterControl = character.GetComponent<Character>();
        characterControl.availableMoves--;
        Tile tileScript = characterControl.Occupiedtile;
        tileScript._occupied = false;
        SelectedTile._occupied = true;
        for (int i = 0; i < turnStartAIManager.summonedArmy.Count; i++)
        {
            if (character == turnStartAIManager.summonedArmy[i])
            {
                GameObject actualCharacter = turnStartAIManager.summonedArmy[i];
                MethodVariables methodVariables = new MethodVariables();
                methodVariables.ExecutableMethod = MethodVariables.Methods.Move;
                methodVariables.variables = new object[] { actualCharacter, SelectedTile };
                methodsForTurn.Add(methodVariables);
            }
        }
        for (int i = 0; i < turnStartAIManager.summonedWorkers.Count; i++)
        {
            if (character == turnStartAIManager.summonedWorkers[i])
            {
                GameObject actualCharacter = turnStartPlayerManager.summonedWorkers[i];
                MethodVariables methodVariables = new MethodVariables();
                methodVariables.ExecutableMethod = MethodVariables.Methods.Move;
                methodVariables.variables = new object[] { actualCharacter, SelectedTile };
                methodsForTurn.Add(methodVariables);
            }
        }
        
      //  CurrentTile = SelectedTile.gameObject;
    }

    //Simulates an attack and creates the apropriate MethodVariables object and adds it to the list for the turn
    public void UpdateBoardAttack(GameObject attacker, GameObject defender)
    {
        
        if (defender == playerManager.gameObject)
        {
            playerManager.takeDamage(attacker.GetComponent<CharacterScriptable>().damage);
            
            MethodVariables methodVariables = new MethodVariables();
            methodVariables.ExecutableMethod = MethodVariables.Methods.Attack;
            methodVariables.variables = new object[] { attacker, defender };
            methodsForTurn.Add(methodVariables);
            
        }

        if (defender == AIManager.gameObject)
        {
           AIManager.takeDamage(attacker.GetComponent<CharacterScriptable>().damage);
        }

        if (defender != AIManager && defender != playerManager.gameObject)
        {
            defender.GetComponent<Character>().TakeDamage(attacker.GetComponent<CharacterScriptable>().damage);
            MethodVariables methodVariables = new MethodVariables();
            methodVariables.ExecutableMethod = MethodVariables.Methods.Attack;
            methodVariables.variables = new object[] { attacker, defender };
            methodsForTurn.Add(methodVariables);
        }
        
        //Add destroy if needed
    }

    //Simulates a citymanager healing creates the apropriate MethodVariables object and adds it to the list for the turn
    public void UpdateBoardHeal(TurnManager.TurnOrder turnPlayer)
    {
        if (turnPlayer == TurnManager.TurnOrder.AI)
        {
            AIManager._cityHealth += 10;
            MethodVariables methodVariables = new MethodVariables();
            methodVariables.ExecutableMethod = MethodVariables.Methods.Heal;
            methodsForTurn.Add(methodVariables);
        }
        else
        {
            playerManager._cityHealth += 10;
        }
    }

    //Simulates a new character to the board and creates the apropriate MethodVariables object and adds it to the list for the turn
    public void UpdateBoardSummon(GameObject chosenPiece, TurnManager.TurnOrder turnPlayer)
    {
       //TODO talk to josh about instantiate vs creat new gameobject and add character component as above
        if (turnPlayer == TurnManager.TurnOrder.AI)
        { 
            GameObject TargetPosition = AIManager.tileBelow.GetComponent<Tile>().getSurroundingBlocks();
            Vector3 spawnPosition = new Vector3(TargetPosition.transform.position.x, TargetPosition.transform.position.y+10.1f,TargetPosition.transform.position.z); 
            GameObject newCharacter = Instantiate(chosenPiece, spawnPosition, Quaternion.identity); 
            Character newCharacterControl = newCharacter.GetComponent<Character>();
           aiArmy.Add(newCharacter);
           MethodVariables methodVariables = new MethodVariables();
           methodVariables.ExecutableMethod = MethodVariables.Methods.Summon;
           methodVariables.variables = new object[] { chosenPiece };
           methodsForTurn.Add(methodVariables);
          /* if (newCharacterControl.characterScript.CharacterType == CharacterScriptable.characterType.Miner)
           {
               aiWorkers.Add(newCharacter);
           }
           else
           {
               aiArmy.Add(newCharacter);
           }*/
        }
        if(turnPlayer == TurnManager.TurnOrder.Player1)
        {
            GameObject TargetPosition = AIManager.tileBelow.GetComponent<Tile>().getSurroundingBlocks();
            Vector3 spawnPosition = new Vector3(TargetPosition.transform.position.x, TargetPosition.transform.position.y+10.1f,TargetPosition.transform.position.z);
            GameObject newCharacter = Instantiate(chosenPiece, spawnPosition, Quaternion.identity);
            Character newCharacterControl = newCharacter.GetComponent<Character>();
            aiArmy.Add(newCharacter);
          /*  if (newCharacterControl.characterScript.CharacterType == CharacterScriptable.characterType.Miner)
            {
                playerWorkers.Add(newCharacter);
            }
            else
            {
                playerArmy.Add(newCharacter);
            }*/
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    
    
}

public class MethodVariables
{
    public enum Methods
    {
        Move,
        Attack,
        Summon,
        Heal
    }

    public  Methods ExecutableMethod;

    public object[] variables;
    
}
