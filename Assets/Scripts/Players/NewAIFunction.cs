using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Polytope;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class NewAIFunction : MonoBehaviour
{
      private GameObject[] presortedTiles;
    public List<GameObject> Munits, Hunits;
    private GameObject[,] tiles = new GameObject[20,20];
    private GameObject Hbase, Mbase, ChosenUnit;
    private Tile HbaseTile, MbaseTile;
    private Dictionary<int, LavaLocations> _lavaLocationsMap = new Dictionary<int, LavaLocations>();
    private Dictionary<int, GoldLocations> _goldLocationsMap = new Dictionary<int, GoldLocations>();
    private GameObject selectedMoveUnit;
    private GameObject goldTarget;
    private float Archers = 0, army = 0;
    public  event Action onAIReinforcedPressed;
    public  event Action SpawnAIA;
    public  event Action SpawnAIM;
    public  event Action SpawnAIR;
    public event Action UpdateBoard;
    public BoardState _BoardState;
    public TurnManager _TurnManager;
    public TurnManager.TurnOrder turnPlayer;
    public AiGameState _AiGameState;
    public List<float> gameStateScores;
    private enum Summonable
    {
        Army,
        ranged,
        Miner
    }
    
    private enum action
    {
        noAction,
        Attack,
        Defend,
        Summon,
        move
    }

    private action currentAction = action.noAction;
    public enum move
    { 
       noMove,
       left,
       right,
       forward,
       back
    }

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public Difficulty currentD = Difficulty.Easy;
    private move currentMove = move.noMove;
    
    private Summonable WhatToSummon;

    private void OnEnable()
    {
        turnPlayer = TurnManager.TurnOrder.AI;
        _BoardState = GameObject.FindGameObjectWithTag("BoardState").GetComponent<BoardState>();
        if (presortedTiles == null)
        {
            presortedTiles = GameObject.FindGameObjectsWithTag("Tile");
            int sentinal = 0, lavas = 0, golds = 0;
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    tiles[i, j] = presortedTiles[sentinal];
                    sentinal++;
                    _lavaLocationsMap.Add(lavas, new LavaLocations(i, j));
                    _goldLocationsMap.Add(lavas, new GoldLocations(i, j));
                    lavas++;
                    golds++;
                }

            }
            if (Mbase == null)
            {

                //TODO Switch between difficulties
                switch (currentD)
                {
                    case Difficulty.Easy:
                        Gamemanager.Instance.placeAITown(tiles[16, 8]);
                        break;
                    case Difficulty.Medium:
                        Gamemanager.Instance.placeAITown(tiles[9, 6]);
                        break;
                    case Difficulty.Hard:
                        Gamemanager.Instance.placeAITown(tiles[9, 16]);
                        break;
                }

                Mbase = GameObject.FindGameObjectWithTag("MonsterB");
                MbaseTile = Mbase.GetComponent<CityManager>().tileBelow.GetComponent<Tile>();
             //   Mbase = _BoardState.AIManager.gameObject;
             //   MbaseTile = _BoardState.AIManager.tileBelow.GetComponent<Tile>();

            }
            _BoardState.TurnStartUpdateBoardState();
            tiles = _BoardState.tiles;
            RunAIUtility();


        }
    }

    public void RunAIUtility()
    {
        _BoardState.TurnStartUpdateBoardState();
        tiles = _BoardState.tiles;
        for (int i = 0; i < 5; i++)
        {
            AIUtilityFunction(); 
           
        }
        _BoardState.listOfTurns.Add(_BoardState.methodsForTurn);
        _BoardState.methodsForTurn.Clear();
        float gameStateScore =
            _AiGameState.CalculateGameState(turnPlayer, _BoardState.playerManager, _BoardState.AIManager);
        gameStateScores.Add(gameStateScore);
    }

    public void AIUtilityFunction()
    {
        _BoardState.TurnStartUpdateBoardState();
        //assigns bases
        if (Hbase == null)
        {
           // Hbase = _BoardState.playerManager.gameObject;
           // HbaseTile = _BoardState.playerManager.tileBelow.GetComponent<Tile>();
            Hbase = GameObject.FindGameObjectWithTag("HumanB");
            HbaseTile = Hbase.GetComponent<CityManager>().tileBelow.GetComponent<Tile>();
        }

        

       /* Munits.Clear();
        Hunits.Clear();
        Archers = 0;
        army = 0;
        //assigns most resent units to list
        Munits = GameObject.FindGameObjectsWithTag("Monster").ToList();
        Hunits = GameObject.FindGameObjectsWithTag("Human").ToList();
        */
       Munits = _BoardState.aiArmy;
       foreach (var character in _BoardState.playerArmy)
       {
           Hunits.Add(character);
       }
       foreach (var character in _BoardState.playerWorkers)
       {
           Hunits.Add(character);
       }
        //Sets to how many units there are, used to act as a reference pointer to a specific character in the utility array
            float[] unitUtilitiy = new float[Munits.Count];
            float[] unitUtilitiyH = new float[Hunits.Count];
            GameObject MoveUnit = null,AttackUnit = null,summonUnit;
            //The utility of each option, set negative because welook for the highest
            float Mmax = -1000, Dmax = -1000, Amax = -1000, Smax = -1000, utility1, utility2;
            int position = 0;
        if (Munits.Count > 0)
        {
            for (int i = 0; i < Munits.Count; i++)
            {
                //stores all unit utilities in an array to select the biggest and use that as the piece to move, which piece we want to move
                unitUtilitiy[i] = piecetoMove(Munits[i]);
                Debug.Log("Unit utility = " + unitUtilitiy);
                
            }
            //Sorting to see which utility is the max of each unit, winner gets to move
            for (int i = 0; i < unitUtilitiy.Length; i++)
            {
                if (unitUtilitiy[i] > Mmax)
                {
                    Mmax = unitUtilitiy[i];
                    selectedMoveUnit    = Munits[i];
                    position = i;
                    
                }
            }
            //chosen moving unit
            MoveUnit = Munits[position];
           
            
        //Utility for attacking
        for (int i = 0; i < Munits.Count; i++)
        {
            //stores all unit utilities in an array to select the biggest and use that as the piece to attack 
            unitUtilitiy[i] = UAttack(Munits[i]);
            Debug.Log("Attack utility =" + unitUtilitiy[i]);
        }

        //Sorting highest ustility to attack
        for (int i = 0; i < unitUtilitiy.Length; i++)
        {
            if (unitUtilitiy[i] > Amax)
            {
                Amax = unitUtilitiy[i];
                Gamemanager.Instance.selectedunit = Munits[i];

                position = i;
            }
        }
            AttackUnit = Munits[position];
            
        }
        
        //Calls what to summon, doesn't need array to sort
        Smax = whatToSummon(); 
            
        //Utility for defending
        if (Hunits.Count >0)
        {
            

            for (int i = 0; i < Hunits.Count; i++)
            {
                //stores all unit utilities in an array to select the biggest and use that as the piece to attack 
                unitUtilitiyH[i] = Udefend(Hunits[i]);
                Debug.Log("Unit defend utility = " + unitUtilitiyH[i]);
            }
        }
      
        //Sorting dfence utility array
        for (int i = 0; i < unitUtilitiyH.Length; i++)
        {
            if (unitUtilitiyH[i] > Dmax)
            {
                Dmax = unitUtilitiyH[i];
            }
        }
        
         
        //Decides between move and defend
        action uAction1 = action.noAction, uAction2 = action.noAction;

        if (Mmax > Dmax)
        {
            utility1 = Mmax;
            uAction1 = action.move;
        }
        else
        {
            utility1 = Dmax;
            uAction1 = action.Defend;
        }

        //Decides between attack and summon
        if (Amax > Smax)
        {
            utility2 = Amax;
            uAction2 = action.Attack;

        }
        else
        {
            utility2 = Smax;
            uAction2 = action.Summon;
        }

        //Decides between the previous 2 decisions and the winner is the executed action
        if (utility1 > utility2)
        {
            currentAction = uAction1;
            
        }
        else
        {
            currentAction = uAction2;
        }
        
        Debug.Log("Chosen action =" + currentAction.ToString());
        
        //Calls the function of the decied action
        switch (currentAction)
        {
            case action.move:
                if (MoveUnit!= null)
                {
                    Move(MoveUnit);
                    
                }
                break;
            case action.Attack:
                Attack(AttackUnit);
                break;
            case action.Defend:
                Defend();
                break;
            case action.Summon:
                Summon();
                break;
        }
        
    }

