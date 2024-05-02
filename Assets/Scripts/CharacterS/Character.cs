using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        availableMoves = characterScript.moveableTiles;
        TurnManager.RoundEnd += GoldUpdate;

    }

    private void OnMouseEnter()
    {
        Color highlightcolor = Color.cyan;
       // _renderer.material.color =   highlightcolor;
        characterInfoUI?.Invoke(this);
    }

    private void OnMouseDown()
    {
        characterActionUI?.Invoke(this);
    }

   

    
    void GoldUpdate()
    {
        RaycastHit info;
        Physics.Raycast(transform.position, Vector3.down, out info, 12);
        GameObject CurrentTile = info.collider.gameObject;
        Tile tileScript = CurrentTile.GetComponent<Tile>();
        if (characterScript.CharacterType == CharacterScriptable.characterType.Miner && tileScript.HasMine )
        {
            Incrasegold?.Invoke(tileScript.tileInfo.mineValue);
        }
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
    
    public override void Death()
    {
        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
