using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerData : NetworkBehaviour {
    /// <summary>
    /// This class contains general player data
    /// </summary>
    //VARIABLES 
    [SyncVar]
    public string gameID;
    [SyncVar]
    public string playerID;
    [SyncVar]
    private string firmName;


    //GETTERS & SETTERS
    public void SetGameID(string gameID) { this.gameID = gameID; }
    public string GetGameID() { return gameID; }
    public void SetPlayerID(string playerID) { this.playerID = playerID; }
    public string GetPlayerID() { return playerID; }
    public void SetFirmName(string firmName) { this.firmName = firmName; }
    public string GetFirmName() { return firmName; }



    //Add me when I awake - this is called on both the clients and the host, so everyone will know me
    protected virtual void Awake()
    {   
        
    }

    public override void  OnStartClient()
    {
        Debug.Log(gameID + "/" + playerID + "/" + this);

        if (GameHandler.allPlayers[gameID].ContainsKey(playerID) == false)
        {
            GameHandler.allPlayers[gameID].Add(playerID, this);

            //GameObject game = GameHandler.allGames[gameID];


            //game.GetComponent<GameData>().AddPlayerToGame(this.gameObject);

        }
    }

    public override void OnStartServer()
    {
        Debug.Log(gameID + "/" + playerID + "/" + this);

        if (GameHandler.allPlayers[gameID].ContainsKey(playerID) == false)
        {
            GameHandler.allPlayers[gameID].Add(playerID, this);

            //GameObject game = GameHandler.allGames[gameID];


            //game.GetComponent<GameData>().AddPlayerToGame(this.gameObject);

        }
    }



    // Use this for initialization - 
    void Start ()
    {

        
    }

    

    // Update is called once per frame
    void Update ()
    {
		
	}

    //METHODS

    [Command]
    public void CmdChangeFirmName(string firmName)
    {
        this.firmName = firmName;
    }




}
