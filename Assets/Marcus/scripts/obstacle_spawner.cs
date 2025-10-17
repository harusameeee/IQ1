using System.Collections.Generic;
using UnityEngine;

using Random = System.Random;
public class obstacle_spawner : MonoBehaviour
{
    public float spawnfrequency = 2.0f;
    public float timer = 0.0f;
    public int maxoffset = 5;
    public float offset_dist = 50;
    public Transform indicator_transform;
    public List<GameObject> obstacles = new List<GameObject>();
    public List<indicator> indicators = new List<indicator>();
    public player_mover player;
    float indicator_countdown = 7.0f;
    //note need to allow for making double wide obstacles
    //need to make indicator for where obstacle will spawn
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
            timer = 0.0f;
            indicator obstacleindicator = null;
            foreach (var ind in indicators)
            {
                    if (!ind.gameObject.activeSelf)
                {
                    obstacleindicator = ind;
                    break;
                }
            }
            Random rng = new Random(System.DateTime.Now.Millisecond);
            float temp = (float)rng.Next(-maxoffset, maxoffset);


            var obstacle = Instantiate(obstacles[rng.Next(0, obstacles.Count)]);
            Vector3 spawnpos = player.getobstaclespawnpos(temp, offset_dist, out bool valid);
            if (!valid)
            {
                obstacleindicator.gameObject.SetActive(false);
                Destroy(obstacle);
                return;
            }
            obstacleindicator.transform.localPosition = new Vector3(temp*150,0, 0);
            obstacleindicator.gameObject.SetActive(true);
            obstacle.transform.position = spawnpos;
            obstacleindicator.setvalues(player, obstacle.transform);
            //var obstacle = Instantiate(obstacles[rng.Next(0, obstacles.Count)]);
            //obstacle.transform.position = spawnpos;
        }
    }
}
