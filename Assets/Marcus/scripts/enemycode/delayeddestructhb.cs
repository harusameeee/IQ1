using UnityEngine;

public class delayeddestructhb : MonoBehaviour
{
    [HideInInspector] public hitbox hb;
    public float delaycountdown = 3;
    void Start()
    {
        hb = GetComponentInChildren<hitbox>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hb.active)
            return;
        if (delaycountdown > 0)
        {
            delaycountdown -= Time.deltaTime;
            return;
        }
        Destroy(this.gameObject);
    }
}
