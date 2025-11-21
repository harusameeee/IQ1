using UnityEngine;
using UnityEngine.EventSystems;

public class UISelectSound : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;   // ← PlaySE を使うやつ
    private GameObject lastSelected;

    void Start()
    {
        // 最初に選択されている UI を記録しておく
        lastSelected = EventSystem.current.currentSelectedGameObject;
    }

    void Update()
    {
        // 現在選択されている UI
        var current = EventSystem.current.currentSelectedGameObject;

        // 選択が変わった時だけ SE 再生
        if (current != lastSelected)
        {
            if (current != null)
            {
                SoundManager.Instance.PlaySFX(audioClip);
            }

            lastSelected = current;
        }
    }
}
