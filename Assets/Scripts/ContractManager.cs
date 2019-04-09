using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ContractManager : NetworkBehaviour
{   //VARIABLES

    public GameObject contractPrefab;

    public Dictionary<string, Contract> myContracts = new Dictionary<string, Contract>();

    private int myContractsCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void TryToAddContractToMyContracts(string contractID, Contract contract)
    {   if (!myContracts.ContainsKey(contractID))
        {
            myContracts.Add(contractID, contract);
            myContractsCount++;
            Debug.Log(myContractsCount);
        }
        
    }


    //METHODS

    public void CmdCreateContract(string providerID, string developerID)
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
    }

    
    

}
