using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    public float distance, displacement,secondDisplace;
    private float DesertBioms, minesAllowed, currentMines = 0;
    public GameObject forest, snow, lava, mountain, desert;
    // Start is called before the first frame update
    void Start()
    {
        float mapCoverage = Random.Range(((distance*distance)/5),(distance*distance)/2),bioNum = Random.Range(0, 10);
        minesAllowed = distance / 3;
        displacement = Random.Range(4, 7);//4 7
        secondDisplace = Random.Range(8, 30);//8 30

        for (int x = 0; x < distance; x++)
        {
            for (int z = 0; z < distance; z++)
            {
                Vector3 position = new Vector3(x * 6, PerlNoise(x,z,displacement) *secondDisplace, z * 6);
                GameObject tile = Instantiate(Biomeselector(position,mapCoverage,bioNum),position,Quaternion.identity) as GameObject;
                if (currentMines <minesAllowed && Random.Range(0, 40) == Random.Range(0, 40))
                {
                    tile.GetComponent<Tile>().hasMine();
                    currentMines++;
                }
                tile.transform.SetParent(this.transform);
            }
        }
    }

    private float PerlNoise(int x, int z, float displace)
    {
        float xDisplace = (x + this.transform.position.x) / displace;
        float zDisplace = (z + this.transform.position.y) / displace;
        return Mathf.PerlinNoise(xDisplace, zDisplace);
    }

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
