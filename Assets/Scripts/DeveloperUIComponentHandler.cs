using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeveloperUIComponentHandler : MonoBehaviour
{   
    public TextMeshProUGUI firmNameText;
    public TextMeshProUGUI firmDescriptionsText;
    
    public void SetFirmName(string firmName)
    {
        firmNameText.text = firmName;
    }

    public void SetFirmDescription(string firmDescription)
    {
        firmDescriptionsText.text = firmDescription;
    }
}
