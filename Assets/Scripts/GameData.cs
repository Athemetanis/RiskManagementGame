using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SyncDictionaryStringString : SyncDictionary<string, string> { };


/// <summary>
/// Contains information about one particular game, such as: game name, ID, list of players, round, lists of developers/provieders and their firm names
/// </summary>
public class GameData : NetworkBehaviour
{
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
    //private int readyPlayersCount;

    private SyncListString readyPlayers = new SyncListString();
    private int playerEvaluated;

    private int developersEvaluatedCount;
           
    //--------------<playerID, GameObject player>-----------------//  Q: How am I syncing this? A: When script playerData starts on client/server it requests adding its game object into these lists. 
    private Dictionary<string, GameObject> playerList;
    private Dictionary<string, GameObject> developerList;
    private Dictionary<string, GameObject> providerList;

    private int developersCount;
    private int providersCount;

    //-------------------<firmName, playerID>---------------------------------------//
    private SyncDictionaryStringString allFirms = new SyncDictionaryStringString();
    
    private SyncDictionaryStringString developersFirms = new SyncDictionaryStringString();
    private SyncDictionaryStringString providersFirms = new SyncDictionaryStringString();

    //-------------------<firmName, description>---------------------------------------//
    private SyncDictionaryStringString allFirmDescriptions = new SyncDictionaryStringString();

    //-------------------<playerID, firmName>
    //private SyncDictionaryStringString allFirmsRevers = new SyncDictionaryStringString();
    private Dictionary<string, string> allFirmsRevers = new Dictionary<string, string>();

    //this variable holds reference on script of UI representation of game (if it exists) //LOCAL !!!
    private GameUIHandler gameUIHandler;

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
    public Dictionary<string, GameObject> GetPlayerList() { return playerList; }
    public Dictionary<string, GameObject> GetProviderList() { return providerList; }

    public void SetGameUIHandler(GameUIHandler gameUIHandler) { this.gameUIHandler = gameUIHandler; }
    
    //Add me when I awake - this is called on both the clients and the host, so everyone will know me 
    protected virtual void Awake()
    {       
         //syncvar not initialized...                 
    }

    public override void OnStartClient()
    {
        // Syn lists/dicts are already populated with anything the server set up
        // but we can subscribe to the callback in case it is updated later on
        developersFirms.Callback += OnDevelopersFirmsChange;
        allFirmDescriptions.Callback += OnFirmDescriptionChange;
        allFirms.Callback += OnAllFirmsChange;
        RecreateAllFirmsReverse();
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
            Debug.Log("Game with ID " + gameID + "was created");
        }

