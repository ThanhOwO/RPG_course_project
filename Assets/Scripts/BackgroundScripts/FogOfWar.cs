using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogOfWar : MonoBehaviour
{
    public Tilemap fogTilemap;
    public Transform player;
    public float fogClearRadius = 2f;
    public TileBase fogTile;
    private HashSet<Vector3Int> clearedTiles = new HashSet<Vector3Int>();

    void Update()
    {
        ClearFogAroundPlayer();
    }

    private void ClearFogAroundPlayer()
    {
        Vector3Int playerTilePos = fogTilemap.WorldToCell(player.position);
        int range = Mathf.CeilToInt(fogClearRadius);
        
        for(int x = -range; x <= range; x++)
        {
            for(int y = -range; y <= range; y++)
            {
                Vector3Int tilePos = new Vector3Int(playerTilePos.x + x, playerTilePos.y + y, 0);
                if(Vector3.Distance(fogTilemap.CellToWorld(tilePos), player.position) <= fogClearRadius)
                {
                    if(fogTilemap.HasTile(tilePos))
                    {
                        fogTilemap.SetTile(tilePos, null);
                        clearedTiles.Add(tilePos);
                    }
                }
            }
        }
    }

    public void SaveFogState(ref GameData _data)
    {
        _data.clearedFogTiles = new List<Vector3Int>(clearedTiles);
    }

    public void RestoreClearedTiles(List<Vector3Int> savedTiles)
    {
        if (savedTiles == null) return;

        foreach (Vector3Int tilePos in savedTiles)
        {
            fogTilemap.SetTile(tilePos, null);
            clearedTiles.Add(tilePos);
        }
    }
}
