using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Data/Tool Action/Plow")]
public class Plow : ToolAction
{
    public override bool OnApplyToTileMap(Vector2 position, Vector3Int gridPosition, TilemapReadManager tileManager, Item item)
    {
        TileBase tileToPlow = tileManager.GetTileBase(position);

        if (tileToPlow != null)
        {
            GameManager.instance.playerAnim.SetTrigger("Plow");
            tileManager.cropsManager.Plow(gridPosition);
            GameManager.instance.audioManager.PlaySoundEffect("dig");
            return true;
        }


        return false;
    }
}
