using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = System.Random;
public class obstacle_spawner : MonoBehaviour
{
    public float spawnfrequency = 2.0f;
    public float timer = 0.0f;
    public int maxoffset = 5;
    public float offset_dist = 50;
    public Transform indicator_transform;
    public hitboxvisualizer hitboxvis;//will remove later
    public List<GameObject> obstacles = new List<GameObject>();
    public List<indicator> indicators = new List<indicator>();

    public List<entity> damagables = new List<entity>();
    public List<Sprite> obstacle_sprites = new List<Sprite>();
    public player_mover player;
    float indicator_countdown = 7.0f;
    //note need to allow for making double wide obstacles
    //need to make indicator for where obstacle will spawn
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        entity.onspawn += addtodamagables;
        for (int i = 0; i < 10; i++)
        {
            var indicatorobj = Instantiate(Resources.Load<GameObject>("indicator"), indicator_transform);
            indicatorobj.SetActive(false);
            indicators.Add(indicatorobj.GetComponent<indicator>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > spawnfrequency)
        {

            Random rng = new Random(System.DateTime.Now.Millisecond);
            timer = (float)rng.Next(-3, 3) / 10;
            indicator obstacleindicator = null;
            foreach (var ind in indicators)
            {
                if (!ind.gameObject.activeSelf)
                {
                    obstacleindicator = ind;
                    break;
                }
            }
            rng = new Random(System.DateTime.Now.Millisecond + 1);
            float temp = (float)rng.Next(-maxoffset, maxoffset);


            Vector3 spawnpos = player.getobstaclespawnpos(temp, offset_dist, out bool valid, out float new_t);
            if (!valid)
            {

                return;
            }
            obstacle obs = Instantiate(obstacles[rng.Next(0, obstacles.Count)]).GetComponent<obstacle>();
            obs.player = player;
            obs.pos.x = temp;
            hitboxvis.additionalhitboxes.Add(new hitboxvisualizer.hitboxpair { todraw = obs, hbcolor = Color.red });
            obs.tvalue = new_t;
            obs.damagables = damagables;
            obs.hitboxvis = hitboxvis;
            obs.reftransform = player.transform;
            obstacleindicator.transform.localPosition = new Vector3(temp * 100, 0, 0);
            obstacleindicator.gameObject.SetActive(true);
            obs.transform.position = spawnpos;
            obstacleindicator.setvalues(player, obs.transform);
            //var obstacle = Instantiate(obstacles[rng.Next(0, obstacles.Count)]);
            //obstacle.transform.position = spawnpos;
        }
    }
    void addtodamagables(entity d)
    {
        Debug.Log($"Hitbox registering {d.name} to damagables");
        if (!damagables.Contains(d))
        {
            damagables.Add(d);
        }
    }

}
