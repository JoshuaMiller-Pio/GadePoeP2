using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterSuper : MonoBehaviour
{
    public float currentHealth, damage, moveableTiles;
    public int abilityType;
    public TurnManager.TurnOrder controllingPlayer;
    private GameObject CurrentTile, nextTile;
    Tile  SelectedTile;
    private RaycastHit info;
    public float MoveableTiles
    {
        get => moveableTiles;
        set => moveableTiles = value;
    }

    private void Start()
    {
        Physics.Raycast(transform.position, Vector3.forward, out info, 12);
        CurrentTile = info.collider.gameObject;
        Tile.TileSelected += selectTile;
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
        Tile tileScript = CurrentTile.GetComponent<Tile>();

        for (int i = 0; i < 4; i++)
        {
            if (SelectedTile.gameObject == tileScript.getMovmentBlocks(i))
            {
                tileScript._occupied = false;
                SelectedTile._occupied = true;
                Vector3 MoveTarget = new Vector3(SelectedTile.transform.position.x, SelectedTile.transform.position.y + 10.1f, SelectedTile.transform.position.z);
                gameObject.transform.position = MoveTarget;
            }
        }
       
        
        
        //if raycasted tile == selected tile and selected tile isnt occupied then move and set to occupied and set previous tile to open.
        
        //deduct AP
    }

    public void Attack()
    {
        
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
