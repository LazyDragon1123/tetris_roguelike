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

        public GameOption(string text, UnityAction act)
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
    }
    public void Initialize(Board _board, Ghost _ghost)
    {

        // // Subscribe button click events
        // option1Button.onClick.AddListener(Option1);
        // option2Button.onClick.AddListener(Option2);
        // option3Button.onClick.AddListener(Option3);
        board = _board;
        ghost = _ghost;
    }

    public void InitializeOptions()
    {
        SetGameOptions();
        SetupOptionsUI();
    }

    private void SetGameOptions()
    {
        availableOptions.Clear();
        availableOptions.Add(new GameOption("Increase Speed", IncreaseFallingSpeed));
        availableOptions.Add(new GameOption("Clear Ghost", ClearAllGhosts));
        availableOptions.Add(new GameOption("Activate Power", ActivatePower));
    }

    private void SetupOptionsUI()
    {
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
        Debug.Log("Increasing falling speed");
    }

    private void ClearAllGhosts()
    {
        Debug.Log("Clearing all ghosts");
    }

    private void ActivatePower()
    {
        Debug.Log("Activating power");
    }
}
