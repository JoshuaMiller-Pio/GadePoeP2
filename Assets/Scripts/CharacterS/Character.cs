using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Character : CharacterSuper
{
    public CharacterScriptable characterScript;
    public float moveSpeed = 5;
    public GameObject target;
    private MeshRenderer _renderer;
    public static event Action<Character> characterInfoUI;
    public static event Action<Character> characterActionUI;
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
