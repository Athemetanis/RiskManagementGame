using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssincTextRewriter : MonoBehaviour
{   
    //VARIABLES
    private string warningString;

    //GETTERS & SETTERS
    public void SetWarningString(string warningString) { this.warningString = warningString; }

    // Update is called once per frame
    void Update()
    {
        UpdateWarningText(warningString);
    }

    //METHODS
    public void UpdateWarningText(string text)
    {
        this.gameObject.GetComponent<Text>().text = warningString;
    }
        
}
