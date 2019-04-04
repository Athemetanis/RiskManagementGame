using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameData : NetworkBehaviour {

    //VARIABLES
    [SyncVar(hook = "OnChangeGameID")]
    public string gameID;
    [SyncVar(hook = "OnChangeGameName")]
    public string gameName;          //in the end set this to private
    [SyncVar]
    public string password;
    [SyncVar(hook = "OnChangeGameRound")]
    private int gameRound;
    [SyncVar(hook = "OnChangePlayersCount")]
    private int playersCount;

    //this variable holds reference on script of UI representation of game (if it exists) 
    private GameUIHandler gameUIHandler;


    //--------------<playerID, GameObject player>---------------------------------------//   HOW I WILL BE SYNCING THIS??? /// pri každom awaku playerdat - na cliente i na serveri sa zavola add
    private Dictionary<string, GameObject> playerList = new Dictionary<string, GameObject>();
    
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

    public void SetGameUIHandler(GameUIHandler gameUIHandler) { this.gameUIHandler = gameUIHandler; }


     
    //Add me when I awake - this is called on both the clients and the host, so everyone will know me
    protected virtual void Awake()
    {       
         //syncvar not initialized...                 
    }


    // Use this for initialization
    void Start ()
    {   

        if (GameHandler.allGames.ContainsKey(this.gameID) == false)
        {
            GameHandler.allGames.Add(this.gameID, this);
        }

        if (GameHandler.allPlayers.ContainsKey(this.GetGameID()) == false)
        {
            GameHandler.allPlayers.Add(this.GetGameID(), new Dictionary<string, PlayerData>());
            Debug.Log("Game with ID " + gameID + "was created");
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

    public void GameUIUpdate()
    {   
        gameUIHandler.ChangeIDText(gameID);
        gameUIHandler.ChangeNameText(gameName);
        gameUIHandler.ChangeRoundText(gameRound);
        gameUIHandler.ChangePlayersCountText(playersCount);
    }

    public void AddPlayerToGame(GameObject player)
    {
        string playerID = player.GetComponent<PlayerData>().playerID;
        if (playerList.ContainsKey(playerID) == false)
        {
            playerList.Add(playerID, player);
            playersCount++;
        }
    }

    public void RemovePlayerFromGame(GameObject player)
    {
        string playerID = player.GetComponent<PlayerData>().playerID;
        if (playerList.ContainsKey(playerID) == true)
        {
            playerList.Remove(playerID);
            playersCount--;
        }
    }

    


   
}
