using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SubmitDataManager : NetworkBehaviour
{

    private ContractManager contractManager;
    private PlayerData playerData;
    private FeatureManager featureManager;

    // Start is called before the first frame update
    void Start()
    {
        contractManager = this.gameObject.GetComponent<ContractManager>();
        playerData = this.gameObject.GetComponent<PlayerData>();
        featureManager = this.gameObject.GetComponent<FeatureManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void MoveToNextQuarter()
    {
        CmdMoveToNextQuarter();
        CmdMoveGameQuarter();

    }

    [Command]
    public void CmdMoveToNextQuarter()
    {
        foreach (GameObject player in GameHandler.allGames[playerData.GetGameID()].GetPlayerList().Values)
        {
            //player.GetComponent<ContractManager>().EvaluateContracts();
            //player.GetComponent<ContractManager>().RpcEvaluateContracts();
        }

    }

    [Command]
    public void CmdMoveGameQuarter()
    {
        GameHandler.allGames[playerData.GetGameID()].SetGameRound( GameHandler.allGames[playerData.GetGameID()].GetGameRound() + 1 );
    }

}
