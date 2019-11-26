using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstructorRiskStatHandler : MonoBehaviour
{
    public Image imageColor;
    public TextMeshProUGUI riskNameID;
    public TextMeshProUGUI likelihood;
    public TextMeshProUGUI impact;
    public Toggle monitorToggle;

    private string riskID;
    private Color riskColor;

    private IndividualDecisionsRiskManagementQuarterHandler indivDecRiskManagQuarterHandler;

    public void SetupValues(IndividualDecisionsRiskManagementQuarterHandler indivDecRiskManagQuarterHandler, string name, int likelihood, int impact, bool monitor, Color color)
    {
        this.indivDecRiskManagQuarterHandler = indivDecRiskManagQuarterHandler;
        riskNameID.text = name;
        this.likelihood.text = likelihood.ToString();
        this.impact.text = impact.ToString();
        monitorToggle.isOn = monitor;
        monitorToggle.interactable = false;
        imageColor.color = color;
    }

    public void HighlightRisk()  //trigered by event  - on cursor hover
    {
        string riskID = riskNameID.text;
        if (indivDecRiskManagQuarterHandler != null)
        {
            indivDecRiskManagQuarterHandler.HighlightRisk(riskID);
        }
    }

    public void EndHighLighRisk()
    {
        if (indivDecRiskManagQuarterHandler != null)
        {
            indivDecRiskManagQuarterHandler.EndHighlightRisk();
        }
    }
}
