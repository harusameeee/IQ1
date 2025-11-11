using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ChangeLanguage : MonoBehaviour
{
    //Canvas‚ð“ü‚ê‘Ö‚¦‚é
    [SerializeField] Canvas[] languageCanvas;
    //
    [SerializeField] TMP_Dropdown dropdown;

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(dropdown.value);
        switch (dropdown.value){

            //japanese
            case 0:
                languageCanvas[0].gameObject.SetActive(true); 
                languageCanvas[1].gameObject.SetActive(false); 
                languageCanvas[2].gameObject.SetActive(false);
                break;
            
            //English
            case 1:
                languageCanvas[0].gameObject.SetActive(false);
                languageCanvas[1].gameObject.SetActive(true);
                languageCanvas[2].gameObject.SetActive(false);
                break;

            //Suomalainen
            case 2:
                languageCanvas[0].gameObject.SetActive(false);
                languageCanvas[1].gameObject.SetActive(false);
                languageCanvas[2].gameObject.SetActive(true);
                break;


        }

    }
}
