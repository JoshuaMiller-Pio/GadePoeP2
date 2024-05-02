using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile", menuName = "CreateTile")]
public class TileScriptable : ScriptableObject
{
    
    public enum TileType
    {
        Forest,
        Desert,
        Lava,
        Snow,
        Mountain
    }

    public string[] names = new string[5] { "Forest", "Desert", "Lava", "Snow", "Mountain" }; 
    public string[] tileDescriptions = new string[5] { "A lush, beautiful forest that allows for safe and easy traversal", 
        "A harsh, hot, weather beaten expanse. The soft sand makes the land difficult to traverse", 
        "Covered with the molten hot blood of the land, standing in this terrain deals 5 damage to characters per turn", 
        "A hard, frozen, weather beaten expanse. The the cold climate and harsh weather make the land difficult to traverse", 
        "Wide open cliffs give expansive views, but steep elevation makes for the most difficult traversal" };
    public Color Color;
    public string name;
    public string tileDescription;
    public bool hasMine = true;
    public float tickDamage = 0, APneeded = 1,mineValue;
    public GameObject mine;
    public TileType tileType;
}
