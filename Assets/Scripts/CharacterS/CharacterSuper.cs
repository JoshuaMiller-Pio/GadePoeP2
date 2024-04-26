using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterSuper : MonoBehaviour
{
    public float currentHealth, damage, moveableTiles;
    public int abilityType;
    
    public float MoveableTiles
    {
        get => moveableTiles;
        set => moveableTiles = value;
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
    public abstract void Move();

    public abstract void Attack();

    public abstract void TakeDamage(float incomingDamage);

    public abstract void UseAbility(int ability);

    public abstract void Death();

}
