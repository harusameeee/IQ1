using System.Collections.Generic;
using UnityEngine;

public class obstacle_spawner : MonoBehaviour
{
    // -------- spawn timing --------
    public float spawnfrequency = 2.0f;
    public float item_spawn_frequency = 5.0f;
    public float timer = 0.0f;
    public float item_timer = 0.0f;

    // ★★★ 出るタイミングのランダム幅（Inspectorで調整） ★★★
    [Header("Spawn Timing Offset (T Value)")]
    [Tooltip("x = 最速, y = 最遅 / 0.1 = スプライン全長の10%")]
    public Vector2 spawnOffsetRange = new Vector2(0.05f, 0.15f);

    [Header("Obstacle Spawn Interval")]
    public float spawnIntervalMin = 1f;
    public float spawnIntervalMax = 3f;

    float nextSpawnTime;

    // -------- lane / offset --------
    public int maxoffset = 5;
    public float mindist = 5;
    public float offset_dist = 50;

    // -------- references --------
    public Transform indicator_transform;
    public hitboxvisualizer hitboxvis;
    public player_mover player;

    // -------- pools --------
    public List<GameObject> obstacles = new List<GameObject>();
    public List<GameObject> items = new List<GameObject>();
    public List<indicator> indicators = new List<indicator>();

    // -------- entities --------
    public static List<entity> damagables = new List<entity>();

    void Awake()
    {
        damagables = new List<entity>();
        entity.onspawn += addtodamagables;
    }

    void Start()
    {
        nextSpawnTime = Random.Range(spawnIntervalMin, spawnIntervalMax);

        for (int i = 0; i < 30; i++)
        {
            var obj = Instantiate(
                Resources.Load<GameObject>("indicator"),
                indicator_transform
            );

            obj.SetActive(false);
            indicators.Add(obj.GetComponent<indicator>());
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        item_timer += Time.deltaTime;

        if (timer >= nextSpawnTime)
        {
            spawnobstacle();

            timer = 0f;
            nextSpawnTime = Random.Range(spawnIntervalMin, spawnIntervalMax);
        }

        if (item_timer > item_spawn_frequency)
            spawnitem();
    }

    void addtodamagables(entity d)
    {
        if (!damagables.Contains(d))
            damagables.Add(d);
    }

    // ----------------------------------------
    void spawnobstacle()
    {
        timer = 0f;

        indicator obstacleindicator = getFreeIndicator();
        if (obstacleindicator == null) return;

        float lane = getValidLane();

        Vector4 spawnpos = player.getobstaclespawnpos(
            lane,
            offset_dist,
            out bool valid,
            out float newT
        );

        if (!valid) return;

        obstacle obs = Instantiate(
            obstacles[UnityEngine.Random.Range(0, obstacles.Count)]
        ).GetComponent<obstacle>();

        obs.player = player;
        obs.pos.x = lane;

        // ===== 出るタイミングを Inspector で制御 =====
        obs.positionT = newT;

        float spawnOffset = UnityEngine.Random.Range(
            spawnOffsetRange.x,
            spawnOffsetRange.y
        );

        obs.spawnT = newT + spawnOffset;

        if (obs.spawnT < 0f) obs.spawnT += 1f;
        if (obs.spawnT > 1f) obs.spawnT -= 1f;
        // ===========================================

        obs.hitboxvis = hitboxvis;

        hitboxvis.additionalhitboxes.Add(
            new hitboxvisualizer.hitboxpair
            {
                todraw = obs,
                hbcolor = Color.red
            }
        );

        obstacleindicator.transform.localPosition =
            new Vector3(lane * 80f, 0f, 0f);

        obstacleindicator.gameObject.SetActive(true);
        obstacleindicator.setvalues(player, obs.transform, lane);

        obs.transform.position =
            new Vector3(spawnpos.x, spawnpos.y, spawnpos.z);

        obs.transform.rotation =
            Quaternion.Euler(0f, spawnpos.w, 0f);
    }

    // ----------------------------------------
    void spawnitem()
    {
        item_timer = 0f;

        indicator itemindicator = getFreeIndicator();
        if (itemindicator == null) return;

        float lane = getValidLane();

        Vector4 spawnpos = player.getobstaclespawnpos(
            lane,
            offset_dist,
            out bool valid,
            out float newT
        );

        if (!valid) return;

        obstacle obs = Instantiate(
            items[UnityEngine.Random.Range(0, items.Count)]
        ).GetComponent<obstacle>();

        obs.player = player;
        obs.pos.x = lane;

        obs.positionT = newT;

        float spawnOffset = UnityEngine.Random.Range(
            spawnOffsetRange.x,
            spawnOffsetRange.y
        );

        obs.spawnT = newT + spawnOffset;

        if (obs.spawnT < 0f) obs.spawnT += 1f;
        if (obs.spawnT > 1f) obs.spawnT -= 1f;

        obs.hitboxvis = hitboxvis;

        hitboxvis.additionalhitboxes.Add(
            new hitboxvisualizer.hitboxpair
            {
                todraw = obs,
                hbcolor = Color.yellow
            }
        );

        itemindicator.transform.localPosition =
            new Vector3(lane * 80f, 0f, 0f);

        itemindicator.gameObject.SetActive(true);
        itemindicator.setitem(obs.sprite);
        itemindicator.setvalues(player, obs.transform, lane);

        obs.transform.position =
            new Vector3(spawnpos.x, spawnpos.y, spawnpos.z);

        obs.transform.rotation = player.transform.rotation;
    }

    // -------- helpers --------
    indicator getFreeIndicator()
    {
        foreach (var ind in indicators)
            if (!ind.gameObject.activeSelf)
                return ind;
        return null;
    }

    float getValidLane()
    {
        float lane = 0f;
        bool valid = false;
        int attempts = 0;

        while (!valid && attempts < 10)
        {
            lane = UnityEngine.Random.Range(-maxoffset, maxoffset);
            valid = true;

            foreach (indicator ind in indicators)
            {
                if (ind.gameObject.activeSelf &&
                    Mathf.Abs(ind.offsetpos - lane) < mindist)
                {
                    valid = false;
                    break;
                }
            }
            attempts++;
        }
        return lane;
    }
}
