using UnityEngine;
using UnityEngine.UI;

public class player_mover : MoveLoop
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public barfill progressBar;
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        //Debug.Log($"Progress: {current_t_normalized}");
        progressBar.fill_val = current_t_normalized;
    }
}
