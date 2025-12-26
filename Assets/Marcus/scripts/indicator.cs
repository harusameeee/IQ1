using System;
using UnityEngine;
using UnityEngine.UI;

public class indicator : MonoBehaviour
{
    public Image indicator_colour;
    public Transform danger_icon_transform;
    public Image item_icon;
    public Image timer_ring;

    public Transform obstacle_transform;
    public player_mover player;

    public float offsetpos = 0f;

    [SerializeField] int segmentCount = 20;
    [SerializeField] float startDelay = 0.5f;

    float maxDistance;
    float delayTimer;
    bool started;

    public static Action<int, float> onIndicatorEnd;

    void Awake()
    {
        // Ensure unique material per indicator
        timer_ring.material = new Material(timer_ring.material);
    }

    void OnEnable()
    {
        delayTimer = 0f;
        started = false;

        // Reset shader state
        timer_ring.material.SetFloat("_segmentcount", segmentCount);
        timer_ring.material.SetFloat("_removesegment", 0);
    }

    void Update()
    {
        if (obstacle_transform == null || player == null)
            return;

        // Delay start
        if (!started)
        {
            delayTimer += Time.deltaTime;
            if (delayTimer >= startDelay)
                started = true;
            else
                return;
        }

        float dist = player.get_dist(
            player.transform.position,
            obstacle_transform.position);

        float t = 1f - Mathf.Clamp01(dist / maxDistance);

        int value = Mathf.Clamp(
            Mathf.RoundToInt(segmentCount * t),
            0,
            segmentCount);

        timer_ring.material.SetFloat("_removesegment", value);

        if (value >= segmentCount)
        {
            EndIndicator();
        }
    }

    public void setvalues(player_mover player, Transform obstacle, float offsetpos = 0f)
    {
        this.player = player;
        this.obstacle_transform = obstacle;
        this.offsetpos = offsetpos;

        maxDistance = player.get_dist(
            player.transform.position,
            obstacle_transform.position);

        gameObject.SetActive(true);
    }

    void EndIndicator()
    {
        obstacle_transform = null;
        player = null;

        indicator_colour.color = Color.red;
        item_icon.gameObject.SetActive(true);

        gameObject.SetActive(false);
    }

    public void setitem(Sprite icon)
    {
        indicator_colour.color = Color.blue;
        item_icon.gameObject.SetActive(true);
        item_icon.sprite = icon;
        danger_icon_transform.gameObject.SetActive(false);
    }
}
