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
    private Dictionary<int, LavaLocations> _lavaLocationsMap = new Dictionary<int, LavaLocations>();
    private Dictionary<int, GoldLocations> _goldLocationsMap = new Dictionary<int, GoldLocations>();
    private enum Summonable
    {
        Army,
        Miner
    }
    
    private Summonable WhatToSummon;

    private void OnEnable()
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

    }

    public void AIUtilityFunction()
    {
        if (Hbase == null)
        {
            Hbase = GameObject.FindGameObjectWithTag("HumanB");
        }
        if (Mbase == null)
        {
            Mbase = GameObject.FindGameObjectWithTag("MonsterB");
        }

      
        
        Munits = GameObject.FindGameObjectsWithTag("Monster").ToList();
        Hunits = GameObject.FindGameObjectsWithTag("Human").ToList();
        
        float[] unitUtilitiy = new float[Munits.Count];
        float  max = -1000, position;
        for (int i = 0; i < Munits.Count; i++)
        {
            //stores all untilites in an array to select the biggest and use that as the movement
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
        
        if (charScript.characterScript.CharacterType == CharacterScriptable.characterType.Miner)
        {
            float gmin = 1000, gdistance, lmin = 1000, ldistance;
            for (int i = 0; i < _goldLocationsMap.Count; i++)
            {
                gdistance = distance(unit.GetComponent<Character>().Occupiedtile.x, _goldLocationsMap[i].x, unit.GetComponent<Character>().Occupiedtile.y, _goldLocationsMap[i].y);
                if (gmin>gdistance)
                {
                    gmin = gdistance;
                }
            }
            for (int i = 0; i < _lavaLocationsMap.Count; i++)
            {
                ldistance = distance(unit.GetComponent<Character>().Occupiedtile.x, _lavaLocationsMap[i].x, unit.GetComponent<Character>().Occupiedtile.y, _lavaLocationsMap[i].y);
                if (lmin>ldistance)
                {
                    lmin = ldistance;
                }
            }

            final = Convert.ToInt32(charScript.canMove) * ((Hbase.GetComponent<CityManager>().TGold/Mbase.GetComponent<CityManager>().TGold) * (gmin +lmin) );
        }
        else
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
                edistance = distance(unit.GetComponent<Character>().Occupiedtile.x, Hunits[i].GetComponent<Character>().Occupiedtile.x, unit.GetComponent<Character>().Occupiedtile.x, Hunits[i].GetComponent<Character>().Occupiedtile.y);
                if (emin > edistance)
                {
                    emin = edistance;
                }
            } 
            final = Convert.ToInt32(charScript.canMove) * ((Hbase.GetComponent<CityManager>().TGold/Mbase.GetComponent<CityManager>().TGold) *  (emin +lmin) );
            
            
        }
        
 

        return final;
    }

    public float whereToMove()
    {
        /*
            Utility for where for an army to move:  (distance to enemy city - distance to lava) OR (distance to enemy piece - distance to lava)
            Utility for where a miner should move = distance to gold - Distance to lava
         */
        return 0;
    }

    public float whatToSummon()
    {
        /*
         * Summon Army: (Bool space is empty) (distance to enemy piece - distance to lava) / 100 OR (if no enemy pieces spawned (DEC - DL) / 100
            Summon Miner: (distance to gold - Distance to lava) / 100
         */
        return 0;
    }

    public float Attack()
    {
        /*
         * If distance to city greater than attack range : Utility for where to attack : (Target health after attack) / 100
            If distance to city less than attack range: (Target health after attack) / 100
         */
        return 0;
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
