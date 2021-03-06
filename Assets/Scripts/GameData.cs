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

    private int maxNumberOfPlayers = 30;

    private GameObject instructor;
    //private int readyPlayersCount;

    private SyncListString readyPlayers = new SyncListString();
    private int playerEvaluated;
    private int developersEvaluatedCount;
           
    //--------------<playerID, GameObject player>-----------------//  Q: How am I syncing this? A: When script playerData starts on client/server it requests adding its game object into these lists. 
    private Dictionary<string, GameObject> playerList = new Dictionary<string, GameObject>() { };
    private Dictionary<string, GameObject> developerList = new Dictionary<string, GameObject>() { };
    private Dictionary<string, GameObject> providerList = new Dictionary<string, GameObject>() { };

    private int developersCount;
    private int providersCount;

    //-------------------<firmName, playerID>---------------------------------------//
    private SyncDictionaryStringString allFirms = new SyncDictionaryStringString();
    
    private SyncDictionaryStringString developersFirms = new SyncDictionaryStringString();
    private SyncDictionaryStringString providersFirms = new SyncDictionaryStringString();
    //-------------------<firmName, description>---------------------------------------//
    private SyncDictionaryStringString allFirmDescriptions = new SyncDictionaryStringString();

    //-------------------<playerID, firmName>
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
    public Dictionary<string, GameObject> GetDeveloperList() { return developerList; }

    public void SetGameUIHandler(GameUIHandler gameUIHandler) { this.gameUIHandler = gameUIHandler; }
    public void SetInstructor(GameObject instructor) { this.instructor = instructor; }
    public GameObject GetInstructor() { return instructor; }

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
        readyPlayers.Callback += OnReadyPlayersChange;
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
    /// <summary>
    /// Adds new player to the game when he joins the game for the first time.
    /// Called on the start of the script PlayerManager.
    /// </summary>
    /// <param name="player"> player's object </param>
    public void AddPlayerToGame(GameObject player)
    {
        PlayerManager playerData = player.GetComponent<PlayerManager>();
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

    /// <summary>
    /// This methos is not used yet. Intention of this method was removing unactive player from the game but it causes more problems than solves. 
    /// </summary>
    /// <param name="player">player's object </param>
    public void RemovePlayerFromGame(GameObject player)
    {
        PlayerManager playerData = player.GetComponent<PlayerManager>();
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

    /// <summary>
    /// According ID it returns developer's object if developer joined this game.
    /// </summary>
    /// <param name="developerID"> ID of the developer which object i want to get. </param>
    /// <returns> Developer's game object</returns>
    public GameObject GetDeveloper(string developerID)
    {
        return developerList[developerID];
    }
    /// <summary>
    /// According ID it returns provider's object if provider joined this game.
    /// </summary>
    /// <param name="providerID"> ID of the provider which object i want to get.</param>
    /// <returns> Provider's game object</returns>
    public GameObject GetProvider(string providerID)
    {
        return providerList[providerID];
    }
    /// <summary>
    /// According ID it returns player's object if provider joined this game.
    /// </summary>
    /// <param name="playerID"> ID of the player which object i want to get.</param>
    /// <returns>player's game object</returns>
    public GameObject GetPlayer(string playerID)
    {
        return playerList[playerID];
    }
    
    //GAME HOOKS  - updates values on client when the values was changed on server
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
        if(instructor != null)
        {
            instructor.GetComponent<InstructorManager>().RefreshInstructorUI();
        }
        if (GameHandler.singleton.GetLocalPlayer() != null  && playersCount != 0)
        {
            if (GameHandler.singleton.GetLocalPlayer().GetMyPlayerUIObject() != null)
            {
                TabUIHandler tabUIHandler = GameHandler.singleton.GetLocalPlayer().GetMyPlayerUIObject().GetComponent<TabUIHandler>();
                tabUIHandler.HandlingQTabs();
            }
        }
    }
    public void OnChangePlayersCount(int playersCount)
    {
        this.playersCount = playersCount;
        if (gameUIHandler != null)
        {
            gameUIHandler.ChangePlayersCountText(playersCount);
        }
        if (instructor != null)
        {
            instructor.GetComponent<InstructorManager>().RefreshInstructorUI();
        }
        if (GameHandler.singleton.GetLocalPlayer() != null && playersCount != 0)
        {   
            if(GameHandler.singleton.GetLocalPlayer().GetMyPlayerUIObject() != null)
            {
                ChatUIHandler chatUIHandler = GameHandler.singleton.GetLocalPlayer().GetMyPlayerUIObject().GetComponent<ChatUIHandler>();
                chatUIHandler.GeneratePlayersContent();
            }           
        }
    }

    //METHODS FOR UPDATING UI ELEMENTS

    /// <summary>
    /// Updates all vlaues of UI element which represents this game.
    /// </summary>
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

    /// <summary>
    /// Adds new firm to the game
    /// </summary>
    /// <param name="playerID"> ID of the player which owns the firm </param>
    /// <param name="firmName"> Name of the firm</param>
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
    /// <summary>
    /// Updates developer's firm's name of this game
    /// </summary>
    /// <param name="playerID"> ID of the player which owns the firm</param>
    /// <param name="newFirmName"> new name of the firm</param>
    /// <param name="oldFirmName">old name of the firm </param>
    public void AddDevevelopersFirm(string playerID, string newFirmName, string oldFirmName)
    {
        Debug.Log("Developerova firma pridana do dev zoznamu");
        developersFirms.Remove(oldFirmName);
        developersFirms.Add(newFirmName, playerID);
        
    }
    /// <summary>
    /// Updates providerds's firm's name of this game
    /// </summary>
    /// <param name="playerID"> ID of the player which owns the firm</param>
    /// <param name="newFirmName"> new name of the firm</param>
    /// <param name="oldFirmName">old name of the firm </param>
    public void AddProvidersFirm(string playersID, string newFirmName, string oldFirmName)
    {
        Debug.Log("Providerova firma pridana do providerovho zoznamu");
        providersFirms.Remove(oldFirmName);
        providersFirms.Add(newFirmName, playersID);
        
    }
    /// <summary>
    /// Get all development firms. 
    /// </summary>
    /// <returns> List of all development firms </returns>
    public List<string> GetDevelopersFirms()
    {
        return new List<string>(developersFirms.Keys);
    }

    /// <summary>
    /// This methods enables changing the firm name 
    /// </summary>
    /// <param name="playerID"> player's ID which owns the firm</param>
    /// <param name="newFirmName">new firm name</param>
    /// <param name="oldFirmName">old firm name </param>
    /// <returns></returns>
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

    /// <summary>
    /// Updates description of the firm 
    /// </summary>
    /// <param name="firmName">name of the firm which description will be changed </param>
    /// <param name="firmDescription"> new description of the firm </param>
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

    /// <summary>
    /// Get ID of the owner of development firm
    /// </summary>
    /// <param name="firmsName"> development firm name </param>
    /// <returns> ID of the player which owns the firm </returns>
    public string GetDevelopersFirmPlayerID(string firmsName)
    {
        return developersFirms[firmsName];
    }
    /// <summary>
    /// Get ID of the owner of provider firm
    /// </summary>
    /// <param name="firmsName">provider firm name </param>
    /// <returns>ID of the player which owns the firm</returns>
    public string GetProvidersFirmPlayerID(string firmsName)
    {
        return providersFirms[firmsName];
    }
    public string GetFirmPlayerID(string firmName)
    {
        return allFirms[firmName];
    }

    /// <summary>
    /// Recereated list of playerID + firmName  in reverse order
    /// </summary>
    public void RecreateAllFirmsReverse()
    {
        allFirmsRevers.Clear();
        foreach (KeyValuePair<string, string> pair in allFirms)
        {
            allFirmsRevers.Add(pair.Value, pair.Key);
        }
    }

    /// <summary>
    /// get description of all developers firms
    /// </summary>
    /// <returns>dictionary of developers firm names + descriptions</returns>
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

    /// <summary>
    /// get description of all providres firms
    /// </summary>
    /// <returns>dictionary of providers firm names + descriptions</returns>
    public Dictionary<string, string> GetListProviderFirmNameDescription()
    {
        Dictionary<string, string> providerFirmNameDescrition = new Dictionary<string, string>();

        foreach (string firmName in providersFirms.Keys)
        {
            if (allFirmDescriptions.ContainsKey(firmName))
            {
                providerFirmNameDescrition.Add(firmName, allFirmDescriptions[firmName]);
            }
        }
        return providerFirmNameDescrition;
    }


    /// <summary>
    /// Get name of the firm from playerID
    /// </summary>
    /// <param name="playerID"> ID of the player </param>
    /// <returns> firm owned by this player </returns>
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
                contractUIHandler.UpdateProviderFirmList(GetListProviderFirmNameDescription());
                contractUIHandler.UpdateUIContractListsContents();
            }
        }
    }

    public void OnFirmDescriptionChange(SyncDictionaryStringString.Operation op, string firmName, string description)
    {
        if (GameHandler.singleton.GetLocalPlayer().GetMyPlayerData() != null)
        {
            if (GameHandler.singleton.GetLocalPlayer().GetMyPlayerData().GetPlayerRole() == PlayerRoles.Provider)
            {
                ContractUIHandler contractUIHandler = GameHandler.singleton.GetLocalPlayer().GetMyPlayerUIObject().GetComponent<ContractUIHandler>();

                contractUIHandler.UpdateDeveloperFirmList(GetListDeveloperFirmNameDescription());
            }
            else
            {
                ContractUIHandler contractUIHandler = GameHandler.singleton.GetLocalPlayer().GetMyPlayerUIObject().GetComponent<ContractUIHandler>();
                contractUIHandler.UpdateProviderFirmList(GetListProviderFirmNameDescription());

            }
        }
        if (GameHandler.singleton.GetLocalPlayer().GetMyPlayerUIObject() != null)
        {
            ChatUIHandler chatUIHandler = GameHandler.singleton.GetLocalPlayer().GetMyPlayerUIObject().GetComponent<ChatUIHandler>();
            chatUIHandler.GeneratePlayersContent();
        }
    }
    public void OnAllFirmsChange(SyncDictionaryStringString.Operation op, string firmName, string description)
    {   
        RecreateAllFirmsReverse();
        if (instructor != null)
        {
            instructor.GetComponent<InstructorManager>().RefreshInstructorUI();
        }
        if (GameHandler.singleton.GetLocalPlayer() != null)
        {
            if (GameHandler.singleton.GetLocalPlayer().GetMyPlayerUIObject() != null)
            {
                ChatUIHandler chatUIHandler = GameHandler.singleton.GetLocalPlayer().GetMyPlayerUIObject().GetComponent<ChatUIHandler>();
                chatUIHandler.GeneratePlayersContent();
            }
        }
        if (GameHandler.singleton.GetLocalPlayer().GetMyPlayerUIObject() != null)
        {
            ChatUIHandler chatUIHandler = GameHandler.singleton.GetLocalPlayer().GetMyPlayerUIObject().GetComponent<ChatUIHandler>();
            chatUIHandler.GeneratePlayersContent();
        }
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
            InvokeGameEvents();
            
        }
    }
    public void OnReadyPlayersChange(SyncListString.Operation op, int index, string item)
    {
        if (instructor != null)
        {
            instructor.GetComponent<InstructorManager>().RefreshInstructorUI();
        }
    }

    [Server]
    public void InvokeGameEvents()
    {
        foreach (GameObject playerGO in playerList.Values)
        {
            playerGO.GetComponent<EventManager>().InvokeGameEvent();
        }
        EvaluateGameRoundServer();
    }

    [Server]
    public void EvaluateGameRoundServer()
    {
        foreach(GameObject developerGO in developerList.Values)
        {
            developerGO.GetComponent<PlayerManager>().EvaluateContracts();
        }
    }

    [Server]
    public void AddDeveloperToEvaluated()
    {
        developersEvaluatedCount++;
        if (developersEvaluatedCount == developersCount)
        {   
            foreach(GameObject developerGo in developerList.Values)
            {
                developerGo.GetComponent<PlayerManager>().UpdateCurrentQuarterDataDeveloper();
            }
            foreach (GameObject providerGo in providerList.Values)
            {
                providerGo.GetComponent<PlayerManager>().UpdateCurrentQuarterDataProvider();
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

    public bool IsPlayerReady(string playerID)
    {
        return readyPlayers.Contains(playerID);
    }


    public void ForceNextQuarter()
    {
        foreach(string playerID in playerList.Keys)
        {
            if(!readyPlayers.Contains(playerID))
            {
                TryToAddPlayersToReadyServer(playerID);
            }
        }
    }

}
