using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeveloperUIComponentHandler : MonoBehaviour
{
    public Text firmNameText;
    public Text firmDescriptionsText;
    
    public void SetFirmName(string firmName)
    {
        firmNameText.text = firmName;
    }

    public void SetFirmDescription(string firmDescription)
    {
        firmDescriptionsText.text = firmDescription;
    }
}
