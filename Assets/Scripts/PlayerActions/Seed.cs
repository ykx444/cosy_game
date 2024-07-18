using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Tool Action/Seed")]
public class Seed : ToolAction
{
    public override bool OnApplyToTileMap(Vector2 worldpPosition, Vector3Int tilePosition, TilemapReadManager tileManager, Item item)
    {
        if (tileManager.cropsManager.CheckPlowed(tilePosition))
        {
            if (!tileManager.cropsManager.CheckSeeded(tilePosition))
            {
                tileManager.cropsManager.Seed(tilePosition, item.crop);
                return true;
            }

        }

        return false;
    }

    public override void OnItemUsed(Item usedItem, ItemContainer inventory)
    {
        inventory.RemoveItem(usedItem);
    }
}