/*                                    _____KEY_____
                        *Boolean Can move = BM (either = 0 or 1)
    Distance to Lava = DL distance check from game object to to closest lava tile / 100
    Distance to Enemy Piece = DEP distance check from game object to the closest Enemy Piece / 100
    Distance to Enemy City = DEC distance check from game object to the Enemy City / 100
    Distance to Gold Tile = DG distance check from game object to the closest Gold Tile / 100

 */

    //determines which piece we want to move, gets fed the unit array and returns the float of the utility (for each member of the array)
    public float piecetoMove(GameObject unit)
    {
        float final=0;
        Character charScript = unit.GetComponent<Character>();
     
        
        if (charScript.characterScript.CharacterType == CharacterScriptable.characterType.Miner && charScript.canMove )
        {
            //g = gold, l = lava. Final float added to unitUtility array
            float gmin = 1000, gdistance, lmin = 1000, ldistance;
            
            for (int i = 0; i < _goldLocationsMap.Count; i++)
            {
                gdistance = distance(charScript.Occupiedtile.x, _goldLocationsMap[i].x, charScript.Occupiedtile.y, _goldLocationsMap[i].y)/40;
                if (gmin>gdistance)
                {
                    gmin = gdistance;
                    goldTarget =tiles[_goldLocationsMap[i].x,_goldLocationsMap[i].y].gameObject;
                }
            }
            for (int i = 0; i < _lavaLocationsMap.Count; i++)
            {
                ldistance = distance(charScript.Occupiedtile.x, _lavaLocationsMap[i].x, charScript.Occupiedtile.y, _lavaLocationsMap[i].y);
                if (lmin>ldistance)
                {
                    lmin = ldistance;
                }
            }

            final = (Convert.ToInt32(charScript.canMove) * ((Hbase.GetComponent<CityManager>().TGold/Mbase.GetComponent<CityManager>().TGold) * (gmin +lmin) ))/40;
            Debug.Log(String.Format("gold:{0} lava:{1} final:{2}", gmin, lmin, final));
        }
        else if(charScript.canMove)
        {
            float emin = 1000, edistance, lmin = 1000, ldistance;
        
                for (int i = 0; i < _lavaLocationsMap.Count; i++)
                {
                    ldistance = distance(charScript.Occupiedtile.x, _lavaLocationsMap[i].x, charScript.Occupiedtile.x, _lavaLocationsMap[i].y);

                    if (lmin>ldistance)
                    {
                        lmin = ldistance;
                    }
                    
                }
            for (int i = 0; i < Hunits.Count; i++)
            {
                if (Hunits[i].GetComponent<Character>().Occupiedtile != null && charScript.Occupiedtile != null)
                {
                    edistance = distance(charScript.Occupiedtile.x, Hunits[i].GetComponent<Character>().Occupiedtile.x, charScript.Occupiedtile.x, Hunits[i].GetComponent<Character>().Occupiedtile.y);
                    
                    if (emin > edistance)
                    {
                        emin = edistance;
                    }
                }
               
            }

            float DEC = distance(charScript.Occupiedtile.x, HbaseTile.x, charScript.Occupiedtile.x, HbaseTile.y);
            if (DEC> emin)
            {
                final = (Convert.ToInt32(charScript.canMove) * ((Hbase.GetComponent<CityManager>().TGold/Mbase.GetComponent<CityManager>().TGold) *  (emin +lmin) ))/40;
            }
            else
            {
                final = (Convert.ToInt32(charScript.canMove) * ((Hbase.GetComponent<CityManager>().TGold/Mbase.GetComponent<CityManager>().TGold) *  (DEC +lmin) ))/40;

            }
            
        }
       Debug.Log(unit.name +final);
        return  final;
    }

    //Called after the action has been selected, takes in the game object that has been selected to move
    //Calculates utility of each direction based off of distance to target
    public void whereToMove(GameObject unit)
    {
        move optimalMoveY,optimalMoveX;
        float final = 0;
        GameObject target = null, closestUnit= null;
        if ( Hunits.Count > 0)
        {
            closestUnit= Hunits[0];
        }
        Character charScript = unit.GetComponent<Character>();
        float emin = 1000, edistance;
        float Ld=9999, Rd=9999, Fd=9999, Bd=9999;
        Gamemanager.Instance.selectedunit = selectedMoveUnit;
        for (int i = 0; i < Hunits.Count; i++)
        {
            if (true )
            {
                edistance = distance(charScript.Occupiedtile.x, Hunits[i].GetComponent<Character>().Occupiedtile.x, charScript.Occupiedtile.x, Hunits[i].GetComponent<Character>().Occupiedtile.y);
                if (emin > edistance)
                {
                    emin = edistance;
                    closestUnit = Hunits[i];
                }
            }
           
        }

       
            float cDistance = distance(charScript.Occupiedtile.x, HbaseTile.x, charScript.Occupiedtile.x, HbaseTile.y);

            //determins which target is closer and sets that as the target
         
                  if (emin > cDistance)
            {
                target = Hbase;
                //left
                if (charScript.Occupiedtile.x != 0 && !tiles[charScript.Occupiedtile.x - 1, charScript.Occupiedtile.y]
                        .GetComponent<Tile>()._occupied)
                {
               
                    Ld = distance(charScript.Occupiedtile.x - 1,
                        target.GetComponent<CityManager>().tileBelow.GetComponent<Tile>().x, charScript.Occupiedtile.y,
                        target.GetComponent<CityManager>().tileBelow.GetComponent<Tile>().y);
                }

                //back
                if (charScript.Occupiedtile.y != 0 && !tiles[charScript.Occupiedtile.x, charScript.Occupiedtile.y - 1]
                        .GetComponent<Tile>()._occupied)
                {
                    Bd = distance(charScript.Occupiedtile.x,
                        target.GetComponent<CityManager>().tileBelow.GetComponent<Tile>().x, charScript.Occupiedtile.y - 1,
                        target.GetComponent<CityManager>().tileBelow.GetComponent<Tile>().y);
                }

                //right
                if (charScript.Occupiedtile.x != 19 && !tiles[charScript.Occupiedtile.x + 1, charScript.Occupiedtile.y]
                        .GetComponent<Tile>()._occupied)
                {
                    Rd = distance(charScript.Occupiedtile.x + 1,
                        target.GetComponent<CityManager>().tileBelow.GetComponent<Tile>().x, charScript.Occupiedtile.y,
                        target.GetComponent<CityManager>().tileBelow.GetComponent<Tile>().y);
                }

                //forwards
                if (charScript.Occupiedtile.y != 19 && !tiles[charScript.Occupiedtile.x, charScript.Occupiedtile.y + 1]
                        .GetComponent<Tile>()._occupied)
                {
                    Fd = distance(charScript.Occupiedtile.x,
                        target.GetComponent<CityManager>().tileBelow.GetComponent<Tile>().x, charScript.Occupiedtile.y + 1,
                        target.GetComponent<CityManager>().tileBelow.GetComponent<Tile>().y);
                }
            }
                  else
                  {
                      if (closestUnit!= null)
                    {
                        target = closestUnit;

                    }

                      if (target != null)
                      {
                         //left
                            if (charScript.Occupiedtile.x != 0 && !tiles[charScript.Occupiedtile.x - 1, charScript.Occupiedtile.y]
                                    .GetComponent<Tile>()._occupied)
                            {
                           
                                Ld = distance(charScript.Occupiedtile.x - 1,
                                    target.GetComponent<Character>().Occupiedtile.x, charScript.Occupiedtile.y,
                                    target.GetComponent<Character>().Occupiedtile.y);
                            }

                            //back
                            if (charScript.Occupiedtile.y != 0 && !tiles[charScript.Occupiedtile.x, charScript.Occupiedtile.y - 1]
                                    .GetComponent<Tile>()._occupied)
                            {
                                Bd = distance(charScript.Occupiedtile.x,
                                    target.GetComponent<Character>().Occupiedtile.x, charScript.Occupiedtile.y - 1,
                                    target.GetComponent<Character>().Occupiedtile.y);
                            }

                            //right
                            if (charScript.Occupiedtile.x != 19 && !tiles[charScript.Occupiedtile.x + 1, charScript.Occupiedtile.y]
                                    .GetComponent<Tile>()._occupied)
                            {
                                Rd = distance(charScript.Occupiedtile.x + 1,
                                    target.GetComponent<Character>().Occupiedtile.x, charScript.Occupiedtile.y,
                                    target.GetComponent<Character>().Occupiedtile.y);
                            }

                            //forwards
                            if (charScript.Occupiedtile.y != 19 && !tiles[charScript.Occupiedtile.x, charScript.Occupiedtile.y + 1]
                                    .GetComponent<Tile>()._occupied)
                            {
                                Fd = distance(charScript.Occupiedtile.x,
                                    target.GetComponent<Character>().Occupiedtile.x, charScript.Occupiedtile.y + 1,
                                    target.GetComponent<Character>().Occupiedtile.y);
                            }
                      }
                    
     
            }
            
          
     
            
            
          //Determines between forward and back vs left or right
            float ylowest, xlowest;
            if (Fd < Bd)
            {
                optimalMoveY = move.forward;
                ylowest = Fd;
            }
            else
            {

                optimalMoveY = move.back;
                ylowest = Bd;


            }

            if (Rd < Ld)
            {
                optimalMoveX = move.right;
                xlowest = Rd;

            }
            else
            {
                optimalMoveX = move.left;
                xlowest = Ld;
            }

            if (xlowest < ylowest)
            {
                currentMove = optimalMoveX;
            }
            else
            {
                currentMove = optimalMoveY;

            }
    }

    //Calculates utility of which unit is needed at the time, currently only selects between archers and army
    public float whatToSummon()
    {
        Debug.Log(tiles[_BoardState.AIManager.tileBelow.GetComponent<Tile>().x, _BoardState.AIManager.tileBelow.GetComponent<Tile>().y].name);
        if (!tiles[_BoardState.AIManager.tileBelow.GetComponent<Tile>().x+1,_BoardState.AIManager.tileBelow.GetComponent<Tile>().y].GetComponent<Tile>()._occupied ||!tiles[_BoardState.AIManager.tileBelow.GetComponent<Tile>().x-1,_BoardState.AIManager.tileBelow.GetComponent<Tile>().y]||!tiles[_BoardState.AIManager.tileBelow.GetComponent<Tile>().x,_BoardState.AIManager.tileBelow.GetComponent<Tile>().y + 1].GetComponent<Tile>()._occupied ||!tiles[_BoardState.AIManager.tileBelow.GetComponent<Tile>().x,_BoardState.AIManager.tileBelow.GetComponent<Tile>().y -1].GetComponent<Tile>()._occupied)
        {
            
       
            float final, ArcherEQ, ArmyEQ;
            float maxarmy=2,maxArchers=1;

            for (int i = 0; i < Munits.Count; i++)
            {
                if (Munits[i].GetComponent<Character>().characterScript.CharacterType == CharacterScriptable.characterType.Ranged)
                {
                    Archers++;
                }
                else
                {
                    army++;
                }
            }
            
            //determins how many units of each type there are
            for (int i = 0; i < Munits.Count; i++)
            {
                if (Munits[i].GetComponent<Character>().characterScript.CharacterType == CharacterScriptable.characterType.Ranged)
                {
                    Archers++;
                }
                else
                {
                    army++;
                }
            }
            ArcherEQ = ((maxArchers - Archers) / maxArchers + (Mbase.GetComponent<CityManager>().TGold / 100)) / 2;
            ArmyEQ = ((maxarmy - army) / maxarmy + (Mbase.GetComponent<CityManager>().TGold / 100)) / 2;
            if (ArcherEQ > ArmyEQ)
            {
                final = ArcherEQ;
                WhatToSummon = Summonable.ranged;
            }
            else
            {
                final = ArmyEQ;
                WhatToSummon = Summonable.Army;
            }
            return final;

        }

        return 0;
    }

    //Calculates the utility of each piece that can attack, if distance is < 1 the utility is set to 0
    public float UAttack(GameObject unit)
    {
        Character charScript = unit.GetComponent<Character>();

        float final =0, distC,distE;
        float eminDist = 1000, edistance;

        if (unit.GetComponent<Character>().characterScript.CharacterType != CharacterScriptable.characterType.Miner)
        {
            distC = distance(charScript.Occupiedtile.x, HbaseTile.x, charScript.Occupiedtile.y, HbaseTile.y);
            for (int i = 0; i < Hunits.Count; i++)
            {
                distE = distance(charScript.Occupiedtile.x, Hunits[i].GetComponent<Character>().Occupiedtile.x, charScript.Occupiedtile.y, Hunits[i].GetComponent<Character>().Occupiedtile.y);
                if (eminDist > distE)
                {
                    eminDist = distE;
                    Gamemanager.Instance.selectedEnemy = Hunits[i];
                }
            }
            if (distC < eminDist)
            {
                final = 1/distC ;
               

            }
            else
            {
                final = 1 / eminDist;
            }

            if (final != 1)
            {
                final = 0;
            }

        }
        else
        {
            final = 0;
        }
     
        return final;
    }

    //calculates if city health is low and heals if needed
    public float Udefend(GameObject unit)
    {
        float final =(20- Mbase.GetComponent<CityManager>().CityHealth)/20;
 
        
           
        
        return final;
    }
    
    //Tells the apropriate character script to move and where to move
    void Move(GameObject unit)
    {
        whereToMove(unit);
        
        switch (currentMove)
        {
            case move.forward:
                _BoardState.UpdateBoardMove(unit ,tiles[unit.GetComponent<Character>().Occupiedtile.x,
                    unit.GetComponent<Character>().Occupiedtile.y + 1]);
              /*  unit.GetComponent<Character>().SelectedTile = tiles[unit.GetComponent<Character>().Occupiedtile.x,
                    unit.GetComponent<Character>().Occupiedtile.y + 1].GetComponent<Tile>();*/
               
                break;
            case move.back:
                _BoardState.UpdateBoardMove(unit ,tiles[unit.GetComponent<Character>().Occupiedtile.x,
                    unit.GetComponent<Character>().Occupiedtile.y - 1]);
                /*unit.GetComponent<Character>().SelectedTile = tiles[unit.GetComponent<Character>().Occupiedtile.x,
                    unit.GetComponent<Character>().Occupiedtile.y  -1].GetComponent<Tile>();*/
                break;
            case move.left:
                _BoardState.UpdateBoardMove(unit ,tiles[unit.GetComponent<Character>().Occupiedtile.x-1,
                    unit.GetComponent<Character>().Occupiedtile.y ]);
                /*unit.GetComponent<Character>().SelectedTile = tiles[unit.GetComponent<Character>().Occupiedtile.x-1,
                    unit.GetComponent<Character>().Occupiedtile.y ].GetComponent<Tile>();*/
                break;
            case move.right:
                _BoardState.UpdateBoardMove(unit ,tiles[unit.GetComponent<Character>().Occupiedtile.x+1,
                    unit.GetComponent<Character>().Occupiedtile.y ]);
               /* unit.GetComponent<Character>().SelectedTile = tiles[unit.GetComponent<Character>().Occupiedtile.x+1,
                    unit.GetComponent<Character>().Occupiedtile.y ].GetComponent<Tile>();*/
                break;
            case move.noMove:
                Debug.Log("AINoMove");
                break;
            
        }
        //unit.GetComponent<Character>().Move();
        
    }

    //Tells the apropriate character script to attack and which character to attack
    void Attack(GameObject unit)
    {
        
       /* float distC = distance(unit.GetComponent<Character>().Occupiedtile.x, HbaseTile.x, unit.GetComponent<Character>().Occupiedtile.y, HbaseTile.y);
        if (distC ==1)
        {
          //  Hbase.GetComponent<CityManager>().takeDamage(unit.GetComponent<Character>().Damage);
            
        }
        else
        {
            unit.GetComponent<Character>().Attack();   
            
        }*/
        _BoardState.UpdateBoardAttack(unit, _BoardState.playerManager.gameObject);  
    }

    //Tells the city manager to heal
    void Defend()
    {
       _BoardState.UpdateBoardHeal(turnPlayer);
        //onAIReinforcedPressed?.Invoke();
    }

    //Tells the appropriate city manager what to summon.
    void Summon()
    {

        switch (WhatToSummon)
        {
            case Summonable.Army:
                //Army
              //  SpawnAIA?.Invoke();
                if (turnPlayer == TurnManager.TurnOrder.AI)
                {
                    _BoardState.UpdateBoardSummon(_BoardState.AIManager.meele, TurnManager.TurnOrder.AI);
                    
                }
                if (turnPlayer == TurnManager.TurnOrder.Player1)
                {
                    _BoardState.UpdateBoardSummon(_BoardState.AIManager.meele, TurnManager.TurnOrder.Player1);
                }
                break;
            case Summonable.ranged:
                //ranger
                if (turnPlayer == TurnManager.TurnOrder.AI)
                {
                    _BoardState.UpdateBoardSummon(_BoardState.AIManager.ranger, TurnManager.TurnOrder.AI);
                }
                if (turnPlayer == TurnManager.TurnOrder.Player1)
                {
                    _BoardState.UpdateBoardSummon(_BoardState.AIManager.ranger, TurnManager.TurnOrder.Player1);
                }
               /* SpawnAIR?.Invoke();*/
                break;
            /*case Summonable.Miner:
                //miner
                SpawnAIM?.Invoke();
                break;*/
        }
       
        
        

    }
    
    
    //Just a distance check
    public float distance(int x1, int x2, int y1, int y2)
    {
       float distance = (float)Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)); 
        return distance;
    }
    
}



