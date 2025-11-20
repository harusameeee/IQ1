using UnityEngine;
using UnityEngine.UI;

public class SliderControllerUI : MonoBehaviour
{
    [SerializeField] private Slider[] sliders;  // 操作したいスライダーをInspectorで登録
    [SerializeField] private float sliderMoveSpeed = 0.5f; // スライダー値の動く速さ

    [SerializeField] private Button closeButton;

    [SerializeField] private Image[] submitImages;

    private int currentIndex = 0;     // 現在選択中のスライダー番号
    private bool isAdjusting = false; // スライダー操作中かどうか
    private float inputCooldown = 0.2f;
    private float lastInputTime = 0f;

    void Start()
    {
        HighlightSlider(currentIndex);

        for (int i = 0; i < submitImages.Length; i++) 
        {
            submitImages[i].enabled = false;
        }
    }

    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        if (!isAdjusting)
        {
            HandleSliderSelection(vertical);

            // Aボタン（Submit）でスライダー調整モードに入る
            if (Input.GetButtonDown("Submit"))
            {
                isAdjusting = true;
                submitImages[currentIndex].enabled = true;
            }

            if (Input.GetButtonDown("Cancel"))
            {
                closeButton.onClick.Invoke();
            }
        }
        else
        {
            // 左右でスライダー値を変更
            HandleSliderAdjustment(horizontal);

            // Bボタン（Cancel）で調整モードを抜ける
            if (Input.GetButtonDown("Cancel"))
            {
                isAdjusting = false;
                submitImages[currentIndex].enabled = false;

            }
        }
    }

    // スライダーを上下で選択
    void HandleSliderSelection(float vertical)
    {
        if (Time.time - lastInputTime < inputCooldown) return;

        if (vertical > 0.5f)
        {
            currentIndex = Mathf.Max(currentIndex - 1, 0);
            UpdateSelection();
        }
        else if (vertical < -0.5f)
        {
            currentIndex = Mathf.Min(currentIndex + 1, sliders.Length - 1);
            UpdateSelection();
        }
    }

    // スライダー値を左右で操作
    void HandleSliderAdjustment(float horizontal)
    {
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            sliders[currentIndex].value += horizontal * sliderMoveSpeed * Time.deltaTime;
        }
    }

    // 選択更新
    void UpdateSelection()
    {
        lastInputTime = Time.time;
        HighlightSlider(currentIndex);
    }

    // 選択中スライダーを強調表示
    void HighlightSlider(int index)
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            var colors = sliders[i].colors;
            colors.normalColor = (i == index) ? Color.yellow : Color.white;
            if (isAdjusting) { colors.selectedColor = (i == index) ? Color.red : Color.yellow; }
            sliders[i].colors = colors;
        }
    }
}
