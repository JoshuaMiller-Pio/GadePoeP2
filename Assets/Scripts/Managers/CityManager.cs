using System.Collections;
using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class CityManager : MonoBehaviour
{
    private float _tGold = 10, _cityHealth = 100, _bPop = 1, _aPop =0, _gpt = 1;
    public static event Func<Vector3, GameObject> CityManagerTile; 
    private Renderer _renderer;
    private bool player1turn = true;
    public GameObject tileBelow, meele, ranger, worker;
    // Start is called before the first frame update
  
    void Start()
    {
        TurnManager.RoundEnd += switchPlayer;
        if (gameObject.tag == "Human")
        {
            ButtonManager.SpawnHA += spawnRanger;
            ButtonManager.SpawnHM += spawnUnit ;
            ButtonManager.SpawnHMi += spawnWorker ;
            
        }
        else 
        {
            ButtonManager.SpawnMA += spawnRanger;
            ButtonManager.SpawnMM += spawnUnit;
            ButtonManager.SpawnMMi += spawnWorker;
        }
        
       
    }
    
    
    public static event Action<CityManager> cityInfoUI;
    public static event Action<CityManager> cityActionUI;
    public static event Action<bool> gameOver;
    private void OnMouseEnter()
    {
     //   Color highlightcolor = Color.cyan;
     //   _renderer.material.color =   highlightcolor;
        cityInfoUI?.Invoke(this);
    }
    
    //removes highlight
    private void OnMouseExit()
    {
        
       //_renderer.material.color = tileInfo.Color;
    }
    private void OnMouseDown()
    {
        cityActionUI?.Invoke(this);
    }
    
    
    public float TGold => _tGold;
    public float CityHealth => _cityHealth;
    public float BPop => _bPop;
    public float APop => _aPop;
    public float Gpt => _gpt;

    
    public void spawnUnit()
    {
        GameObject TargetPosition = tileBelow.GetComponent<Tile>().getSurroundingBlocks();
        Vector3 spawnPosition = new Vector3(TargetPosition.transform.position.x, TargetPosition.transform.position.y+10.1f,TargetPosition.transform.position.z);
        if (TargetPosition != null && (_tGold - 5) >=0 )
        {
            _aPop++;
            _tGold -= 5;
            Tile gameobjectTile = TargetPosition.GetComponent<Tile>();

            gameobjectTile._occupied = true;
            Instantiate(meele, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.Log("NoSpace");
        }

    }
    public void spawnRanger()
    {
        GameObject TargetPosition = tileBelow.GetComponent<Tile>().getSurroundingBlocks();
        Vector3 spawnPosition = new Vector3(TargetPosition.transform.position.x, TargetPosition.transform.position.y+10.1f,TargetPosition.transform.position.z);
        if (TargetPosition != null&& (_tGold - 5) >=0)
        {
            Tile gameobjectTile = TargetPosition.GetComponent<Tile>();

            _aPop++;
            _tGold -= 5;
            gameobjectTile._occupied = true;
            Debug.Log(_tGold);
            Instantiate(ranger, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.Log("NoSpace");
        }

    }

    public void Death()
    {
        gameOver?.Invoke(player1turn);
    }
    public void spawnWorker()
    {
        GameObject TargetPosition = tileBelow.GetComponent<Tile>().getSurroundingBlocks();
        Vector3 spawnPosition = new Vector3(TargetPosition.transform.position.x, TargetPosition.transform.position.y+10.1f,TargetPosition.transform.position.z);
        if (TargetPosition != null && (_tGold - 5) >=0)
        {
            Tile gameobjectTile = TargetPosition.GetComponent<Tile>();

            _bPop++;
            _tGold -= 5;
            gameobjectTile._occupied = true;
            Instantiate(worker, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.Log("NoSpace");
        }

    }
    void switchPlayer()
    {
        if (player1turn)
        {
            player1turn = false;
        }
        else
        {
            player1turn = true;
        }
    }
  
    private void increaseGold()
    {
        _tGold += _gpt;
    }

    private void takeDamage(float damage)
    {
        _cityHealth -= damage;
        if (_cityHealth <= 0)
        {
            Death();
        }
    }
   
}
