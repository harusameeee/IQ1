using UnityEngine;

public class MagicTrigger : MonoBehaviour
{
    [Range(0f, 1f)]
    public float triggerPoint; // MoveLoop.current_t_normalizedのどのタイミングで発動するか
    public GameObject magicPrefab; // このトリガーで出す魔法（種類を変えてもOK）

    [HideInInspector] public bool triggered = false;
}
