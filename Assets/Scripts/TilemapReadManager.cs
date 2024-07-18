using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class TilemapReadManager : MonoBehaviour
{
    [SerializeField] public Tilemap tilemap;
    [SerializeField] TileData tileDataSO;

    public CropsManager cropsManager;

    public TileBase GetTileBase(Vector2 position)
    {
        //we wanna know if a mouse clicks a farmable tile
        //first convert mous pos to world pos, then to grid pos
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(position);
        Vector3Int gridPos = tilemap.WorldToCell(worldPos);

        TileBase tile = tilemap.GetTile(gridPos);

        if (tile != null && IsFarmable(tile))
        {
            if (IsObstructed(worldPos))
            {
                Debug.Log("The tile is farmable but obstructed by another object.");
                return null;
            }
            else
            {
                Debug.Log("The tile is farmable and not obstructed.");
                return tile;
            }
        }
        else
        {
            Debug.Log("The tile is not farmable.");
            return null;
        }
    }

    private bool IsObstructed(Vector3 worldPos)
    {
        // Use a small raycast from the tile position to check for obstructions
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider != null && hit.collider.tag!="Player")
        {
            // Something is on top of the tile
            Debug.Log("Obstructed by: " + hit.collider.gameObject.name);
            return true;
        }

        return false;
    }


    private bool IsFarmable(TileBase tile)
    {
        // Check if the tile is in the farmable tiles array from the ScriptableObject
        foreach (TileBase farmableTile in tileDataSO.tile)
        {
            if (tile == farmableTile)
            {
                return true;
            }
        }
        return false;
    }

    public Vector3Int GetGridPosition(Vector2 position, bool mouseOver)
    {
        //get a position and return world position
        Vector3 worldPosition;
        if (mouseOver)
        {
            worldPosition = Camera.main.ScreenToWorldPoint(position);
        }
        else
        {
            worldPosition = position;
        }

        Vector3Int gridPosition = tilemap.WorldToCell(worldPosition);
        return gridPosition;

    }

    public void Hide(bool selectable)
    {
        tilemap.gameObject.SetActive(selectable);
    }
}
