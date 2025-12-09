using UnityEngine;
using UnityEngine.UI;

public class SliderControllerUI : MonoBehaviour
{
    [SerializeField] private Slider[] sliders;
    [SerializeField] private float sliderMoveSpeed = 0.5f;

    [SerializeField] private Button closeButton;

    [SerializeField] private Image[] submitImages;

    private int currentIndex = 0;
    private bool isAdjusting = false;
    private float inputCooldown = 0.2f;
    private float lastInputTime = 0f;

    void Start()
    {
        HighlightSlider(currentIndex);

        // Submit画像を全て半透明で初期化
        for (int i = 0; i < submitImages.Length; i++)
        {
            SetSubmitAlpha(submitImages[i], 0.1f);
        }
        // 選択中だけ半透明表示（既に半透明なのでOK）
        SetSubmitAlpha(submitImages[currentIndex], 0.6f);
    }

    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        if (!isAdjusting)
        {
            HandleSliderSelection(vertical);

            // Aボタンで調整モードへ
            if (Input.GetButtonDown("Submit"))
            {
                isAdjusting = true;
                // 完全表示
                SetSubmitAlpha(submitImages[currentIndex], 1f);
            }

            if (Input.GetButtonDown("Cancel"))
            {
                closeButton.onClick.Invoke();
            }
        }
        else
        {
            // 左右で値調整
            HandleSliderAdjustment(horizontal);

            if (Input.GetButtonDown("Cancel"))
            {
                isAdjusting = false;
                // 半透明に戻す
                SetSubmitAlpha(submitImages[currentIndex], 0.6f);
            }
        }
    }

    void HandleSliderSelection(float vertical)
    {
        if (Time.time - lastInputTime < inputCooldown) return;

        int oldIndex = currentIndex;

        if (vertical > 0.5f)
        {
            currentIndex = Mathf.Max(currentIndex - 1, 0);
        }
        else if (vertical < -0.5f)
        {
            currentIndex = Mathf.Min(currentIndex + 1, sliders.Length - 1);
        }

        if (oldIndex != currentIndex)
        {
            UpdateSelection(oldIndex);
        }
    }

    void HandleSliderAdjustment(float horizontal)
    {
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            sliders[currentIndex].value += horizontal * sliderMoveSpeed * Time.deltaTime;
        }
    }

    void UpdateSelection(int oldIndex)
    {
        lastInputTime = Time.time;

        // 前選択を半透明に戻す
        SetSubmitAlpha(submitImages[oldIndex], 0.1f);

        // 新選択を少し明るめに
        SetSubmitAlpha(submitImages[currentIndex], isAdjusting ? 1f : 0.6f);

        HighlightSlider(currentIndex);
    }

    void HighlightSlider(int index)
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            ColorBlock colors = sliders[i].colors;

            if (i == index)
            {
                colors.normalColor = isAdjusting ? Color.red : Color.yellow;
            }
            else
            {
                colors.normalColor = Color.white;
            }
            sliders[i].colors = colors;
        }
    }

    void SetSubmitAlpha(Image img, float alpha)
    {
        var c = img.color;
        c.a = alpha;
        img.color = c;
    }
}
