using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeveloperResearchCompetitorsEmployeesUIComponentHandler : MonoBehaviour
{
    public TextMeshProUGUI firmsNameText;
    public TextMeshProUGUI employeesCountText;

    public void SetUpDeveloperResearchCompetitorsEmployeesUIComponent(string firmsName, int employeesCount)
    {
        firmsNameText.text = firmsName;
        employeesCountText.text = employeesCount.ToString("n0");


    }


}
