using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PoseMenu : MonoBehaviour
{
    public Button[] buttons;
    private Outline[] outlines;
    private int currentIndex = 0;

    private bool canMove = true; // 移動可能フラグ

    void Start()
    {
        // Outline取得＆全部OFF
        outlines = new Outline[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            outlines[i] = buttons[i].GetComponent<Outline>();
            if (outlines[i] != null) outlines[i].enabled = false;
        }

        // 初期位置は一番最初のボタン
        currentIndex = 0;
        EventSystem.current.SetSelectedGameObject(buttons[currentIndex].gameObject);
        UpdateOutline();
    }

    void Update()
    {
        float v = Input.GetAxisRaw("Vertical");
        // スティック/キーが中立になったら再度移動可能
        if (Mathf.Abs(v) < 0.1f)
        {
            canMove = true;
        }

        if (canMove)
        {
            if (v > 0.5f)
            {
                MoveSelection(-1);
                canMove = false;
            }
            else if (v < -0.5f)
            {
                MoveSelection(1);
                canMove = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            buttons[currentIndex].onClick.Invoke();
        }
    }

    void MoveSelection(int direction)
    {
        currentIndex += direction;
        if (currentIndex < 0) currentIndex = buttons.Length - 1;
        if (currentIndex >= buttons.Length) currentIndex = 0;

        EventSystem.current.SetSelectedGameObject(buttons[currentIndex].gameObject);
        UpdateOutline();
    }

    void UpdateOutline()
    {
        for (int i = 0; i < outlines.Length; i++)
        {
            if (outlines[i] != null) outlines[i].enabled = (i == currentIndex);
        }
    }
}