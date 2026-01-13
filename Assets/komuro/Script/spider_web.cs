using UnityEngine;

public class spider_web : MonoBehaviour
{
    [SerializeField] Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("PlayAnim", 2f);
    }

    void PlayAnim()
    {
        animator.Play("spider_web");
    }
}
