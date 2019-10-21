using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProviderResearchPartnersReliabilityUIComponentHandler : MonoBehaviour
{
    public TextMeshProUGUI possiblePartnerFirmNameText;
    public TextMeshProUGUI possiblePartnerReliabilityText;
    
    public void SetUpProviderResearchPartnersReliability(string firmName, int reliability)
    {
        possiblePartnerFirmNameText.text = firmName;
        possiblePartnerReliabilityText.text = reliability.ToString();

    }
}
