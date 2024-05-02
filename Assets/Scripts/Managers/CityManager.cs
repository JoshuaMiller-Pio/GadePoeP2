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
    
    public GameObject tileBelow, meele, ranger, worker;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    
    public static event Action<CityManager> cityInfoUI;
    public static event Action<CityManager> cityActionUI;
    
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
        Vector3 TargetPosition = tileBelow.GetComponent<Tile>().getSurroundingBlocks();
        Vector3 spawnPosition = new Vector3(TargetPosition.x, TargetPosition.y+10.1f,TargetPosition.z);
        if (TargetPosition != new Vector3(0,0,0))
        {
            _aPop++;
            Instantiate(meele, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.Log("NoSpace");
        }

    }
    
    public void spawnWorker()
    {
        Vector3 TargetPosition = tileBelow.GetComponent<Tile>().getSurroundingBlocks();
        Vector3 spawnPosition = new Vector3(TargetPosition.x, TargetPosition.y+10.1f,TargetPosition.z);
        if (TargetPosition != new Vector3(0,0,0))
        {
            _bPop++;
            Instantiate(meele, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.Log("NoSpace");
        }

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