        if (GameHandler.allPlayers.ContainsKey(this.GetGameID()) == false)
        {
            GameHandler.allPlayers.Add(this.GetGameID(), new Dictionary<string, GameObject>());
            Debug.Log("Playerslots for game" + gameID + "were created");
        }
        if(GameHandler.singleton.GetGeneratedGameList())
        {
            GameHandler.singleton.RefreshGamesList();
        }
        
    }

    //GAME METHODS
    public void AddPlayerToGame(GameObject player)
    {
        PlayerData playerData = player.GetComponent<PlayerData>();
        if (playerList.ContainsKey(playerData.GetPlayerID()) == false)
        {
            playerList.Add(playerData.GetPlayerID(), player);
            playersCount = playerList.Keys.Count;
            if (playerData.GetPlayerRole() == PlayerRoles.Developer)
            {
                developerList.Add(playerData.GetPlayerID(), player);
                developersCount++;
            }
            else
            {
                providerList.Add(playerData.GetPlayerID(), player);
                providersCount++;
            }
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
    
    //GAME HOOKS
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

    //METHODS FOR UPDATING UI ELEMENTS
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

    //FIRM METHODS 
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
    public List<string> GetDevelopersFirms()
    {
        return new List<string>(developersFirms.Keys);
    }

    public bool TryToChangeFirmName(string playerID, string newFirmName, string oldFirmName)
    {
        if (allFirms.ContainsKey(newFirmName))
        {
            return false;
        }
        else
        {
            allFirms.Remove(oldFirmName);
            allFirms.Add(newFirmName, playerID);
            //updating firms name in description dictionary
            string description = allFirmDescriptions[oldFirmName];
            allFirmDescriptions.Remove(oldFirmName);
            allFirmDescriptions.Add(newFirmName, description);
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
    public void UpdateFirmDescription(string firmName, string firmDescription)
    {
        if (allFirmDescriptions.ContainsKey(firmName))
        {
            allFirmDescriptions[firmName] = firmDescription;
        }
        else
        {
            allFirmDescriptions.Add(firmName, firmDescription);
        }
    }

    public string GetDevelopersFirmPlayerID(string firmsName)
    {
        return developersFirms[firmsName];
    }
    public string GetProvidersFirmPlayerID(string firmsName)
    {
        return providersFirms[firmsName];
    }


    public void RecreateAllFirmsReverse()
    {
        allFirmsRevers.Clear();
        foreach (KeyValuePair<string, string> pair in allFirms)
        {
            allFirmsRevers.Add(pair.Value, pair.Key);
        }
    }
    public Dictionary<string, string> GetListDeveloperFirmNameDescription()
    {
        Dictionary<string, string> developerFirmNameDescritpion = new Dictionary<string, string>();

        foreach (string firmName in developersFirms.Keys)
        {
            if (allFirmDescriptions.ContainsKey(firmName))
            {
                developerFirmNameDescritpion.Add(firmName, allFirmDescriptions[firmName]);
            }
        }
        return developerFirmNameDescritpion;
    }
    public string GetFirmName(string playerID)
    {
        return allFirmsRevers[playerID];
    }
    
    //FIRMS HOOKS
    public void OnDevelopersFirmsChange(SyncDictionaryStringString.Operation op, string firmName, string playerID)
    {
        if (GameHandler.singleton.GetLocalPlayer().GetMyPlayerData() != null)
        {
            if (GameHandler.singleton.GetLocalPlayer().GetMyPlayerData().GetPlayerRole() == PlayerRoles.Provider)
            {
                ContractUIHandler contractUIHandler = GameHandler.singleton.GetLocalPlayer().GetMyPlayerUIObject().GetComponent<ContractUIHandler>();
                contractUIHandler.UpdateDeveloperDropdownOptions(GetDevelopersFirms());
                contractUIHandler.UpdateDeveloperFirmList(GetListDeveloperFirmNameDescription());
                contractUIHandler.UpdateUIContractListsContents();
            }
            else
            {
                ContractUIHandler contractUIHandler = GameHandler.singleton.GetLocalPlayer().GetMyPlayerUIObject().GetComponent<ContractUIHandler>();
                contractUIHandler.UpdateUIContractListsContents();
            }
        }
    }
    public void OnFirmDescriptionChange(SyncDictionaryStringString.Operation op, string firmName, string description)
    {

        if (true)
        {
            if (GameHandler.singleton.GetLocalPlayer().GetMyPlayerData() != null)
            {
                if (GameHandler.singleton.GetLocalPlayer().GetMyPlayerData().GetPlayerRole() == PlayerRoles.Provider)
                {   
                    ContractUIHandler contractUIHandler = GameHandler.singleton.GetLocalPlayer().GetMyPlayerUIObject().GetComponent<ContractUIHandler>();

                    contractUIHandler.UpdateDeveloperFirmList(GetListDeveloperFirmNameDescription());
                }
            }

        }
    }
    public void OnAllFirmsChange(SyncDictionaryStringString.Operation op, string firmName, string description)
    {
        RecreateAllFirmsReverse();
    }
    

    //METHODS FOR VALUATING GAMES AND MOVING TO NEXT QUARTER

    [Server]
    public void TryToAddPlayersToReadyServer(string playerID)
    {
        if (!readyPlayers.Contains(playerID))
        {
            readyPlayers.Add(playerID);
        }
        if (readyPlayers.Count == playerList.Count)
        {
            EvaluateGameRoundServer();
        }
    }

    [Server]
    public void EvaluateGameRoundServer()
    {
        foreach(GameObject developerGO in developerList.Values)
        {
            developerGO.GetComponent<PlayerData>().EvaluateContracts();
        }
    }

    [Server]
    public void AddDeveloperToEvaluated()
    {
        developersEvaluatedCount++;
        if (developersEvaluatedCount == developersCount)
        {
            foreach(GameObject providerGo in providerList.Values)
            {
                providerGo.GetComponent<PlayerData>().UpdateCurrentQuarterDataProvider();
            }

        }
    }
    [Server]
    public void AddPlayerToEvaluated()
    {
        playerEvaluated++;
        if(playerEvaluated == playersCount)
        {
            MoveToNextRound();
        }
    }

    [Server]
    public void MoveToNextRound()
    {
        gameRound = gameRound + 1;
        readyPlayers.Clear();
        playerEvaluated = 0;
        developersEvaluatedCount = 0;

    }



}
