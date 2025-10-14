using UnityEngine;
using UnityEngine.UI;

public class witch_mover : MoveLoop
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public Slider progressBar;
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        progressBar.value = current_t;
        
    }
}
