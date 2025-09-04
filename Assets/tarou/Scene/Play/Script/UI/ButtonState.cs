using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // シーン管理に必要

public class ButtonState : MonoBehaviour
{
    public Pose poseScript; // PoseスクリプトをInspectorでセット

    public void OnCloseMenuButton()
    {
        // PoseスクリプトのResumeGame()を呼び出す
        if (poseScript != null)
        {
            poseScript.ResumeGame();
        }
    }

    // プレイシーンを最初からやるコード
    public void OnRestartPlaySceneButton()
    {      
        if (poseScript != null)
        {
            poseScript.ResumeGame();
        }

        Time.timeScale = 1f;
        // 現在のシーンをリロード（最初からやり直す）
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 指定のシーンに飛ぶコード（名前指定）
    public void OnGoToSceneButton(string sceneName)
    {
        if (poseScript != null)
        {
            poseScript.ResumeGame();
        }

        Time.timeScale = 1f;
        // 引数で渡されたシーン名でシーンをロード
        SceneManager.LoadScene(sceneName);
    }

}
