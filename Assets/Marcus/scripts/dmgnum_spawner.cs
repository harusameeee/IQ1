using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class dmgnum_spawner : MonoBehaviour
{
    [SerializeField] private Sprite[] dmgnum_sprites = new Sprite[5];
    private List<dmgnum> dmgnumprefabs = new List<dmgnum>();
    public void Update()
    {
    }
    void Start()
    {
        witch_Ai2.enemyhit += SpawnDmgNum;
        for(int i = 0; i < 25; i++)
        {
            dmgnum temp = Instantiate(Resources.Load<dmgnum>("dmgnum"), this.transform);

            dmgnumprefabs.Add(temp);
            temp.gameObject.SetActive(false);
        }
    }
    public void SpawnDmgNum(float dmgamount,List<damagable_type> dmgtypes = null, Vector2 hitpoint = new Vector2())
    {

        foreach (dmgnum prefab in dmgnumprefabs)
        {
            if (!prefab.gameObject.activeInHierarchy)
            {
                if (dmgtypes == null)
                {
                    prefab.dmgnumimg.gameObject.SetActive(false);
                    prefab.dmgnumtext.color = Color.white;
                }
                else
                {
                    dmgtypes.Max<damagable_type>();
                    prefab.dmgnumimg.gameObject.SetActive(true);
                    if (dmgtypes[0] == damagable_type.poison_deluge || dmgtypes[0] == damagable_type.poison)
                    {
                        prefab.dmgnumtext.color = Color.green;

                    }
                    else
                    {
                        prefab.dmgnumtext.color = Color.white;
                    }
                    prefab.dmgnumimg.sprite = dmgnum_sprites[(int)dmgtypes[0]];


                }
                prefab.transform.localPosition = hitpoint;
                prefab.gameObject.SetActive(true);
                prefab.dmgnumtext.text = dmgamount.ToString();
                prefab.dmgnumanim.Play();
                return;
            }
        }
    }
}
