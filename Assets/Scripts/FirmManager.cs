using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
/// <summary>
/// 
/// </summary>
public class FirmManager : NetworkBehaviour
{
    public class SyncDictionaryStringString : SyncDictionary<string, string> { };

    //VARIABLES
    [SyncVar(hook = "OnChangeFirmName")]
    private string firmName;
    [SyncVar(hook = "OnChangeFirmDescription")]
    private string firmDescription;
    [SyncVar(hook = "OnChangeFirmNameChanged")]
    private bool firmNameChanged;
    [SyncVar(hook = "OnChangePlayerName")]
    private string playerName;
    [SyncVar(hook = "OnChangePlayerNameChanged")]
    private bool playerNameChanged;

    //Firm decisions
    [SyncVar]
    private bool marketSize1;
    [SyncVar]
    private bool marketSize2;
    [SyncVar]
    private bool marketSize3;

    [SyncVar]
    private bool competitivePosture1;
    [SyncVar]
    private bool competitivePosture2;
    [SyncVar]
    private bool competitivePosture3;
    [SyncVar]
    private bool competitivePosture4;
    [SyncVar]
    private bool competitivePosture5;

    [SyncVar]
    private bool distictiveComp1;
    [SyncVar]
    private bool distictiveComp2;
    [SyncVar]
    private bool distictiveComp3;
    [SyncVar]
    private bool distictiveComp4;
    [SyncVar]
    private bool distictiveComp5;

    [SyncVar]
    private bool businesspartnerDiversity1;
    [SyncVar]
    private bool businesspartnerDiversity2;
    [SyncVar]
    private bool businesspartnerDiversity3;

    [SyncVar]
    private bool contractPriorities1;
    [SyncVar]
    private bool contractPriorities2;
    [SyncVar]
    private bool contractPriorities3;
    [SyncVar]
    private bool contractPriorities4;

    [SyncVar]
    private bool accountingStrategies1;
    [SyncVar]
    private bool accountingStrategies2;
    [SyncVar]
    private bool accountingStrategies3;

    [SyncVar]
    private bool growthStrategies1;
    [SyncVar]
    private bool growthStrategies2;
    [SyncVar]
    private bool growthStrategies3;

    [SyncVar]
    private bool developmentStrategies1;
    [SyncVar]
    private bool developmentStrategies2;
    [SyncVar]
    private bool developmentStrategies3;

    private string playerID;
    private string gameID;

    private FirmUIHandler firmUIHandler;

    //GETTERS & SETTERS
    public string GetFirmName() { return firmName; }
    public string GetFirmDescription() { return firmDescription; }
    public void SetFirmUIHandler(FirmUIHandler firmUIHandler) { this.firmUIHandler = firmUIHandler; }
    public bool GetFirmNameChanged() { return firmNameChanged; }
    public bool GetPlayerNameChanged() { return playerNameChanged; }
    public string GetPlayerName() { return playerName; }

    private void Start()
    {
        if (isServer)
        {
            gameID = this.gameObject.GetComponent<PlayerData>().GetGameID();
            playerID = this.gameObject.GetComponent<PlayerData>().GetPlayerID();
            firmName = playerID;
            playerName = "unknown player";
            firmDescription = "default_description";
            GameHandler.allGames[gameID].AddFirmName(playerID, firmName);
            GameHandler.allGames[gameID].UpdateFirmDescription(firmName, firmDescription);
        }
        else
        {
            gameID = this.gameObject.GetComponent<PlayerData>().GetGameID();
            playerID = this.gameObject.GetComponent<PlayerData>().GetPlayerID();
        }
    
    }

    //METHODS

