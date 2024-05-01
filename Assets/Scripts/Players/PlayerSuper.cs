using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSuper : MonoBehaviour
{
    public float currentAP;
    public float maxAP;
    public TurnManager.TurnOrder player;
    public Gamemanager _gameManager;
    
    public float CurrentAP
    {
        get => currentAP;
        set => currentAP = value;
    }
    
    public float MaxAP
    {
        get => maxAP;
        set => maxAP = value;
    }

    public abstract void RestartGame();
    public abstract void MovePiece(GameObject movingPiece, Vector3 targetLocation);
    public abstract void SummonPiece(GameObject summonedPrefab, Vector3 targetPosition);
    public abstract void AttackPiece(GameObject attackingPiece, GameObject damagedPiece);
    public abstract void Win();
    public abstract void Lose();
}
