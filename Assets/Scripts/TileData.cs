using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName ="Data/Tile Data")]
public class TileData : ScriptableObject
{
    public bool plowable;
    public List<TileBase> tile;


}
