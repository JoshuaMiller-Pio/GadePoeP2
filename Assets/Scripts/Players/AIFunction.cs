using System;
using Unity.VisualScripting;
using UnityEngine;

public class AIFunction : Singleton<AIFunction>
{
    private GameObject[] Munits, Hunits, presortedTiles;
    private GameObject[,] tiles = new GameObject[20,20];
    private GameObject Hbase, Mbase;
    private enum Summonable
    {
        Army,
        Miner
    }
    
    private Summonable WhatToSummon;

    private void OnEnable()
    {
        presortedTiles = GameObject.FindGameObjectsWithTag("Tile");
        int sentinal = 0;
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                tiles[i,j] = presortedTiles[sentinal];
                sentinal++;
            }
            
        }

    }

    public  void AIUtilityFunction()
    {
        if (Hbase == null)
        {
            Hbase = GameObject.FindGameObjectWithTag("HumanB");
        }
        if (Mbase == null)
        {
            Mbase = GameObject.FindGameObjectWithTag("MonsterB");
        }

      
        
        Munits = GameObject.FindGameObjectsWithTag("Monster");
        Hunits = GameObject.FindGameObjectsWithTag("Human");
        
        float[] unitUtilitiy = new float[Munits.Length];
        float  max = -1000, position;
        for (int i = 0; i < Munits.Length; i++)
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
        if (charScript.characterScript.CharacterType == CharacterScriptable.characterType.Melee &&charScript.characterScript.CharacterType == CharacterScriptable.characterType.Ranged )
        {
            final = Convert.ToInt32(charScript.canMove) * ((Hbase.GetComponent<CityManager>().TGold/Mbase.GetComponent<CityManager>().TGold) /* distance to gold + distance to lava */);
        }
        else
        {
            final = Convert.ToInt32(charScript.canMove) * ((Hbase.GetComponent<CityManager>().TGold/Mbase.GetComponent<CityManager>().TGold) /* distance to enemy + distance to lava */);
            
        }
        
 

        return final;
    }

    public float whereToMove()
    {
        /*
            Utility for where for an army to move:  (DEC - DL) OR (DEP - DL)
            Utility for where a miner should move = DG - DL
         */
        return 0;
    }

    public float whatToSummon()
    {
        /*
         * Summon Army: (Bool space is empty) (DEP - DL) / 100 OR (if no enemy pieces spawned (DEC - DL) / 100
            Summon Miner: (DG - DL) / 100
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
    
    
}
