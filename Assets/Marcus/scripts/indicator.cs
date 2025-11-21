    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class indicator : MonoBehaviour
{
    public Image indicator_colour;
    public Transform danger_icon_transform;
    public Image item_icon;
    public Image timer_ring;
    public Transform obstacle_transform;
    public float offsetpos = 0;
    int initail_segment_count = 0;
    public player_mover player;
    public static Action<int,float> onIndicatorEnd;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer_ring.material = Instantiate(timer_ring.material);
    }

    // Update is called once per frame
    void Update()
    {
        if (obstacle_transform != null && player != null)
        {
            int val = Mathf.RoundToInt(player.get_dist(player.transform.position, obstacle_transform.position) / 2);
            timer_ring.material.SetFloat("_removesegment", initail_segment_count-val);
            if(initail_segment_count-val>= initail_segment_count)
            {
                //onIndicatorEnd?.Invoke(obstacleindex, offsetpos);
                obstacle_transform = null;
                player = null;
                
                item_icon.gameObject.SetActive(false);
                danger_icon_transform.gameObject.SetActive(true);
                indicator_colour.color = Color.red;
                this.gameObject.SetActive(false);
            }
        }
    }
    public void setvalues(player_mover player, Transform obstacle, float offsetpos = 0)
    {
        this.player = player;
        obstacle_transform = obstacle;
        this.offsetpos = offsetpos;
        initail_segment_count = Mathf.RoundToInt(this.player.get_dist(player.transform.position, obstacle_transform.transform.position) / 2);
        timer_ring.material.SetFloat("_segmentcount", initail_segment_count);

    }
    public void setitem(Sprite icon)
    {
        indicator_colour.color = Color.blue;
        item_icon.gameObject.SetActive(true);
        item_icon.sprite = icon;
        danger_icon_transform.gameObject.SetActive(false);

    }
}
