using UnityEngine;
using UnityEngine.SceneManagement; // シーン切り替えに必要

public class Scenechange: MonoBehaviour
{
    // インスペクターで指定したいシーン名を入力できるようにする
    [SerializeField] private string sceneName = "NextScene";

    void Update()
    {
        // スペースキーが押されたらシーン遷移
        if (Input.GetKeyDown("joystick button 0"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
