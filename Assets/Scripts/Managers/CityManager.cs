using System.Collections;
using System;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    private float _tGold = 10, _cityHealth = 100, _bPop = 1, _aPop =0, _gpt = 1;
    public static event Func<Vector3, GameObject> CityManagerTile; 

    public GameObject tileBelow, meele, ranger, worker;
    // Start is called before the first frame update
    void Start()
    {
    }
    
    public float TGold => _tGold;
    public float CityHealth => _cityHealth;
    public float BPop => _bPop;
    public float APop => _aPop;
    public float Gpt => _gpt;

    
    private void spawnUnit()
    {
        _aPop++;
        Tile tile;
        
        Vector3 up = new Vector3(tileBelow.transform.localPosition.x, tileBelow.transform.localPosition.y, tileBelow.transform.localPosition.z);
        Vector3 down = new Vector3(tileBelow.transform.localPosition.x, tileBelow.transform.localPosition.y, tileBelow.transform.localPosition.z);
        Vector3 left = new Vector3(tileBelow.transform.localPosition.x, tileBelow.transform.localPosition.y, tileBelow.transform.localPosition.z);
        Vector3 right = new Vector3(tileBelow.transform.localPosition.x, tileBelow.transform.localPosition.y, tileBelow.transform.localPosition.z);
        
        GameObject TileCheck = CityManagerTile?.Invoke(up);
        tile = TileCheck.GetComponent<Tile>();

        if (!tile._occupied)
        {
            tile._occupied = true;
            
            //TODO change vector 3
            Instantiate(meele, up, Quaternion.identity);
        }

        //access grid manager to find tile
        //check if tile is free
        //spawn game object

    }
    
    private void spawnWorker()
    {
        _bPop++;
        //access grid manager to find tile
        // check if tile is free
        //spawn game object
    }
    
    private void increaseGold()
    {
        _tGold += _gpt;
    }

    private void takeDamage(float damage)
    {
        _cityHealth -= damage;
    }
   
}
