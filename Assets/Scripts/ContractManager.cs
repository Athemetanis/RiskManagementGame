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


    //METHODS
    [Command]
    public void CmdCreateContract(string providerID, string developerID)
    {
        /*GameObject newContractObject = Instantiate(contractPrefab);
        Contract newContract = newContractObject.GetComponent<Contract>();
        newContract.SetDeveloperID(developerID);
        newContract.SetProviderID(providerID);
        newContract.SetState(ContractState.Proposal);
        newContract.SetContractId(newContractObject.GetInstanceID().ToString());
        //myContracts.Add(newContract.GetContractID(), newContract);
        newContract.gameObject.SetActive(true);
        NetworkServer.Spawn(newContractObject);
        // newContract.set*/




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

    

}
