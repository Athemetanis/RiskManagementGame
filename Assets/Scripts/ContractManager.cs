using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;



public class ContractManager : NetworkBehaviour
{   //VARIABLES

    public GameObject contractPrefab;
    private Dictionary<string, Contract> myContracts = new Dictionary<string, Contract>();

    private PlayerRoles playerRole;
    private string providerID;

    private int myContractsCount;
    // Start is called before the first frame update
    void Start()
    {
        playerRole = this.gameObject.GetComponent<PlayerData>().GetPlayerRole();
        providerID = this.gameObject.GetComponent<PlayerData>().GetPlayerID();
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
        }
    }

    public void CreateContract(string developersFirmName, string featureID)
    {
        string contractID = GameHandler.singleton.GenerateUniqueID();
        string gameID = this.gameObject.GetComponent<PlayerData>().GetGameID();
        string developerID = GameHandler.allGames[gameID].GetDevelopersFirmPlayerID(developersFirmName);
        Feature selectedFeature = this.gameObject.GetComponent<FeatureManager>().GetOutsourcedFeatures()[featureID];
        ContractState state = ContractState.Proposal;
        int turn = 1;

        CmdCreateContract(contractID, gameID, this.providerID, developerID, selectedFeature, state, turn);

    }





    //METHODS
    [Command]
    public void CmdCreateContract(string contractID, string gameID, string providerID, string developerID, Feature feature, ContractState state, int turn)
    {
        Contract newContract = new Contract(contractID, gameID, providerID, developerID, feature, state, turn);
        ContractManager developerCM = GameHandler.allGames[gameID].GetDeveloper(developerID).GetComponent<ContractManager>();   
        ContractManager providerCM = GameHandler.allGames[gameID].GetProvider(providerID).GetComponent<ContractManager>();
        developerCM.TryToAddContractToMyContracts(contractID, newContract);      //adding contract on server objects
        providerCM.TryToAddContractToMyContracts(contractID, newContract);       //adding contract on server objects

        developerCM.RpcCreateContract(contractID, gameID, providerID, developerID, feature, state, turn);
        providerCM.RpcCreateContract(contractID, gameID, providerID, developerID, feature, state, turn);
    }

    [ClientRpc]
    public void RpcCreateContract(string contractID, string gameID, string providerID, string developerID, Feature feature, ContractState state, int turn)
    {
        Contract newContract = new Contract(contractID, gameID, providerID, developerID, feature, state, turn);
        this.TryToAddContractToMyContracts(contractID, newContract);
    }

    [Command]
    public void CmdSendModifiedContract(string contractID, int price, int delivery)
    {



    }

    public void SetNotice(int turn)
    {   
        if ( (turn % 2 == 0) &&   string.Equals(playerRole.ToString(), "provider"))
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

   /* [Command]
    public void CmdCreateContract(string gameID, string providerID, string developerID, Feature feature, ContractState state, int turn)
    {
        GameObject newContractObject = Instantiate(contractPrefab);
        Contract newContract = newContractObject.GetComponent<Contract>();
        newContract.SetDeveloperID(developerID);
        newContract.SetProviderID(providerID);
        newContract.SetState(ContractState.Proposal);
        newContract.SetContractId(newContractObject.GetInstanceID().ToString());
        //myContracts.Add(newContract.GetContractID(), newContract);
        newContract.gameObject.SetActive(true);
        NetworkServer.Spawn(newContractObject);
        // newContract.set
    }*/

}
