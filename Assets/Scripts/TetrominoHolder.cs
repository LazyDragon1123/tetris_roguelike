using UnityEngine;

public class TetrominoHolder : MonoBehaviour
{
    public TetrominoData[] heldTetrominos = new TetrominoData[1];
    public int holdCapacity = 1;
    public int holdCount = 0;
    public bool canHold = true;

    private Board board;

    public void Initialize(Board board)
    {
        this.board = board;
    }

    public void SwapOrHoldTetromino()
    {
        if (!canHold) return;

        if (board.activePiece == null)
        {
            Debug.LogError("No active piece to hold or swap.");
            return;
        }


        if (holdCount < holdCapacity)
        {
            // Hold the current piece if there's room
            Debug.Log("Holding piece");
            heldTetrominos[holdCount++] = board.activePiece.data;
            board.Clear(board.activePiece);
            board.SpawnPiece();
        } 
        else if (holdCount > 0)
        {
            // Swap the current piece with the first held piece
            Debug.Log("Swapping piece");
            TetrominoData temp = heldTetrominos[0];
            for (int i = 0; i < holdCount - 1; i++)
            {
                heldTetrominos[i] = heldTetrominos[i + 1];
            }
            heldTetrominos[holdCount - 1] = board.activePiece.data;
            board.Clear(board.activePiece);
            board.SetPiece(temp);
        }

        canHold = false;  // Player must make a move before holding again
    }

    public void ResetCanHold()
    {
        canHold = true;
    }
}