    public void TryToChangeFirmName(string newfirmName)
    {
        CmdTryChangeFirmsName(playerID, newfirmName, this.firmName);
    }
    [Command]
    public void CmdTryChangeFirmsName(string playerID, string newFirmName, string oldFirmName)
    {
        if (GameHandler.allGames[gameID].TryToChangeFirmName(playerID, newFirmName, oldFirmName))
        {
            CmdSetFirmsName(newFirmName);
        }
        else
        {
            RpcRevertFirmName(oldFirmName);
        }
    }
    [Command]  
    public void CmdSetFirmsName(string newFirmName)
    {
        this.firmName = newFirmName;
        firmNameChanged = true;

    }
    [Command]
    public void CmdSetFirmsDescription(string firmDescription)
    {
        this.firmDescription = firmDescription;
    }
    [ClientRpc]
    public void RpcRevertFirmName(string oldfirmName)
    {
        if (firmUIHandler != null)
        {
            firmUIHandler.SetUIFirmNameOld(oldfirmName);
        }
    }
    
    public void ChangeFirmDescription(string firmDescription)
    {
        CmdChangeFirmDescription(firmDescription);
        CmdSetFirmsDescription(firmDescription);
    }
    [Command]
    public void CmdChangeFirmDescription(string firmDescription)
    {
        GameHandler.allGames[gameID].UpdateFirmDescription(firmName, firmDescription);
    }

    public void SetPlayerName(string playerName)
    {
        CmdSetPlayerName(playerName);
    }
    [Command]
    public void CmdSetPlayerName(string playerName)
    {
        this.playerName = playerName;
        playerNameChanged = true;
    }
    
    
    public void SetFirmDecisionsMarketSize(bool marketSize1, bool marketSize2, bool marketSize3)
    {
        CmdSetFirmDecisionsMarketSize(marketSize1, marketSize2, marketSize3);
    }

    [Command]
    public void CmdSetFirmDecisionsMarketSize(bool marketSize1, bool marketSize2, bool marketSize3)
    {
        this.marketSize1 = marketSize1;
        this.marketSize2 = marketSize2;
        this.marketSize3 = marketSize3;
    }

    public void SetCompetitivePosture(bool competitivePosture1, bool competitivePosture2, bool competitivePosture3, bool competitivePosture4, bool competitivePosture5)
    {
        CmdSetCompetitivePosture(competitivePosture1, competitivePosture2, competitivePosture3, competitivePosture4, competitivePosture5);
    }

    [Command]
    public void CmdSetCompetitivePosture(bool competitivePosture1, bool competitivePosture2, bool competitivePosture3, bool competitivePosture4, bool competitivePosture5)
    {
        this.competitivePosture1 = competitivePosture1;
        this.competitivePosture2 = competitivePosture2;
        this.competitivePosture3 = competitivePosture3;
        this.competitivePosture4 = competitivePosture4;
        this.competitivePosture5 = competitivePosture5;
    }

    public void SetDistictiveCompetencies(bool distictiveComp1, bool distictiveComp2, bool distictiveComp3, bool distictiveComp4, bool distictiveComp5 )
    {
        CmdSetDistictiveCompetencies(distictiveComp1, distictiveComp2, distictiveComp3, distictiveComp4, distictiveComp5);
    }

    [Command]
    public void CmdSetDistictiveCompetencies(bool distictiveComp1, bool distictiveComp2, bool distictiveComp3, bool distictiveComp4, bool distictiveComp5)
    {   
        this.distictiveComp1 = distictiveComp1;
        this.distictiveComp2 = distictiveComp2;
        this.distictiveComp3 = distictiveComp3;
        this.distictiveComp4 = distictiveComp4;
        this.distictiveComp5 = distictiveComp5;
    }

    public void SetBusinessDiversity(bool businesspartnerDiversity1, bool businesspartnerDiversity2, bool businesspartnerDiversity3)
    {
        CmdSetBusinessDiversity(businesspartnerDiversity1, businesspartnerDiversity2, businesspartnerDiversity3);
    }
    [Command]
    public void CmdSetBusinessDiversity(bool businesspartnerDiversity1, bool businesspartnerDiversity2, bool businesspartnerDiversity3)
    {
        this.businesspartnerDiversity1 = businesspartnerDiversity1;
        this.businesspartnerDiversity2 = businesspartnerDiversity2;
        this.businesspartnerDiversity3 = businesspartnerDiversity3;
    }

