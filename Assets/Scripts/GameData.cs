using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameData : NetworkBehaviour {

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
   
    private int developersCount;
    
    private int providersCount;
    
    private int readyPlayersCount;


    //this variable holds reference on script of UI representation of game (if it exists) //LOCAL !!!
    private GameUIHandler gameUIHandler;


    //--------------<playerID, GameObject player>---------------------------------------//   HOW I WILL BE SYNCING THIS??? /// pri každom starte playerdat - na cliente i na serveri sa zavola add

    private Dictionary<string, GameObject> playerList;
    private Dictionary<string, GameObject> developerList;
    private Dictionary<string, GameObject> providerList;

  
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
	
	// Update is called once per frame
	void Update () {

        
		
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

       // string playerID = player.GetComponent<PlayerData>().playerID;
        if (playerList.ContainsKey(playerData.GetPlayerID()) == false)
        {
            playerList.Add(playerData.GetPlayerID(), player);
            playersCount = playerList.Keys.Count;
            if(playerData.GetPlayerRole() == PlayerRoles.Developer)
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
        //string playerID = player.GetComponent<PlayerData>().playerID;
       

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

    


   
}
