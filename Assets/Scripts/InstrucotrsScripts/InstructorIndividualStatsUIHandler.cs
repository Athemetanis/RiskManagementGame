using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructorIndividualStatsUIHandler : MonoBehaviour
{
    public GameObject PlayerTogglePrefab;

    public GameObject playerToggleContent;

    public IndividualDecisionsRiskManagementQuarterHandler riskQ1Handler;
    public IndividualDecisionsRiskManagementQuarterHandler riskQ2Handler;
    public IndividualDecisionsRiskManagementQuarterHandler riskQ3Handler;
    public IndividualDecisionsRiskManagementQuarterHandler riskQ4Handler;
    public IndividualDecisionsRiskManagementQuarterHandler riskQ5Handler;

    private string gameID;
    private int quarter;

    //REFERENCES
    private InstructorGameInfoUIHandler instructorGameInfoUIHandler;

    //GETTERS & SETTERS
    public void SetInstructorGameInfoUIHandler(InstructorGameInfoUIHandler instructorGameInfoUIHandler) { this.instructorGameInfoUIHandler = instructorGameInfoUIHandler; }


    // Start is called before the first frame update
    void Start()
    {
        UpdateContent();
    }

    public void UpdateContent()
    {   
        gameID = instructorGameInfoUIHandler.GetGameID();
        quarter = instructorGameInfoUIHandler.GetGame().GetGameRound();
        GeneratePlayersToggles();

        if (quarter == 1)
        {
            return;
        }

    }

    public void GeneratePlayersToggles()
    {
        foreach (Transform child in playerToggleContent.transform)
        { GameObject.Destroy(child.gameObject); }

        if (GameHandler.allGames[gameID].GetPlayersCount() == 0)
        {
            Debug.Log("game has no player in");
            return;
        }

        Dictionary<string, GameObject> players = GameHandler.allGames[gameID].GetPlayerList();

        foreach (KeyValuePair<string, GameObject> player in players)
        {
            PlayerData playerData = player.Value.GetComponent<PlayerData>();
            FirmManager firmManager = player.Value.GetComponent<FirmManager>();
            string playerID = playerData.GetPlayerID();

            string firmName = instructorGameInfoUIHandler.GetGame().GetFirmName(playerID);

            GameObject playerToggle = Instantiate(PlayerTogglePrefab);
            playerToggle.transform.SetParent(playerToggleContent.transform, false);
            IndividulaDecisionsPlayerToggleHandler playerToggleHandler = playerToggle.GetComponent<IndividulaDecisionsPlayerToggleHandler>();
            playerToggleHandler.SetInstructorIndividualStatsUIHandler(this);
            playerToggleHandler.SetUpPlayerDecisionToggle(playerToggleContent.GetComponent<ToggleGroup>(), playerID, firmName, firmManager.GetPlayerName());
        }
    }

    public void GeneratePlayerDecisions(string playerID)
    {
        quarter = instructorGameInfoUIHandler.GetGame().GetGameRound();
        if (quarter == 1)
        {
            return;
        }

        if (playerID == null)
        {
            Debug.LogError("INSTRUCTOR: PlayerID in toggle is null");
            return;
        }

        GameObject player = instructorGameInfoUIHandler.GetGame().GetPlayer(playerID);

        RiskManager rm = player.GetComponent<RiskManager>();

        if (quarter > 1)
        {
            riskQ1Handler.SetUpQuarterRisksDescriptions(rm.GetRisk1DescriptionQ(1), rm.GetRisk1ImpactActionsQ(1), rm.GetRisk2DescriptionQ(1), rm.GetRisk2ImpactActionsQ(1), rm.GetRisk3DescriptionQ(1), rm.GetRisk3ImpactActionsQ(1));
        }
        if (quarter > 2)
        {
            riskQ1Handler.SetUpQuarterRisksDescriptions(rm.GetRisk1DescriptionQ(1), rm.GetRisk1ImpactActionsQ(1), rm.GetRisk2DescriptionQ(1), rm.GetRisk2ImpactActionsQ(1), rm.GetRisk3DescriptionQ(1), rm.GetRisk3ImpactActionsQ(1));
            riskQ2Handler.SetUpQuarterRisksDescriptions(rm.GetRisk1DescriptionQ(2), rm.GetRisk1ImpactActionsQ(2), rm.GetRisk2DescriptionQ(2), rm.GetRisk2ImpactActionsQ(2), rm.GetRisk3DescriptionQ(2), rm.GetRisk3ImpactActionsQ(2));

        }
        if (quarter > 3)
        {
            riskQ1Handler.SetUpQuarterRisksDescriptions(rm.GetRisk1DescriptionQ(1), rm.GetRisk1ImpactActionsQ(1), rm.GetRisk2DescriptionQ(1), rm.GetRisk2ImpactActionsQ(1), rm.GetRisk3DescriptionQ(1), rm.GetRisk3ImpactActionsQ(1));
            riskQ2Handler.SetUpQuarterRisksDescriptions(rm.GetRisk1DescriptionQ(2), rm.GetRisk1ImpactActionsQ(2), rm.GetRisk2DescriptionQ(2), rm.GetRisk2ImpactActionsQ(2), rm.GetRisk3DescriptionQ(2), rm.GetRisk3ImpactActionsQ(2));
            riskQ3Handler.SetUpQuarterRisksDescriptions(rm.GetRisk1DescriptionQ(3), rm.GetRisk1ImpactActionsQ(3), rm.GetRisk2DescriptionQ(3), rm.GetRisk2ImpactActionsQ(3), rm.GetRisk3DescriptionQ(3), rm.GetRisk3ImpactActionsQ(3));
            riskQ3Handler.SetUpQuarterMatrix(rm.GetRisksQ(3), rm.GetLikelihoodQ(3), rm.GetImpactQ(3), rm.GetMitigationQ(3), rm.GetColorsQ(3));


        }
        if (quarter > 4)
        {
            riskQ1Handler.SetUpQuarterRisksDescriptions(rm.GetRisk1DescriptionQ(1), rm.GetRisk1ImpactActionsQ(1), rm.GetRisk2DescriptionQ(1), rm.GetRisk2ImpactActionsQ(1), rm.GetRisk3DescriptionQ(1), rm.GetRisk3ImpactActionsQ(1));
            riskQ2Handler.SetUpQuarterRisksDescriptions(rm.GetRisk1DescriptionQ(2), rm.GetRisk1ImpactActionsQ(2), rm.GetRisk2DescriptionQ(2), rm.GetRisk2ImpactActionsQ(2), rm.GetRisk3DescriptionQ(2), rm.GetRisk3ImpactActionsQ(2));
            riskQ3Handler.SetUpQuarterRisksDescriptions(rm.GetRisk1DescriptionQ(3), rm.GetRisk1ImpactActionsQ(3), rm.GetRisk2DescriptionQ(3), rm.GetRisk2ImpactActionsQ(3), rm.GetRisk3DescriptionQ(3), rm.GetRisk3ImpactActionsQ(3));
            riskQ3Handler.SetUpQuarterMatrix(rm.GetRisksQ(3), rm.GetLikelihoodQ(3), rm.GetImpactQ(3), rm.GetMitigationQ(3), rm.GetColorsQ(3));
            riskQ4Handler.SetUpQuarterRisksDescriptions(rm.GetRisk1DescriptionQ(4),rm.GetRisk2DescriptionQ(4), rm.GetRisk3DescriptionQ(4));
            riskQ4Handler.SetUpQuarterMatrix(rm.GetRisksQ(4), rm.GetLikelihoodQ(4), rm.GetImpactQ(4), rm.GetMitigationQ(4), rm.GetColorsQ(4));
        }


    }


   
   

}
