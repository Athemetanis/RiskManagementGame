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
    //private int gameQuarter;


    public GameData GetGame() { return game; }
    public void SetGameID(string gameID) { this.gameID = gameID; }
    public string GetGameID() { return gameID; }
    public void SetInstructorManager(InstructorManager instructorManager) { this.instructorManager = instructorManager; }

    public InstructorAllPlayersStatsUIHandler instructorAllPlayersStatsUIHandler;
    public InstructorProviderStatsUIHandler instructorProviderStatsUIHandler;
    public InstructorDeveloperStatsUIHandler instructorDeveloperStatsUIHandler;
    public InstructorIndividualStatsUIHandler instructorIndividualStatsUIHandler;

    private void Start()
    {
        game = GameHandler.allGames[gameID].GetComponent<GameData>();
        instructorManager = null;

        instructorAllPlayersStatsUIHandler = this.gameObject.GetComponent<InstructorAllPlayersStatsUIHandler>();
        instructorAllPlayersStatsUIHandler.SetInstructorGameInfoUIHandler(this);
        instructorIndividualStatsUIHandler = this.gameObject.GetComponent<InstructorIndividualStatsUIHandler>();
        instructorIndividualStatsUIHandler.SetInstructorGameInfoUIHandler(this);
        instructorProviderStatsUIHandler = this.gameObject.GetComponent<InstructorProviderStatsUIHandler>();
        instructorProviderStatsUIHandler.SetInstructorGameInfoUIHandler(this);
        instructorDeveloperStatsUIHandler = this.gameObject.GetComponent<InstructorDeveloperStatsUIHandler>();
        instructorDeveloperStatsUIHandler.SetInstructorGameInfoUIHandler(this);

        UpdateTextInfo();
        GeneratePlayerGameStateList();
        instructorAllPlayersStatsUIHandler.enabled = true;
        instructorIndividualStatsUIHandler.enabled = true;
        instructorProviderStatsUIHandler.enabled = true;
        instructorDeveloperStatsUIHandler.enabled = true;
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
        foreach (Transform child in playerGameStateContainer.transform)
        { GameObject.Destroy(child.gameObject); }

        foreach (string playerID in game.GetPlayerList().Keys)
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
        instructorManager = GameHandler.singleton.GetInstructor();
        instructorManager.ForceGameNextQuarter(gameID);
    }

    public void RefreshGameInstructorUI()
    {
        UpdateTextInfo();
        GeneratePlayerGameStateList();
        instructorAllPlayersStatsUIHandler.UpdateContent();
        instructorIndividualStatsUIHandler.UpdateContent();
        instructorProviderStatsUIHandler.UpdateContent();
        instructorDeveloperStatsUIHandler.UpdateContent();
    }


}
