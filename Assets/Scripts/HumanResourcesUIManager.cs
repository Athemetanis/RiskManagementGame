using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanResourcesUIManager : MonoBehaviour
{   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdditonOfOne(InputField inputfield)
    {
        int value = int.Parse(inputfield.text) + 1;
        inputfield.text = value.ToString();
    }

    public void SubstractionOfOne(InputField inputfield)
    {
        int value = int.Parse(inputfield.text) - 1;

        if(value < 0)
        {
            inputfield.text = "0";
        }
        else
        {
            inputfield.text = value.ToString();
        }

 
    }


}
