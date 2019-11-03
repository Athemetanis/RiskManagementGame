using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResearchProductUIComponentHandler : MonoBehaviour
{
    public TextMeshProUGUI firmsNameText;
    public TextMeshProUGUI functionalityText;
    public TextMeshProUGUI integrabilityText;
    public TextMeshProUGUI userFriendlinessText;

    public void SetUpProductResearchUIComponent(string firmName, int functionality, int integrability, int userFriendliness)
    {
        firmsNameText.text = firmName;
        functionalityText.text = functionality.ToString();
        integrabilityText.text = integrability.ToString();
        userFriendlinessText.text = userFriendliness.ToString();
    }

}
