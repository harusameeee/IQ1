using UnityEngine;

public class TriggerResetPosition : MonoBehaviour
{
    [Header("ワープさせたい位置")]
    public Vector3 resetPosition;

    private void OnTriggerEnter(Collider other)
    {
        // 例："Movable"タグのオブジェクトだけを対象にする
        if (other.CompareTag("MoveOb"))
        {
            other.transform.position = resetPosition;
        }
    }
}
