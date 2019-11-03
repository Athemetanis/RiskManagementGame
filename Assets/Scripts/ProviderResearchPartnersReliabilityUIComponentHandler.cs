using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProviderResearchPartnersReliabilityUIComponentHandler : MonoBehaviour
{
    public TextMeshProUGUI possiblePartnerFirmNameText;
    public TextMeshProUGUI possiblePartnerReliabilityText;
    public TextMeshProUGUI possiblePartnerEmployeesCount;
    

    public void SetUpProviderResearchPartnersReliability(string firmName, int reliability, int emplyeeCount)
    {
        possiblePartnerFirmNameText.text = firmName;
        possiblePartnerReliabilityText.text = reliability.ToString();
        possiblePartnerEmployeesCount.text = emplyeeCount.ToString("n0");
    }
}
