using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    public float distance, displacement,secondDisplace;
    public GameObject forest, snow, lava, mountain, desert;
    private float DesertBioms, minesAllowed, currentMines = 0;
    public  Dictionary<Vector3, GameObject> _tiles;
    public GameObject[] Towns;
    public static event Action<GameObject[]> MapCreated;
    // Start is called before the first frame update
    void Start()
    {
        //event from the tile script
        Tile.tileLocator += GetTileAtPosition;
        CityManager.CityManagerTile += GetTileAtPosition;
        float mapCoverage = Random.Range(((distance*distance)/5),(distance*distance)/2),bioNum = Random.Range(0, 10);
        
        //sets the max mines allowed on the map
        minesAllowed = distance / 3;
        
        //displaces the perlin noise for randomness
        displacement = Random.Range(4, 7);//4 7
        
        //allows for more randomness
        secondDisplace = Random.Range(8, 30);//8 30
       
        //creates the dictionary 
        _tiles = new Dictionary<Vector3, GameObject>();
     
        //
        for (int x = 0; x < distance; x++)
        {
            for (int z = 0; z < distance; z++)
            {
                float y = PerlNoise(x, z, displacement) * secondDisplace;
                Debug.Log(z);
                Vector3 position = new Vector3(x * 6, y , z * 6);
                GameObject tile = Instantiate(Biomeselector(position,mapCoverage,bioNum),position,Quaternion.identity);
                if (currentMines <minesAllowed && Random.Range(0, 40) == Random.Range(0, 40))
                {
                    tile.GetComponent<Tile>().hasMine();
                    currentMines++;
                }
                tile.transform.SetParent(this.transform);
                _tiles[tile.transform.localPosition] = tile;
            }
        }
        Debug.Log("gridManager off");
        Gamemanager.Instance.subscribe();
        MapCreated?.Invoke(Towns);
    }
    //adds mountains and revines to the map
    private float PerlNoise(int x, int z, float displace)
    {
        float xDisplace = (x + this.transform.position.x) / displace;
        float zDisplace = (z + this.transform.position.y) / displace;
        return Mathf.PerlinNoise(xDisplace, zDisplace);
    }

    //accesses the dictionary through and event
    public GameObject GetTileAtPosition(Vector3 position)
    {
        if (_tiles.TryGetValue(position, out GameObject tile))
        {
            return tile;
        }

        return null;

    }
    
    //decides what biome the instantiated game object should be
    private GameObject Biomeselector(Vector3 position, float mapCoverage,float  bioNum)
    {
      
        
        
        if (position.y >= 14 && position.y < 17)
        {
            return mountain;
        }
        else if (position.y >= 17)
        {
            return snow;
        }
        else if (position.y <= 2.5)
        {
            return lava;
        }
        
        //checks if desert map coverage
        if (DesertBioms <= mapCoverage)
        {
            RaycastHit info;
            if ( DesertBioms <= bioNum && Random.Range(0, 40) == Random.Range(0, 40))
            {
                DesertBioms++;
                return desert;
            }

            if ((Physics.Raycast(position,Vector3.back,out info,12)&& info.collider.gameObject.name == "Desert(Clone)")||(Physics.Raycast(position,Vector3.left,out info,12) && info.collider.gameObject.name == "Desert(Clone)")||(Physics.Raycast(position,Vector3.forward,out info,12)&& info.collider.gameObject.name == "Desert(Clone)")||(Physics.Raycast(position, Vector3.right, out info, 12)&& info.collider.gameObject.name == "Desert(Clone)"))
            {

                    DesertBioms++;
                    Debug.Log(info.collider.gameObject.name);
                    return desert;

            }
     
        }
        
        return forest;
    }
}
