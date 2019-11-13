using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstructorGameInfoUIHandler : MonoBehaviour
{
    public TextMeshProUGUI gameIDText;
    public TextMeshProUGUI passwordText;
    public TextMeshProUGUI quarterText;
    public TextMeshProUGUI numberOfPlayersText;

    public GameObject playerGameStateContainer;
    public GameObject playerGameStatePrefab;

    private InstructorManager instructorManager;
    private string gameID;
    private GameData game;
    private int gameQuarter;


    public GameData GetGame() { return game; }
    public void SetGameID(string gameID) { this.gameID = gameID; }
    public string GetGameID() { return gameID; }
    public void SetInstructorManager(InstructorManager instructorManager) { this.instructorManager = instructorManager; }

    public InstructorAllPlayersStatsUIHandler instructorAllPlayersStatsUIHandler;
      

    private void Start()
    {
        game = GameHandler.allGames[gameID].GetComponent<GameData>();
        instructorAllPlayersStatsUIHandler = this.gameObject.GetComponent<InstructorAllPlayersStatsUIHandler>();
        instructorAllPlayersStatsUIHandler.SetInstructorGameInfoUIHandler(this);
        UpdateTextInfo();
        GeneratePlayerGameStateList();
        instructorAllPlayersStatsUIHandler.enabled = true;
    }

    public void UpdateTextInfo()
    {
        gameIDText.text = game.GetGameName();
        passwordText.text = game.GetPassword();
        quarterText.text = game.GetGameRound().ToString();
        numberOfPlayersText.text = game.GetPlayersCount().ToString();

    }

    public void GeneratePlayerGameStateList()
    {
        foreach(string playerID in game.GetPlayerList().Keys)
        {
            string firmName = game.GetFirmName(playerID);
            bool playerGameState = game.IsPlayerReady(playerID);
            GameObject playerGameStateUIComponent = Instantiate(playerGameStatePrefab);
            playerGameStateUIComponent.transform.SetParent(playerGameStateContainer.transform, false);
            PlayerGameStateUIComponentHandler playerGameStateUIComponentHandler = playerGameStateUIComponent.GetComponent<PlayerGameStateUIComponentHandler>();
            playerGameStateUIComponentHandler.SetUpPlayerGameStateUIComponent(playerID, firmName, playerGameState);                        
        }
    }

    public void ForceGameToNextRound()
    {
        instructorManager.ForceGameNextQuarter(gameID);
    }

}
