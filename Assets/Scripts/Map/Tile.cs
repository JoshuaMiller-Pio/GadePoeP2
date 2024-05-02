using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    public TileScriptable tileInfo;
    private bool _hasMine;
    private Renderer _renderer;
    public  bool _occupied;
    public static event Action<GameObject> placeTown;
    public static event Action<Tile> TileSelected;
    public static event Func<Vector3, GameObject> tileLocator; 
    public static event Action<TileScriptable> tileUI;
    //on mouse hover highlights game object
    private void OnMouseEnter()
    {
        Color highlightcolor = Color.cyan;
        _renderer.material.color =   highlightcolor;
        tileUI?.Invoke(tileInfo);
    }
    
    //removes highlight
    private void OnMouseExit()
    {
        
        _renderer.material.color = tileInfo.Color;
    }

    //when mouse is clicked will send off block information TODO try move into own script
    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        Vector3 newTileLocation = new Vector3(transform.localPosition.x, transform.localPosition.y,transform.localPosition.z);
        
        
        GameObject selectedBlock = tileLocator?.Invoke(newTileLocation);

        if (selectedBlock != null)
        {
            
            if (Gamemanager.Instance.canPlaceTown)
            {
                placeTown?.Invoke(selectedBlock);
            }
            else
            {
                TileSelected?.Invoke(this);
            }
            //send event of a click
            
        }
        else
        {
            Debug.Log("null");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       // tileInfo = gameObject.GetComponent<TileScriptable>();
        _renderer = GetComponent<Renderer>();
        tileInfo.name = tileInfo.names[Convert.ToInt32(tileInfo.tileType)];
        tileInfo.tileDescription = tileInfo.tileDescriptions[Convert.ToInt32(tileInfo.tileType)];
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

   public GameObject getSurroundingBlocks()
   {
       RaycastHit info;
       
      
        if (Physics.Raycast(transform.position, Vector3.forward, out info, 12))
       {
           Tile gameobjectTile = info.collider.gameObject.GetComponent<Tile>();
           
           if (gameobjectTile._occupied ==false)
           {
               return info.collider.gameObject;
           }
       }
        if (Physics.Raycast(transform.position, Vector3.back, out info, 12))
       {
           Tile gameobjectTile = info.collider.gameObject.GetComponent<Tile>();
           
           if (gameobjectTile._occupied ==false)
           {
               return info.collider.gameObject;
           }
       }
        if (Physics.Raycast(transform.position, Vector3.left, out info, 12))
       {
           Tile gameobjectTile = info.collider.gameObject.GetComponent<Tile>();
           
           if (gameobjectTile._occupied ==false)
           {
               return info.collider.gameObject;
           }
       }
        if (Physics.Raycast(transform.position, Vector3.right, out info, 12))
       {
           Tile gameobjectTile = info.collider.gameObject.GetComponent<Tile>();
           
           if (gameobjectTile._occupied ==false)
           {
               return info.collider.gameObject;
           }
       }
    Debug.Log("wrong");
       return null;
   }
   
   
   public GameObject getMovmentBlocks(int passthrough)
   {
       RaycastHit info;
       
      
       if (Physics.Raycast(transform.position, Vector3.forward, out info, 12) && passthrough == 0)
       {
           Tile gameobjectTile = info.collider.gameObject.GetComponent<Tile>();
           
           if (gameobjectTile._occupied ==false)
           {
               return info.collider.gameObject;
           }
       }
       if (Physics.Raycast(transform.position, Vector3.back, out info, 12) && passthrough == 1)
       {
           Tile gameobjectTile = info.collider.gameObject.GetComponent<Tile>();
           
           if (gameobjectTile._occupied ==false)
           {
               return info.collider.gameObject;
           }
       }
       if (Physics.Raycast(transform.position, Vector3.left, out info, 12) && passthrough == 2)
       {
           Tile gameobjectTile = info.collider.gameObject.GetComponent<Tile>();
           
           if (gameobjectTile._occupied ==false)
           {
               return info.collider.gameObject;
           }
       }
       if (Physics.Raycast(transform.position, Vector3.right, out info, 12)&& passthrough == 3)
       {
           Tile gameobjectTile = info.collider.gameObject.GetComponent<Tile>();
           
           if (gameobjectTile._occupied ==false)
           {
               return info.collider.gameObject;
           }
       }
       Debug.Log("wrong");
       return null;
   }
    // Update is called once per frame
    void Update()
    {
 
    }
}
