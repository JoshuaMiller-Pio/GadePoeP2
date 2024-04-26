using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileScriptable tileInfo;
    private bool _hasMine;
    public GridManager GridManager;
    private Renderer _renderer;

    private void OnMouseEnter()
    {
        Color highlightcolor = Color.cyan;
        _renderer.material.color =   highlightcolor;

    }

    private void OnMouseExit()
    {

        _renderer.material.color = tileInfo.Color;
    }

    private void OnMouseDown()
    {
        GameObject test = GridManager.GetTileAtPosition(new Vector3(transform.localPosition.x,transform.localPosition.y, transform.localPosition.z));
        
        if (test != null)
        {
            
            Debug.Log($"entered {test.name}");
        }
        else
        {
            Debug.Log("null");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GridManager = GameObject.FindWithTag("GameController").GetComponent<GridManager>();
        _renderer = GetComponent<Renderer>();
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
