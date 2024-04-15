using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isGamePaused = false;
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0;  // This freezes everything influenced by Time, e.g., animations, physics.
    }

    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1;  // Restore normal time flow.
    }
}