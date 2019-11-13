using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePasswordVerification : MonoBehaviour
{
    public Text WarningText;
    private GameData gameData;
    private string userGamePassword;
    
    ///GETTERS & SETTERS
   public void SetUserGamePassword(InputField passwordInputfield)
    {
        userGamePassword = passwordInputfield.text;
    }

   public void SetGameData(GameData gameData) { this.gameData = gameData; }


     
    public void GamePasswordVerify()
    {
        if (gameData == null)
        {
            Debug.Log("gamedata is null");
            return;
        }

        if(GameHandler.singleton.GetLocalPlayer() == null)
        {
            Debug.Log("localplayers is null");
            return;
        } 

        if (gameData.GetPassword().Equals(userGamePassword))
        {
            WarningText.GetComponent<Text>().enabled= false;
            GameHandler.singleton.GetLocalPlayer().SetPlayerGameID(gameData.GetGameID());
            Destroy(this.gameObject);
        }
        else
        {
            WarningText.GetComponent<Text>().enabled = true;
        }

    }







}
