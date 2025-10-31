using UnityEngine;

public class obstacle : hitbox
{
    public float tvalue = 0;
    public Vector2 range_window = new Vector2(1.0f, 2.0f);//x represents the start range and y represents the end range
    public float despawn_range = 10.0f;
    public Vector2 pos = new Vector2(0, 0);
    public player_mover player;
    public Vector2 dim = new Vector2(1, 1);
    public override Vector2 position => pos;    
    public override Vector2 dimension => dim;
    public hitboxvisualizer hitboxvis;//will remove later
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {

        if (tvalue > player.current_t - range_window.x && tvalue < player.current_t + range_window.y)
        {
            base.FixedUpdate();
            active = true;
        }
        else if (tvalue <= player.current_t - despawn_range)
        {
            Debug.Log("Obstacle Despawned");
            hitboxvis.additionalhitboxes.Remove(hitboxvis.additionalhitboxes.Find(hb => hb.todraw == this));
            Destroy(this.gameObject);
        }
        else
        {
            active = false;
        }
        

    }
}
