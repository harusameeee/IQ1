using UnityEngine;

public class score_counter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float currentscore = 0;
    int combo = 0;
    public TMPro.TMP_Text scoretext;
    public TMPro.TMP_Text ComboText;
    void Start()
    {
        entity.onHit += addscore;
        scoretext.text = "Score: " + ((int)currentscore);
        ComboText.text = "Combo: " + combo;

    }
    public void addscore(float scoretoadd,bool comboable)
    {
        if (scoretoadd > 0)
        {
            
            currentscore += scoretoadd * (1f + combo * 0.1f);
            if (comboable)
            {    
            combo += 1;
            }
        }
        else
        {
            combo = 0;
            currentscore += scoretoadd;

        }
        
            scoretext.text = "Score: " +  ((int)currentscore);
            ComboText.text = "Combo: " + combo;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
