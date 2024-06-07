using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterSuper : MonoBehaviour
{
    public float currentHealth, damage, moveableTiles;
    public int abilityType;
    public TurnManager.TurnOrder controllingPlayer;
    public GameObject CurrentTile,EnemyTile;
    public Tile  SelectedTile;
    public Tile Occupiedtile;
    private RaycastHit info ;
    public float availableMoves;
    public string tag;
    public GameObject player;
    public bool nearEnemyCity, canMove;
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
            
            GameObject citytile = Gamemanager.Instance.Mcitytile;
            
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
                    Gamemanager.Instance.DecreaseAP();
                    CurrentTile = SelectedTile.gameObject;
                    Occupiedtile = SelectedTile;


                }
                if (availableMoves <=0)
                {
                    Debug.Log("no moves left");
                    canMove = false;
                }

    
                tileScript = CurrentTile.GetComponent<Tile>();
                Occupiedtile = CurrentTile.GetComponent<Tile>();
                if (  citytile == tileScript.getCityBlocks(i) )
                {
                     deductHP = Gamemanager.Instance.Mcity.GetComponent<CityManager>();
                    nearEnemyCity = true;
                }

              
            }
        }

        else if (tag == "Monster" && player == Gamemanager.Instance.selectedunit && (TurnManager.TurnPlayer == TurnManager.TurnOrder.Player2 ||TurnManager.TurnPlayer == TurnManager.TurnOrder.AI ))
        {

            Physics.Raycast(transform.position, Vector3.down, out info, 12);
            CurrentTile = info.collider.gameObject;
            
            GameObject citytile = Gamemanager.Instance.Pcitytile;

            Tile tileScript = CurrentTile.GetComponent<Tile>();
           
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
                    Occupiedtile = CurrentTile.GetComponent<Tile>();
                }

                if (availableMoves <=0)
                {
                    Debug.Log("no moves left");
                }

                
               
                tileScript = CurrentTile.GetComponent<Tile>();
                if (  citytile == tileScript.getCityBlocks(i) )
                {
                    deductHP = Gamemanager.Instance.Mcity.GetComponent<CityManager>();
                    nearEnemyCity = true;
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
            
           
             if ( tag == "Human" && TurnManager.TurnPlayer == TurnManager.TurnOrder.Player1 && player == Gamemanager.Instance.selectedunit && Gamemanager.Instance.selectedEnemy != null )
            {
                
                for (int i = 0; i < 4; i++)
                {
                    if (EtileScript != null && EtileScript.gameObject == PtileScript.getAttackBlocks(i) && Gamemanager.Instance._currentAP != 0)
                    {
                        Gamemanager.Instance.selectedEnemy.GetComponent<Character>().TakeDamage(damage);
                        Gamemanager.Instance.DecreaseAP();
                    }
                }
                
            }
            else if (tag == "Monster" && player == Gamemanager.Instance.selectedunit && Gamemanager.Instance.selectedEnemy != null && (TurnManager.TurnPlayer == TurnManager.TurnOrder.Player2||TurnManager.TurnPlayer == TurnManager.TurnOrder.AI) )
            {
               
                for (int i = 0; i < 4; i++)
                {
                    if (EtileScript != null && EtileScript.gameObject == PtileScript.getAttackBlocks(i) && Gamemanager.Instance._currentAP != 0)
                    {
                        Gamemanager.Instance.selectedEnemy.GetComponent<Character>().TakeDamage(damage);
                        Gamemanager.Instance.DecreaseAP();

                    }
                }
            }
        }
        else
        {
            if (nearEnemyCity && tag == "Human" && deductHP.gameObject.tag != "HumanB"  && TurnManager.TurnPlayer == TurnManager.TurnOrder.Player1 && player == Gamemanager.Instance.selectedunit )
            {
                deductHP.takeDamage(damage);
                Gamemanager.Instance.DecreaseAP();

                return;
            }
            if (nearEnemyCity && tag == "Monster" && deductHP.gameObject.tag != "MonsterB"  &&  player == Gamemanager.Instance.selectedunit && (TurnManager.TurnPlayer == TurnManager.TurnOrder.Player2 || TurnManager.TurnPlayer == TurnManager.TurnOrder.AI))
            { 
                Debug.Log("base");
                deductHP.takeDamage(damage);
                Gamemanager.Instance.DecreaseAP();

                return;
            }
                Debug.Log("outsidebase");
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
