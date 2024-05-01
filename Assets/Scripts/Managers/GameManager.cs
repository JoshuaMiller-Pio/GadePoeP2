using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : Singleton<Gamemanager>
{
    public TurnManager _turnManager;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        _turnManager = GameObject.FindGameObjectWithTag("TurnManager").GetComponent<TurnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
