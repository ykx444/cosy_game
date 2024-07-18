using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    ingredient,
    tool,
    seed,
    placeable
}

[CreateAssetMenu(menuName = "Data/Item")]
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public bool stackable;
    public Sprite icon;
    public ToolAction onAction;
    public ToolAction onTilemapAction;
    public Crop crop;
    public ItemType itemType;
    public int price;
}
