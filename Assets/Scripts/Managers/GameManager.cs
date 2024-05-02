using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : Singleton<Gamemanager>
{
    public TurnManager _turnManager;
    public HumanPlayer _HumanPlayer;
    public AIPlayer _AIPlayer;
    public bool canPlaceTown;
    private GameObject[] _towns;
    private int _townsPlaced = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
        Application.targetFrameRate = 60;
        _turnManager = GameObject.FindGameObjectWithTag("TurnManager").GetComponent<TurnManager>();
    }

    public void subscribe()
    {
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
        if (_townsPlaced <  2)
        {
            Vector3 castleLocation = new Vector3(SBLocation.transform.position.x, SBLocation.transform.position.y + 9.59f, SBLocation.transform.position.z);
           GameObject town = Instantiate(_towns[_townsPlaced], castleLocation, Quaternion.identity);
           town.GetComponent<CityManager>().tileBelow = SBLocation;
           SBLocation.GetComponent<Tile>()._occupied = true;
        }
        else
        {
            canPlaceTown = false;
            Tile.placeTown -= TownPlaced;

        }
        _townsPlaced++;
    }

    private void Update()
    {
    }
}
