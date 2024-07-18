using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Tool Action/Harvest")]
public class Harvest : ToolAction
{
    public override bool OnApplyToTileMap(Vector2 worldpPosition, Vector3Int tilePosition, TilemapReadManager tileManager, Item item)
    {
        bool isharvested = tileManager.cropsManager.Harvest(tilePosition);
        return isharvested;
    }
}
