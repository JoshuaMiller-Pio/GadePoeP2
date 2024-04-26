using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : CharacterSuper
{
    public CharacterScriptable characterScript;
    public float moveSpeed = 5;
    public GameObject target;
    // Start is called before the first frame update
    public enum ability
    {
        Mine,
        KnockBack,
        Poison
    };

    void Start()
    {
        characterScript = gameObject.GetComponent<CharacterScriptable>();
        currentHealth = characterScript.maxHealth;
        damage = characterScript.damage;
        moveableTiles = characterScript.moveableTiles;
        abilityType = characterScript.abilityType;
        
    }

    public override void Move()
    {
        gameObject.transform.position =
            Vector3.Lerp(gameObject.transform.position, target.transform.position, moveSpeed);
    }

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public override void TakeDamage(float incomingDamage)
    {
        currentHealth = currentHealth - incomingDamage;
        if (currentHealth <= 0)
        {
            Death();
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
