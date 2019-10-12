using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum PlayerRoles { Developer, Provider }

public class PlayerData : NetworkBehaviour {

    /// This class contains general information about player - variables are set during creation and do not change during game!

    //VARIABLES 
    [SyncVar]
    private string gameID;
    [SyncVar]
    private string playerID;
    [SyncVar]
    private PlayerRoles playerRole;

    //LOCAL VARIABLES
    private GameObject playerUI;

    //REFERENCES - MANAGERS
    private ContractManager contractMamanger;
    private FeatureManager featureManager;      //provider only
    private ProductManager productManager;      //provider only
    private CustomersManager customerManager;   //provider only
    private MarketingManager marketingManager;  //provider only
    private ScheduleManager scheduleManager;    //developer only
    private HumanResourcesManager humanResourcesManager; //developer only
    private RiskManager riskManager;
    private SubmitDataManager submitDataManager;
    private ProviderAccountingManager providerAccountingManager;    //provider only
    private DeveloperAccountingManager developerAccountingManager; //developer only


    //GETTERS & SETTERS
    public void SetGameID(string gameID) { this.gameID = gameID; }
    public string GetGameID() { return gameID; }

    public void SetPlayerID(string playerID) { this.playerID = playerID; }
    public string GetPlayerID() { return playerID; }

    public void SetPlayerRole(PlayerRoles playerRole) { this.playerRole = playerRole; }
    public PlayerRoles GetPlayerRole() { return playerRole; }

    public void SetPlayerUI(GameObject playerUI) { this.playerUI = playerUI; }
    public GameObject GetPlayerUI() { return playerUI;  }

    //METHODS
    void Start()
    {   
        if (GameHandler.allPlayers[gameID].ContainsKey(playerID) == false)
        {
            Debug.Log("player registered to all players: " + playerID);
            GameHandler.allPlayers[gameID].Add(playerID, this.gameObject);
        }

        if (GameHandler.allGames[gameID] == true)
        {
            Debug.Log("player tries to be added into game " + gameID);
            GameHandler.allGames[gameID].AddPlayerToGame(this.gameObject);
            
        }

        contractMamanger = this.gameObject.GetComponent<ContractManager>();
        featureManager = this.gameObject.GetComponent<FeatureManager>();
        productManager = this.gameObject.GetComponent<ProductManager>();
        customerManager = this.gameObject.GetComponent<CustomersManager>();
        marketingManager = this.gameObject.GetComponent<MarketingManager>();
        scheduleManager = this.gameObject.GetComponent<ScheduleManager>();
        humanResourcesManager = this.gameObject.GetComponent<HumanResourcesManager>();
        riskManager = this.gameObject.GetComponent<RiskManager>();
        submitDataManager = this.gameObject.GetComponent<SubmitDataManager>();
        providerAccountingManager = this.gameObject.GetComponent<ProviderAccountingManager>();
        developerAccountingManager = this.gameObject.GetComponent<DeveloperAccountingManager>();

    }


    public void MoveToNextQuarter()
    {
        //1. akutalizuj realne data na zaklade kontrakt managera
        //2.posun sa do noveho kvartalu v gameData????
        //2. prirad nove reference na objekty nadchadzajuceho Q a aktualizuj ich?
        if(playerRole == PlayerRoles.Developer)
        {
            contractMamanger.EvaluateContractsServer();
            developerAccountingManager.UpdateCurrentQuarterDataServer(); 
        }
        else //provider
        {
            featureManager.UpdateCurrentQuarterData();
            productManager.UpdateCurrentQuarterData();
            customerManager.UpdateCurrentQuarterData();
            providerAccountingManager.UpdateCurrentQuarterData();

        }


    }

}
