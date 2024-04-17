using UnityEngine;
using UnityEngine.UI;  // Needed for UI components

public class GameModifier : MonoBehaviour
{
    public GameObject optionsPanel;
    public Button option1Button;
    public Button option2Button;
    public Button option3Button;
    private GameManager gameManager;
    private Board board;
    private Ghost ghost;

    private void Awake()
    {
        optionsPanel.SetActive(false);
    }
    public void Initialize(Board _board, Ghost _ghost)
    {

        // Subscribe button click events
        option1Button.onClick.AddListener(Option1);
        option2Button.onClick.AddListener(Option2);
        option3Button.onClick.AddListener(Option3);
        board = _board;
        ghost = _ghost;
    }

    public void ShowOptions()
    {   
        GameManager.Instance.PauseGame();
        optionsPanel.SetActive(true);
    }

    private void Option1()
    {
        // Implement the logic to increase falling speed
        Debug.Log("Increased falling speed!");
        board.IncreaseFallingSpeed();
        ClosePanel();
    }

    private void Option2()
    {
        // Implement option 2 logic
        Debug.Log("Option 2 selected!");
        ghost.ClearAllTiles();
        ghost.SetActive(false);
        ClosePanel();
    }

    private void Option3()
    {
        // Implement option 3 logic
        Debug.Log("Option 3 selected!");
        ClosePanel();
    }

    private void ClosePanel()
    {
        optionsPanel.SetActive(false);
        GameManager.Instance.ResumeGame();
    }
}
