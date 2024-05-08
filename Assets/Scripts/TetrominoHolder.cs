using UnityEngine;

public class TetrominoHolder : MonoBehaviour
{
    public TetrominoData[] heldTetrominos = new TetrominoData[1];
    public int holdCapacity;
    public int holdCount = 0;
    public bool canHold = true;
    public bool isActivated = true;

    private Board board;
    public HolderDisplay holderDisplay;
    public HolderGrid holderGrid;

    public void Initialize(Board board)
    {
        this.board = board;
        heldTetrominos = new TetrominoData[holdCapacity];
        holderDisplay = GetComponentInChildren<HolderDisplay>();
        holderDisplay.Initialize(this);
    }

    public void SwapOrHoldTetromino()
    {
        if (!isActivated) return;
        if (!canHold) return;

        if (board.activePiece == null)
        {
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
        holderGrid.Disable_1();
        holderGrid.Disable_2();
    }
    public void Enable()
    {
        isActivated = true;
        holderGrid.Enable_1();
        holderGrid.Enable_2();
    }
    public void IncreaseCapacity()
    {
        holdCapacity++;
        if (holdCapacity == 1)
        {
            holderGrid.Enable_1();
            holderGrid.Disable_2();
        } else if (holdCapacity == 2)
        {   
            holderGrid.Enable_1();
            holderGrid.Enable_2();
        } else {
            return;
        }


        TetrominoData[] temp = (TetrominoData[])heldTetrominos.Clone();
        heldTetrominos = new TetrominoData[holdCapacity];
        for (int i = 0; i < holdCount; i++)
        {
            heldTetrominos[i] = temp[i];
        }
        holderDisplay.UpdateDisplay();

    }
}
