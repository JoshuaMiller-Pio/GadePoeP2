using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    public float length, breath, displacementt,secondDisplace;
    
    public GameObject forest, snow, lava, mountain, desert;
    // Start is called before the first frame update
    void Start()
    {

        displacementt = Random.Range(5, 10);
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
        else if (position.y <= 5)
        {
            return lava;
        }
        
        
        
        return forest;
    }
}
