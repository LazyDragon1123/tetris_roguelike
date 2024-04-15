using UnityEngine;
using UnityEngine.UI;  // Needed for UI components

public class GameModifier : MonoBehaviour
{
    public GameObject optionsPanel;
    public Button increaseSpeedButton;
    public Button option2Button;
    public Button option3Button;
    private GameManager gameManager;

    private void Awake()
    {
        optionsPanel.SetActive(false);
    }
    public void Initialize()
    {

        // Subscribe button click events
        increaseSpeedButton.onClick.AddListener(IncreaseFallingSpeed);
        option2Button.onClick.AddListener(Option2);
        option3Button.onClick.AddListener(Option3);
    }

    public void ShowOptions()
    {   
        GameManager.Instance.PauseGame();
        optionsPanel.SetActive(true);
    }

    private void IncreaseFallingSpeed()
    {
        // Implement the logic to increase falling speed
        Debug.Log("Increased falling speed!");
        ClosePanel();
    }

    private void Option2()
    {
        // Implement option 2 logic
        Debug.Log("Option 2 selected!");
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
