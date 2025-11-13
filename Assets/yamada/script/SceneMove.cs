using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
            FadeManager.Instance.LoadScene(1);
            Debug.Log("ÉVÅ[ÉìïœÇÌÇ¡ÇΩÇÊ,");
        }
    }
}
