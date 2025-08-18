using UnityEngine;

public class MoveLoop : MonoBehaviour
{
    [Header("ˆÚ“®İ’è")]
    public float speed = 2.0f; // ˆÚ“®‘¬“x
    public Vector3 direction = new Vector3(0, 0, 1); // ‰œ(Z)•ûŒü‚É“®‚­


    void Update()
    {
        // ˆÚ“®ˆ—
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
