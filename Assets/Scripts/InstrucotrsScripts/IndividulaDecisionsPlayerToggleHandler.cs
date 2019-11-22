using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IndividulaDecisionsPlayerToggleHandler : MonoBehaviour
{

    public TextMeshProUGUI firmName;
    public TextMeshProUGUI playerName;

    private Toggle playerToggle;
    private string playerID;

    private InstructorIndividualStatsUIHandler instructorIndividualStatsUIHandler;

    public void SetInstructorIndividualStatsUIHandler(InstructorIndividualStatsUIHandler instructorIndividualStatsUIHandler) { this.instructorIndividualStatsUIHandler = instructorIndividualStatsUIHandler; }

    public void SetUpPlayerDecisionToggle(ToggleGroup tg, string playerID,  string firmName, string playerName)
   {    
        playerToggle = this.GetComponent<Toggle>();
        playerToggle.group = tg;

        this.playerID = playerID;
        this.firmName.text = firmName;
        this.playerName.text = playerName;        
        playerToggle.onValueChanged.AddListener(delegate { ToggleValueChanged(playerToggle); });
    }


    void ToggleValueChanged(Toggle change)
    {   
        if (playerToggle.isOn)
        {
            instructorIndividualStatsUIHandler.GeneratePlayerDecisions(playerID);
        }
    }
}
