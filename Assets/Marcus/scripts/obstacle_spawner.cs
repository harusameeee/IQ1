using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class obstacle_spawner : MonoBehaviour
{
    // -------- spawn timing --------
    public float spawnfrequency = 2.0f;          // obstacle spawn interval
    public float item_spawn_frequency = 5.0f;    // item spawn interval
    public float timer = 0.0f;
    public float item_timer = 0.0f;

    // -------- lane / offset --------
    public int maxoffset = 5;                     // max lane offset
    public float mindist = 5;                     // minimum distance between lanes
    public float offset_dist = 50;                // distance forward on spline

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

    float indicator_countdown = 7.0f;

    // ----------------------------------------
    void Awake()
    {
        damagables = new List<entity>();
        entity.onspawn += addtodamagables;
    }

    void Start()
    {
        // indicator pool
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

        if (timer > spawnfrequency)
        {
            spawnobstacle();
        }

        if (item_timer > item_spawn_frequency)
        {
            spawnitem();
        }
    }

    // ----------------------------------------
    void addtodamagables(entity d)
    {
        if (!damagables.Contains(d))
        {
            damagables.Add(d);
        }
    }

    // ----------------------------------------
    void spawnobstacle()
    {
        Random rng = new Random(System.DateTime.Now.Millisecond);
        timer = (float)rng.Next(-3, 3) / 10f;

        // find unused indicator
        indicator obstacleindicator = null;
        foreach (var ind in indicators)
        {
            if (!ind.gameObject.activeSelf)
            {
                obstacleindicator = ind;
                break;
            }
        }

        // decide lane offset
        bool validpos = false;
        int attempts = 0;
        float temp = 0;

        while (!validpos && attempts < 10)
        {
            rng = new Random(System.DateTime.Now.Millisecond + attempts);
            temp = rng.Next(-maxoffset, maxoffset);

            validpos = true;
            foreach (indicator ind in indicators)
            {
                if (ind.gameObject.activeSelf)
                {
                    if (Mathf.Abs(ind.offsetpos - temp) < mindist)
                    {
                        validpos = false;
                        break;
                    }
                }
            }
            attempts++;
        }

        // get spawn position on spline (t-driven)
        Vector4 spawnpos = player.getobstaclespawnpos(
            temp,
            offset_dist,
            out bool valid,
            out float new_t
        );

        if (!valid)
            return;

        // create obstacle
        obstacle obs = Instantiate(
            obstacles[rng.Next(0, obstacles.Count)]
        ).GetComponent<obstacle>();

        obs.player = player;
        obs.pos.x = temp;
        obs.tvalue = new_t;
        obs.reftransform = player.transform;
        obs.hitboxvis = hitboxvis;

        hitboxvis.additionalhitboxes.Add(
            new hitboxvisualizer.hitboxpair
            {
                todraw = obs,
                hbcolor = Color.red
            }
        );

        // indicator
        obstacleindicator.transform.localPosition =
            new Vector3(temp * 80f, 0f, 0f);

        obstacleindicator.gameObject.SetActive(true);
        obstacleindicator.setvalues(player, obs.transform, temp);

        // final transform
        obs.transform.position =
            new Vector3(spawnpos.x, spawnpos.y, spawnpos.z);

        obs.transform.rotation =
            Quaternion.Euler(0f, spawnpos.w, 0f);
    }

    // ----------------------------------------
    void spawnitem()
    {
        Random rng = new Random(System.DateTime.Now.Millisecond * 2);
        item_timer = (float)rng.Next(-3, 3) / 10f;

        indicator itemindicator = null;
        foreach (var ind in indicators)
        {
            if (!ind.gameObject.activeSelf)
            {
                itemindicator = ind;
                break;
            }
        }

        bool validpos = false;
        int attempts = 0;
        float temp = 0;

        while (!validpos && attempts < 10)
        {
            rng = new Random(System.DateTime.Now.Millisecond + attempts);
            temp = rng.Next(-maxoffset, maxoffset);

            validpos = true;
            foreach (indicator ind in indicators)
            {
                if (ind.gameObject.activeSelf)
                {
                    if (Mathf.Abs(ind.offsetpos - temp) < mindist)
                    {
                        validpos = false;
                        break;
                    }
                }
            }
            attempts++;
        }

        Vector4 spawnpos = player.getobstaclespawnpos(
            temp,
            offset_dist,
            out bool valid,
            out float new_t
        );

        if (!valid)
            return;

        obstacle obs = Instantiate(
            items[rng.Next(0, items.Count)]
        ).GetComponent<obstacle>();

        obs.player = player;
        obs.pos.x = temp;
        obs.tvalue = new_t;
        obs.reftransform = player.transform;
        obs.hitboxvis = hitboxvis;

        hitboxvis.additionalhitboxes.Add(
            new hitboxvisualizer.hitboxpair
            {
                todraw = obs,
                hbcolor = Color.yellow
            }
        );

        itemindicator.transform.localPosition =
            new Vector3(temp * 80f, 0f, 0f);

        itemindicator.gameObject.SetActive(true);
        itemindicator.setitem(obs.sprite);
        itemindicator.setvalues(player, obs.transform, temp);

        obs.transform.position =
            new Vector3(spawnpos.x, spawnpos.y, spawnpos.z);

        obs.transform.rotation = player.transform.rotation;
    }
}
