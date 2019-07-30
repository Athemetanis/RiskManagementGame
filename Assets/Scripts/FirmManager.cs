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


    private string playerID;
    private string gameID;

    private FirmUIHandler firmUIHandler;


    //GETTERS & SETTERS
    public string GetFirmName() { return firmName; }
    public string GetFirmDescription() { return firmDescription; }
    public void SetFirmUIHandler(FirmUIHandler firmUIHandler) { this.firmUIHandler = firmUIHandler; }


    private void Start()
    {
        if (isServer)
        {
            gameID = this.gameObject.GetComponent<PlayerData>().GetGameID();
            playerID = this.gameObject.GetComponent<PlayerData>().GetPlayerID();
            firmName = playerID;
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

    // Update is called once per frame
    void Update()
    {

    }


    [Command]  //this command updates Local Gui 
    public void CmdSetFirmsName(string newFirmName)
    {
        this.firmName = newFirmName;

    }
    [Command]
    public void CmdSetFirmsDescription(string firmDescription)
    {
        this.firmDescription = firmDescription;
    }


    public void TryToChangeFirmName(string newfirmName)
    {
        CmdTryChangeFirmsName(playerID, newfirmName, this.firmName);
    }

    [Command]
    public void CmdTryChangeFirmsName(string playerID, string newFirmName, string oldFirmName)
    {
        if (GameHandler.allGames[gameID].TryToChangeFirmName(playerID, newFirmName, oldFirmName))
        {
            Debug.Log("developerovo meno uspesne zmenene");
            CmdSetFirmsName(newFirmName);
        }
        else
        {
            RpcRevertFirmName(oldFirmName);
        }
          

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
        Debug.Log(firmName + " a " + firmDescription);

        CmdChangeFirmDescription(firmDescription);
        CmdSetFirmsDescription(firmDescription);

    }

    [Command]
    public void CmdChangeFirmDescription(string firmDescription)
    {
        Debug.Log("SERVER: " + firmName + " a " + firmDescription);
        GameHandler.allGames[gameID].UpdateFirmDescription(firmName, firmDescription);
    }





    //---------HOOKS-------------------------///
    public void OnChangeFirmName(string firmName)
    {
        Debug.Log("nastavujem meno firmy na klientovi");
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
        Debug.Log("nastavujem popis firmy na klientovi");
        this.firmDescription = firmDescription;
        if (firmUIHandler != null)
        {
            firmUIHandler.SetUIFirmDescription(firmDescription);
        }

    }


    public List<string> GetDeveloperList()
    {
        return GameHandler.allGames[gameID].GetDevelopersFirms();
    }

}
