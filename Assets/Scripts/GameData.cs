using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/// <summary>
/// Contains information about one particular game, such as: game name, ID, list of players, round, lists of developers/provieders and their firm names
/// </summary>
public class GameData : NetworkBehaviour
{
    public class SyncDictionaryStringString : SyncDictionary<string, string> { };

    //VARIABLES
    [SyncVar(hook = "OnChangeGameID")]
    private string gameID;
    [SyncVar(hook = "OnChangeGameName")]
    private string gameName;          
    [SyncVar]
    private string password;
    [SyncVar(hook = "OnChangeGameRound")]
    private int gameRound;
    [SyncVar(hook = "OnChangePlayersCount")]
    private int playersCount;
    private int readyPlayersCount;

    //this variable holds reference on script of UI representation of game (if it exists) //LOCAL !!!
    private GameUIHandler gameUIHandler;
    
    //--------------<playerID, GameObject player>---------------------------------------//   HOW I WILL BE SYNCING THIS??? /// pri každom starte playerdat - na cliente i na serveri sa zavola add

    private Dictionary<string, GameObject> playerList;
    private Dictionary<string, GameObject> developerList;
    private Dictionary<string, GameObject> providerList;

    private int developersCount;
    private int providersCount;

    //-------------------<firmName, playerID>---------------------------------------//
    private SyncDictionaryStringString allFirms = new SyncDictionaryStringString();
    private SyncDictionaryStringString allFirmDescriptions = new SyncDictionaryStringString();
    private SyncDictionaryStringString developersFirms = new SyncDictionaryStringString();
    private SyncDictionaryStringString providersFirms = new SyncDictionaryStringString();
    
    //GETTERS & SETTERS
    public void SetGameID(string gameID) { this.gameID = gameID; }
    public string GetGameID() { return gameID; }
    public void SetGameName(string gameName) { this.gameName = gameName; }
    public string GetGameName() { return gameName; }
    public void SetPassoword(string password) { this.password = password; }
    public string GetPassword() { return password; }
    public void SetGameRound(int gameRound) { this.gameRound = gameRound; }
    public int GetGameRound() { return gameRound; }
    public void SetPlayersCount(int playersCount) { this.playersCount = playersCount;  }
    public int GetPlayersCount() { return playersCount; }
    public void SetDevelopersCount(int developersCount) { this.developersCount = developersCount; }
    public int GetDevelopersCount() { return developersCount; }
    public void SetProvidersCount(int providersCount) { this.providersCount = providersCount; }
    public int GetProvidersCount() { return providersCount; }

    public void SetGameUIHandler(GameUIHandler gameUIHandler) { this.gameUIHandler = gameUIHandler; }
    
    //Add me when I awake - this is called on both the clients and the host, so everyone will know me 
    protected virtual void Awake()
    {       
         //syncvar not initialized...                 
    }

    public override void OnStartClient()
    {
        // Equipment is already populated with anything the server set up
        // but we can subscribe to the callback in case it is updated later on
        developersFirms.Callback += OnDevelopersFirmsChange;
    }

    // Use this for initialization
    void Start ()
    {
        playerList = new Dictionary<string, GameObject>();
        developerList = new Dictionary<string, GameObject>();
        providerList = new Dictionary<string, GameObject>();
        if (GameHandler.allGames.ContainsKey(this.gameID) == false)
        {
            GameHandler.allGames.Add(this.gameID, this);
        }

        if (GameHandler.allPlayers.ContainsKey(this.GetGameID()) == false)
        {
            GameHandler.allPlayers.Add(this.GetGameID(), new Dictionary<string, GameObject>());
            Debug.Log("Game with ID " + gameID + "was created");
        }
        if(GameHandler.singleton.GetGeneratedGameList())
        {
            GameHandler.singleton.RefreshGamesList();
        }
        
    }

    //METHODS
    public void OnChangeGameID(string gameID)
    {
        this.gameID = gameID;
        if (gameUIHandler != null)
        {
            gameUIHandler.ChangeIDText(gameID);
        }
    }

    public void OnChangeGameName(string gameName)
    {
        this.gameName = gameName;
        if (gameUIHandler != null)
        {
            gameUIHandler.ChangeNameText(gameName);
        }
    }

    public void OnChangeGameRound(int gameRound)
    {
        this.gameRound = gameRound;
        if (gameUIHandler != null)
        {
            gameUIHandler.ChangeRoundText(gameRound);
        }
    }

    public void OnChangePlayersCount(int playersCount)
    {
        this.playersCount = playersCount;
        if (gameUIHandler != null)
        {
            gameUIHandler.ChangePlayersCountText(playersCount);
        }
    }

    public void GameUIUpdateAll()
    {
        if (gameUIHandler != null)
        {
            gameUIHandler.ChangeIDText(gameID);
            gameUIHandler.ChangeNameText(gameName);
            gameUIHandler.ChangeRoundText(gameRound);
            gameUIHandler.ChangePlayersCountText(playersCount);

        }
    }
    
