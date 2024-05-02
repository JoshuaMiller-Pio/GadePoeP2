using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfomanager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TurnManager.RoundEnd += updateMetrics;
    }
    
    //on round end updates metrics
    public void updateMetrics()
    {
        
    }
    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
