using UnityEngine;

public class expandinghb : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
   
    [HideInInspector] public hitbox hb;
    public float delaycountdown = 3;
    public Vector2 lerpsize = new Vector2(5f, 5f);
    public float lerptime = 0.1f;
    public bool destroyonend = false;

    // Update is called once per frame
    void Start()
    {
        hb = GetComponentInChildren<hitbox>();
    }
    void Update()
    {
        if (!hb.active)
            return;
        if (delaycountdown > 0)
        {
            delaycountdown -= Time.deltaTime;
            return;
        }
        hb.transform.localScale = Vector3.LerpUnclamped(hb.transform.localScale, new Vector3(lerpsize.x, lerpsize.y, lerpsize.x), lerptime * Time.deltaTime);

        if(Vector2.Distance(new Vector2(hb.transform.localScale.x, hb.transform.localScale.y), lerpsize) < 0.1f)
        {
            if (destroyonend)
                Destroy(this.gameObject);
        }
    }
}
