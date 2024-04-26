using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "CreateCharacter")]

public class CharacterScriptable : ScriptableObject
{
  public float moveableTiles, maxHealth, damage;

  public int abilityType;
}
