using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InstructorManager : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameHandler.singleton.GenerateGamesListUIForInstructor(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void CreateGame(string gameName, string gamePassword)
    {
        CmdCreateGame(gameName, gamePassword);
    }

    [Command]
    public void CmdCreateGame(string gameName, string gamePassword)
    {
        GameHandler.singleton.CreateGame(gameName, gamePassword);
    }


}
