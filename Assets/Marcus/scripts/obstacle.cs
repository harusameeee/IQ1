using UnityEngine;

public class obstacle : hitbox
{
    public float tvalue = 0f;

    public Sprite sprite;

    public Vector2 range_window = new Vector2(1.0f, 2.0f);
    public float despawn_range = 10.0f;

    public float lifeTime = 8.0f;   
    float lifeTimer = 0f;

    public Vector2 pos = Vector2.zero;
    public Vector2 dim = new Vector2(1, 1);

    public player_mover player;
    public hitboxvisualizer hitboxvis;

    public override Vector2 position => pos;
    public override Vector2 dimension => dim;

    void Start()
    {
        active = false;
    }

    public override void FixedUpdate()
    {
        lifeTimer += Time.fixedDeltaTime;

        float splineLength = player.splinecont.Spline.GetLength();

        float dt = tvalue - player.current_t_normalized;
        if (dt < 0f) dt += 1f;

        float dist = dt * splineLength;

        // activation window
        if (dist >= -range_window.x && dist <= range_window.y)
        {
            active = true;
            base.FixedUpdate();
        }
        else
        {
            active = false;
        }

        // despawn by distance OR time
        if (dist < -despawn_range || lifeTimer > lifeTime)
        {
            hitboxvis.additionalhitboxes.Remove(
                hitboxvis.additionalhitboxes.Find(hb => hb.todraw == this)
            );

            Destroy(gameObject);
        }
    }
}
