using UnityEngine;
using UnityEngine.Tilemaps;

public class UpcomingTetrominosDisplay : MonoBehaviour
{
    public Tilemap upcomingDisplayTilemap;
    private UpcomingTetrominos upcomingTetrominos;
    private bool isActivated = false;

    private void Awake()
    {
        isActivated = true;
    }

    public void Initialize(UpcomingTetrominos upcomingTetrominos)
    {
        this.upcomingTetrominos = upcomingTetrominos;
    }
    public void UpdateDisplay()
    {
        upcomingDisplayTilemap.ClearAllTiles(); // Clear previous display
        int index = 0;

        foreach (var tetrominoData in upcomingTetrominos.upcomingTetrominos)
        {
            DisplayTetromino(tetrominoData, index++);
        }
    }

    public void DisplayTetromino(TetrominoData tetrominoData, int index)
    {
        foreach (var cell in tetrominoData.cells)
        {
            Vector3Int tilePosition = new Vector3Int(cell.x + 8, cell.y - index * 3 + 5, 0); // Offset each Tetromino vertically
            upcomingDisplayTilemap.SetTile(tilePosition, tetrominoData.tile);
        }
    }
}
