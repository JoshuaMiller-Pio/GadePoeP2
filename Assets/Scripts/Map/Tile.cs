using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileScriptable tileInfo;
    private bool _hasMine;
    private Renderer _renderer;

    public static event Func<Vector3, GameObject> tileLocator; 
    //on mouse hover highlights game object
    private void OnMouseEnter()
    {
        Color highlightcolor = Color.cyan;
        _renderer.material.color =   highlightcolor;

    }
    
    //removes highlight
    private void OnMouseExit()
    {

        _renderer.material.color = tileInfo.Color;
    }

    //when mouse is clicked will send off block information TODO try move into own script
    private void OnMouseDown()
    {
        Vector3 newTileLocation = new Vector3(transform.localPosition.x, transform.localPosition.y,transform.localPosition.z);
        
        
        GameObject selectedBlock = tileLocator?.Invoke(newTileLocation);

        if (selectedBlock != null)
        {

            Debug.Log($"entered {selectedBlock.name}");
        }
        else
        {
            Debug.Log("null");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    //called from grid manager, populates if there are mines
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
