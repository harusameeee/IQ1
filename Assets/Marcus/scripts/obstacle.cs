using UnityEngine;

public class obstacle : hitbox
{
    // -------- spline --------
    public float positionT;   // 見た目の位置（Spline）
    public float spawnT;      // 出現タイミング（判定用）

    // -------- visuals --------
    public Sprite sprite;

    // -------- activation --------
    public Vector2 range_window = new Vector2(1.0f, 2.0f);
    public float despawn_range = 10.0f;

    // -------- lifetime --------
    public float lifeTime = 8.0f;
    float lifeTimer = 0f;

    // -------- hitbox --------
    public Vector2 pos = Vector2.zero;
    public Vector2 dim = new Vector2(1, 1);

    // -------- refs --------
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

        // ---- 出現タイミング判定 ----
        float dt = spawnT - player.current_t_normalized;
        if (dt < 0f) dt += 1f;

        float dist = dt * splineLength;

        // ---- activation ----
        if (dist >= -range_window.x && dist <= range_window.y)
        {
            active = true;
            base.FixedUpdate();
        }
        else
        {
            active = false;
        }

        // ---- despawn ----
        if (dist < -despawn_range || lifeTimer > lifeTime)
        {
            hitboxvis.additionalhitboxes.Remove(
                hitboxvis.additionalhitboxes.Find(hb => hb.todraw == this)
            );

            Destroy(gameObject);
        }
    }
}
