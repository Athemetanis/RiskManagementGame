using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ContractManager : NetworkBehaviour
{   //VARIABLES
    private Dictionary<string, Contract> myContracts = new Dictionary<string, Contract>();
    private int myContractsCount;

    private PlayerRoles playerRole;
    private string playerID;
    private string gameID;

    private ContractUIHandler contractUIHandler;
    private FirmManager firmManager;
    private ScheduleManager scheduleManager;
    private CustomersManager customersManager;
    private FeatureManager featureManager;


    //GETTERS & SETTERS
    public FeatureManager GetFeatureManager() { return featureManager; }
    public ScheduleManager GetScheduleManager() { return scheduleManager; }
    public void SetContractUIHandler(ContractUIHandler contractUIHandler) { this.contractUIHandler = contractUIHandler; }
    public ContractUIHandler GetContractUIHandler() { return contractUIHandler; }
    public PlayerRoles GetPlayerRole() { return playerRole; }
    public string GetGameID() { return gameID; }
    public Dictionary<string, Contract> GetMyContracts() { return myContracts; }


    void Start() //both server & client
    {
        playerRole = this.gameObject.GetComponent<PlayerData>().GetPlayerRole();
        gameID = this.gameObject.GetComponent<PlayerData>().GetGameID();
        playerID = this.gameObject.GetComponent<PlayerData>().GetPlayerID();
        firmManager = this.gameObject.GetComponent<FirmManager>();
        scheduleManager = this.gameObject.GetComponent<ScheduleManager>();
        customersManager = this.gameObject.GetComponent<CustomersManager>();
        featureManager = this.gameObject.GetComponent<FeatureManager>();
    }


    public override void OnStartAuthority()
    {
        Debug.Log("zistujem ci mam autoritu: " + hasAuthority);
        CmdSyncContractListOnClient();
    }

    //METHODS
    public void TryToAddContractToMyContracts(string contractID, Contract contract)
    {
        if (!myContracts.ContainsKey(contractID))
        {
            myContracts.Add(contractID, contract);
            myContractsCount++;
            //Debug.Log(myContractsCount);

            if (contractUIHandler != null)
            {
                contractUIHandler.UpdateUIContractListsContents();
            }
            SetNotice(contractID);
        }
        
    }
    public void CreateContract(string developersFirmName, string featureID)
    {
        string contractID = GameHandler.singleton.GenerateUniqueID();
        string developerID = GameHandler.allGames[gameID].GetDevelopersFirmPlayerID(developersFirmName);
        Feature selectedFeature = this.gameObject.GetComponent<FeatureManager>().GetOutsourcedFeatures()[featureID];
        ContractState state = ContractState.Proposal;
        int turn = 1;
        int delivery = 0;
        int price = 0;
        int riskSharingFee = 0;
        int trueDevelopmentTime = 0;
        string providerFirm = firmManager.GetFirmName();
        string[] historyArray =  { "-----------------Turn 0 -----------------", "Contract was created by " + providerFirm };
        
        CmdCreateContract(contractID, gameID, this.playerID, developerID, selectedFeature, state, turn, delivery, price, historyArray, riskSharingFee, trueDevelopmentTime);
    }

    [Command]
    public void CmdCreateContract(string contractID, string gameID, string providerID, string developerID, Feature feature, ContractState contractState, int turn, int delivery, int price, string[] history, int riskSharingFee, int trueDevelopmentTime)
    {
        Contract newContract = new Contract(contractID, gameID, providerID, developerID, feature, contractState, turn, delivery, price, history, riskSharingFee, trueDevelopmentTime);
        ContractManager developerCM = GameHandler.allGames[gameID].GetDeveloper(developerID).GetComponent<ContractManager>();
        ContractManager providerCM = GameHandler.allGames[gameID].GetProvider(providerID).GetComponent<ContractManager>();
        developerCM.TryToAddContractToMyContracts(contractID, newContract);      //adding contract on server objects
        providerCM.TryToAddContractToMyContracts(contractID, newContract);       //adding contract on server objects

        developerCM.RpcCreateContract(contractID, gameID, providerID, developerID, feature, contractState, turn, delivery, price, history, riskSharingFee, trueDevelopmentTime);
        providerCM.RpcCreateContract(contractID, gameID, providerID, developerID, feature, contractState, turn, delivery, price, history, riskSharingFee, trueDevelopmentTime);
        //Schedule
        if (developerCM.GetScheduleManager() != null)
        {
            Debug.Log("creating scheduled feature on server developer");
            developerCM.GetScheduleManager().CreateScheduledFeature(contractID, providerID, contractState, feature);
        }
    }
    [ClientRpc]
    public void RpcCreateContract(string contractID, string gameID, string providerID, string developerID, Feature feature, ContractState contractState, int turn, int delivery, int price, string[] historyArray, int riskSharingFee, int trueDevelopmentTime)
    {
        Contract newContract = new Contract(contractID, gameID, providerID, developerID, feature, contractState, turn, delivery, price, historyArray, riskSharingFee, trueDevelopmentTime);
        this.TryToAddContractToMyContracts(contractID, newContract);        //adding contract on client objects
        //Schedule
        if (scheduleManager != null)
        {
            scheduleManager.CreateScheduledFeature(contractID, providerID, contractState, feature);
        }
    }

    [Command]
    public void CmdSyncContractListOnClient()
    {
        foreach (Contract contract in myContracts.Values)
        {
            RpcCreateContract(contract.GetContractID(), contract.GetContractGameID(), contract.GetProviderID(), contract.GetDeveloperID(), contract.GetContractFeature(), contract.GetContractState(), contract.GetContractTurn(), contract.GetContractDelivery(), contract.GetContractPrice(), contract.GetContractHistory().ToArray(), contract.GetContractRiskSharingFee(), contract.GetTrueDevelopmentTime());
        }
    }

    public void ModifyContract(string contractID, int price, int delivery, int riskSharingFee)
    {
        Debug.Log("changes registered");
        CmdModifyContract(contractID, price, delivery, riskSharingFee);
    }
    [Command]
    public void CmdModifyContract(string contractID, int price, int delivery, int riskSharingFee)
    {
        Debug.LogWarning(contractID);
        ContractManager developerCM = GameHandler.allGames[gameID].GetDeveloper(myContracts[contractID].GetDeveloperID()).GetComponent<ContractManager>();
        ContractManager providerCM = GameHandler.allGames[gameID].GetProvider(myContracts[contractID].GetProviderID()).GetComponent<ContractManager>();
        if (myContracts[contractID].GetContractState() == ContractState.Proposal)
        {
            myContracts[contractID].SetContractState(ContractState.InNegotiations);
            //SCHEDULE
            if (developerCM.GetScheduleManager() != null)
            {
                developerCM.GetScheduleManager().ChangeStateOfScheduledFeature(contractID, ContractState.InNegotiations);
            }
        }
        if (price != myContracts[contractID].GetContractPrice())
        {
            myContracts[contractID].SetContractPrice(price);
            string message = firmManager.GetFirmName() + " changed price to: " + price;
            myContracts[contractID].AddHistoryRecord(message);

            developerCM.RpcModifyContractPrice(contractID, price, message);
            providerCM.RpcModifyContractPrice(contractID, price, message);

        }
        if (delivery != myContracts[contractID].GetContractDelivery())
        {
            myContracts[contractID].SetContractDelivery(delivery);
            string message = firmManager.GetFirmName() + " changed delivery to: " + delivery;
            myContracts[contractID].AddHistoryRecord(message);

            developerCM.RpcModifyContractDelivery(contractID, delivery, message);
            providerCM.RpcModifyContractDelivery(contractID, delivery, message);
        }
        if(riskSharingFee != myContracts[contractID].GetContractRiskSharingFee())
        {
            myContracts[contractID].SetContractRiskSharingFee(riskSharingFee);
            string message = firmManager.GetFirmName() + "changed risk sharing fee to: " + riskSharingFee;
            myContracts[contractID].AddHistoryRecord(message);

            developerCM.RpcModifyContractRiskSharingFee(contractID, riskSharingFee, message);
            providerCM.RpcModifyContractRiskSharingFee(contractID, riskSharingFee, message);

        }
        myContracts[contractID].MoveTurn();
        developerCM.RpcModifyContractTurn(contractID);
        providerCM.RpcModifyContractTurn(contractID);
    }
    [ClientRpc]
    public void RpcModifyContractPrice(string contractID, int price, string message)
    {
        Debug.LogWarning("contractID: " + contractID);
        foreach(Contract contract in myContracts.Values)
        {
            Debug.Log(contract.GetContractID());
        }
        if (myContracts[contractID].GetContractState() == ContractState.Proposal)
        {
            myContracts[contractID].SetContractState(ContractState.InNegotiations);
            //SCHEDULE
            if (scheduleManager != null)
            {
                scheduleManager.ChangeStateOfScheduledFeature(contractID, ContractState.InNegotiations);
            }
        }
        Debug.Log("changes applied");
        myContracts[contractID].SetContractPrice(price);
        myContracts[contractID].AddHistoryRecord(message);
        if (contractUIHandler != null)
        {
            contractUIHandler.UpdateUIContractListsContents();
            if (playerRole == PlayerRoles.Provider)
            {
                contractUIHandler.UpdateContractOverview();
            }
        }
    }
    [ClientRpc]
    public void RpcModifyContractDelivery(string contractID, int delivery, string message)
    {
        Debug.LogWarning(contractID);
        if (myContracts[contractID].GetContractState() == ContractState.Proposal)
        {
            myContracts[contractID].SetContractState(ContractState.InNegotiations);
            //SCHEDULE
            if (scheduleManager != null)
            {
                scheduleManager.ChangeStateOfScheduledFeature(contractID, ContractState.InNegotiations);
            }
        }
        Debug.Log("changes applied");
        myContracts[contractID].SetContractDelivery(delivery);
        myContracts[contractID].AddHistoryRecord(message);
        if (contractUIHandler != null)
        {
            contractUIHandler.UpdateUIContractListsContents();
            if(playerRole == PlayerRoles.Provider)
            {
                contractUIHandler.UpdateContractOverview();
            }
        }
    }
    [ClientRpc]
    public void RpcModifyContractRiskSharingFee(string contractID, int riskSharingFee, string message)
    {
        Debug.LogWarning(contractID);
        if (myContracts[contractID].GetContractState() == ContractState.Proposal)
        {
            myContracts[contractID].SetContractState(ContractState.InNegotiations);
            //SCHEDULE
            if (scheduleManager != null)
            {
                scheduleManager.ChangeStateOfScheduledFeature(contractID, ContractState.InNegotiations);
            }
        }
        Debug.Log("changes applied");
        myContracts[contractID].SetContractRiskSharingFee(riskSharingFee);
        myContracts[contractID].AddHistoryRecord(message);
        if (contractUIHandler != null)
        {
            contractUIHandler.UpdateUIContractListsContents();
            if (playerRole == PlayerRoles.Provider)
            {
                contractUIHandler.UpdateContractOverview();
            }
        }
    }
    [ClientRpc]
    public void RpcModifyContractTurn(string contractID)
    {
        myContracts[contractID].MoveTurn();
        SetNotice(contractID);
        if (contractUIHandler != null)
        {
            contractUIHandler.UpdateUIContractListsContents();
            if (playerRole == PlayerRoles.Provider)
            {
                contractUIHandler.UpdateContractOverview();
                
            }
        }
    }

    public string GetFirmName(string playerID)
    {
        return GameHandler.allGames[gameID].GetFirmName(playerID);
    }

    public void SetNotice(string contractID)
    {
        if (contractUIHandler != null)
        {
            if ((myContracts[contractID].GetContractTurn() % 2 == 0) && playerRole == PlayerRoles.Provider)  //my turn conditions
            {
                contractUIHandler.ContracNotificationON();

            }
            if ((myContracts[contractID].GetContractTurn() % 2 != 0) && playerRole == PlayerRoles.Developer)
            {
                contractUIHandler.ContracNotificationON();
            }
        }
        //ak rola tohoto objektu odpovedá hodnote ťahu
        //na tabe contracts zobraz vykríčník
    }

    public void AcceptContract(string contractID)
    {
        Debug.Log("Accepting contract");
        CmdAcceptContract(contractID);
    }
    public void RejectContract(string contractID)
    {
        Debug.Log("Rejecting contract");
        CmdRejectContract(contractID);

    }
    public void FinalContract(string contractID)
    {
        CmdFinalContract(contractID);
    }

    [Command]
    public void CmdAcceptContract(string contractID)
    {   
        ContractManager developerCM = GameHandler.allGames[gameID].GetDeveloper(myContracts[contractID].GetDeveloperID()).GetComponent<ContractManager>();
        ContractManager providerCM = GameHandler.allGames[gameID].GetProvider(myContracts[contractID].GetProviderID()).GetComponent<ContractManager>();
        myContracts[contractID].SetContractState(ContractState.Accepted);

        if(providerCM.GetFeatureManager() != null)
        {
            providerCM.GetFeatureManager().AddFeatureInDevelopmentServer(myContracts[contractID].GetContractFeature().nameID);
            providerCM.GetFeatureManager().RemoveFeatureForOutsourcingServer(myContracts[contractID].GetContractFeature().nameID);
        }
        
        string message = firmManager.GetFirmName() + " accepted contract. ";
        myContracts[contractID].AddHistoryRecord(message);
        developerCM.RpcAcceptContract(contractID, message);
        providerCM.RpcAcceptContract(contractID, message);

        foreach (Contract contract in myContracts.Values)
        {
            if (contract.GetContractFeature().Equals(myContracts[contractID].GetContractFeature()) && contract.GetContractID() != contractID && contract.GetContractState() != ContractState.Rejected)
            {
                contract.SetContractState(ContractState.Rejected);
                message = firmManager.GetFirmName() + " rejected contract. ";
                contract.AddHistoryRecord(message);
                developerCM.RpcRejectContract(contract.GetContractID(), message);
                providerCM.RpcRejectContract(contract.GetContractID(), message);
                //SCHEDULE
                if (developerCM.GetScheduleManager() != null)
                {
                    developerCM.GetScheduleManager().DeleteScheduledFeature(contract.GetContractID());
                }
            }
        }
        //SCHEDULE
        if (developerCM.GetScheduleManager() != null)
        {
            developerCM.GetScheduleManager().ChangeStateOfScheduledFeature(contractID, ContractState.Accepted);
        }
    }
    [ClientRpc]
    public void RpcAcceptContract(string contractID, string message)
    {
        myContracts[contractID].SetContractState(ContractState.Accepted);
        myContracts[contractID].AddHistoryRecord(message);
        if (contractUIHandler != null)
        {
            contractUIHandler.UpdateUIContractListsContents();
        }
        //SCHEDULE
        if (scheduleManager != null)
        {
            scheduleManager.ChangeStateOfScheduledFeature(contractID, ContractState.Accepted);
        }
        //CUSTOMERS MANAGER
        if (playerRole == PlayerRoles.Provider)
        {
            customersManager.UpdateCustomersCount();
        }
        //ACCOUNTING
        DeveloperAccountingManager developerAM = GameHandler.allGames[gameID].GetDeveloper(myContracts[contractID].GetDeveloperID()).GetComponent<DeveloperAccountingManager>();
        ProviderAccountingManager providerAM = GameHandler.allGames[gameID].GetProvider(myContracts[contractID].GetProviderID()).GetComponent<ProviderAccountingManager>();
        developerAM.UpdateRevenue();
        providerAM.UpdateContractPayments();
        providerAM.UpdateRevenue();        
    }
    [Command]
    public void CmdRejectContract(string contractID)
    {
        ContractManager developerCM = GameHandler.allGames[gameID].GetDeveloper(myContracts[contractID].GetDeveloperID()).GetComponent<ContractManager>();
        ContractManager providerCM = GameHandler.allGames[gameID].GetProvider(myContracts[contractID].GetProviderID()).GetComponent<ContractManager>();
        myContracts[contractID].SetContractState(ContractState.Rejected);
        string message = firmManager.GetFirmName() + " rejected contract. ";
        myContracts[contractID].AddHistoryRecord(message);

        developerCM.RpcRejectContract(contractID, message);
        providerCM.RpcRejectContract(contractID, message);
        //SCHEDULE
        if (developerCM.GetScheduleManager() != null)
        {
            developerCM.GetScheduleManager().DeleteScheduledFeature(contractID);
        }

    }
    [ClientRpc]
    public void RpcRejectContract(string contractID, string message)
    {
        myContracts[contractID].SetContractState(ContractState.Rejected);
        myContracts[contractID].AddHistoryRecord(message);
        if (contractUIHandler != null)
        {
            contractUIHandler.UpdateUIContractListsContents();
            if(playerRole == PlayerRoles.Provider)
            {
                contractUIHandler.UpdateContractOverview();
            }
        }
        //SCHEDULE
        if(scheduleManager != null)
        {
            scheduleManager.DeleteScheduledFeature(contractID);
        }
    }
    [Command]
    public void CmdFinalContract(string contractID)
    {
        ContractManager developerCM = GameHandler.allGames[gameID].GetDeveloper(myContracts[contractID].GetDeveloperID()).GetComponent<ContractManager>();
        ContractManager providerCM = GameHandler.allGames[gameID].GetProvider(myContracts[contractID].GetProviderID()).GetComponent<ContractManager>();
        myContracts[contractID].SetContractState(ContractState.Final);
        string message = firmManager.GetFirmName() + " set contract as FINAL. ";
        myContracts[contractID].AddHistoryRecord(message);
        myContracts[contractID].MoveTurn();
        developerCM.RpcFinalContract(contractID, message);
        providerCM.RpcFinalContract(contractID, message);
        developerCM.RpcModifyContractTurn(contractID);
        providerCM.RpcModifyContractTurn(contractID);
        //SCHEDULE
        if (developerCM.GetScheduleManager() != null)
        {
            developerCM.GetScheduleManager().ChangeStateOfScheduledFeature(contractID, ContractState.Final);
        }
    }
    [ClientRpc]
    public void RpcFinalContract(string contractID, string message)
    {
        myContracts[contractID].SetContractState(ContractState.Final);
        myContracts[contractID].AddHistoryRecord(message);
        if (contractUIHandler != null)
        {
            contractUIHandler.UpdateUIContractListsContents();
        }
        //SCHEDULE
        if (scheduleManager != null)
        {
            scheduleManager.ChangeStateOfScheduledFeature(contractID, ContractState.Final);
        }

    }


    // NEXT QUARTER EVALUATION METHODS...
    public void EvaluateContracts()
    {
        foreach (Contract contract in myContracts.Values)
        {
            if (contract.GetContractState() == ContractState.Accepted)
            {
                contract.SetContractState(ContractState.Done);
            }
        }
    }

    [Command]
    public void CmdEvaluateContracts()
    {
        foreach (Contract contract in myContracts.Values)
        {
            if(contract.GetContractState() == ContractState.Accepted)
            {
                contract.SetContractState(ContractState.Done);
            }
        }
        RpcEvaluateContracts();
    }
    [ClientRpc]
    public void RpcEvaluateContracts()
    {
        foreach (Contract contract in myContracts.Values)
        {
            if (contract.GetContractState() == ContractState.Accepted)
            {
                contract.SetContractState(ContractState.Done);
            }
        }

        if (contractUIHandler != null)
        {
            contractUIHandler.UpdateUIContractListsContents();
        }
    }


}
