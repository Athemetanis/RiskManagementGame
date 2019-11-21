using System.Collections.Generic;
using UnityEngine;

public class InstructorDeveloperStatsUIHandler : MonoBehaviour
{
    public GameObject playerMoneyListContent;
    public GameObject playerMoneyStatsUIComponentPrefab;

    public GameObject playerQuarterStats1Prefab;
    public GameObject playerReliabilityContent;

    public GameObject playerQuarterStats3Prefab;
    public GameObject playerEmployeesStatsContent;
    public GameObject playerSalariesStatsContent;

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
        if (quarter == 1)
        {
            //InfoText.enabled = true;
            return;
        }
        //InfoText.enabled = false;
        GenerateDeveloperMoneyList();
        GenerateReliabilityList();
        GenerateEmploeesList();
        GenerateSalariesList();

    }

    public void GenerateDeveloperMoneyList()
    {
        foreach (Transform child in playerMoneyListContent.transform)
        { GameObject.Destroy(child.gameObject); }


        if (GameHandler.allGames[gameID].GetPlayersCount() == 0)
        {
            Debug.Log("game has no player in");
            return;
        }

        Dictionary<string, GameObject> developers = GameHandler.allGames[gameID].GetDeveloperList();

        foreach (KeyValuePair<string, GameObject> developer in developers)
        {
            string moneyQ1 = "-";
            string moneyQ2 = "-";
            string moneyQ3 = "-";
            string moneyQ4 = "-";

            PlayerData playerData = developer.Value.GetComponent<PlayerData>();
            PlayerRoles playerRole = playerData.GetPlayerRole();

            DeveloperAccountingManager devAccMan = developer.Value.GetComponent<DeveloperAccountingManager>();
            if (quarter > 1)
            {
                moneyQ1 = devAccMan.GetEndCashBalanceQuarter(1).ToString("n0");
            }
            if (quarter > 2)
            {
                moneyQ2 = devAccMan.GetEndCashBalanceQuarter(2).ToString("n0");
            }
            if (quarter > 3)
            {
                moneyQ3 = devAccMan.GetEndCashBalanceQuarter(3).ToString("n0");
            }
            if (quarter > 4)
            {
                moneyQ4 = devAccMan.GetEndCashBalanceQuarter(4).ToString("n0");
            }


            GameObject playerMoneyListUIComponent = Instantiate(playerMoneyStatsUIComponentPrefab);
            playerMoneyListUIComponent.transform.SetParent(playerMoneyListContent.transform, false);
            PlayerMoneyStatsUIComponentHandler playerMoneyStatsUIComponentHandler = playerMoneyListUIComponent.GetComponent<PlayerMoneyStatsUIComponentHandler>();

            string firmName = GameHandler.allGames[gameID].GetFirmName(developer.Key);
            playerMoneyStatsUIComponentHandler.SetUpPlayerMoneyUIComponent(firmName, moneyQ1, moneyQ2, moneyQ3, moneyQ4);
        }
    }

    public void GenerateReliabilityList()
    {
        foreach (Transform child in playerReliabilityContent.transform)
        { GameObject.Destroy(child.gameObject); }

        if (GameHandler.allGames[gameID].GetPlayersCount() == 0)
        {
            Debug.Log("game has no player in");
            return;
        }

        Dictionary<string, GameObject> developers = GameHandler.allGames[gameID].GetDeveloperList();

        foreach (KeyValuePair<string, GameObject> developer in developers)
        {
            string reliabilityQ1 = "-";
            string reliabilityQ2 = "-";
            string reliabilityQ3 = "-";
            string reliabilityQ4 = "-";

            PlayerData playerData = developer.Value.GetComponent<PlayerData>();
            PlayerRoles playerRole = playerData.GetPlayerRole();


            ContractManager provContractMan = developer.Value.GetComponent<ContractManager>();
            if (quarter > 1)
            {
                reliabilityQ1 = provContractMan.GetReliabilityQuater(1).ToString("n0");
            }
            if (quarter > 2)
            {
                reliabilityQ2 = provContractMan.GetReliabilityQuater(2).ToString("n0");
            }
            if (quarter > 3)
            {
                reliabilityQ3 = provContractMan.GetReliabilityQuater(3).ToString("n0");
            }
            if (quarter > 4)
            {
                reliabilityQ4 = provContractMan.GetReliabilityQuater(4).ToString("n0");
            }

            GameObject playerAdvertisementListUIComponent = Instantiate(playerQuarterStats1Prefab);
            playerAdvertisementListUIComponent.transform.SetParent(playerReliabilityContent.transform, false);
            PlayerQuarterStats1UIComponentHandler playerQuarterStats1UIComponentHandler = playerAdvertisementListUIComponent.GetComponent<PlayerQuarterStats1UIComponentHandler>();

            string firmName = GameHandler.allGames[gameID].GetFirmName(developer.Key);
            playerQuarterStats1UIComponentHandler.SetUpPlayerQuartalStatsUIComponent(firmName, reliabilityQ1, reliabilityQ2, reliabilityQ3, reliabilityQ4);
        }
    }

    public void GenerateEmploeesList()
    {
        foreach (Transform child in playerEmployeesStatsContent.transform)
        { GameObject.Destroy(child.gameObject); }

        if (GameHandler.allGames[gameID].GetPlayersCount() == 0)
        {
            Debug.Log("game has no player in");
            return;
        }

        Dictionary<string, GameObject> developers = GameHandler.allGames[gameID].GetDeveloperList();

        foreach (KeyValuePair<string, GameObject> developer in developers)
        {
            HumanResourcesManager providerHumanResourcesManager = developer.Value.GetComponent<HumanResourcesManager>();

            string programmersQ1 = "-";
            string intSpecQ1 = "-";
            string uiSpecQ1 = "-";
            string programmersQ2 = "-";
            string intSpecQ2 = "-";
            string uiSpecQ2 = "-";
            string programmersQ3 = "-";
            string intSpecQ3 = "-";
            string uiSpecQ3 = "-";
            string programmersQ4 = "-";
            string intSpecQ4 = "-";
            string uiSpecQ4 = "-";

            if (quarter > 1)
            {
                programmersQ1 = providerHumanResourcesManager.GetProgrammersCountQuarter(1).ToString("n0");
                intSpecQ1 = providerHumanResourcesManager.GetIntegrabilitySpecialistsCountQuarter(1).ToString("n0");
                uiSpecQ1 = providerHumanResourcesManager.GetUISpecialistsCountQuarter(1).ToString("n0");
            }
            if (quarter > 2)
            {
                programmersQ2 = providerHumanResourcesManager.GetProgrammersCountQuarter(2).ToString("n0");
                intSpecQ2 = providerHumanResourcesManager.GetIntegrabilitySpecialistsCountQuarter(2).ToString("n0");
                uiSpecQ2 = providerHumanResourcesManager.GetUISpecialistsCountQuarter(2).ToString("n0");
            }
            if (quarter > 3)
            {
                programmersQ3 = providerHumanResourcesManager.GetProgrammersCountQuarter(3).ToString("n0");
                intSpecQ3 = providerHumanResourcesManager.GetIntegrabilitySpecialistsCountQuarter(3).ToString("n0");
                uiSpecQ3 = providerHumanResourcesManager.GetUISpecialistsCountQuarter(3).ToString("n0");
            }
            if (quarter > 4)
            {
                programmersQ4 = providerHumanResourcesManager.GetProgrammersCountQuarter(4).ToString("n0");
                intSpecQ4 = providerHumanResourcesManager.GetIntegrabilitySpecialistsCountQuarter(4).ToString("n0");
                uiSpecQ4 = providerHumanResourcesManager.GetUISpecialistsCountQuarter(4).ToString("n0");
            }


            GameObject playerEmployeesStatsUIComponent = Instantiate(playerQuarterStats3Prefab);
            playerEmployeesStatsUIComponent.transform.SetParent(playerEmployeesStatsContent.transform, false);
            PlayerQuarterStats3UIComponent handler = playerEmployeesStatsUIComponent.GetComponent<PlayerQuarterStats3UIComponent>();

            string firmName = GameHandler.allGames[gameID].GetFirmName(developer.Key);
            handler.SetUpPlayerStatsQ1(firmName, programmersQ1, intSpecQ1, uiSpecQ1);
            handler.SetUpPlayerStatsQ2(firmName, programmersQ2, intSpecQ2, uiSpecQ2);
            handler.SetUpPlayerStatsQ3(firmName, programmersQ3, intSpecQ3, uiSpecQ3);
            handler.SetUpPlayerStatsQ4(firmName, programmersQ4, intSpecQ4, uiSpecQ4);
        }
    }

    public void GenerateSalariesList()
    {
        foreach (Transform child in playerSalariesStatsContent.transform)
        { GameObject.Destroy(child.gameObject); }

        if (GameHandler.allGames[gameID].GetPlayersCount() == 0)
        {
            Debug.Log("game has no player in");
            return;
        }

        Dictionary<string, GameObject> developers = GameHandler.allGames[gameID].GetDeveloperList();

        foreach (KeyValuePair<string, GameObject> developer in developers)
        {
            HumanResourcesManager providerHumanResourcesManager = developer.Value.GetComponent<HumanResourcesManager>();

            string programmersQ1 = "-";
            string intSpecQ1 = "-";
            string uiSpecQ1 = "-";
            string programmersQ2 = "-";
            string intSpecQ2 = "-";
            string uiSpecQ2 = "-";
            string programmersQ3 = "-";
            string intSpecQ3 = "-";
            string uiSpecQ3 = "-";
            string programmersQ4 = "-";
            string intSpecQ4 = "-";
            string uiSpecQ4 = "-";

            if (quarter > 1)
            {
                programmersQ1 = providerHumanResourcesManager.GetProgrammersSalaryQuarter(1).ToString("n0");
                intSpecQ1 = providerHumanResourcesManager.GetIntegrabilitySpecialistSalaryQuarter(1).ToString("n0");
                uiSpecQ1 = providerHumanResourcesManager.GetUISpecialistSalaryQuarter(1).ToString("n0");
            }
            if (quarter > 2)
            {
                programmersQ2 = providerHumanResourcesManager.GetProgrammersSalaryQuarter(2).ToString("n0");
                intSpecQ2 = providerHumanResourcesManager.GetIntegrabilitySpecialistSalaryQuarter(2).ToString("n0");
                uiSpecQ2 = providerHumanResourcesManager.GetUISpecialistSalaryQuarter(2).ToString("n0");
            }
            if (quarter > 3)
            {
                programmersQ3 = providerHumanResourcesManager.GetProgrammersSalaryQuarter(3).ToString("n0");
                intSpecQ3 = providerHumanResourcesManager.GetIntegrabilitySpecialistSalaryQuarter(3).ToString("n0");
                uiSpecQ3 = providerHumanResourcesManager.GetUISpecialistSalaryQuarter(3).ToString("n0");
            }
            if (quarter > 4)
            {
                programmersQ4 = providerHumanResourcesManager.GetProgrammersSalaryQuarter(4).ToString("n0");
                intSpecQ4 = providerHumanResourcesManager.GetIntegrabilitySpecialistSalaryQuarter(4).ToString("n0");
                uiSpecQ4 = providerHumanResourcesManager.GetUISpecialistSalaryQuarter(4).ToString("n0");
            }


            GameObject playerSalariesUIComponent = Instantiate(playerQuarterStats3Prefab);
            playerSalariesUIComponent.transform.SetParent(playerSalariesStatsContent.transform, false);
            PlayerQuarterStats3UIComponent handler = playerSalariesUIComponent.GetComponent<PlayerQuarterStats3UIComponent>();

            string firmName = GameHandler.allGames[gameID].GetFirmName(developer.Key);
            handler.SetUpPlayerStatsQ1(firmName, programmersQ1, intSpecQ1, uiSpecQ1);
            handler.SetUpPlayerStatsQ2(firmName, programmersQ2, intSpecQ2, uiSpecQ2);
            handler.SetUpPlayerStatsQ3(firmName, programmersQ3, intSpecQ3, uiSpecQ3);
            handler.SetUpPlayerStatsQ4(firmName, programmersQ4, intSpecQ4, uiSpecQ4);
        }
    }

}
