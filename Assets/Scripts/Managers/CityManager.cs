using System.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CityManager : MonoBehaviour
{
    public float _tGold = 10, _cityHealth = 20, _bPop = 1, _aPop =0, _gpt = 5;
    public static event Func<Vector3, GameObject> CityManagerTile; 
    private Renderer _renderer;
    private bool player1turn = true;
    public GameObject tileBelow, meele, ranger, worker;
    public static event Action<CityManager> cityInfoUI;
    public static event Action<CityManager> cityActionUI;
    public static event Action<String> gameOver;
    public List<GameObject> summonedArmy;
    public List<GameObject> summonedWorkers;
    bool canheal = false;

  
    void Start()
    {
        
        Character.Incrasegold += increaseGold;
        TurnManager.RoundEnd += roundEnd;
        if (gameObject.tag == "HumanB")
        {
            ButtonManager.SpawnHA += spawnRanger;
            ButtonManager.SpawnHM += spawnUnit ;
            ButtonManager.SpawnHMi += spawnWorker ;
            ButtonManager.onHReinforcedPressed += reinforced ;

        }
        else 
        {
            ButtonManager.SpawnMA += spawnRanger;
            ButtonManager.SpawnMM += spawnUnit;
            ButtonManager.SpawnMMi += spawnWorker;
            ButtonManager.onMReinforcedPressed += reinforced ;
            Debug.Log("Monster");
          

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
            GameObject newCharacter;
            _aPop++;
            _tGold -= 5;
            Tile gameobjectTile = TargetPosition.GetComponent<Tile>();

            gameobjectTile._occupied = true;
            newCharacter = Instantiate(meele, spawnPosition, Quaternion.identity);
            summonedArmy.Add(newCharacter);
            Gamemanager.Instance.DecreaseAP();
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
            GameObject newCharacter;
            _aPop++;
            _tGold -= 5;
            gameobjectTile._occupied = true;
            Debug.Log(_tGold);
            newCharacter = Instantiate(ranger, spawnPosition, Quaternion.identity);
            summonedArmy.Add(newCharacter);
            Gamemanager.Instance.DecreaseAP();
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
    
    private void reinforced()
    {
        if (_tGold - 2 >= 0 && !canheal)
        {
            canheal = true;
            _tGold -= 2;
        }
    }
    public void spawnWorker()
    {
        GameObject TargetPosition = tileBelow.GetComponent<Tile>().getSurroundingBlocks();
        Vector3 spawnPosition = new Vector3(TargetPosition.transform.position.x, TargetPosition.transform.position.y+10.1f,TargetPosition.transform.position.z);
        if (TargetPosition != null && (_tGold - 5) >=0)
        {
            Tile gameobjectTile = TargetPosition.GetComponent<Tile>();
            GameObject newCharacter;
            _bPop++;
            _tGold -= 5;
            gameobjectTile._occupied = true;
            newCharacter = Instantiate(worker, spawnPosition, Quaternion.identity);
            summonedWorkers.Add(newCharacter);
        }
        else
        {
            Debug.Log("NoSpace/ no gold");
        }

    }

    private void roundEnd()
    {
        

        if (this.gameObject.tag == "HumanB" && TurnManager.TurnPlayer == TurnManager.TurnOrder.Player1)
        {
             _tGold += _gpt;
             if (canheal)
             {
                 heal();
                
             }
        }
        if (this.gameObject.tag == "MonsterB" && TurnManager.TurnPlayer == TurnManager.TurnOrder.Player2)
        {
            _tGold += _gpt;
            if (canheal)
            {
                heal();
                
            }
        }

        canheal = false;
    }

    private void heal()
    {
        _cityHealth += 3;
    }
    private void increaseGold(float mineGold)
    {
        _tGold += mineGold;
    }

    public void takeDamage(float damage)
    {
        _cityHealth -= damage;
        Debug.Log(_cityHealth);
        if (_cityHealth <= 0)
        {
            Death();
        }
    }
   
}
