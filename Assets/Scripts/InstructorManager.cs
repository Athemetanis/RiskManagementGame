using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InstructorManager : NetworkBehaviour
{   
    //VARIABLES
    [SyncVar(hook = "OnChangeGameID")]
    private string gameID;

    //REFERENCES
    private GameObject myInstructorUIObject;

    //GETTERS & SETTERS
    public void SetMyInstructorUIObject(GameObject instructorUI) { myInstructorUIObject = instructorUI; }
    public GameObject GetMyInstructorUIObject() { return myInstructorUIObject; }

    public override void OnStartServer()
    {
        gameID = "";
    }

    public override void OnStartClient()
    {
        GameHandler.singleton.SetInstructor(this);
    }

    public override void OnStartAuthority()
    {
        GameHandler.singleton.GenerateGamesListUIForInstructor();
    }


    //METHODS
    public void CreateGame(string gameName, string gamePassword)
    {
        CmdCreateGame(gameName, gamePassword);
    }
    [Command]
    public void CmdCreateGame(string gameName, string gamePassword)
    {
        GameHandler.singleton.CreateGame(gameName, gamePassword);
    }

    public void SetGameID(string gameID)
    {
        CmdSetGameID(gameID);
    }

    [Command]
    public void CmdSetGameID(string gameID)
    {
        this.gameID = gameID;
    }
    

    public void ForceGameNextQuarter(string gameID)
    {
        CmdForceGameNextQuarter(gameID);
    }

    public void CmdForceGameNextQuarter(string gameID)
    {
        GameHandler.allGames[gameID].ForceNextQuarter();
    }
    
    //HOOKS
    public void OnChangeGameID(string gameID)
    {
        Debug.Log("Instructor: GameID changed");
        this.gameID = gameID;
        if (GameHandler.allGames.ContainsKey(gameID))
        {
            GameHandler.allGames[gameID].SetInstructor(this.gameObject);
        }    
    }


    public void RefreshInstructorUI()
    {
        if(myInstructorUIObject != null)
        {
            myInstructorUIObject.GetComponent<InstructorGameInfoUIHandler>().RefreshGameInstructorUI();
        }
    }

}