    public void AddPlayerToGame(GameObject player)
    {
        PlayerData playerData = player.GetComponent<PlayerData>();
        if (playerList.ContainsKey(playerData.GetPlayerID()) == false)
        {
            playerList.Add(playerData.GetPlayerID(), player);
            playersCount = playerList.Keys.Count;
            Debug.Log(playerData.GetPlayerRole());
            if (playerData.GetPlayerRole() == PlayerRoles.Developer)
            {
                Debug.Log("Hrac priadny do developerov");
                developerList.Add(playerData.GetPlayerID(), player);
                developersCount++;
            }
            else
            {
                Debug.Log("Hrac priadny do providerov");
                providerList.Add(playerData.GetPlayerID(), player);
                providersCount++;
            }
        }

        foreach (GameObject playerg in playerList.Values)
        {
            Debug.Log("values"+ playerg.GetComponent<PlayerData>().GetPlayerID());
        }
        foreach (string playerg in playerList.Keys)
        {
            Debug.Log("keys"+playerg);
        }
    }

    public void RemovePlayerFromGame(GameObject player)
    {
        PlayerData playerData = player.GetComponent<PlayerData>();
        if (playerList.ContainsKey(playerData.GetPlayerID()) == true)
        {
            playerList.Remove(playerData.GetPlayerID());
            playersCount--;
            if (playerData.GetPlayerRole() == PlayerRoles.Developer)
            {
                developerList.Remove(playerData.GetPlayerID());
                developersCount--;
            }
            else
            {
                providerList.Remove(playerData.GetPlayerID());
                providersCount--;
            }
        }

    }

    public GameObject GetDeveloper(string developerID)
    {
        return developerList[developerID];
    }

    public GameObject GetProvider(string providerID)
    {
        return providerList[providerID];
    }



    //---------------------------- FIRM METHODS -----------------------------------///
    public void AddFirmName(string playerID, string firmName)
    {
        allFirms.Add(playerID, firmName);
        Debug.Log("pocet developerov" + developerList.Count);
        if (developerList.ContainsKey(playerID))
        {
           
            Debug.Log("Novy developer pridany");
            developersFirms.Add(firmName, playerID);
        }
        else
        {
            Debug.Log("pocet developerov" + developerList.Count);
            Debug.Log("Novy provider pridany");
            providersFirms.Add(firmName, playerID);
        }

    }

    public bool TryToChangeFirmName(string playerID, string newFirmName, string oldFirmName)
    {
        if (allFirms.ContainsKey(newFirmName))
        {
            Debug.Log("Firm name already exists");
            return false;

        }
        else
        {
            Debug.Log("FirmNameWillBeSet");
            allFirms.Remove(oldFirmName);
            allFirms.Add(newFirmName, playerID);
            //updating firms name in description dictionary
            string description = allFirmDescriptions[oldFirmName];
            allFirmDescriptions.Remove(oldFirmName);
            allFirmDescriptions.Add(newFirmName, description);
            Debug.Log(allFirms.Count);
            if (developerList.ContainsKey(playerID))
            {
                AddDevevelopersFirm(playerID, newFirmName, oldFirmName);
            }
            else
            {
                AddProvidersFirm(playerID, newFirmName, oldFirmName);
            }

            return true;
        }
    }

    public void AddDevevelopersFirm(string playerID, string newFirmName, string oldFirmName)
    {
        Debug.Log("Developerova firma pridana do dev zoznamu");
        developersFirms.Remove(oldFirmName);
        developersFirms.Add(newFirmName, playerID);
        
    }

    public void AddProvidersFirm(string playersID, string newFirmName, string oldFirmName)
    {
        Debug.Log("Providerova firma pridana do providerovho zoznamu");
        providersFirms.Remove(oldFirmName);
        providersFirms.Add(newFirmName, playersID);
        
    }


    public void UpdateFirmDescription(string firmName, string firmDescription)
    {
        Debug.Log("Entering function: UPDATE firm Description");
        if (allFirmDescriptions.ContainsKey(firmName))
        {
            Debug.Log(firmName);
            allFirmDescriptions[firmName] = firmDescription;
        }
        else
        {
            allFirmDescriptions.Add(firmName, firmDescription);
        } 
    }

    public List<string> GetDevelopersFirms()
    {
        return new List<string>(developersFirms.Keys);
    }


    //------------------------------------- GET PLAYER ID FROM FIRM NAME-------------------------------------------
    public string GetDevelopersFirmPlayerID(string firmsName)
    {
        return developersFirms[firmsName];
    }

    public string GetProvidersFirmPlayerID(string firmsName)
    {
        return providersFirms[firmsName];
    }

    //------------------------------ FIRMS HOOKS
    public void OnDevelopersFirmsChange(SyncDictionaryStringString.Operation op, string firmName, string playerID)
    {
        GameHandler.singleton.GetLocalPlayer().GetMyPlayerUIObject().GetComponent<ContractUIHandler>().UpdateDeveloperDropdownOptions(GetDevelopersFirms());
    }


}
