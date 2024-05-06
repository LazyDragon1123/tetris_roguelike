using System.Collections.Generic;
using UnityEngine;

public class UpcomingTetrominos : MonoBehaviour
{
    public Queue<TetrominoData> upcomingTetrominos = new Queue<TetrominoData>();
    public int previewCount = 2;
    private Board board;

    private UpcomingTetrominosDisplay upcomingTetrominosDisplay;

    public void Initialize(Board board)
    {
        this.board = board;
        upcomingTetrominos = new Queue<TetrominoData>();
        upcomingTetrominosDisplay = GetComponentInChildren<UpcomingTetrominosDisplay>();
        upcomingTetrominosDisplay.Initialize(this);
    }
    public void AddNewTetromino(TetrominoData tetrominoData)
    {
        if (upcomingTetrominos.Count >= previewCount)
        {
            upcomingTetrominos.Dequeue(); // Remove the oldest if exceeding the preview count
        }
        upcomingTetrominos.Enqueue(tetrominoData);
        upcomingTetrominosDisplay.UpdateDisplay();
    }

    // Method to get the next Tetromino and remove it from the queue
    public TetrominoData GetNextTetromino()
    {
        if (upcomingTetrominos.Count <= 0)
        {
            Debug.Log("No upcoming Tetrominos");
        }
        TetrominoData nextTetromino = upcomingTetrominos.Dequeue();
        upcomingTetrominosDisplay.UpdateDisplay();
        return nextTetromino;
    }   
}
