using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SubmitDataManager : NetworkBehaviour
{
    [SyncVar(hook = "OnChangeSubmitData")]
    private bool submitData;   //default value = false

    string gameID;
    //REFERENCES
    private ContractManager contractManager;
    private PlayerData playerData;
    private FeatureManager featureManager;


    //GETTERS AND SETTERS
    public void SetSubmitData(bool submitData) { this.submitData = submitData; }
    public bool GetSubmitData() { return submitData; }



    // Start is called before the first frame update
    public override void OnStartServer()
    {
        submitData = false;
        gameID = this.gameObject.GetComponent<PlayerData>().GetGameID();
        contractManager = this.gameObject.GetComponent<ContractManager>();
        playerData = this.gameObject.GetComponent<PlayerData>();
        featureManager = this.gameObject.GetComponent<FeatureManager>();
    }

    void StartClient()
    {
        contractManager = this.gameObject.GetComponent<ContractManager>();
        playerData = this.gameObject.GetComponent<PlayerData>();
        featureManager = this.gameObject.GetComponent<FeatureManager>();
    }

    public void SubmitData()
    {   
        CmdSubmitData();
    }

    [Command]
    public void CmdSubmitData()
    {
        submitData = true;
        GameHandler.allGames[gameID].TryToAddPlayersToReadyServer(playerData.GetPlayerID());
    }

    public void OnChangeSubmitData(bool submitData)
    {
        if (submitData)
        {
            //cover my screeen with image and text abut wating for other player .... bla bla bla 
        }
    }


    [Server]
    public void MoveToNextQuarter()
    {
        submitData = false;

    }
    
    


}
