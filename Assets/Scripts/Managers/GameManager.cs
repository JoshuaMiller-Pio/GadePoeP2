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
        Debug.Log("canPlace");
    }

    private void TownPlaced(Vector3 SBLocation)
    {
        Debug.Log("inside");
        if (_townsPlaced <= 2)
        {
            Debug.Log("spawn");

            canPlaceTown = false;
            Vector3 castleLocation = new Vector3(SBLocation.x, SBLocation.y + 9.59f, SBLocation.z);
            Debug.Log(SBLocation.y);
            Instantiate(_towns[_townsPlaced], castleLocation, Quaternion.identity);
        }
        _townsPlaced++;

    }

}
