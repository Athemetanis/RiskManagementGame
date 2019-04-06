using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerData : NetworkBehaviour {

    /// This class contains general information about player - variables are set during creation and dont change during game

    //VARIABLES 
    [SyncVar]
    public string gameID;
    [SyncVar]
    public string playerID;   //in the end set this to private
    [SyncVar]
    public string playerRole;


    
    //GETTERS & SETTERS
    public void SetGameID(string gameID) { this.gameID = gameID; }
    public string GetGameID() { return gameID; }
    public void SetPlayerID(string playerID) { this.playerID = playerID; }
    public string GetPlayerID() { return playerID; }
    public void SetPlayerRole(string playerRole) { this.playerRole = playerRole; }
    public string GetPlayerRole() { return playerRole; }


    //METHODS
    void Start()
    {
        if (GameHandler.allPlayers[gameID].ContainsKey(playerID) == false)
        {
            GameHandler.allPlayers[gameID].Add(playerID, this);
        }

        if (GameHandler.allGames[gameID] == true)
        {
            GameHandler.allGames[gameID].AddPlayerToGame(this.gameObject);
        }

    }






}
