using UnityEngine;
using UnityEngine.SceneManagement; // シーン遷移用

public class TitleSceneController : MonoBehaviour
{
    [SerializeField] private GameObject firstCanvas;   // 最初のCanvas
    [SerializeField] private GameObject secondCanvas;  // 2つ目のCanvas

    void Start()
    {
        // 最初の状態
        firstCanvas.SetActive(true);
        secondCanvas.SetActive(false);
    }

    void Update()
    {
        // スペースキーで画面切り替え
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowSecondCanvas();
        }
    }

    // --- 切り替え処理 ---
    public void ShowFirstCanvas()
    {
        firstCanvas.SetActive(true);
        secondCanvas.SetActive(false);
    }

    public void ShowSecondCanvas()
    {
        firstCanvas.SetActive(false);
        secondCanvas.SetActive(true);
    }

    // --- シーン遷移処理 ---
    public void LoadNextScene(string sceneName)
    {
        SceneManager.LoadScene("Stage1");
    }
}
