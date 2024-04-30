using UnityEngine;
using UnityEngine.Tilemaps;

public enum TetrominoBoss
{
    o
}

[System.Serializable]
public struct TetrominoBossData
{
    public Tile tile;
    public Tile specialTile;
    public TetrominoBoss tetromino;

    public Vector2Int[] cells { get; private set; }
    public Vector2Int[,] wallKicks { get; private set; }

    public void Initialize()
    {
        cells = Data.Cells[tetromino];
        wallKicks = Data.WallKicks[tetromino];
    }
}
