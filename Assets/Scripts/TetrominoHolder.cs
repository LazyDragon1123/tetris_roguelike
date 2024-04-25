using UnityEngine;

public class TetrominoHolder : MonoBehaviour
{
    public TetrominoData[] heldTetrominos = new TetrominoData[1];
    public int holdCapacity = 1;
    public int holdCount = 0;
    public bool canHold = true;
    public bool isActivated = true;

    private Board board;
    public HolderDisplay holderDisplay;

    public void Initialize(Board board)
    {
        this.board = board;

        holderDisplay = GetComponentInChildren<HolderDisplay>();
        holderDisplay.Initialize(this);
    }

    public void SwapOrHoldTetromino()
    {
        if (!isActivated) return;
        if (!canHold) return;

        if (board.activePiece == null)
        {
            Debug.LogError("No active piece to hold or swap.");
            return;
        }


        if (holdCount < holdCapacity)
        {
            heldTetrominos[holdCount++] = board.activePiece.data;
            board.Clear(board.activePiece);
            board.SpawnPiece();
        } 
        else if (holdCount > 0)
        {
            TetrominoData temp = heldTetrominos[0];
            for (int i = 0; i < holdCount - 1; i++)
            {
                heldTetrominos[i] = heldTetrominos[i + 1];
            }
            heldTetrominos[holdCount - 1] = board.activePiece.data;
            board.Clear(board.activePiece);
            board.SetPiece(temp);
        }
        holderDisplay.UpdateDisplay();
        canHold = false;  // Player must make a move before holding again
    }

    public void ResetCanHold()
    {
        canHold = true;
    }
    public void Disable()
    {
        isActivated = false;
    }
}
