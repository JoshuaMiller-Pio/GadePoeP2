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
    private GameObject Hbase, Mbase;
    private Tile HbaseTile, MbaseTile;
    private Dictionary<int, LavaLocations> _lavaLocationsMap = new Dictionary<int, LavaLocations>();
    private Dictionary<int, GoldLocations> _goldLocationsMap = new Dictionary<int, GoldLocations>();
    private enum Summonable
    {
        Army,
        Miner
    }
    public enum move
    { 
        //TODO reinitilize after moving with no move
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
                    Gamemanager.Instance.placeAITown(tiles[16,8]);
                    break;
                case Difficulty.Medium:
                    Gamemanager.Instance.placeAITown(tiles[9,6]);
                    break;
                case Difficulty.Hard:
                    Gamemanager.Instance.placeAITown(tiles[9,16]);
                    break;
            }
            Mbase = GameObject.FindGameObjectWithTag("MonsterB");
            MbaseTile = Mbase.GetComponent<CityManager>().tileBelow.GetComponent<Tile>();
            
        }

      
        //assigns most resent units to list
        Munits = GameObject.FindGameObjectsWithTag("Monster").ToList();
        Hunits = GameObject.FindGameObjectsWithTag("Human").ToList();
        
        //TODO look over this and see if can be simplified and corrected
        float[] unitUtilitiy = new float[Munits.Count];
        float  max = -1000, position;
        for (int i = 0; i < Munits.Count; i++)
        {
            //stores all unit utilities in an array to select the biggest and use that as the piece to move 
            
            unitUtilitiy[i] = piecetoMove(Munits[i]);
        }

        for (int i = 0; i < unitUtilitiy.Length; i++)
        {
            if (unitUtilitiy[i] > max)
            {
                max = unitUtilitiy[i];
                position = i;
            }
            
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

    public move whereToMove(GameObject unit)
    {

       
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
        
        //TODO simulate the move and if distance is less then set that as the move
        if (charScript.Occupiedtile.x != 0)
        {
            Ld = distance(charScript.Occupiedtile.x - 1,
                target.GetComponent<CityManager>().tileBelow.GetComponent<Tile>().x, charScript.Occupiedtile.y,
                target.GetComponent<CityManager>().tileBelow.GetComponent<Tile>().y);
        }
        if (charScript.Occupiedtile.y != 0)
        {
            Bd = distance(charScript.Occupiedtile.x - 1,
                target.GetComponent<CityManager>().tileBelow.GetComponent<Tile>().x, charScript.Occupiedtile.y,
                target.GetComponent<CityManager>().tileBelow.GetComponent<Tile>().y);
        }
        if (charScript.Occupiedtile.x != 19)
        {
            Rd = distance(charScript.Occupiedtile.x + 1,
                target.GetComponent<CityManager>().tileBelow.GetComponent<Tile>().x, charScript.Occupiedtile.y,
                target.GetComponent<CityManager>().tileBelow.GetComponent<Tile>().y);
        }
        if (charScript.Occupiedtile.y != 19)
        {
            Fd = distance(charScript.Occupiedtile.x + 1,
                target.GetComponent<CityManager>().tileBelow.GetComponent<Tile>().x, charScript.Occupiedtile.y,
                target.GetComponent<CityManager>().tileBelow.GetComponent<Tile>().y);
        }

        float ylowest, xlowest;
        move optimalMoveY,optimalMoveX;
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
        
        
       
       
        return currentMove;
    }

    public float whatToSummon(GameObject unit)
    {
        float final;
        float miners=0, army =0,maxarmy=15,maxMiners=10;
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

        if (unit.GetComponent<Character>().characterScript.CharacterType == CharacterScriptable.characterType.Miner)
        {
            final = ((maxMiners - miners) / maxMiners + (Mbase.GetComponent<CityManager>().TGold / 100)) / 2;
        }
        else
        {
            final = ((maxarmy - army) / maxarmy + (Mbase.GetComponent<CityManager>().TGold / 100)) / 2;
        }

        return final;
    }

    public float Attack(GameObject unit)
    {
        Character charScript = unit.GetComponent<Character>();

        float final, distC,distE;
        distC = distance(charScript.Occupiedtile.x, HbaseTile.x, charScript.Occupiedtile.y, HbaseTile.y);
        float eminDist = 1000, edistance;
        for (int i = 0; i < Hunits.Count; i++)
        {
            distE = distance(charScript.Occupiedtile.x, HbaseTile.x, charScript.Occupiedtile.y, HbaseTile.y);
            if (eminDist > distE)
            {
                eminDist = distE;
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
