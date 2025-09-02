using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    //　経過時間
    float deltatime;
    //スケール
    private float scaleY=0.01f;
    //
    [SerializeField] Image circle;
  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnTriggerStay(Collider other)
    {
        //Playerとの当たり判定を取る
        if (other.CompareTag("Player"))
        {
            //ボタン押してる風→Scale変える
            transform.localScale = new Vector3(1.0f,scaleY,5.0f);

            deltatime += Time.deltaTime;
            //3秒
            if (deltatime > 3.0f)
            {

            }
            

        }
    }
}
