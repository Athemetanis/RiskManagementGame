using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ContractManager : NetworkBehaviour
{   //VARIABLES

    public GameObject contractPrefab;


    private Dictionary<string, Contract> myContracts = new Dictionary<string, Contract>();

    private PlayerRoles playerRole;
    private string playerID;
    private string gameID;

    private int myContractsCount;

    private ContractUIHandler contractUIHandler;
    private FirmManager firmManager;
    //GETTERS & SETTERS

    public void SetContractUIHandler(ContractUIHandler contractUIHandler) { this.contractUIHandler = contractUIHandler; }
    public PlayerRoles GetPlayerRole() { return playerRole; }
    public string GetGameID() { return gameID; }
    public Dictionary<string, Contract> GetMyContracts() { return myContracts; }

    // Start is called before the first frame update
    void Start()
    {
        playerRole = this.gameObject.GetComponent<PlayerData>().GetPlayerRole();
        gameID = this.gameObject.GetComponent<PlayerData>().GetGameID();
        playerID = this.gameObject.GetComponent<PlayerData>().GetPlayerID();
        firmManager = this.gameObject.GetComponent<FirmManager>();


        //Debug.Log("zistujem ci som client: " + isClient);
        //Debug.Log("zistujem ci som server: " + isServer);
        //Debug.Log("zistujem ci som local player: " + isLocalPlayer);


    }


    public override void OnStartAuthority()
    {
        Debug.Log("zistujem ci mam autoritu: " + hasAuthority);
        CmdSyncContractListOnClient();
    }


    // Update is called once per frame
    void Update()
    {

    }


    public void TryToAddContractToMyContracts(string contractID, Contract contract)
    {
        if (!myContracts.ContainsKey(contractID))
        {
            myContracts.Add(contractID, contract);
            myContractsCount++;
            Debug.Log(myContractsCount);

            if (contractUIHandler != null)
            {
                contractUIHandler.UpdateUIContractListsContents();
            }
        }
    }

    public void CreateContract(string developersFirmName, string featureID)
    {
        string contractID = GameHandler.singleton.GenerateUniqueID();
        string developerID = GameHandler.allGames[gameID].GetDevelopersFirmPlayerID(developersFirmName);
        Feature selectedFeature = this.gameObject.GetComponent<FeatureManager>().GetOutsourcedFeatures()[featureID];
        ContractState state = ContractState.InNegotiations;
        int turn = 1;
        int delivery = 0;
        int price = 0;
        string providerFirm = firmManager.GetFirmName();
        string[] historyArray = new string[1] { "Contract was created by " + providerFirm };

        CmdCreateContract(contractID, gameID, this.playerID, developerID, selectedFeature, state, turn, delivery, price, historyArray);
    }


    //METHODS
    [Command]
    public void CmdCreateContract(string contractID, string gameID, string providerID, string developerID, Feature feature, ContractState state, int turn, int delivery, int price, string[] history)
    {
        Contract newContract = new Contract(contractID, gameID, providerID, developerID, feature, state, turn, delivery, price, history);
        ContractManager developerCM = GameHandler.allGames[gameID].GetDeveloper(developerID).GetComponent<ContractManager>();
        ContractManager providerCM = GameHandler.allGames[gameID].GetProvider(providerID).GetComponent<ContractManager>();
        developerCM.TryToAddContractToMyContracts(contractID, newContract);      //adding contract on server objects
        providerCM.TryToAddContractToMyContracts(contractID, newContract);       //adding contract on server objects

        developerCM.RpcCreateContract(contractID, gameID, providerID, developerID, feature, state, turn, delivery, price, history);
        providerCM.RpcCreateContract(contractID, gameID, providerID, developerID, feature, state, turn, delivery, price, history);
    }

    [ClientRpc]
    public void RpcCreateContract(string contractID, string gameID, string providerID, string developerID, Feature feature, ContractState state, int turn, int delivery, int price, string[] historyArray)
    {
        Contract newContract = new Contract(contractID, gameID, providerID, developerID, feature, state, turn, delivery, price, historyArray);
        this.TryToAddContractToMyContracts(contractID, newContract);        //adding contract on client objects
    }


    [Command]
    public void CmdSyncContractListOnClient()
    {
        foreach (Contract contract in myContracts.Values)
        {
            RpcCreateContract(contract.GetContractID(), contract.GetContractGameID(), contract.GetProviderID(), contract.GetDeveloperID(), contract.GetContractFeature(), contract.GetContractState(), contract.GetContractTrun(), contract.GetContractDelivery(), contract.GetContractPrice(), contract.GetContractHistory().ToArray());
        }
    }

    public void ModifyContract(string contractID, int price, int delivery)
    {
        Debug.Log("changes registered");
        CmdModifyContract(contractID, price, delivery);
    }

    [Command]
    public void CmdModifyContract(string contractID, int price, int delivery)
    {
        ContractManager developerCM = GameHandler.allGames[gameID].GetDeveloper(myContracts[contractID].GetDeveloperID()).GetComponent<ContractManager>();
        ContractManager providerCM = GameHandler.allGames[gameID].GetProvider(myContracts[contractID].GetProviderID()).GetComponent<ContractManager>();

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
        myContracts[contractID].MoveTurn();
        developerCM.RpcModifyContractTurn(contractID);
        providerCM.RpcModifyContractTurn(contractID);
    }

    [ClientRpc]
    public void RpcModifyContractPrice(string contractID, int price, string message)
    {
        Debug.Log("changes applied");
        myContracts[contractID].SetContractPrice(price);
        myContracts[contractID].AddHistoryRecord(message);
        Debug.Log(myContracts[contractID].GetContractTrun());
        //myContracts[contractID].MoveTurn();
        Debug.Log(myContracts[contractID].GetContractTrun());
        if (contractUIHandler != null)
        {
            contractUIHandler.UpdateUIContractListsContents();
        }
    }

    [ClientRpc]
    public void RpcModifyContractDelivery(string contractID, int delivery, string message)
    {
        Debug.Log("changes applied");
        myContracts[contractID].SetContractDelivery(delivery);
        myContracts[contractID].AddHistoryRecord(message);
        // myContracts[contractID].MoveTurn();
        if (contractUIHandler != null)
        {
            contractUIHandler.UpdateUIContractListsContents();
        }
    }

    [ClientRpc]
    public void RpcModifyContractTurn(string contractID)
    {
        myContracts[contractID].MoveTurn();
        if (contractUIHandler != null)
        {
            contractUIHandler.UpdateUIContractListsContents();
        }
    }

    public string GetFirmName(string playerID)
    {
        return GameHandler.allGames[gameID].GetFirmName(playerID);
    }


    public void SetNotice(int turn)
    {
        if ((turn % 2 == 0) && string.Equals(playerRole.ToString(), "provider"))
        {
            Debug.Log("Providers turn");
        }
        if ((turn % 2 != 0) && string.Equals(playerRole.ToString(), "developer"))
        {
            Debug.Log("Developers turn");
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

    [Command]
    public void CmdAcceptContract(string contractID)
    {
        ContractManager developerCM = GameHandler.allGames[gameID].GetDeveloper(myContracts[contractID].GetDeveloperID()).GetComponent<ContractManager>();
        ContractManager providerCM = GameHandler.allGames[gameID].GetProvider(myContracts[contractID].GetProviderID()).GetComponent<ContractManager>();
        myContracts[contractID].SetContractState(ContractState.Accepted);
        string message = firmManager.GetFirmName() + " accepted contract. ";
        myContracts[contractID].AddHistoryRecord(message);

        developerCM.RpcAcceptContract(contractID, message);
        providerCM.RpcAcceptContract(contractID, message);

    }


    [Command]
    public void CmdRejectContract(string contractID)
    {
        ContractManager developerCM = GameHandler.allGames[gameID].GetDeveloper(myContracts[contractID].GetDeveloperID()).GetComponent<ContractManager>();
        ContractManager providerCM = GameHandler.allGames[gameID].GetProvider(myContracts[contractID].GetProviderID()).GetComponent<ContractManager>();
        myContracts[contractID].SetContractState(ContractState.Rejected);
        string message = firmManager.GetFirmName() + " Rejected contract. ";
        myContracts[contractID].AddHistoryRecord(message);

        developerCM.RpcRejectContract(contractID, message);
        providerCM.RpcRejectContract(contractID, message);

    }

    [ClientRpc]
    public void RpcRejectContract(string contractID, string message)
    {
        myContracts[contractID].SetContractState(ContractState.Rejected);
        myContracts[contractID].AddHistoryRecord(message);
        if (contractUIHandler != null)
        {
            contractUIHandler.UpdateUIContractListsContents();
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
    }

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
