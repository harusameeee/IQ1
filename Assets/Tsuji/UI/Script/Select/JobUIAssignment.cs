using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class JobUIAssignment : MonoBehaviour
{
    //1P
    [NamedArray(new string[] 
    { "近接攻撃", "遠距離攻撃", "スキル", 
        "防御","プレイヤーアイコン","コイン(マーライオン用)" })]
    [SerializeField] Image[] PlayerCommands=new Image[6];

    //何を選択したか
    //[SerializeField] private SelectedJob selectedJob;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //名前を持ってくる
        //string jobname = Job.name;
        //if(FindAnyObjectByType(typeof(Job.name)))
    }
}
