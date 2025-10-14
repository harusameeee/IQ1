using UnityEngine;

using Random = System.Random;
public class witch_Ai : MonoBehaviour
{
    
    protected static Random rng = new();
    public witch_mover mover;
    //will add spawn patern soon
    public GameObject projectile;
    public float shootInterval = 2.0f;
    private float shootTimer = 0.0f;
    public int maxdist = 5;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            shootTimer = 0.0f;
            // Shoot a projectile
            Random rng = new Random(System.DateTime.Now.Millisecond);
            int xOffset = rng.Next(-maxdist, maxdist);
            Vector3 spawnPos = mover.getobstaclespawnpos(0, 2.0f, out bool valid);
            if (valid)
            {
                var temp = Instantiate(projectile, spawnPos, Quaternion.identity);
                moving_obstacle mov = temp.GetComponent<moving_obstacle>();
                mov.splinecont = mover.splinecont;
                mov.obstacle.localPosition = new Vector3(xOffset,mov.obstacle.localPosition.y,mov.obstacle.localPosition.z);
            }
        }
    }
}
