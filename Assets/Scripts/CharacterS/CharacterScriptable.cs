using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "CreateCharacter")]

public class CharacterScriptable : ScriptableObject
{
  public enum characterType
  {
    Melee,
    Ranged,
    Miner
  }

  public characterType CharacterType;
  public string[] characterNames = new string[3] { "Soldier", "Ranger", "Miner" };
  public string characterName;
  public float moveableTiles, maxHealth, damage;

  public int abilityType;

  public TurnManager.TurnOrder player;
}
