    using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class movinghb : MonoBehaviour
{
    
    [HideInInspector] public hitbox hb;
    [HideInInspector] public LineRenderer lr;
    private Vector3[] path;
    [HideInInspector] public int index = 0;
    public float speed = 1.0f;
    public bool destroyonend = false;
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        path = new Vector3[lr.positionCount];
        
        hb = GetComponentInChildren<hitbox>();
        lr.GetPositions(path);
        hb.transform.localPosition = new Vector3(path[0].x, path[0].y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!hb.active)
            return;
        if (index < path.Length)
        {
            hb.transform.localPosition = Vector3.LerpUnclamped(hb.transform.localPosition, path[index], speed * Time.deltaTime);
            if (Vector3.Distance(hb.transform.localPosition, path[index]) < 1)
            {
                index++;
            }
        }
        else if (destroyonend)
        {
            Destroy(this.gameObject);
        }
    }
    void OnDrawGizmos()
    {

    }
}
