using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterSuper : MonoBehaviour
{
    public float currentHealth, damage, moveableTiles;
    public int abilityType;
    public TurnManager.TurnOrder controllingPlayer;
    private GameObject CurrentTile,EnemyTile;
    Tile  SelectedTile;
    private RaycastHit info ;
    public float availableMoves;
    public string tag;
    public GameObject player;
    private bool nearEnemyCity;
    private CityManager deductHP;
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
            
            GameObject citytile = Gamemanager.Instance.Mcity.collider.gameObject;
            Tile tileScript = CurrentTile.GetComponent<Tile>();
            for (int i = 0; i < 4; i++)
            {
                if (SelectedTile != null&& SelectedTile.gameObject == tileScript.getMovmentBlocks(i) && !SelectedTile._occupied && availableMoves >0 && Gamemanager.Instance._currentAP != 0)
                {
                    availableMoves--;
                    tileScript._occupied = false;
                    SelectedTile._occupied = true;
                    Vector3 MoveTarget = new Vector3(SelectedTile.transform.position.x, SelectedTile.transform.position.y + 10.1f, SelectedTile.transform.position.z);
                    gameObject.transform.position = MoveTarget;
                    Gamemanager.Instance._currentAP--;
                    CurrentTile = SelectedTile.gameObject;
                     tileScript = CurrentTile.GetComponent<Tile>();
                }
                if (availableMoves <=0)
                {
                    Debug.Log("no moves left");
                }

                //check if near enemy city
                if (tileScript.getAttackBlocks(i) != null)
                {
                    Debug.Log($"citytile is at {citytile} + {citytile.transform.localPosition} + {citytile.tag}");
                }
                else if (  citytile ==tileScript.getAttackBlocks(i) )
                {
                     deductHP = citytile.GetComponent<CityManager>();
                     Debug.Log("successful");
                    nearEnemyCity = true;
                }

              
            }
        }

        else if (tag == "Monster" && player == Gamemanager.Instance.selectedunit)
        {

            Debug.Log(tag + gameObject.name);
            Physics.Raycast(transform.position, Vector3.down, out info, 12);
            CurrentTile = info.collider.gameObject;
            Tile tileScript = CurrentTile.GetComponent<Tile>();
            GameObject citytile = info.collider.gameObject;
            for (int i = 0; i < 4; i++)
            {
                if (SelectedTile != null&& SelectedTile.gameObject == tileScript.getMovmentBlocks(i) && !SelectedTile._occupied && availableMoves >0 && Gamemanager.Instance._currentAP != 0)
                {
                    availableMoves--;
                    Gamemanager.Instance.DecreaseAP();                    
                    tileScript._occupied = false;
                    SelectedTile._occupied = true;
                    Vector3 MoveTarget = new Vector3(SelectedTile.transform.position.x, SelectedTile.transform.position.y + 10.1f, SelectedTile.transform.position.z);
                    gameObject.transform.position = MoveTarget;
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
        //add enemy block below
        if (Gamemanager.Instance.selectedEnemy != null && !nearEnemyCity)
        {
            Physics.Raycast(Gamemanager.Instance.selectedEnemy.transform.position, Vector3.down, out info, 12);
            EnemyTile = info.collider.gameObject;
            Tile EtileScript = EnemyTile.GetComponent<Tile>();
        
        
            Physics.Raycast(transform.position, Vector3.down, out info, 12);
            CurrentTile = info.collider.gameObject;
            Tile PtileScript = CurrentTile.GetComponent<Tile>();
            
            if (nearEnemyCity && tag == "Human" && deductHP.gameObject.tag != "Human"  && TurnManager.TurnPlayer == TurnManager.TurnOrder.Player1 && player == Gamemanager.Instance.selectedunit )
            {
                deductHP.takeDamage(damage);
                return;
            }
            if (nearEnemyCity && tag == "Monster" && deductHP.gameObject.tag != "Monster"  &&  player == Gamemanager.Instance.selectedunit )
            {
                deductHP.takeDamage(damage);
                return;
            }
             if ( tag == "Human" && TurnManager.TurnPlayer == TurnManager.TurnOrder.Player1 && player == Gamemanager.Instance.selectedunit && Gamemanager.Instance.selectedEnemy != null )
            {
                
                for (int i = 0; i < 4; i++)
                {
                    if (EtileScript != null && EtileScript.gameObject == PtileScript.getAttackBlocks(i) && Gamemanager.Instance._currentAP != 0)
                    {
                        Gamemanager.Instance.DecreaseAP();                    
                        Debug.Log("Attack");
                        Gamemanager.Instance.selectedEnemy.GetComponent<Character>().TakeDamage(damage);
                        
                    }
                }
                
            }
             //TODO FIX ELSE CANT ATTACK AS MONSTER
            else if (tag == "Monster" && player == Gamemanager.Instance.selectedunit && Gamemanager.Instance.selectedEnemy != null)
            {
               
                for (int i = 0; i < 4; i++)
                {
                    if (EtileScript != null && EtileScript.gameObject == PtileScript.getAttackBlocks(i) && Gamemanager.Instance._currentAP != 0)
                    {
                        Gamemanager.Instance.DecreaseAP();                    
                        Debug.Log("Attack");
                        Gamemanager.Instance.selectedEnemy.GetComponent<Character>().TakeDamage(damage);
                        
                    }
                }
            }
        }
       

    }

    public  void TakeDamage(float incomingDamage)
    {
        currentHealth = currentHealth - incomingDamage;
        if (currentHealth <= 0)
        {
            CurrentTile.GetComponent<Tile>()._occupied = false;
            Death();
        }
    }

    public abstract void UseAbility(int ability);

    public abstract void Death();

}
