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

    public IndividualDecisionsFirmStrategiesHandler developerFirmStrategiesHandler;
    public IndividualDecisionsFirmStrategiesHandler providerFirmStrategiesHandler;

    public GameObject developerFirmContent;
    public GameObject providerFirmContent;

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
            PlayerManager playerData = player.Value.GetComponent<PlayerManager>();
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
        FirmManager fm = player.GetComponent<FirmManager>();
        PlayerManager pd = player.GetComponent<PlayerManager>();

        PlayerRoles playerRole = pd.GetPlayerRole();

        if (quarter > 1)
        {
            riskQ1Handler.SetUpQuarterRisksDescriptions(rm.GetRisk1DescriptionQ(1), rm.GetRisk1ImpactActionsQ(1), rm.GetRisk2DescriptionQ(1), rm.GetRisk2ImpactActionsQ(1), rm.GetRisk3DescriptionQ(1), rm.GetRisk3ImpactActionsQ(1));
            if(playerRole == PlayerRoles.Developer)
            {
                developerFirmContent.SetActive(true);
                providerFirmContent.SetActive(false);
                developerFirmStrategiesHandler.SetMarketsize(fm.GetMarketSize());
                developerFirmStrategiesHandler.SetCompetitivePosture(fm.GetCompetitivePosture());
                developerFirmStrategiesHandler.SetDistinctiveCompetencies(fm.GetDistinctiveCompetenceis());
                developerFirmStrategiesHandler.SetBusinessPartnerDiversity(fm.GetBusinessPartnerDiversity());
                developerFirmStrategiesHandler.SetContractPriorities(fm.GetContractPriorities());
                developerFirmStrategiesHandler.SetAccountingStrategies(fm.GetFirmAccouningStrategies());
                developerFirmStrategiesHandler.SetGrowthStrategies(fm.GetGrowthStrategies());
                developerFirmStrategiesHandler.SetDevelopmentStrategies(fm.GetDevelopmentStrategies());
            }
            else
            {
                developerFirmContent.SetActive(false);
                providerFirmContent.SetActive(true);
                providerFirmStrategiesHandler.SetMarketsize(fm.GetMarketSize());
                providerFirmStrategiesHandler.SetCompetitivePosture(fm.GetCompetitivePosture());
                providerFirmStrategiesHandler.SetDistinctiveCompetencies(fm.GetDistinctiveCompetenceis());
                providerFirmStrategiesHandler.SetBusinessPartnerDiversity(fm.GetBusinessPartnerDiversity());
                providerFirmStrategiesHandler.SetContractPriorities(fm.GetContractPriorities());
                providerFirmStrategiesHandler.SetAccountingStrategies(fm.GetFirmAccouningStrategies());
                providerFirmStrategiesHandler.SetGrowthStrategies(fm.GetGrowthStrategies());
                providerFirmStrategiesHandler.SetDevelopmentStrategies(fm.GetDevelopmentStrategies());
            }

        }
        if (quarter > 2)
        {
            riskQ1Handler.SetUpQuarterRisksDescriptions(rm.GetRisk1DescriptionQ(1), rm.GetRisk1ImpactActionsQ(1), rm.GetRisk2DescriptionQ(1), rm.GetRisk2ImpactActionsQ(1), rm.GetRisk3DescriptionQ(1), rm.GetRisk3ImpactActionsQ(1));
            riskQ2Handler.SetUpQuarterRisksDescriptions(rm.GetRisk1DescriptionQ(2), rm.GetRisk1ImpactActionsQ(2), rm.GetRisk2DescriptionQ(2), rm.GetRisk2ImpactActionsQ(2), rm.GetRisk3DescriptionQ(2), rm.GetRisk3ImpactActionsQ(2));
            if (playerRole == PlayerRoles.Developer)
            {
                developerFirmContent.SetActive(true);
                providerFirmContent.SetActive(false);
                developerFirmStrategiesHandler.SetMarketsize(fm.GetMarketSize());
                developerFirmStrategiesHandler.SetCompetitivePosture(fm.GetCompetitivePosture());
                developerFirmStrategiesHandler.SetDistinctiveCompetencies(fm.GetDistinctiveCompetenceis());
                developerFirmStrategiesHandler.SetBusinessPartnerDiversity(fm.GetBusinessPartnerDiversity());
                developerFirmStrategiesHandler.SetContractPriorities(fm.GetContractPriorities());
                developerFirmStrategiesHandler.SetAccountingStrategies(fm.GetFirmAccouningStrategies());
                developerFirmStrategiesHandler.SetGrowthStrategies(fm.GetGrowthStrategies());
                developerFirmStrategiesHandler.SetDevelopmentStrategies(fm.GetDevelopmentStrategies());
            }
            else
            {
                developerFirmContent.SetActive(false);
                providerFirmContent.SetActive(true);
                providerFirmStrategiesHandler.SetMarketsize(fm.GetMarketSize());
                providerFirmStrategiesHandler.SetCompetitivePosture(fm.GetCompetitivePosture());
                providerFirmStrategiesHandler.SetDistinctiveCompetencies(fm.GetDistinctiveCompetenceis());
                providerFirmStrategiesHandler.SetBusinessPartnerDiversity(fm.GetBusinessPartnerDiversity());
                providerFirmStrategiesHandler.SetContractPriorities(fm.GetContractPriorities());
                providerFirmStrategiesHandler.SetAccountingStrategies(fm.GetFirmAccouningStrategies());
                providerFirmStrategiesHandler.SetGrowthStrategies(fm.GetGrowthStrategies());
                providerFirmStrategiesHandler.SetDevelopmentStrategies(fm.GetDevelopmentStrategies());
            }
        }
        if (quarter > 3)
        {
            riskQ1Handler.SetUpQuarterRisksDescriptions(rm.GetRisk1DescriptionQ(1), rm.GetRisk1ImpactActionsQ(1), rm.GetRisk2DescriptionQ(1), rm.GetRisk2ImpactActionsQ(1), rm.GetRisk3DescriptionQ(1), rm.GetRisk3ImpactActionsQ(1));
            riskQ2Handler.SetUpQuarterRisksDescriptions(rm.GetRisk1DescriptionQ(2), rm.GetRisk1ImpactActionsQ(2), rm.GetRisk2DescriptionQ(2), rm.GetRisk2ImpactActionsQ(2), rm.GetRisk3DescriptionQ(2), rm.GetRisk3ImpactActionsQ(2));
            riskQ3Handler.SetUpQuarterRisksDescriptions(rm.GetRisk1DescriptionQ(3), rm.GetRisk1ImpactActionsQ(3), rm.GetRisk2DescriptionQ(3), rm.GetRisk2ImpactActionsQ(3), rm.GetRisk3DescriptionQ(3), rm.GetRisk3ImpactActionsQ(3));
            riskQ3Handler.SetUpQuarterMatrix(rm.GetRisksQ(3), rm.GetLikelihoodQ(3), rm.GetImpactQ(3), rm.GetMitigationQ(3), rm.GetColorsQ(3));
            if (playerRole == PlayerRoles.Developer)
            {
                developerFirmContent.SetActive(true);
                providerFirmContent.SetActive(false);
                developerFirmStrategiesHandler.SetMarketsize(fm.GetMarketSize());
                developerFirmStrategiesHandler.SetCompetitivePosture(fm.GetCompetitivePosture());
                developerFirmStrategiesHandler.SetDistinctiveCompetencies(fm.GetDistinctiveCompetenceis());
                developerFirmStrategiesHandler.SetBusinessPartnerDiversity(fm.GetBusinessPartnerDiversity());
                developerFirmStrategiesHandler.SetContractPriorities(fm.GetContractPriorities());
                developerFirmStrategiesHandler.SetAccountingStrategies(fm.GetFirmAccouningStrategies());
                developerFirmStrategiesHandler.SetGrowthStrategies(fm.GetGrowthStrategies());
                developerFirmStrategiesHandler.SetDevelopmentStrategies(fm.GetDevelopmentStrategies());
            }
            else
            {
                developerFirmContent.SetActive(false);
                providerFirmContent.SetActive(true);
                providerFirmStrategiesHandler.SetMarketsize(fm.GetMarketSize());
                providerFirmStrategiesHandler.SetCompetitivePosture(fm.GetCompetitivePosture());
                providerFirmStrategiesHandler.SetDistinctiveCompetencies(fm.GetDistinctiveCompetenceis());
                providerFirmStrategiesHandler.SetBusinessPartnerDiversity(fm.GetBusinessPartnerDiversity());
                providerFirmStrategiesHandler.SetContractPriorities(fm.GetContractPriorities());
                providerFirmStrategiesHandler.SetAccountingStrategies(fm.GetFirmAccouningStrategies());
                providerFirmStrategiesHandler.SetGrowthStrategies(fm.GetGrowthStrategies());
                providerFirmStrategiesHandler.SetDevelopmentStrategies(fm.GetDevelopmentStrategies());
            }

        }
        if (quarter > 4)
        {
            riskQ1Handler.SetUpQuarterRisksDescriptions(rm.GetRisk1DescriptionQ(1), rm.GetRisk1ImpactActionsQ(1), rm.GetRisk2DescriptionQ(1), rm.GetRisk2ImpactActionsQ(1), rm.GetRisk3DescriptionQ(1), rm.GetRisk3ImpactActionsQ(1));
            riskQ2Handler.SetUpQuarterRisksDescriptions(rm.GetRisk1DescriptionQ(2), rm.GetRisk1ImpactActionsQ(2), rm.GetRisk2DescriptionQ(2), rm.GetRisk2ImpactActionsQ(2), rm.GetRisk3DescriptionQ(2), rm.GetRisk3ImpactActionsQ(2));
            riskQ3Handler.SetUpQuarterRisksDescriptions(rm.GetRisk1DescriptionQ(3), rm.GetRisk1ImpactActionsQ(3), rm.GetRisk2DescriptionQ(3), rm.GetRisk2ImpactActionsQ(3), rm.GetRisk3DescriptionQ(3), rm.GetRisk3ImpactActionsQ(3));
            riskQ3Handler.SetUpQuarterMatrix(rm.GetRisksQ(3), rm.GetLikelihoodQ(3), rm.GetImpactQ(3), rm.GetMitigationQ(3), rm.GetColorsQ(3));
            riskQ4Handler.SetUpQuarterRisksDescriptions(rm.GetRisk1DescriptionQ(4),rm.GetRisk2DescriptionQ(4), rm.GetRisk3DescriptionQ(4));
            riskQ4Handler.SetUpQuarterMatrix(rm.GetRisksQ(4), rm.GetLikelihoodQ(4), rm.GetImpactQ(4), rm.GetMitigationQ(4), rm.GetColorsQ(4));
            riskQ5Handler.SetUpQuarterRisksDescriptions(rm.GetRisk1DescriptionQ(5), rm.GetRisk2DescriptionQ(5), rm.GetRisk3DescriptionQ(5));
            if (playerRole == PlayerRoles.Developer)
            {
                developerFirmContent.SetActive(true);
                providerFirmContent.SetActive(false);
                developerFirmStrategiesHandler.SetMarketsize(fm.GetMarketSize());
                developerFirmStrategiesHandler.SetCompetitivePosture(fm.GetCompetitivePosture());
                developerFirmStrategiesHandler.SetDistinctiveCompetencies(fm.GetDistinctiveCompetenceis());
                developerFirmStrategiesHandler.SetBusinessPartnerDiversity(fm.GetBusinessPartnerDiversity());
                developerFirmStrategiesHandler.SetContractPriorities(fm.GetContractPriorities());
                developerFirmStrategiesHandler.SetAccountingStrategies(fm.GetFirmAccouningStrategies());
                developerFirmStrategiesHandler.SetGrowthStrategies(fm.GetGrowthStrategies());
                developerFirmStrategiesHandler.SetDevelopmentStrategies(fm.GetDevelopmentStrategies());
            }
            else
            {
                developerFirmContent.SetActive(false);
                providerFirmContent.SetActive(true);
                providerFirmStrategiesHandler.SetMarketsize(fm.GetMarketSize());
                providerFirmStrategiesHandler.SetCompetitivePosture(fm.GetCompetitivePosture());
                providerFirmStrategiesHandler.SetDistinctiveCompetencies(fm.GetDistinctiveCompetenceis());
                providerFirmStrategiesHandler.SetBusinessPartnerDiversity(fm.GetBusinessPartnerDiversity());
                providerFirmStrategiesHandler.SetContractPriorities(fm.GetContractPriorities());
                providerFirmStrategiesHandler.SetAccountingStrategies(fm.GetFirmAccouningStrategies());
                providerFirmStrategiesHandler.SetGrowthStrategies(fm.GetGrowthStrategies());
                providerFirmStrategiesHandler.SetDevelopmentStrategies(fm.GetDevelopmentStrategies());
            }
        }
    }


   
   

}
