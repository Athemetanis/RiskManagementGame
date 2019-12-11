using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalEvaluationUIHandler : MonoBehaviour
{
    public GameObject playerMoneyListContent;
    public GameObject developerMoneyListContent;
    public GameObject providerMoneyListContent;

    public GameObject playerMoneyStatsUIComponentPrefab;

    public GameObject providerAdvertisementContent;
    public GameObject developerReliabilityContent;

    public GameObject playerQuarterStats1Prefab;

    public GameObject developerEmployeeContent;
    public GameObject providerProductContent;

    public GameObject providerProductStatsUIComponentPrefab;
    public GameObject developerEmployeeStatsUIComponentPrefab;

    private GameObject myPlayerDataObject;
    private string gameID;

    void Start()
    {
        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        gameID = myPlayerDataObject.GetComponent<PlayerManager>().GetGameID();

    }

    public void GenerateContent()
    {
        GeneratePlayerMoneyList();
        GenerateDeveloperMoneyList();
        GenerateReliabilityList();
        GenerateEmployeesList();

        GenerateProviderMoneyList();
        GenerateAdvertisementList();
        GenerateProductList();
    }

    public void GeneratePlayerMoneyList()
    {
        foreach (Transform child in playerMoneyListContent.transform)
        { GameObject.Destroy(child.gameObject); }

        Dictionary<string, GameObject> players = GameHandler.allGames[gameID].GetPlayerList();

        foreach (KeyValuePair<string, GameObject> player in players)
        {
            string moneyQ1 = "-";
            string moneyQ2 = "-";
            string moneyQ3 = "-";
            string moneyQ4 = "-";

            PlayerManager playerData = player.Value.GetComponent<PlayerManager>();
            PlayerRoles playerRole = playerData.GetPlayerRole();

            if (playerRole == PlayerRoles.Developer)
            {
                DeveloperAccountingManager devAccMan = player.Value.GetComponent<DeveloperAccountingManager>();

                moneyQ1 = devAccMan.GetEndCashBalanceQuarter(1).ToString("n0");
                moneyQ2 = devAccMan.GetEndCashBalanceQuarter(2).ToString("n0");
                moneyQ3 = devAccMan.GetEndCashBalanceQuarter(3).ToString("n0");
                moneyQ4 = devAccMan.GetEndCashBalanceQuarter(4).ToString("n0");

            }
            else
            {
                ProviderAccountingManager provAccMan = player.Value.GetComponent<ProviderAccountingManager>();

                moneyQ1 = provAccMan.GetEndCashBalanceQuarter(1).ToString("n0");
                moneyQ2 = provAccMan.GetEndCashBalanceQuarter(2).ToString("n0");
                moneyQ3 = provAccMan.GetEndCashBalanceQuarter(3).ToString("n0");
                moneyQ4 = provAccMan.GetEndCashBalanceQuarter(4).ToString("n0");

            }
            GameObject playerMoneyListUIComponent = Instantiate(playerMoneyStatsUIComponentPrefab);
            playerMoneyListUIComponent.transform.SetParent(playerMoneyListContent.transform, false);
            PlayerMoneyStatsUIComponentHandler playerMoneyStatsUIComponentHandler = playerMoneyListUIComponent.GetComponent<PlayerMoneyStatsUIComponentHandler>();

            string firmName = GameHandler.allGames[gameID].GetFirmName(player.Key);
            playerMoneyStatsUIComponentHandler.SetUpPlayerMoneyUIComponent(firmName, moneyQ1, moneyQ2, moneyQ3, moneyQ4);
        }
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

            PlayerManager playerData = developer.Value.GetComponent<PlayerManager>();
            PlayerRoles playerRole = playerData.GetPlayerRole();

            DeveloperAccountingManager devAccMan = developer.Value.GetComponent<DeveloperAccountingManager>();

            moneyQ1 = devAccMan.GetEndCashBalanceQuarter(1).ToString("n0");
            moneyQ2 = devAccMan.GetEndCashBalanceQuarter(2).ToString("n0");
            moneyQ3 = devAccMan.GetEndCashBalanceQuarter(3).ToString("n0");
            moneyQ4 = devAccMan.GetEndCashBalanceQuarter(4).ToString("n0");

            GameObject playerMoneyListUIComponent = Instantiate(playerMoneyStatsUIComponentPrefab);
            playerMoneyListUIComponent.transform.SetParent(developerMoneyListContent.transform, false);
            PlayerMoneyStatsUIComponentHandler playerMoneyStatsUIComponentHandler = playerMoneyListUIComponent.GetComponent<PlayerMoneyStatsUIComponentHandler>();

            string firmName = GameHandler.allGames[gameID].GetFirmName(developer.Key);
            playerMoneyStatsUIComponentHandler.SetUpPlayerMoneyUIComponent(firmName, moneyQ1, moneyQ2, moneyQ3, moneyQ4);
        }
    }
    public void GenerateReliabilityList()
    {
        foreach (Transform child in developerReliabilityContent.transform)
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

            PlayerManager playerData = developer.Value.GetComponent<PlayerManager>();
            PlayerRoles playerRole = playerData.GetPlayerRole();


            ContractManager provContractMan = developer.Value.GetComponent<ContractManager>();

            reliabilityQ1 = provContractMan.GetReliabilityQuater(1).ToString("n0");
            reliabilityQ2 = provContractMan.GetReliabilityQuater(2).ToString("n0");
            reliabilityQ3 = provContractMan.GetReliabilityQuater(3).ToString("n0");
            reliabilityQ4 = provContractMan.GetReliabilityQuater(4).ToString("n0");


            GameObject playerAdvertisementListUIComponent = Instantiate(playerQuarterStats1Prefab);
            playerAdvertisementListUIComponent.transform.SetParent(developerReliabilityContent.transform, false);
            PlayerQuarterStats1UIComponentHandler playerQuarterStats1UIComponentHandler = playerAdvertisementListUIComponent.GetComponent<PlayerQuarterStats1UIComponentHandler>();

            string firmName = GameHandler.allGames[gameID].GetFirmName(developer.Key);
            playerQuarterStats1UIComponentHandler.SetUpPlayerQuartalStatsUIComponent(firmName, reliabilityQ1, reliabilityQ2, reliabilityQ3, reliabilityQ4);
        }
    }
    public void GenerateEmployeesList()
    {
        foreach (Transform child in developerEmployeeContent.transform)
        { GameObject.Destroy(child.gameObject); }


        Dictionary<string, GameObject> developers = GameHandler.allGames[gameID].GetDeveloperList();

        foreach (KeyValuePair<string, GameObject> developer in developers)
        {

            PlayerManager playerData = developer.Value.GetComponent<PlayerManager>();
            PlayerRoles playerRole = playerData.GetPlayerRole();

            HumanResourcesManager humanResourcesManager = developer.Value.GetComponent<HumanResourcesManager>();

            string program = humanResourcesManager.GetProgrammersCountQuarter(4).ToString("n0");
            string integrab = humanResourcesManager.GetIntegrabilitySpecialistsCountQuarter(4).ToString("n0");
            string uiSpec = humanResourcesManager.GetUISpecialistsCountQuarter(4).ToString("n0");


            string programSalary = humanResourcesManager.GetProgrammersSalaryQuarter(4).ToString("n0");
            string integrabSalary = humanResourcesManager.GetIntegrabilitySpecialistSalaryQuarter(4).ToString("n0");
            string uiSpecSalary = humanResourcesManager.GetUISpecialistSalaryQuarter(4).ToString("n0");


            GameObject developerEmployeeStatsUIComp = Instantiate(developerEmployeeStatsUIComponentPrefab);
            developerEmployeeStatsUIComp.transform.SetParent(developerEmployeeContent.transform, false);
            DeveloperFinalEmployeeStatsUIHandler playerQuarterStats1UIComponentHandler = developerEmployeeStatsUIComp.GetComponent<DeveloperFinalEmployeeStatsUIHandler>();

            string firmName = GameHandler.allGames[gameID].GetFirmName(developer.Key);
            playerQuarterStats1UIComponentHandler.SetupFinalProductStats(firmName, program, integrab, uiSpec, programSalary, integrabSalary, uiSpecSalary);
        }
    }


    public void GenerateProviderMoneyList()
    {
        foreach (Transform child in providerMoneyListContent.transform)
        { GameObject.Destroy(child.gameObject); }

        if (GameHandler.allGames[gameID].GetPlayersCount() == 0)
        {
            Debug.Log("game has no player in");
            return;
        }

        Dictionary<string, GameObject> providers = GameHandler.allGames[gameID].GetProviderList();

        foreach (KeyValuePair<string, GameObject> provider in providers)
        {
            string moneyQ1 = "-";
            string moneyQ2 = "-";
            string moneyQ3 = "-";
            string moneyQ4 = "-";

            PlayerManager playerData = provider.Value.GetComponent<PlayerManager>();
            PlayerRoles playerRole = playerData.GetPlayerRole();


            ProviderAccountingManager provAccMan = provider.Value.GetComponent<ProviderAccountingManager>();

            moneyQ1 = provAccMan.GetEndCashBalanceQuarter(1).ToString("n0");
            moneyQ2 = provAccMan.GetEndCashBalanceQuarter(2).ToString("n0");
            moneyQ3 = provAccMan.GetEndCashBalanceQuarter(3).ToString("n0");
            moneyQ4 = provAccMan.GetEndCashBalanceQuarter(4).ToString("n0");

            GameObject providerMoneyListUIComponent = Instantiate(playerMoneyStatsUIComponentPrefab);
            providerMoneyListUIComponent.transform.SetParent(providerMoneyListContent.transform, false);
            PlayerMoneyStatsUIComponentHandler playerMoneyStatsUIComponentHandler = providerMoneyListUIComponent.GetComponent<PlayerMoneyStatsUIComponentHandler>();

            string firmName = GameHandler.allGames[gameID].GetFirmName(provider.Key);
            playerMoneyStatsUIComponentHandler.SetUpPlayerMoneyUIComponent(firmName, moneyQ1, moneyQ2, moneyQ3, moneyQ4);
        }
    }
    public void GenerateAdvertisementList()
    {
        foreach (Transform child in providerAdvertisementContent.transform)
        { GameObject.Destroy(child.gameObject); }

        if (GameHandler.allGames[gameID].GetPlayersCount() == 0)
        {
            Debug.Log("game has no player in");
            return;
        }

        Dictionary<string, GameObject> providers = GameHandler.allGames[gameID].GetProviderList();

        foreach (KeyValuePair<string, GameObject> provider in providers)
        {
            string advQ1 = "-";
            string advQ2 = "-";
            string advQ3 = "-";
            string advQ4 = "-";

            PlayerManager playerData = provider.Value.GetComponent<PlayerManager>();
            PlayerRoles playerRole = playerData.GetPlayerRole();


            MarketingManager provMarMan = provider.Value.GetComponent<MarketingManager>();

            advQ1 = provMarMan.GetAdvertismenetCoverageQuarters(1).ToString("n0");
            advQ2 = provMarMan.GetAdvertismenetCoverageQuarters(2).ToString("n0");
            advQ3 = provMarMan.GetAdvertismenetCoverageQuarters(3).ToString("n0");
            advQ4 = provMarMan.GetAdvertismenetCoverageQuarters(4).ToString("n0");

            GameObject playerAdvertisementListUIComponent = Instantiate(playerQuarterStats1Prefab);
            playerAdvertisementListUIComponent.transform.SetParent(providerAdvertisementContent.transform, false);
            PlayerQuarterStats1UIComponentHandler playerQuarterStats1UIComponentHandler = playerAdvertisementListUIComponent.GetComponent<PlayerQuarterStats1UIComponentHandler>();

            string firmName = GameHandler.allGames[gameID].GetFirmName(provider.Key);
            playerQuarterStats1UIComponentHandler.SetUpPlayerQuartalStatsUIComponent(firmName, advQ1, advQ2, advQ3, advQ4);
        }
    }
    public void GenerateProductList()
    {
        foreach (Transform child in providerProductContent.transform)
        { GameObject.Destroy(child.gameObject); }
        if (GameHandler.allGames[gameID].GetPlayersCount() == 0)
        {
            Debug.Log("game has no player in");
            return;
        }

        Dictionary<string, GameObject> providers = GameHandler.allGames[gameID].GetProviderList();

        foreach (KeyValuePair<string, GameObject> provider in providers)
        {
            ProductManager providerProductManager = provider.Value.GetComponent<ProductManager>();
            MarketingManager marketingManager = provider.Value.GetComponent<MarketingManager>();
            CustomersManager customersManager = provider.Value.GetComponent<CustomersManager>();

            string functionality = "-";
            string integrability = "-";
            string userFriendliness = "-";
            string enterprisePrice = "-";
            string businessPrice = "-";
            string IndividualPrice = "-";
            string enterpriseCustomers = "-";
            string busineessCustomers = "-";
            string individualCustomers = "-";


            functionality = providerProductManager.GetFunctionalityQuarter(4).ToString("n0");
            integrability = providerProductManager.GetIntegrabilityQuarter(4).ToString("n0");
            userFriendliness = providerProductManager.GetUserFriendlinessQuarter(4).ToString("n0");

            enterprisePrice = marketingManager.GetEnterprisePriceQuarter(4).ToString("n0");
            businessPrice = marketingManager.GetBusinessPriceQuarter(4).ToString("n0");
            IndividualPrice = marketingManager.GetIndividualPriceQuarter(4).ToString("n0");


            enterpriseCustomers = customersManager.GetEnterpriseCustomersQ(4).ToString("n0");
            busineessCustomers = customersManager.GetBusinesCustomersQ(4).ToString("n0");
            individualCustomers = customersManager.GetIndividualCustomersQ(4).ToString("n0");

            GameObject playerProductStatsUIComponent = Instantiate(providerProductStatsUIComponentPrefab);
            playerProductStatsUIComponent.transform.SetParent(providerProductContent.transform, false);
            ProviderFinalProductsStatsUIHandler handler = playerProductStatsUIComponent.GetComponent<ProviderFinalProductsStatsUIHandler>();

            string firmName = GameHandler.allGames[gameID].GetFirmName(provider.Key);
            handler.SetupFinalProductStats(firmName, functionality, integrability, userFriendliness, enterprisePrice, businessPrice, IndividualPrice, enterpriseCustomers, busineessCustomers, individualCustomers);
        }
    }

}
