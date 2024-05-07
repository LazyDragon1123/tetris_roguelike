using UnityEngine;
using UnityEngine.Tilemaps;

public class HolderDisplay : MonoBehaviour
{
    public Tilemap holderTilemap;
    private TetrominoHolder tetrominoHolder;
    private bool isActivated = false;

    private void Awake()
    {
        isActivated = true;
    }

    public void Initialize(TetrominoHolder tetrominoHolder)
    {
        this.tetrominoHolder = tetrominoHolder;
    }

    // Call this method to update the display whenever Tetrominos are held or swapped
    public void UpdateDisplay()
    {
        // Clear the current display
        holderTilemap.ClearAllTiles();

        // Loop through the held Tetrominos and display them
        for (int i = 0; i < tetrominoHolder.heldTetrominos.Length; i++)
        {
            if (i < tetrominoHolder.holdCount)
            {
                DisplayTetromino(tetrominoHolder.heldTetrominos[i], i);
            }
        }
    }

    // Helper method to display a Tetromino at a specific index in the Tilemap
    private void DisplayTetromino(TetrominoData tetrominoData, int index)
    {
        foreach (var cell in tetrominoData.cells)
        {
            Vector3Int tilePosition = new Vector3Int(cell.x - 10, cell.y + 3 - index * 4, 0); // Offset each Tetromino vertically
            holderTilemap.SetTile(tilePosition, tetrominoData.tile);
        }
    }
    public void Disable()
    {
        isActivated = false;
    }
}
