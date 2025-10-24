using UnityEngine;

public class obstacle : hitbox
{
    public float tvalue = 0;
    public float range_window = 5.0f;
    public float despawn_range = 10.0f;
    public Vector2 pos = new Vector2(0, 0);
    public bool active = false;
    public player_mover player;
    public Vector2 dim = new Vector2(1, 1);
    public override Vector2 position => transform.localPosition-reftransform.localPosition;
    public override Vector2 dimension => dim;
    public hitboxvisualizer hitboxvis;//will remove later
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {

        if (tvalue > player.current_t - range_window && tvalue < player.current_t + range_window)
        {
            Debug.Log("Obstacle Active");
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
