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
    public TetrominoBoss tetromino;

    public Vector2Int[] cells { get; private set; }
    public Vector2Int[,] wallKicks { get; private set; }

    public void Initialize()
    {
        cells = DataBoss.Cells[tetromino];
        wallKicks = DataBoss.WallKicks[tetromino];
    }
}
