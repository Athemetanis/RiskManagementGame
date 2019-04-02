using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameData : NetworkBehaviour {

    //VARIABLES
    [SyncVar]
    public string gameID;
    [SyncVar]
    public string gameName;          //in the end set this to private
    [SyncVar]
    public string password;
    [SyncVar]
    private int gameRound;
    [SyncVar]
    private int playersCount;


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

     
    //Add me when I awake - this is called on both the clients and the host, so everyone will know me
    protected virtual void Awake()
    {       
        if (GameHandler.allPlayers.ContainsKey(this.GetGameID()) == false)
        {
            GameHandler.allPlayers.Add(this.GetGameID(), new Dictionary<string, PlayerData>());
            Debug.Log("Game with ID " + gameID + "was created");
        }                    
    }


    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //METHODS

    public void AddPlayerToGame(GameObject player)
    {
        string playerID = player.GetComponent<PlayerData>().playerID;
        if (playerList.ContainsKey(playerID) == false)
        {
            playerList.Add(playerID, player);
        }

        playersCount++;
        
    }

    public void RemovePlayerFromGame(GameObject player)
    {
        string playerID = player.GetComponent<PlayerData>().playerID;
        if (playerList.ContainsKey(playerID) == true)
        {
            playerList.Remove(playerID);
        }
        playersCount--;

    }




   
}
