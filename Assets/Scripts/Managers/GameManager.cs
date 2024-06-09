using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : Singleton<Gamemanager>
{
   
    public bool canPlaceTown,AIPlayer=false;
    private GameObject[] _towns;
    private int _townsPlaced = 0;
    
    public GameObject selectedunit, selectedEnemy ;
    public GameObject Mcitytile, Pcitytile;
    public GameObject Mcity, Pcity;

    public float _maxAP = 5, _currentAP;
    public float currentAP => _currentAP;
    public float maxAP => _maxAP;

    // Start is called before the first frame update
    void Start()
    {
        
        Application.targetFrameRate = 60;
        _currentAP = maxAP;
    }

    public void subscribe()
    {
        TurnManager.RoundEnd += ResetAP;
        GridManager.MapCreated += AllowPlacement;
        Tile.placeTown += TownPlaced;
    }
    private void AllowPlacement(GameObject[] towns)
    {
        _towns = towns;
        canPlaceTown = true;
    }

    private void TownPlaced(GameObject SBLocation)
    {
        if (_townsPlaced <  2 && !AIPlayer)
        {
            Vector3 castleLocation = new Vector3(SBLocation.transform.position.x, SBLocation.transform.position.y + 9.59f, SBLocation.transform.position.z);
           GameObject town = Instantiate(_towns[_townsPlaced], castleLocation, Quaternion.identity);
           town.GetComponent<CityManager>().tileBelow = SBLocation;
           SBLocation.GetComponent<Tile>()._occupied = true;
           if (_townsPlaced == 0)
           {
               Pcitytile = SBLocation;
               Pcity = town;
           }
           else
           {
               Mcitytile = SBLocation;
               Mcity = town;
           }
        }
        else if (_townsPlaced <1 && AIPlayer)
        {
            Vector3 castleLocation = new Vector3(SBLocation.transform.position.x, SBLocation.transform.position.y + 9.59f, SBLocation.transform.position.z);
            GameObject town = Instantiate(_towns[_townsPlaced], castleLocation, Quaternion.identity);
            town.GetComponent<CityManager>().tileBelow = SBLocation;
            SBLocation.GetComponent<Tile>()._occupied = true;
            if (_townsPlaced == 0)
            {
                Pcitytile = SBLocation;
                Pcity = town;
            }

            AIFunction ai = gameObject.GetComponent<AIFunction>();
            ai.enabled =true;
            NewAIFunction newAI = gameObject.GetComponent<NewAIFunction>();
            newAI.enabled = true;
        }
        else
        {
            canPlaceTown = false;
            Tile.placeTown -= TownPlaced;

        }
        _townsPlaced++;
    }

    public void placeAITown(GameObject SBLocation)
    {
        Vector3 castleLocation = new Vector3(SBLocation.transform.position.x, SBLocation.transform.position.y + 9.59f, SBLocation.transform.position.z);
        GameObject town = Instantiate(_towns[1], castleLocation, Quaternion.identity);
        town.GetComponent<CityManager>().tileBelow = SBLocation;
        SBLocation.GetComponent<Tile>()._occupied = true;
        if (_townsPlaced == 0)
        {
            Mcitytile = SBLocation;
            Mcity = town;
        }
    }
    public void ResetAP()
    {
        _currentAP = _maxAP;
    }
    public void DecreaseAP()
    {
        _currentAP = _currentAP - 1;
    }

    public void AIPlaying()
    {
        AIPlayer = true;
       
    }
    private void Update()
    {
    }
}
