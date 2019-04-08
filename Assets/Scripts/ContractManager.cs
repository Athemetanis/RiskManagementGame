using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ContractManager : NetworkBehaviour
{   //VARIABLES

    public GameObject contractPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //METHODS
    //poriešiť že hráč môže mať viac game dát s rovnakým ID...... FUCK
    public void CmdCreateContract(string ProviderID, string DeveloperID)
    {
        GameObject newContract = Instantiate(contractPrefab);
       // newContract.set

    }


}
