using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;
using TMPro;
using System;

public class GameModifier : MonoBehaviour
{
    [System.Serializable]  // Mark the nested class as Serializable to expose it in the Unity editor.
    public class GameOption
    {
        public string buttonText;
        public UnityAction action;

        public GameOption(string text, UnityAction act)  // TODO: add weight to the options
        {
            buttonText = text;
            action = act;
        }
    }
    public List<GameOption> availableOptions = new List<GameOption>();
    public GameObject optionsPanel;
    public Button[] optionButtons;
    private GameManager gameManager;
    private Board board;
    private Ghost ghost;

    private void Awake()
    {
        optionsPanel.SetActive(false);
        SetGameOptions();
    }
    public void Initialize(Board _board, Ghost _ghost)
    {

        board = _board;
        ghost = _ghost;
    }


    private void SetGameOptions()
    {
        availableOptions.Clear();
        availableOptions.Add(new GameOption("Increase Speed", IncreaseFallingSpeed));
        availableOptions.Add(new GameOption("Clear Ghost", ClearAllGhosts));
        availableOptions.Add(new GameOption("Score +1000", ScoreSmall));
        availableOptions.Add(new GameOption("Score +5000", ScoreMedium));
        availableOptions.Add(new GameOption("Score +10000", ScoreLarge));
        availableOptions.Add(new GameOption("Spawn Rate Up", SpawnRateUp));
        availableOptions.Add(new GameOption("Increase Hold Capacity", IncreaseHoldCapacity));
    }


    private void ShuffleOptions<T>(List<T> list)
    {
        System.Random rng = new System.Random();  // You can also use UnityEngine.Random if you prefer
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    private void SetupOptionsUI()
    {
        ShuffleOptions(availableOptions);
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < availableOptions.Count)
            {
                Debug.Log("Setting up option " + i);
                optionButtons[i].GetComponentInChildren<TMP_Text>().text = availableOptions[i].buttonText;
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(availableOptions[i].action);
                optionButtons[i].onClick.AddListener(ClosePanel);
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void ShowOptions()
    {   
        SetupOptionsUI();
        GameManager.Instance.PauseGame();
        optionsPanel.SetActive(true);
    }

    private void ClosePanel()
    {
        optionsPanel.SetActive(false);
        GameManager.Instance.ResumeGame();
    }

    private void IncreaseFallingSpeed()
    {
        board.IncreaseFallingSpeed();
    }

    private void ClearAllGhosts()
    {
        ghost.ClearAllTiles();
        ghost.SetActive(false);
    }


    private void ScoreSmall()
    {
        board.Score += 1000;
        board.scoreBoard.UpdateScore(board.Score);
    }

    private void ScoreMedium()
    {
        board.Score += 5000;
        board.scoreBoard.UpdateScore(board.Score);
    }

    private void ScoreLarge()
    {
        board.Score += 10000;
        board.scoreBoard.UpdateScore(board.Score);
    }

    private void SpawnRateUp()
    {
        board.activePiece.probSpecial = Mathf.Min(1.0f, board.activePiece.probSpecial + 0.1f);
    }
    private void IncreaseHoldCapacity()
    {
        board.IncreaseHoldCapacity();
    }

}
