using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class BossBoard : MonoBehaviour
{
    public Tilemap bossTilemap { get; private set; }

    public void Awake()
    {
        bossTilemap = GetComponentInChildren<Tilemap>();
    }
    public void SetTile(Vector3Int position, Tile tile)
    {
        bossTilemap.SetTile(position, tile);
    }
}