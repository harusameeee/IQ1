using System;
using UnityEngine;

using Random = System.Random;
public class witch_Ai : entity
{
    
    protected static Random rng = new();
    public witch_mover mover;
    //will add spawn patern soon
    public GameObject projectile;
    public float shootInterval = 2.0f;
    private float shootTimer = 0.0f;
    public int maxdist = 5;
    public Vector2 dim;
    public override Vector2 dimension => dim;
    public override Vector2 position => new Vector2(transform.localPosition.x, transform.localPosition.y + 8f);
    public override void Start()
    {
        base.Start();
    }
    public override bool TakeDamage(float damageAmount,bool comboable = true)
    {
     
        float dmgmult = 1.0f;
        foreach (var buff in buffs)
        {
            if (buff.type == bufftypes.vulnerability)
            {
                dmgmult += buff.pow;
            }
        }
           Debug.Log($"Witch took {damageAmount} damage with mult {dmgmult}");
        onHit?.Invoke((int)(damageAmount * dmgmult),comboable);
        return true;
    }
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        return;
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            shootTimer = 0.0f;
            // Shoot a projectile
            Random rng = new Random(System.DateTime.Now.Millisecond);
            int xOffset = rng.Next(-maxdist, maxdist);
            Vector3 spawnPos = mover.getobstaclespawnpos(0, 2.0f, out bool valid, out float new_t);
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
