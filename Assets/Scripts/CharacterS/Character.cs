using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class Character : CharacterSuper
{
    public CharacterScriptable characterScript;
    private MeshRenderer _renderer;
    public static event Action<Character> characterInfoUI;
    public static event Action<Character> characterActionUI;
    public static event Action<float> Incrasegold;
    public string name;
    // Start is called before the first frame update
    public enum Ability
    {
        Mine,
        KnockBack,
        Poison
    };

    void Start()
    {
       
       // characterScript = gameObject.GetComponent<CharacterScriptable>();
        currentHealth = characterScript.maxHealth;
        damage = characterScript.damage;
        moveableTiles = characterScript.moveableTiles;
        abilityType = characterScript.abilityType;
        name = characterScript.characterNames[Convert.ToInt32(characterScript.CharacterType)];
        Tile.TileSelected += selectTile;
        ButtonManager.onMovePressed += Move;
        ButtonManager.onAttackPressed += Attack;
        TurnManager.RoundEnd += RoundEndUpdate;
        TurnManager.RoundEnd += resetMoves;
        availableMoves = characterScript.moveableTiles;
        tag = gameObject.tag;
        player = this.gameObject;
    }

    private void OnMouseEnter()
    {
        Color highlightcolor = Color.cyan;
       // _renderer.material.color =   highlightcolor;
        characterInfoUI?.Invoke(this);
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        characterActionUI?.Invoke(this);
        if (TurnManager.TurnPlayer == TurnManager.TurnOrder.Player1 && gameObject.tag == "Human")
        {
            Gamemanager.Instance.selectedunit = this.gameObject;
        }
        else if (TurnManager.TurnPlayer == TurnManager.TurnOrder.Player2 && gameObject.tag == "Monster")
        {
            Gamemanager.Instance.selectedunit = this.gameObject;

        }
      

        if (TurnManager.TurnPlayer == TurnManager.TurnOrder.Player1 && gameObject.tag == "Monster")
        {
            Gamemanager.Instance.selectedEnemy = this.gameObject;
        }
        else if (TurnManager.TurnPlayer == TurnManager.TurnOrder.Player2 && gameObject.tag == "Human")
        {
            Gamemanager.Instance.selectedEnemy = this.gameObject;
        }

        
      
    }

   

    
    void RoundEndUpdate()
    {
        RaycastHit info;
        Physics.Raycast(transform.position, Vector3.down, out info, 12);
        GameObject CurrentTile = info.collider.gameObject;
        Tile tileScript = CurrentTile.GetComponent<Tile>();
        if (characterScript.CharacterType == CharacterScriptable.characterType.Miner && tileScript.HasMine )
        {
            Incrasegold?.Invoke(tileScript.tileInfo.mineValue);
        }

        if (tileScript.tileInfo.tileType == TileScriptable.TileType.Lava)
        {
            currentHealth -= tileScript.tileInfo.tickDamage;
        }
    }

    void resetMoves()
    {
        availableMoves = characterScript.moveableTiles;
        canMove = true;

    }

    public override void UseAbility(int ability)
    {
        if (ability == 0)
        {
            
        }
        
        if (ability == 1)
        {
            
        }
        
        if (ability == 2)
        {
            
        }
    }

    private void OnDestroy()
    {
        Tile.TileSelected -= selectTile;
        ButtonManager.onMovePressed -= Move;
        ButtonManager.onAttackPressed -= Attack;
        TurnManager.RoundEnd -= RoundEndUpdate;
        TurnManager.RoundEnd -= resetMoves;    }

    public override void Death()
    {
        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
