using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class GameHandler : NetworkBehaviour {

    /// <summary>
    /// This class creates/deletes games, shows list of games
    /// loads/saves data on server
    /// </summary>
    public GameObject gamePrefab;
    public GameObject gameUIPrefab;
    public GameObject gameUIPrefabInstructor;
    public GameObject gameListUIPrefab;
    public GameObject gameListUIPrefabAdmin;
    public GameObject gamePasswordVerificatorPrefab;

    //private string gameName;
    //private string gamePassword;
    
    private PlayerManager localPlayer;
    private InstructorManager instructor;

    private GameObject gameListUIGameObject;
    private GameObject gameListUIContent;
    private bool generatedGameList;

    public static GameHandler singleton;



    // This List contains all Game objects
    public static Dictionary<string, GameData> allGames;
    // This List contains all GameUI objects
    public static Dictionary<string, GameObject> allGamesUI;

    // This list holds data about all games and all their players
    //----------------------<gameID           playerID, PlayerData reference>------------------------------------------------------------//
    public static Dictionary<string, Dictionary<string, GameObject>> allPlayers = new Dictionary<string, Dictionary<string, GameObject>>();
    public static int count;


    //GETTERS & SETTERS
    public void SetLocalPlayer(PlayerManager localPlayer){ this.localPlayer = localPlayer; }
    public PlayerManager GetLocalPlayer() { return localPlayer; }
    //public void SetGameName(InputField gameName) { this.gameName = gameName.text; }
    //public void SetGamePassword(InputField gamePassword) { this.gamePassword = gamePassword.text; }
    public bool GetGeneratedGameList() { return generatedGameList; }

    public void SetInstructor(InstructorManager instructor) { this.instructor = instructor; }
    public InstructorManager GetInstructor() { return instructor; }

    private void Awake()
    {
        singleton = this;
        allGames = new Dictionary<string, GameData>();
        allGamesUI = new Dictionary<string, GameObject>();
    }


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(count);
    }

    //creates DEMO game when server starts;
    public override void OnStartServer()
    {              //name, password
        CreateGame("DEMO2", "666");
    }


    //This method generates visual representation of list of games
    public void GenerateGamesListUI()
    {
        Debug.Log("generating game list for player");
        //created list of game UI
        gameListUIGameObject = Instantiate(gameListUIPrefab);
        gameListUIContent = gameListUIGameObject.transform.Find("GameScrolList/GameListViewport/GameListContent").gameObject;
        GeneratingGamesUIForPlayer();
        generatedGameList = true;

    }

    public void GenerateGamesListUIForInstructor()
    {   if(instructor == null)
        {
            Debug.LogError("Instructor object in GameHandler is NULL. I connot create game list.");
        }
        Debug.Log("generating game list for player");
        gameListUIGameObject = Instantiate(gameListUIPrefabAdmin);
        gameListUIGameObject.GetComponent<CreateGameUIHandler>().SetInstructorManager(instructor);
        gameListUIContent = gameListUIGameObject.transform.Find("GameScrolList/GameListViewport/GameListContent").gameObject;
        GeneratingGamesUIForInstructor();
        generatedGameList = true;
    }
    

    public void GeneratingGamesUIForPlayer()
    {
        //adding exising games into UI representation
        foreach (GameData gameData in allGames.Values)
        {
            GameObject gameUI = Instantiate(gameUIPrefab);
            allGamesUI.Add(gameUI.GetInstanceID().ToString(), gameUI);
            gameUI.transform.SetParent(gameListUIContent.transform, false);
            gameData.SetGameUIHandler(gameUI.GetComponent<GameUIHandler>());
            gameData.GameUIUpdateAll();
        }
    }

    public void GeneratingGamesUIForInstructor()
    {
        //adding exising games into UI representation
        foreach (GameData gameData in allGames.Values)
        {
            GameObject gameUI = Instantiate(gameUIPrefabInstructor);
            allGamesUI.Add(gameUI.GetInstanceID().ToString(), gameUI);
            gameUI.transform.SetParent(gameListUIContent.transform, false);
            gameData.SetGameUIHandler(gameUI.GetComponent<GameUIHandler>());
            gameData.GameUIUpdateAll();
        }
    }

    public void RefreshGamesList()
    {
        if (GameHandler.singleton.instructor != null)
        {
            Debug.Log("refreshing game list");
            foreach (Transform child in gameListUIContent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            allGamesUI.Clear();
            GeneratingGamesUIForInstructor();
            return;
        }

        if (generatedGameList)
        {
            Debug.Log("refreshing game list");
            foreach (Transform child in gameListUIContent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            allGamesUI.Clear();

            GeneratingGamesUIForPlayer();

        }
    }

    public void DestroyGameListUI()
    {   if(gameListUIGameObject != null)
        {
            Destroy(gameListUIGameObject);
            generatedGameList = false;
        }       
    }

    public void GenerateGamePasswordVerificator(GameObject gameID)
    {
        GameObject GamePasswordVerificator = Instantiate(gamePasswordVerificatorPrefab);
        GamePasswordVerificator.GetComponent<GamePasswordVerification>().SetGameData(GameHandler.allGames[gameID.GetComponent<TextMeshProUGUI>().text].GetComponent<GameData>());
    }

    public void SetInstructorGameID(GameObject gameID)
    {
        GameHandler.singleton.GetLocalPlayer().SetPlayerGameID(gameID.GetComponent<TextMeshProUGUI>().text);
    }

    [Server]
    public void CreateGame(string name, string password)
    {
        Debug.Log("creating game");
        if (isServer)
        {
            GameObject game = Instantiate(gamePrefab);
            GameData gameData = game.GetComponent<GameData>();

            gameData.SetGameID(GenerateUniqueID());
            gameData.SetGameName(name);
            gameData.SetGameRound(1);
            gameData.SetPassoword(password);
            gameData.SetPlayersCount(0);
            gameData.SetProvidersCount(0);
            gameData.SetDevelopersCount(0);
            game.SetActive(true);
            NetworkServer.Spawn(game);
        }
        
    }

    public void DeleteGame(string name)  ///DO I want this? 
    {
        Debug.LogError("Deleting game not implemented");
    }
        
    public string GenerateUniqueID()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        int currentEpochTime = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
        int z1 = UnityEngine.Random.Range(0, 99);
        int z2 = UnityEngine.Random.Range(0, 99);
        string uid = currentEpochTime + ":" + z1 + ":" + z2;
        return uid;
    }




}
