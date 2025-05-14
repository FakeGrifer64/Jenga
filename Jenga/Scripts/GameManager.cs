using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject gameOverCanvas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (gameOverCanvas != null)
                gameOverCanvas.SetActive(false);
        }
    }

    public static void GameOver()
    {
        Time.timeScale = 0f;
        if (Instance.gameOverCanvas != null)
            Instance.gameOverCanvas.SetActive(true);
        else
            Debug.LogError("GameOverCanvas não atribuído no GameManager");
    }
}