using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    public float length, breath, displacementt,secondDisplace;
    private float DesertBioms;
    public GameObject forest, snow, lava, mountain, desert;
    // Start is called before the first frame update
    void Start()
    {

        displacementt = Random.Range(5, 12);
        secondDisplace = Random.Range(8, 30);

        for (int x = 0; x < length; x++)
        {
            for (int z = 0; z < breath; z++)
            {
                Vector3 position = new Vector3(x * 6, PerlNoise(x,z,displacementt) *secondDisplace, z * 6);
                GameObject tile = Instantiate(Biomeselector(position),position,Quaternion.identity) as GameObject;
                tile.transform.SetParent(this.transform);
            }
        }
    }

    private float PerlNoise(int x, int z, float displacment)
    {
        float xDisplace = (x + this.transform.position.x) / displacment;
        float zDisplace = (z + this.transform.position.y) / displacment;
        return Mathf.PerlinNoise(xDisplace, zDisplace);
    }

    private GameObject Biomeselector(Vector3 position)
    {
      
        
        
        if (position.y >= 10 && position.y < 14)
        {
            return mountain;
        }
        else if (position.y >= 14)
        {
            return snow;
        }
        else if (position.y <= 4)
        {
            return lava;
        }
        
        if (DesertBioms <=Random.Range(50,100))
        {
            RaycastHit info;
            if ( DesertBioms <= Random.Range(0,3) && Random.Range(0, 10) == Random.Range(0, 10))
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
        
        Debug.Log("forest");
        return forest;
    }
}
