using System.Collections;
using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CityManager : MonoBehaviour
{
    private float _tGold = 10, _cityHealth = 100, _bPop = 1, _aPop =0, _gpt = 1;
    public static event Func<Vector3, GameObject> CityManagerTile; 
    private Renderer _renderer;
    private bool player1turn = true;
    public GameObject tileBelow, meele, ranger, worker;
    public static event Action<CityManager> cityInfoUI;
    public static event Action<CityManager> cityActionUI;
    public static event Action<String> gameOver;
    
  
    void Start()
    {
        
        Character.Incrasegold += increaseGold;
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
        if (EventSystem.current.IsPointerOverGameObject()) return;
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
        if ((_tGold - 5) <0 ) 
        {
            Debug.Log("noGold");
        }
        if ( (_tGold - 5) >=0 )
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
        gameOver?.Invoke(gameObject.tag);
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

   
    private void increaseGold(float mineGold)
    {
        Debug.Log("IncreasedGold");
        _tGold += _gpt;
        _tGold += mineGold;
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
