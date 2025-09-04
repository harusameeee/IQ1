using UnityEngine;

public class Pose : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas; // ポーズ用Canvas

    private bool isPaused = false;

    void Update()
    {
        // コントローラーの「Start」ボタン（例: JoystickButton7）
        if (Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f; // ゲーム停止
        pauseCanvas.SetActive(true); // ポーズ画面表示
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // ゲーム再開
        pauseCanvas.SetActive(false); // ポーズ画面非表示
        isPaused = false;
    }
}