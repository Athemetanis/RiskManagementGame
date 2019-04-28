using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateGameUIHandler : MonoBehaviour
{
    //VARIABLES
    private InstructorManager instructorManager;
    public InputField gameNameIF;
    public InputField gamePasswordIF;
    public Text warnignText;

    private string gameName;
    private string gamePassword;
   

    //GETTERS & SETTERS
    //public void SetGameName(InputField gameNameIF) { this.gameName = gameNameIF.text; }
    //public void SetGamePassword(InputField gamePasswordIF) { this.gamePassword = gamePasswordIF.text; }
    public void SetInstructorManager(InstructorManager instructorManager) { this.instructorManager = instructorManager; }

    private void Start()
    {
        warnignText.text = "";
    }


    public void TryToCreateGame()
    {
        gameName = gameNameIF.text;
        gamePassword = gamePasswordIF.text;

        if (gameName == "" || gamePassword == "")
        {
            warnignText.text = "Name or password of the game cannot be empty.";

        }
        else
        {
            warnignText.text = "";
            instructorManager.CreateGame(gameName, gamePassword);
            gameNameIF.text = "";
            gamePasswordIF.text = "";

        }
    }

}
