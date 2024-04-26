using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileScriptable tileInfo;
    private bool _hasMine;
 

    private void OnMouseEnter()
    {
        Debug.Log($"entered {gameObject.name}");
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

   public void hasMine()
    {
        _hasMine = true;
        Vector3 pos = new Vector3(0.354f, 0.5098f, 0.0069f);
        GameObject mine = Instantiate(tileInfo.mine, pos, Quaternion.identity);
        mine.transform.SetParent(this.transform);
        mine.transform.localPosition = pos;
    }
    // Update is called once per frame
    void Update()
    {
 
    }
}
