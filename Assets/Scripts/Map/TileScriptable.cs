using System.Collections;
using System.Collections.Generic;
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

    public float tickDamage = 0, APneeded = 1;
    
    public TileType tileType;
}
