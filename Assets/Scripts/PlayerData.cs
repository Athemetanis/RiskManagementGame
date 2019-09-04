using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum PlayerRoles { Developer, Provider }

public class PlayerData : NetworkBehaviour {

    /// This class contains general information about player - variables are set during creation and do not change during game

    //VARIABLES 
    [SyncVar]
    private string gameID;
    [SyncVar]
    private string playerID;
    [SyncVar]
    private PlayerRoles playerRole;

    //LOCAL VARIABLES
    private GameObject playerUI;
        
    //GETTERS & SETTERS
    public void SetGameID(string gameID) { this.gameID = gameID; }
    public string GetGameID() { return gameID; }

    public void SetPlayerID(string playerID) { this.playerID = playerID; }
    public string GetPlayerID() { return playerID; }

    public void SetPlayerRole(PlayerRoles playerRole) { this.playerRole = playerRole; }
    public PlayerRoles GetPlayerRole() { return playerRole; }

    public void SetPlayerUI(GameObject playerUI) { this.playerUI = playerUI; }
    public GameObject GetPlayerUI() { return playerUI;  }

    //METHODS
    void Start()
    {
        if (GameHandler.allPlayers[gameID].ContainsKey(playerID) == false)
        {
            Debug.Log("player registered to all players: " + playerID);
            GameHandler.allPlayers[gameID].Add(playerID, this.gameObject);
        }

        if (GameHandler.allGames[gameID] == true)
        {
            Debug.Log("player tries to be added into game " + gameID);
            GameHandler.allGames[gameID].AddPlayerToGame(this.gameObject);
            
        }
    }

}
