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
    private ChatManager chatManager;
    private FirmManager firmManager;
    private EventManager eventManager;
    private ContractManager contractMamanger;
    private RiskManager riskManager;
    private SubmitDataManager submitDataManager;
    private ResearchManager researchManager;
    private FeatureManager featureManager;      //provider only
    private ProductManager productManager;      //provider only
    private CustomersManager customerManager;   //provider only
    private MarketingManager marketingManager;  //provider only
    private ProviderAccountingManager providerAccountingManager;    //provider only
    private ScheduleManager scheduleManager;    //developer only
    private HumanResourcesManager humanResourcesManager; //developer only
    private DeveloperAccountingManager developerAccountingManager; //developer only
    
    //GETTERS & SETTERS
    public void SetGameID(string gameID) { this.gameID = gameID; }
    public string GetGameID() { return gameID; }

    public void SetPlayerID(string playerID) { this.playerID = playerID; }
    public string GetPlayerID() { return playerID; }

    public void SetPlayerRole(PlayerRoles playerRole) { this.playerRole = playerRole; }
    public PlayerRoles GetPlayerRole() { return playerRole; }

    public void SetPlayerUI(GameObject playerUI) { this.playerUI = playerUI; }
    public GameObject GetPlayerUI() { return playerUI; }

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
        chatManager = this.gameObject.GetComponent<ChatManager>();
        firmManager = this.gameObject.GetComponent<FirmManager>();
        eventManager = this.gameObject.GetComponent<EventManager>();
        contractMamanger = this.gameObject.GetComponent<ContractManager>();
        riskManager = this.gameObject.GetComponent<RiskManager>();
        submitDataManager = this.gameObject.GetComponent<SubmitDataManager>();
        researchManager = this.gameObject.GetComponent<ResearchManager>();
        featureManager = this.gameObject.GetComponent<FeatureManager>();
        productManager = this.gameObject.GetComponent<ProductManager>();
        customerManager = this.gameObject.GetComponent<CustomersManager>();
        marketingManager = this.gameObject.GetComponent<MarketingManager>();
        providerAccountingManager = this.gameObject.GetComponent<ProviderAccountingManager>();
        scheduleManager = this.gameObject.GetComponent<ScheduleManager>();
        humanResourcesManager = this.gameObject.GetComponent<HumanResourcesManager>();
        developerAccountingManager = this.gameObject.GetComponent<DeveloperAccountingManager>();

       

        if (playerRole == PlayerRoles.Provider)
        {
            firmManager.enabled = true;
            featureManager.enabled = true;
            productManager.enabled = true;
            marketingManager.enabled = true;
            customerManager.enabled = true;
            contractMamanger.enabled = true;
            providerAccountingManager.enabled = true;
            riskManager.enabled = true;
            researchManager.enabled = true;
            submitDataManager.enabled = true;
            eventManager.enabled = true;
            

        }
        else //developer
        {
            featureManager.enabled = true;
            humanResourcesManager.enabled = true;
            contractMamanger.enabled = true;
            scheduleManager.enabled = true;
            developerAccountingManager.enabled = true;
            riskManager.enabled = true;
            researchManager.enabled = true;
            submitDataManager.enabled = true;
            eventManager.enabled = true;
        }
    }
    [Server]
    public void InvokeEventsBetweenQurters()
    {
        eventManager.InvokeGameEvent();
    }

    [Server]
    public void EvaluateQuarter()
    {
        //1. akutalizuj realne data na zaklade kontrakt managera
        //2.posun sa do noveho kvartalu v gameData????
        //2. prirad nove reference na objekty nadchadzajuceho Q a aktualizuj ich?
        EvaluateContracts();
        //UpdateCurrentQuarterDataDeveloper();
       // UpdateCurrentQuarterDataProvider();
        //SaveCurretnQuarterData();

        // SaveCurretnQuarterData();
        // MoveOtherManagerToNextQuarter();

    }

    [Server]
    public void EvaluateContracts()
    {
        if (playerRole == PlayerRoles.Developer)
        {
            contractMamanger.EvaluateContractsServer();
        }
    }
    [Server]
    public void UpdateCurrentQuarterDataProvider()
    {
        if (playerRole == PlayerRoles.Provider)
        {
            featureManager.UpdateCurrentQuarterData();
            productManager.UpdateCurrentQuarterData();
            customerManager.UpdateCurrentQuarterData();
            providerAccountingManager.UpdateCurrentQuarterData();
        }
        SaveCurretnQuarterDataProvider();

    }
    [Server]
    public void UpdateCurrentQuarterDataDeveloper()
    {
        if (playerRole == PlayerRoles.Developer)
        {
            developerAccountingManager.UpdateCurrentQuarterDataServer();
        }
        SaveCurretnQuarterDataDeveloper();
    }
    [Server]
    public void SaveCurretnQuarterDataDeveloper()
    {
        if (playerRole == PlayerRoles.Developer)
        {
            humanResourcesManager.SaveCurrentQuarterData();
            developerAccountingManager.SaveCurrentQuarterDataServer();
            riskManager.SaveCurrentQuarterData();
            researchManager.SaveCurrentQuaterData();
        }
    }
    [Server]
    public void SaveCurretnQuarterDataProvider()
    {
        if (playerRole == PlayerRoles.Provider)
        {
            productManager.SaveCurrentQuarterData();
            marketingManager.SaveCurrentQuarterData();
            customerManager.SaveCurrentQuarterData();
            providerAccountingManager.SaveCurrentQuarterData();
            riskManager.SaveCurrentQuarterData();
            researchManager.SaveCurrentQuaterData();
        }
    }

    [Command]
    public void CmdMoveToNextQuarter()
    {
        MoveOtherManagerToNextQuarterProvider();
        MoveOtherManagerToNextQuarterDeveloper();
    }

    [Server]
    public void MoveOtherManagerToNextQuarterDeveloper()
    {
        if (playerRole == PlayerRoles.Developer)
        {
            contractMamanger.MoveToNextQuarter();
            scheduleManager.MoveToNextQuarter();
            humanResourcesManager.MoveToNextQuarter();
            developerAccountingManager.MoveToNextQuarter();
            researchManager.MoveToNextQuarter();
            riskManager.MoveToTheNextQuarter();
            submitDataManager.MoveToNextQuarter();
           
        }
    }
    [Server]
    public void MoveOtherManagerToNextQuarterProvider()
    {
        if (playerRole == PlayerRoles.Provider)
        {
            contractMamanger.MoveToNextQuarter();
            productManager.MoveToNextQuarter();
            marketingManager.MoveToNextQuarter();
            customerManager.MoveToNextQuarter();
            providerAccountingManager.MoveToNextQuarter();
            researchManager.MoveToNextQuarter();
            riskManager.MoveToTheNextQuarter();
            submitDataManager.MoveToNextQuarter();
        }
    }
}
