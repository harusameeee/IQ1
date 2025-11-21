using UnityEngine;

public class PlaySoundEffects : MonoBehaviour
{
    //public static PlaySoundEffects Instance { get; private set; }

    [Header("SEまとめ")]
    [NamedArray(new string[] { "決定", "開く", "カーソル移動" ,"キャンセル"})]
    [SerializeField] private AudioClip[] audioSource=new AudioClip[4];

    public enum Operation { Submit, PanelOpen, PanelClose, Selecting, Cancel }

    private void Start()
    {
        //if (Instance != null && Instance != this)
        //{
        //    Destroy(gameObject);
        //    return;
        //}
        //Instance = this;
    }

    public void PlaySE(Operation operation)
    {
        
        switch (operation)
        {
            case Operation.Submit:
                SoundManager.Instance.PlaySFX(audioSource[0]);
                break;

            case Operation.PanelOpen:
                SoundManager.Instance.PlaySFX(audioSource[1]);
                break;

            case Operation.PanelClose:
            case Operation.Cancel:
                SoundManager.Instance.PlaySFX(audioSource[3]);
                break;

            case Operation.Selecting:
                SoundManager.Instance.PlaySFX(audioSource[2]);
                break;
        }

    }
}
