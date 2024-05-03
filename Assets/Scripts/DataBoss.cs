using System.Collections.Generic;
using UnityEngine;

public static class DataBoss
{
    public static readonly float cos = Mathf.Cos(Mathf.PI / 2f);
    public static readonly float sin = Mathf.Sin(Mathf.PI / 2f);
    public static readonly float[] RotationMatrix = new float[] { cos, sin, -sin, cos };

    public static readonly Dictionary<TetrominoBoss, Vector2Int[]> Cells = new Dictionary<TetrominoBoss, Vector2Int[]>()
    {
        { TetrominoBoss.o, new Vector2Int[] { new Vector2Int(0, 0) } },
    };

    private static readonly Vector2Int[,] WallKickso = new Vector2Int[,] {
        { new Vector2Int(0, 0) },

    };
    public static readonly Dictionary<TetrominoBoss, Vector2Int[,]> WallKicks = new Dictionary<TetrominoBoss, Vector2Int[,]>()
    {
        { TetrominoBoss.o, WallKickso },
    };

}
