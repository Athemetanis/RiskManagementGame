using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HelpUIHandler : MonoBehaviour
{
    public TextMeshProUGUI helptext;
    // Start is called before the first frame update
    void Start()
    {
   
            
            //On.AddListener(delegate { ToggleValueChanged(playerToggle); });
    }
    void OnMouseEnter()
    {
        helptext.gameObject.SetActive(true);
       
    }

    void OnMouseExit()
    {
        helptext.gameObject.SetActive(false);

    }


}
