using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MarkerManager : MonoBehaviour
{
    [SerializeField] Tilemap targetTilemap;
    [SerializeField] TileBase defaultTile;
    [SerializeField] TileBase highlightTile;

    public Vector3Int markedCellPos;
    Vector3Int oldCellPosition;
    bool show;

    private void LateUpdate()
    {
        if (!show)
        {
            return;
        }
        targetTilemap.SetTile(oldCellPosition, null);
        targetTilemap.SetTile(markedCellPos, defaultTile);
        oldCellPosition = markedCellPos;
    }

    internal void Show(bool selectable)
    {
        show = selectable;
        targetTilemap.gameObject.SetActive(show);
    }

   
}
