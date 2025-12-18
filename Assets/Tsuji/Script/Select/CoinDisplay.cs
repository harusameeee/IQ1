using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinDisplay : MonoBehaviour
{
    //ƒXƒLƒ‹‚S‚Â
    [SerializeField] private skilldata[] skilldatas=new skilldata[4];
    [SerializeField] private GameObject[] coins = new GameObject[4];
    private TextMeshProUGUI[] text = new TextMeshProUGUI[4]; 

    private void Start()
    {
        for (int i = 0; i < coins.Length; i++) 
        {
            coins[i].SetActive(false);
            text[i]=coins[i].GetComponentInChildren<TextMeshProUGUI>();
            text[i].text = skilldatas[i].coincost.ToString();
        }
    }

    public void CoinDisp(bool isMerlion)
    {
        for (int i = 0; i < coins.Length; i++)
        {
            coins[i].SetActive(isMerlion);
        }
    }
}
