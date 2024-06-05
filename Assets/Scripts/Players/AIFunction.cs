using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AIFunction : Singleton<AIFunction>
{
    private GameObject[] presortedTiles;
    public List<GameObject> Munits, Hunits;
    private GameObject[,] tiles = new GameObject[20,20];
    private GameObject Hbase, Mbase, ChosenUnit;
    private Tile HbaseTile, MbaseTile;
    private Dictionary<int, LavaLocations> _lavaLocationsMap = new Dictionary<int, LavaLocations>();
    private Dictionary<int, GoldLocations> _goldLocationsMap = new Dictionary<int, GoldLocations>();

    public  event Action onAIReinforcedPressed;
    public  event Action SpawnAIA;
    public  event Action SpawnAIM;
    public  event Action SpawnAIR;
    


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

    private Difficulty currentD = Difficulty.Easy;
    private move currentMove = move.noMove;
    
    private Summonable WhatToSummon;

    private void OnEnable()
    {
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
                    _lavaLocationsMap.Add(lavas,new LavaLocations(i,j));
                    _goldLocationsMap.Add(lavas,new GoldLocations(i,j));
                    lavas++;
                    golds++;
                }
            
            }

            AIUtilityFunction();
        }
       
    }

    public void AIUtilityFunction()
    {
        //assigns bases
        if (Hbase == null)
        {
            Hbase = GameObject.FindGameObjectWithTag("HumanB");
            HbaseTile = Hbase.GetComponent<CityManager>().tileBelow.GetComponent<Tile>();
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

        }


        //assigns most resent units to list
        Munits = GameObject.FindGameObjectsWithTag("Monster").ToList();
        Hunits = GameObject.FindGameObjectsWithTag("Human").ToList();





        float[] unitUtilitiy = new float[Munits.Count];
        GameObject MoveUnit,AttackUnit,summonUnit;
        float Mmax = -1000, Dmax = -1000, Amax = -1000, Smax = -1000, utility1, utility2;
        int position = 0;
        for (int i = 0; i < Munits.Count; i++)
        {
            //stores all unit utilities in an array to select the biggest and use that as the piece to move 
            unitUtilitiy[i] = piecetoMove(Munits[i]);
        }

        for (int i = 0; i < unitUtilitiy.Length; i++)
        {
            if (unitUtilitiy[i] > Mmax)
            {
                Mmax = unitUtilitiy[i];
                position = i;
            }
        }
        //chosen moving unit
        MoveUnit = Munits[position];
        
        
        
        for (int i = 0; i < Munits.Count; i++)
        {
            //stores all unit utilities in an array to select the biggest and use that as the piece to attack 
            unitUtilitiy[i] = UAttack(Munits[i]);
        }

        for (int i = 0; i < unitUtilitiy.Length; i++)
        {
            if (unitUtilitiy[i] > Amax)
            {
                Amax = unitUtilitiy[i];
                position = i;
            }
        }
        AttackUnit = Munits[position];
        Smax = whatToSummon();
        
        for (int i = 0; i < Hunits.Count; i++)
        {
            //stores all unit utilities in an array to select the biggest and use that as the piece to attack 
            unitUtilitiy[i] = Udefend(Hunits[i]);
        }

        for (int i = 0; i < unitUtilitiy.Length; i++)
        {
            if (unitUtilitiy[i] > Dmax)
            {
                Dmax = unitUtilitiy[i];
            }
        }
       
        
        
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

        if (utility1 > utility2)
        {
            currentAction = uAction1;

        }
        else
        {
            currentAction = uAction2;
        }

        switch (currentAction)
        {
            case action.move:
                Move(MoveUnit);
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
    public float piecetoMove(GameObject unit)
    {
        float final=0;
        Character charScript = unit.GetComponent<Character>();
        
        if (charScript.characterScript.CharacterType == CharacterScriptable.characterType.Miner && charScript.canMove)
        {
            float gmin = 1000, gdistance, lmin = 1000, ldistance;
            for (int i = 0; i < _goldLocationsMap.Count; i++)
            {
                gdistance = distance(charScript.Occupiedtile.x, _goldLocationsMap[i].x, charScript.Occupiedtile.y, _goldLocationsMap[i].y);
                if (gmin>gdistance)
                {
                    gmin = gdistance;
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

            final = Convert.ToInt32(charScript.canMove) * ((Hbase.GetComponent<CityManager>().TGold/Mbase.GetComponent<CityManager>().TGold) * (gmin +lmin) );
        }
        else if(charScript.canMove)
        {
            float emin = 1000, edistance, lmin = 1000, ldistance;

            for (int i = 0; i < _lavaLocationsMap.Count; i++)
            {
                ldistance = distance(unit.GetComponent<Character>().Occupiedtile.x, _lavaLocationsMap[i].x, unit.GetComponent<Character>().Occupiedtile.x, _lavaLocationsMap[i].y);
                if (lmin<ldistance)
                {
                    lmin = ldistance;
                }
            }
         
            for (int i = 0; i < Hunits.Count; i++)
            {
                edistance = distance(charScript.Occupiedtile.x, Hunits[i].GetComponent<Character>().Occupiedtile.x, charScript.Occupiedtile.x, Hunits[i].GetComponent<Character>().Occupiedtile.y);
                if (emin > edistance)
                {
                    emin = edistance;
                }
            } 
            final = Convert.ToInt32(charScript.canMove) * ((Hbase.GetComponent<CityManager>().TGold/Mbase.GetComponent<CityManager>().TGold) *  (emin +lmin) );
            
            
        }
        
 

        return final;
    }

    public void whereToMove(GameObject unit)
    {
        move optimalMoveY,optimalMoveX;
        float final = 0;
        GameObject target,closestUnit = Hunits[0];
        Character charScript = unit.GetComponent<Character>();
        float emin = 1000, edistance;
        float Ld=9999, Rd=9999, Fd=9999, Bd=9999;
        for (int i = 0; i < Hunits.Count; i++)
        {
            edistance = distance(charScript.Occupiedtile.x, Hunits[i].GetComponent<Character>().Occupiedtile.x, charScript.Occupiedtile.x, Hunits[i].GetComponent<Character>().Occupiedtile.y);
            if (emin > edistance)
            {
                emin = edistance;
                closestUnit = Hunits[i];
            }
        } 
        
        float cDistance =distance(charScript.Occupiedtile.x, HbaseTile.x, charScript.Occupiedtile.x, HbaseTile.y);
        
        //determins which target is closer and sets that as the target
        if (emin > cDistance)
        {
            target = Hbase;
        }
        else
        {
            target = closestUnit;
        }
        
        //left
        if (charScript.Occupiedtile.x != 0&& !tiles[charScript.Occupiedtile.x - 1, charScript.Occupiedtile.y].GetComponent<Tile>()._occupied)
        {
            Ld = distance(charScript.Occupiedtile.x - 1,
                target.GetComponent<CityManager>().tileBelow.GetComponent<Tile>().x, charScript.Occupiedtile.y,
                target.GetComponent<CityManager>().tileBelow.GetComponent<Tile>().y);
        }
        //back
        if (charScript.Occupiedtile.y != 0&& !tiles[charScript.Occupiedtile.x, charScript.Occupiedtile.y-1].GetComponent<Tile>()._occupied)
        {
            Bd = distance(charScript.Occupiedtile.x ,
                target.GetComponent<CityManager>().tileBelow.GetComponent<Tile>().x, charScript.Occupiedtile.y-1,
                target.GetComponent<CityManager>().tileBelow.GetComponent<Tile>().y);
        }
        //right
        if (charScript.Occupiedtile.x != 19 && !tiles[charScript.Occupiedtile.x + 1, charScript.Occupiedtile.y].GetComponent<Tile>()._occupied )
        {
            Rd = distance(charScript.Occupiedtile.x + 1,
                target.GetComponent<CityManager>().tileBelow.GetComponent<Tile>().x, charScript.Occupiedtile.y,
                target.GetComponent<CityManager>().tileBelow.GetComponent<Tile>().y);
        }
        //forwards
        if (charScript.Occupiedtile.y != 19 && !tiles[charScript.Occupiedtile.x, charScript.Occupiedtile.y+1].GetComponent<Tile>()._occupied )
        {
            Fd = distance(charScript.Occupiedtile.x,
                target.GetComponent<CityManager>().tileBelow.GetComponent<Tile>().x, charScript.Occupiedtile.y+1,
                target.GetComponent<CityManager>().tileBelow.GetComponent<Tile>().y);
        }

        float ylowest, xlowest;
        if (Fd<Bd)
        {
            optimalMoveY = move.forward;
            ylowest = Fd;
        }
        else
        {
            
            optimalMoveY = move.back;
            ylowest = Bd;


        }
        if (Rd<Ld)
        {
            optimalMoveX = move.right;
            xlowest = Rd;

        }
        else
        {
            optimalMoveX = move.left;
            xlowest = Ld;
        }

        if (xlowest<ylowest)
        {
            currentMove = optimalMoveX;
        }
        else
        {
            currentMove = optimalMoveY;

        }

    }

    public float whatToSummon()
    {
        float final, minerEQ, ArmyEQ;
        float miners=0, army =0,maxarmy=15,maxMiners=10;
        
        //determins how many units of each type there are
        for (int i = 0; i < Munits.Count; i++)
        {
            if (Munits[i].GetComponent<Character>().characterScript.CharacterType == CharacterScriptable.characterType.Miner)
            {
                miners++;
            }
            else
            {
                army++;
            }
        }
        minerEQ = ((maxMiners - miners) / maxMiners + (Mbase.GetComponent<CityManager>().TGold / 100)) / 2;
        ArmyEQ = ((maxarmy - army) / maxarmy + (Mbase.GetComponent<CityManager>().TGold / 100)) / 2;
        if (minerEQ > ArmyEQ)
        {
            final = minerEQ;
            WhatToSummon = Summonable.Miner;
        }
        else
        {
            final = ArmyEQ;
            WhatToSummon = Summonable.Army;
        }

        return final;
    }

    public float UAttack(GameObject unit)
    {
        Character charScript = unit.GetComponent<Character>();

        float final, distC,distE;
        distC = distance(charScript.Occupiedtile.x, HbaseTile.x, charScript.Occupiedtile.y, HbaseTile.y);
        float eminDist = 1000, edistance;
        for (int i = 0; i < Hunits.Count; i++)
        {
            distE = distance(charScript.Occupiedtile.x, Hunits[i].GetComponent<Character>().Occupiedtile.x, charScript.Occupiedtile.y, Hunits[i].GetComponent<Character>().Occupiedtile.y);
            if (eminDist > distE)
            {
                eminDist = distE;
                Gamemanager.Instance.selectedEnemy = Hunits[i];
            }
        }
        if (distC > eminDist)
        {
            final = 1/distC ;
        }
        else
        {
            final = 1 / eminDist;
        }
        return final;
    }

    public float Udefend(GameObject unit)
    {
        
            float final = (distance(MbaseTile.x, unit.GetComponent<Character>().Occupiedtile.x, MbaseTile.y,
                unit.GetComponent<Character>().Occupiedtile.y) - 40) / 4;

        
        return final;
    }
    
    void Move(GameObject unit)
    {
        whereToMove(unit);
     
        
        switch (currentMove)
        {
            case move.forward:
                unit.GetComponent<Character>().SelectedTile = tiles[unit.GetComponent<Character>().Occupiedtile.x,
                    unit.GetComponent<Character>().Occupiedtile.y + 1].GetComponent<Tile>();
                break;
            case move.back:
                unit.GetComponent<Character>().SelectedTile = tiles[unit.GetComponent<Character>().Occupiedtile.x,
                    unit.GetComponent<Character>().Occupiedtile.y  -1].GetComponent<Tile>();
                break;
            case move.left:
               
                unit.GetComponent<Character>().SelectedTile = tiles[unit.GetComponent<Character>().Occupiedtile.x-1,
                    unit.GetComponent<Character>().Occupiedtile.y ].GetComponent<Tile>();
                break;
            case move.right:
                unit.GetComponent<Character>().SelectedTile = tiles[unit.GetComponent<Character>().Occupiedtile.x+1,
                    unit.GetComponent<Character>().Occupiedtile.y ].GetComponent<Tile>();
                break;
            case move.noMove:
                Debug.Log("AINoMove");
                break;
            
        }
        unit.GetComponent<Character>().Move();
    }

    void Attack(GameObject unit)
    {
        unit.GetComponent<Character>().Attack();        
    }

    void Defend()
    {
        onAIReinforcedPressed?.Invoke();
    }

    void Summon()
    {

        switch (WhatToSummon)
        {
            case Summonable.Army:
                //Army
                SpawnAIA?.Invoke();
                break;
            case Summonable.ranged:
                //ranger
                SpawnAIR?.Invoke();
                break;
            case Summonable.Miner:
                //miner
                SpawnAIM?.Invoke();
                break;
        }
       
        
        

    }
    
    public float distance(int x1, int x2, int y1, int y2)
    {
       float distance = (float)Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)); 
        return distance;
    }
    
}

class LavaLocations
{
    public int x;
    public int y;

   public LavaLocations(int X, int Y)
    {
        x = X;
        y = Y;
    }
}
class GoldLocations
{
    public int x;
    public int y;

    public GoldLocations(int X, int Y)
    {
        x = X;
        y = Y;
    }
}
