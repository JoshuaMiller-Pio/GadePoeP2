using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : PlayerSuper
{
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = Gamemanager.Instance;
        player = TurnManager.TurnOrder.Player2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void RestartGame()
    {
        throw new System.NotImplementedException();
    }

    public override void MovePiece(GameObject movingPiece, Vector3 targetLocation)
    {
        throw new System.NotImplementedException();
    }

    public override void SummonPiece(GameObject summonedPrefab, Vector3 targetPosition)
    {
        throw new System.NotImplementedException();
    }

    public override void AttackPiece(GameObject attackingPiece, GameObject damagedPiece)
    {
        throw new System.NotImplementedException();
    }

    public override void Win()
    {
        throw new System.NotImplementedException();
    }

    public override void Lose()
    {
        throw new System.NotImplementedException();
    }
}
