using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructorProviderStatsUIHandler : MonoBehaviour
{
    public GameObject playerMoneyListContent;
    public GameObject playerMoneyStatsUIComponentPrefab;

    public GameObject playerQuarterStats1Prefab;
    public GameObject playerAdvertisementContent;

    public GameObject playerQuarterStats3Prefab;
    public GameObject playerProductStatsContent;
    public GameObject playerCustomersStatsContent;
    public GameObject playerPricesStatsContent;



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
        GenerateProviderMoneyList();
    }

    public void GenerateProviderMoneyList()
    {
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

            PlayerData playerData = provider.Value.GetComponent<PlayerData>();
            PlayerRoles playerRole = playerData.GetPlayerRole();


            ProviderAccountingManager provAccMan = provider.Value.GetComponent<ProviderAccountingManager>();
            if (quarter > 1)
            {
                moneyQ1 = provAccMan.GetEndCashBalanceQuarter(1).ToString("n0");
            }
            if (quarter > 2)
            {
                moneyQ2 = provAccMan.GetEndCashBalanceQuarter(2).ToString("n0");
            }
            if (quarter > 3)
            {
                moneyQ3 = provAccMan.GetEndCashBalanceQuarter(3).ToString("n0");
            }
            if (quarter > 4)
            {
                moneyQ4 = provAccMan.GetEndCashBalanceQuarter(4).ToString("n0");
            }

            GameObject playerMoneyListUIComponent = Instantiate(playerMoneyStatsUIComponentPrefab);
            playerMoneyListUIComponent.transform.SetParent(playerMoneyListContent.transform, false);
            PlayerMoneyStatsUIComponentHandler playerMoneyStatsUIComponentHandler = playerMoneyListUIComponent.GetComponent<PlayerMoneyStatsUIComponentHandler>();

            string firmName = GameHandler.allGames[gameID].GetFirmName(provider.Key);
            playerMoneyStatsUIComponentHandler.SetUpPlayerMoneyUIComponent(firmName, moneyQ1, moneyQ2, moneyQ3, moneyQ4);
        }
    }

    public void GenerateAdvertisementList()
    {
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

            PlayerData playerData = provider.Value.GetComponent<PlayerData>();
            PlayerRoles playerRole = playerData.GetPlayerRole();


            MarketingManager provMarMan = provider.Value.GetComponent<MarketingManager>();
            if (quarter > 1)
            {
                advQ1 = provMarMan.GetAdvertismenetCoverageQuarters(1).ToString("n0");
            }
            if (quarter > 2)
            {
                advQ2 = provMarMan.GetAdvertismenetCoverageQuarters(2).ToString("n0");
            }
            if (quarter > 3)
            {
                advQ3 = provMarMan.GetAdvertismenetCoverageQuarters(3).ToString("n0");
            }
            if (quarter > 4)
            {
                advQ4 = provMarMan.GetAdvertismenetCoverageQuarters(4).ToString("n0");
            }

            GameObject playerAdvertisementListUIComponent = Instantiate(playerQuarterStats1Prefab);
            playerAdvertisementListUIComponent.transform.SetParent(playerAdvertisementContent.transform, false);
            PlayerQuarterStats1UIComponentHandler playerQuarterStats1UIComponentHandler = playerAdvertisementListUIComponent.GetComponent<PlayerQuarterStats1UIComponentHandler>();

            string firmName = GameHandler.allGames[gameID].GetFirmName(provider.Key);
            playerQuarterStats1UIComponentHandler.SetUpPlayerQuartalStatsUIComponent(firmName, advQ1, advQ2, advQ3, advQ4);
        }
    }

    public void GenerateProductList()
    {
        if (GameHandler.allGames[gameID].GetPlayersCount() == 0)
        {
            Debug.Log("game has no player in");
            return;
        }

        Dictionary<string, GameObject> providers = GameHandler.allGames[gameID].GetProviderList();

        foreach (KeyValuePair<string, GameObject> provider in providers)
        {
            ProductManager providerProductManager = provider.Value.GetComponent<ProductManager>();

            string functionalityQ1 = "-";
            string integrabilityQ1 = "-";
            string userFriendlinessQ1 = "-";
            string functionalityQ2 = "-";
            string integrabilityQ2 = "-";
            string userFriendlinessQ2 = "-";
            string functionalityQ3 = "-";
            string integrabilityQ3 = "-";
            string userFriendlinessQ3 = "-";
            string functionalityQ4 = "-";
            string integrabilityQ4 = "-";
            string userFriendlinessQ4 = "-";

            if (quarter > 1)
            {
                functionalityQ1 = providerProductManager.GetFunctionalityQuarter(1).ToString("n0");
                integrabilityQ1 = providerProductManager.GetIntegrabilityQuarter(1).ToString("n0");
                userFriendlinessQ1 = providerProductManager.GetUserFriendlinessQuarter(1).ToString("n0");
            }
            if (quarter > 2)
            {
                functionalityQ2 = providerProductManager.GetFunctionalityQuarter(2).ToString("n0");
                integrabilityQ2 = providerProductManager.GetIntegrabilityQuarter(2).ToString("n0");
                userFriendlinessQ2 = providerProductManager.GetUserFriendlinessQuarter(2).ToString("n0");
            }
            if (quarter > 3)
            {
                functionalityQ3 = providerProductManager.GetFunctionalityQuarter(3).ToString("n0");
                integrabilityQ3 = providerProductManager.GetIntegrabilityQuarter(3).ToString("n0");
                userFriendlinessQ3 = providerProductManager.GetUserFriendlinessQuarter(3).ToString("n0");
            }
            if (quarter > 4)
            {
                functionalityQ4 = providerProductManager.GetFunctionalityQuarter(4).ToString("n0");
                integrabilityQ4 = providerProductManager.GetIntegrabilityQuarter(4).ToString("n0");
                userFriendlinessQ4 = providerProductManager.GetUserFriendlinessQuarter(4).ToString("n0");
            }


            GameObject playerProductStatsUIComponent = Instantiate(playerQuarterStats3Prefab);
            playerProductStatsUIComponent.transform.SetParent(playerProductStatsContent.transform, false);
            PlayerQuarterStats3UIComponent handler = playerProductStatsUIComponent.GetComponent<PlayerQuarterStats3UIComponent>();

            string firmName = GameHandler.allGames[gameID].GetFirmName(provider.Key);
            handler.SetUpPlayerStatsQ1(firmName, functionalityQ1, integrabilityQ1, userFriendlinessQ1);
            handler.SetUpPlayerStatsQ2(firmName, functionalityQ2, integrabilityQ2, userFriendlinessQ2);
            handler.SetUpPlayerStatsQ3(firmName, functionalityQ3, integrabilityQ3, userFriendlinessQ3);
            handler.SetUpPlayerStatsQ4(firmName, functionalityQ4, integrabilityQ4, userFriendlinessQ4);
        }
    }

    public void GenerateCustomersList()
    {
        if (GameHandler.allGames[gameID].GetPlayersCount() == 0)
        {
            Debug.Log("game has no player in");
            return;
        }

        Dictionary<string, GameObject> providers = GameHandler.allGames[gameID].GetProviderList();

        foreach (KeyValuePair<string, GameObject> provider in providers)
        {
            CustomersManager providerCutomersManager = provider.Value.GetComponent<CustomersManager>();

            string enterpriseQ1 = "-";
            string businessQ1 = "-";
            string individualsQ1 = "-";
            string enterpriseQ2 = "-";
            string businessQ2 = "-";
            string individualsQ2 = "-";
            string enterpriseQ3 = "-";
            string businessQ3 = "-";
            string individualsQ3 = "-";
            string enterpriseQ4 = "-";
            string businessQ4 = "-";
            string individualsQ4 = "-";

            if (quarter > 1)
            {
                enterpriseQ1 = providerCutomersManager.GetEnterpriseCustomersQ(1).ToString("n0");
                businessQ1 = providerCutomersManager.GetBusinesCustomersQ(1).ToString("n0");
                individualsQ1 = providerCutomersManager.GetIndividualCustomersQ(1).ToString("n0");
            }
            if (quarter > 2)
            {
                enterpriseQ2 = providerCutomersManager.GetEnterpriseCustomersQ(2).ToString("n0");
                businessQ2 = providerCutomersManager.GetBusinesCustomersQ(2).ToString("n0");
                individualsQ2 = providerCutomersManager.GetIndividualCustomersQ(2).ToString("n0");
            }
            if (quarter > 3)
            {
                enterpriseQ3 = providerCutomersManager.GetEnterpriseCustomersQ(3).ToString("n0");
                businessQ3 = providerCutomersManager.GetBusinesCustomersQ(3).ToString("n0");
                individualsQ3 = providerCutomersManager.GetIndividualCustomersQ(3).ToString("n0");
            }
            if (quarter > 4)
            {
                enterpriseQ4 = providerCutomersManager.GetEnterpriseCustomersQ(4).ToString("n0");
                businessQ4 = providerCutomersManager.GetBusinesCustomersQ(4).ToString("n0");
                individualsQ4 = providerCutomersManager.GetIndividualCustomersQ(4).ToString("n0");
            }


            GameObject playerCustomersStatsUIComponent = Instantiate(playerQuarterStats3Prefab);
            playerCustomersStatsUIComponent.transform.SetParent(playerCustomersStatsContent.transform, false);
            PlayerQuarterStats3UIComponent handler = playerCustomersStatsUIComponent.GetComponent<PlayerQuarterStats3UIComponent>();

            string firmName = GameHandler.allGames[gameID].GetFirmName(provider.Key);
            handler.SetUpPlayerStatsQ1(firmName, enterpriseQ1, businessQ1, individualsQ1);
            handler.SetUpPlayerStatsQ2(firmName, enterpriseQ2, businessQ2, individualsQ2);
            handler.SetUpPlayerStatsQ3(firmName, enterpriseQ3, businessQ3, individualsQ3);
            handler.SetUpPlayerStatsQ4(firmName, enterpriseQ4, businessQ4, individualsQ4);
        }
    }


    public void GeneratePriceList()
    {
        if (GameHandler.allGames[gameID].GetPlayersCount() == 0)
        {
            Debug.Log("game has no player in");
            return;
        }

        Dictionary<string, GameObject> providers = GameHandler.allGames[gameID].GetProviderList();

        foreach (KeyValuePair<string, GameObject> provider in providers)
        {
            MarketingManager providerMarketingManager = provider.Value.GetComponent<MarketingManager>();

            string enterpriseQ1 = "-";
            string businessQ1 = "-";
            string individualsQ1 = "-";
            string enterpriseQ2 = "-";
            string businessQ2 = "-";
            string individualsQ2 = "-";
            string enterpriseQ3 = "-";
            string businessQ3 = "-";
            string individualsQ3 = "-";
            string enterpriseQ4 = "-";
            string businessQ4 = "-";
            string individualsQ4 = "-";

            if (quarter > 1)
            {
                enterpriseQ1 = providerMarketingManager.GetEnterprisePriceQuarter(1).ToString("n0");
                businessQ1 = providerMarketingManager.GetBusinessPriceQuarter(1).ToString("n0");
                individualsQ1 = providerMarketingManager.GetIndividualPriceQuarter(1).ToString("n0");
            }
            if (quarter > 2)
            {
                enterpriseQ2 = providerMarketingManager.GetEnterprisePriceQuarter(2).ToString("n0");
                businessQ2 = providerMarketingManager.GetBusinessPriceQuarter(2).ToString("n0");
                individualsQ2 = providerMarketingManager.GetIndividualPriceQuarter(2).ToString("n0");
            }
            if (quarter > 3)
            {
                enterpriseQ3 = providerMarketingManager.GetEnterprisePriceQuarter(3).ToString("n0");
                businessQ3 = providerMarketingManager.GetBusinessPriceQuarter(3).ToString("n0");
                individualsQ3 = providerMarketingManager.GetIndividualPriceQuarter(3).ToString("n0");
            }
            if (quarter > 4)
            {
                enterpriseQ4 = providerMarketingManager.GetEnterprisePriceQuarter(4).ToString("n0");
                businessQ4 = providerMarketingManager.GetBusinessPriceQuarter(4).ToString("n0");
                individualsQ4 = providerMarketingManager.GetIndividualPriceQuarter(4).ToString("n0");
            }


            GameObject playerCustomersStatsUIComponent = Instantiate(playerQuarterStats3Prefab);
            playerCustomersStatsUIComponent.transform.SetParent(playerPricesStatsContent.transform, false);
            PlayerQuarterStats3UIComponent handler = playerCustomersStatsUIComponent.GetComponent<PlayerQuarterStats3UIComponent>();

            string firmName = GameHandler.allGames[gameID].GetFirmName(provider.Key);
            handler.SetUpPlayerStatsQ1(firmName, enterpriseQ1, businessQ1, individualsQ1);
            handler.SetUpPlayerStatsQ2(firmName, enterpriseQ2, businessQ2, individualsQ2);
            handler.SetUpPlayerStatsQ3(firmName, enterpriseQ3, businessQ3, individualsQ3);
            handler.SetUpPlayerStatsQ4(firmName, enterpriseQ4, businessQ4, individualsQ4);
        }
    }
}
