using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstructorAllPlayersStatsUIHandler : MonoBehaviour
{
    public TextMeshProUGUI InfoText;

    public GameObject playerMoneyListContent;
    public GameObject playerMoneyStatsUIComponentPrefab;

    public GameObject contractsListContent;
    public GameObject contractsProvidersNamesContent;
    public GameObject contractDeveloperUIComponentPrefab;
    public GameObject contractProviderNameTextComponentPrefab;

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
        if(quarter == 1)
        {
            InfoText.enabled = true;
            return;
        }
        InfoText.enabled = false;
        GeneratePlayerMoneyList();
        GenerateContractTable();
    }

    public void GeneratePlayerMoneyList()
    {
        foreach (Transform child in playerMoneyListContent.transform)
        { GameObject.Destroy(child.gameObject); }

        if (GameHandler.allGames[gameID].GetPlayersCount() == 0)
        {
            Debug.Log("game has no player in");
            return;
        }

        Dictionary<string, GameObject> players = GameHandler.allGames[gameID].GetPlayerList();

        foreach( KeyValuePair<string, GameObject> player in players)
        {
            string moneyQ1 = "-";
            string moneyQ2 = "-";
            string moneyQ3 = "-";
            string moneyQ4 = "-";

            PlayerManager playerData = player.Value.GetComponent<PlayerManager>();
            PlayerRoles playerRole = playerData.GetPlayerRole();

            if(playerRole == PlayerRoles.Developer)
            {
                DeveloperAccountingManager devAccMan = player.Value.GetComponent<DeveloperAccountingManager>();

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
            }
            else
            {
                ProviderAccountingManager provAccMan = player.Value.GetComponent<ProviderAccountingManager>();
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
            }
            GameObject playerMoneyListUIComponent = Instantiate(playerMoneyStatsUIComponentPrefab);
            playerMoneyListUIComponent.transform.SetParent(playerMoneyListContent.transform, false);
            PlayerMoneyStatsUIComponentHandler playerMoneyStatsUIComponentHandler = playerMoneyListUIComponent.GetComponent<PlayerMoneyStatsUIComponentHandler>();

            string firmName = GameHandler.allGames[gameID].GetFirmName(player.Key);
            playerMoneyStatsUIComponentHandler.SetUpPlayerMoneyUIComponent(firmName,moneyQ1, moneyQ2, moneyQ3, moneyQ4);
        }   
    }

    public void GenerateContractTable()
    {
        foreach (Transform child in contractsListContent.transform)
        { GameObject.Destroy(child.gameObject); }

        foreach (Transform child in contractsProvidersNamesContent.transform)
        { GameObject.Destroy(child.gameObject); }

        if (GameHandler.allGames[gameID].GetPlayersCount() == 0)
        {
            Debug.Log("game has no player in");
            return;
        }

        Dictionary<string, GameObject> providers = GameHandler.allGames[gameID].GetProviderList();
        Dictionary<string, GameObject> developers = GameHandler.allGames[gameID].GetDeveloperList();
        Dictionary<string, int> providerContracts = new Dictionary<string, int>() { };

        foreach (string provider in providers.Keys)
        {
            providerContracts.Add(provider, 0);

            string providerFirmName = GameHandler.allGames[gameID].GetFirmName(provider);

            GameObject providerFirmUIText = Instantiate(contractProviderNameTextComponentPrefab);
            providerFirmUIText.transform.SetParent(contractsProvidersNamesContent.transform, false);
            providerFirmUIText.GetComponent<ContractProviderNameTextHandler>().SetProviderName(providerFirmName);

        }        
        foreach(KeyValuePair<string, GameObject> developer in developers)
        {
            string developerFirmName = GameHandler.allGames[gameID].GetFirmName(developer.Key);

            Dictionary<string, string> contracts = developer.Value.GetComponent<ContractManager>().GetMyContractProviderHistory();

            foreach (KeyValuePair<string, string> contract in contracts)
            {   
                providerContracts[contract.Value] += 1; 
            }

            GameObject developerUIComponent = Instantiate(contractDeveloperUIComponentPrefab);
            developerUIComponent.transform.SetParent(contractsListContent.transform, false);
            ContractDeveloperUIComponentHandler contractDeveloperUIComponentHandler =  developerUIComponent.GetComponent<ContractDeveloperUIComponentHandler>();
            contractDeveloperUIComponentHandler.SetDevelopersFirmName(developerFirmName);
            foreach( KeyValuePair<string,int> contractCount in providerContracts)
            {
                contractDeveloperUIComponentHandler.CreateNewContractCountValue(contractCount.Value);
            }
        }
    }

}
