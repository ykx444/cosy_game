using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ToolAction : ScriptableObject
{
    //base class for every tool actions
    //each action will orverride this method
    public virtual bool OnApply(Vector2 worldPoint)
    {
        return true;
    }

    public virtual bool OnApplyToTileMap(Vector2 worldpPosition, Vector3Int tilePosition, TilemapReadManager tileManager, Item item)
    {
        Debug.LogError("OnApplyToTilemap not implemented");
        return true;
    }

    public virtual void OnItemUsed(Item usedItem, ItemContainer inventory) { }
}