    public void SetContractPrioryties(bool contractPriorities1, bool contractPriorities2, bool contractPriorities3, bool contractPriorities4)
    {
        CmdSetContractPrioryties(contractPriorities1, contractPriorities2, contractPriorities3, contractPriorities4);
    }

    [Command]
    public void CmdSetContractPrioryties(bool contractPriorities1, bool contractPriorities2, bool contractPriorities3, bool contractPriorities4)
    {
        this.contractPriorities1 = contractPriorities1;
        this.contractPriorities2 = contractPriorities2;
        this.contractPriorities3 = contractPriorities3;
        this.contractPriorities4 = contractPriorities4;

    }

    public void SetAccountingStrategies(bool accountingStrategies1, bool accountingStrategies2, bool accountingStrategies3)
    {
        CmdSetAccountingStrategies(accountingStrategies1, accountingStrategies2, accountingStrategies3);
    }

    [Command]
    public void CmdSetAccountingStrategies(bool accountingStrategies1, bool accountingStrategies2, bool accountingStrategies3)
    {
        this.accountingStrategies1 = accountingStrategies1;
        this.accountingStrategies2 = accountingStrategies2;
        this.accountingStrategies3 = accountingStrategies3;
    }

    public void SetGrowthStategies(bool growthStrategies1, bool growthStrategies2, bool growthStrategies3)
    {
        CmdSetGrowthStategies(growthStrategies1, growthStrategies2, growthStrategies3);
    }

    [Command]
    public void CmdSetGrowthStategies(bool growthStrategies1, bool growthStrategies2, bool growthStrategies3)
    {
        this.growthStrategies1 = growthStrategies1;
        this.growthStrategies2 = growthStrategies2;
        this.growthStrategies3 = growthStrategies3;
    }

    public void SetDevelopmentStrategies(bool developmentStrategies1, bool developmentStrategies2, bool developmentStrategies3)
    {
        CmdSetDevelopmentStrategies(developmentStrategies1, developmentStrategies2, developmentStrategies3);
    }
    [Command]
    public void CmdSetDevelopmentStrategies(bool developmentStrategies1, bool developmentStrategies2, bool developmentStrategies3)
    {
        this.developmentStrategies1 = developmentStrategies1;
        this.developmentStrategies2 = developmentStrategies2;
        this.developmentStrategies3 = developmentStrategies3;

    }



    //---------HOOKS-------------------------///
    public void OnChangeFirmName(string firmName)
    {
        this.firmName = firmName;
        if (firmUIHandler != null)
        {            
            firmUIHandler.SetUIFirmName(this.firmName);
            if (firmUIHandler.errorMessageText.IsActive())
            {
                firmUIHandler.errorMessageText.gameObject.SetActive(false);
            }
        }
    }
    public void OnChangeFirmDescription(string firmDescription)
    {
        this.firmDescription = firmDescription;
        if (firmUIHandler != null)
        {
            firmUIHandler.SetUIFirmDescription(firmDescription);
        }
    }
    public void OnChangeFirmNameChanged(bool firmNameChanged)
    {
        this.firmNameChanged = firmNameChanged;
        if(firmUIHandler != null)
        {
            if (firmNameChanged)
            {
                firmUIHandler.DisableChangingFirmName();
            }
        }
    }
    public void OnChangePlayerName(string playerName) 
    {
        this.playerName = playerName;
        if(firmUIHandler != null)
        {
            firmUIHandler.SetPlayerName(playerName);
        }
    }
    public void OnChangePlayerNameChanged(bool playerNameChanged)
    {
        this.playerNameChanged = playerNameChanged;
        if (firmUIHandler != null)
        {
            if (playerNameChanged)
            {
                firmUIHandler.DisableChangePlayerName();
            }
        }
    }
}
