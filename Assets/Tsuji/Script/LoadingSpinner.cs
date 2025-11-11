using UnityEngine;
using UnityEngine.UI;

public class LoadingSpinner : MonoBehaviour
{
    public static LoadingSpinner Instance { get; private set; }

    [SerializeField] private Image spinnerImage;
    [SerializeField] private float rotateSpeed = 200f;

    private bool spinning = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (spinning && spinnerImage != null)
        {
            spinnerImage.transform.Rotate(0, 0, -rotateSpeed * Time.deltaTime);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        spinning = true;
    }

    public void Hide()
    {
        spinning = false;
        gameObject.SetActive(false);
    }
}
