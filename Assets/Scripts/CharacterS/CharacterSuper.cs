using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterSuper : MonoBehaviour
{
    public float currentHealth, damage, moveableTiles;
    public int abilityType;
    public TurnManager.TurnOrder controllingPlayer;
    private GameObject CurrentTile ;
    Tile  SelectedTile;
    private RaycastHit info;
    public float availableMoves;
    public string tag;
    public GameObject player;
    public float MoveableTiles
    {
        get => moveableTiles;
        set => moveableTiles = value;
    }

    private void Start()
    {
        


    }

    public TurnManager.TurnOrder ControllingPlayer
    {
        get => controllingPlayer;
        set => controllingPlayer = value;
    }
    public int AbilityType
    {
        get => abilityType;
        set => abilityType = value;
    }
    public float CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = value;
    }
    public float Damage
    {
        get => damage;
        set => damage = value;
    }

    public void selectTile(Tile tile)
    {
        SelectedTile = tile;
    }
    
    public void Move()
    {

        if ( tag == "Human" && TurnManager.TurnPlayer == TurnManager.TurnOrder.Player1 && player == Gamemanager.Instance.selectedunit )
        {
            Physics.Raycast(transform.position, Vector3.down, out info, 12);
            CurrentTile = info.collider.gameObject;
            Tile tileScript = CurrentTile.GetComponent<Tile>();
            for (int i = 0; i < 4; i++)
            {
                if (SelectedTile != null&& SelectedTile.gameObject == tileScript.getMovmentBlocks(i) && !SelectedTile._occupied && availableMoves >0)
                {
                    availableMoves--;
                    tileScript._occupied = false;
                    SelectedTile._occupied = true;
                    Vector3 MoveTarget = new Vector3(SelectedTile.transform.position.x, SelectedTile.transform.position.y + 10.1f, SelectedTile.transform.position.z);
                    gameObject.transform.position = MoveTarget;
                    Gamemanager.Instance._currentAP--;
                    CurrentTile = SelectedTile.gameObject;
                }


                if (availableMoves <=0)
                {
                    Debug.Log("no moves left");
                }
            }
        }

        else if (tag == "Monster" && player == Gamemanager.Instance.selectedunit)
        {
            Debug.Log(tag + gameObject.name);
            Physics.Raycast(transform.position, Vector3.down, out info, 12);
            CurrentTile = info.collider.gameObject;
            Tile tileScript = CurrentTile.GetComponent<Tile>();
            for (int i = 0; i < 4; i++)
            {
                if (SelectedTile != null&& SelectedTile.gameObject == tileScript.getMovmentBlocks(i) && !SelectedTile._occupied && availableMoves >0)
                {
                    availableMoves--;
                    tileScript._occupied = false;
                    SelectedTile._occupied = true;
                    Vector3 MoveTarget = new Vector3(SelectedTile.transform.position.x, SelectedTile.transform.position.y + 10.1f, SelectedTile.transform.position.z);
                    gameObject.transform.position = MoveTarget;
                    Gamemanager.Instance._currentAP--;
                    CurrentTile = SelectedTile.gameObject;
                }

                if (availableMoves <=0)
                {
                    Debug.Log("no moves left");
                }
            }
        }



    }

    public void Attack()
    {
         if ( tag == "Human" && TurnManager.TurnPlayer == TurnManager.TurnOrder.Player1 && player == Gamemanager.Instance.selectedunit )
        {   
            
        }
    }

    public  void TakeDamage(float incomingDamage)
    {
        currentHealth = currentHealth - incomingDamage;
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public abstract void UseAbility(int ability);

    public abstract void Death();

}
