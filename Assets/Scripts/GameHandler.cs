using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

/// <summary>
/// This class creates/deletes games, shows list of games
/// loads/saves data on server <<NOT_YET
/// </summary>
public class GameHandler : NetworkBehaviour {
    
    public GameObject gamePrefab;
    public GameObject gameUIPrefab;
    public GameObject gameUIPrefabInstructor;
    public GameObject gameListUIPrefab;
    public GameObject gameListUIPrefabAdmin;
    public GameObject gamePasswordVerificatorPrefab;

    //private string gameName;
    //private string gamePassword;
    
    private ConnectionManager localPlayer;
    private InstructorManager instructor;

    private GameObject gameListUIGameObject;
    private GameObject gameListUIContent;
    private bool generatedGameList;

    public static GameHandler singleton;
    

    // This List contains all Game objects - References on all existing GameData scripts. This variables is same for server and all players. New games is added to the list in the Start of the GameData script
    public static Dictionary<string, GameData> allGames;
    // This List contains all GameUI objects - UI elements which represent existing game in the UI list. 
    public static Dictionary<string, GameObject> allGamesUI;

    // This list holds data about all games and all their players
    //----------------------<gameID           playerID, PlayerData reference>------------------------------------------------------------//
    public static Dictionary<string, Dictionary<string, GameObject>> allPlayers = new Dictionary<string, Dictionary<string, GameObject>>();
    public static int count;


    //GETTERS & SETTERS
    public void SetLocalPlayer(ConnectionManager localPlayer){ this.localPlayer = localPlayer; }
    public ConnectionManager GetLocalPlayer() { return localPlayer; }
    //public void SetGameName(InputField gameName) { this.gameName = gameName.text; }
    //public void SetGamePassword(InputField gamePassword) { this.gamePassword = gamePassword.text; }
    public bool GetGeneratedGameList() { return generatedGameList; }

    public void SetInstructor(InstructorManager instructor) { this.instructor = instructor; }
    public InstructorManager GetInstructor() { return instructor; }

    /// <summary>
    /// Called before Start and Start of other scripts in the game. 
    /// </summary>
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


    /// <summary>
    ///Creates DEMO game when server starts;
     /// </summary>
    public override void OnStartServer()
    {              //name, password
        CreateGame("DEMO2", "666");
    }

    /// <summary>
    ///  Generates UI list of games for player. This list is visualy different from list generated for instructor.
    /// </summary>
    public void GenerateGamesListUI()
    {
        // Debug.Log("generating game list for player");
        //created list of game UI
        gameListUIGameObject = Instantiate(gameListUIPrefab);
        gameListUIContent = gameListUIGameObject.transform.Find("GameScrolList/GameListViewport/GameListContent").gameObject;
        GeneratingGamesUIForPlayer();
        generatedGameList = true;

    }

    /// <summary>
    /// Generates UI list of games for instructor. This list is visualy different from list generated for player which do not provide ability to create new game. 
    /// </summary>
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
    
    /// <summary>
    /// Generates UI element for every existing game.
    /// These will be content of the list containing all games for player.
    /// </summary>
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

    /// <summary>
    /// Generates UI element for every existing game.
    /// These will be content of the list containing all games for instructor.
    /// </summary>
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

    /// <summary>
    /// Refreshes any list of games if exists. This methods is used for both instructor and also the player.
    /// </summary>
    public void RefreshGamesList()
    {
        if (GameHandler.singleton.instructor != null)
        {
            Debug.Log("refreshing game list for instructor");
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
            Debug.Log("refreshing game list for player");
            foreach (Transform child in gameListUIContent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            allGamesUI.Clear();

            GeneratingGamesUIForPlayer();

        }
    }

    /// <summary>
    /// This method destroys any visual representation of list of games. Same for instructor and player.
    /// </summary>
    public void DestroyGameListUI()
    {   if(gameListUIGameObject != null)
        {
            Destroy(gameListUIGameObject);
            generatedGameList = false;
        }       
    }

    /// <summary>
    /// This method generates UI windows for password verification.
    /// This method is invoked when UI button is clicked.
    /// </summary>
    /// <param name="gameID"> ID of the game player wants to join and for which is required password </param>
    public void GenerateGamePasswordVerificator(GameObject gameID)
    {
        GameObject GamePasswordVerificator = Instantiate(gamePasswordVerificatorPrefab);
        GamePasswordVerificator.GetComponent<GamePasswordVerification>().SetGameData(GameHandler.allGames[gameID.GetComponent<TextMeshProUGUI>().text].GetComponent<GameData>());
    }

    /// <summary>
    /// This methods stores ID of the game selected by instructor for detail viewing 
    /// </summary>
    /// <param name="gameID">ID of the game instructor want to view </param>
    public void SetInstructorGameID(GameObject gameID)
    {
        GameHandler.singleton.GetLocalPlayer().SetPlayerGameID(gameID.GetComponent<TextMeshProUGUI>().text);
    }

    /// <summary>
    /// Creates new game. Called by the UI button click - "Create new game". Avaible only for instructor. 
    /// </summary>
    /// <param name="name">Name of the game instructor wants to create. Obtained from UI Inputfield. </param>
    /// <param name="password">Password for the game instructor wants to create. Obtained UI from Inputfield. </param>
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

    //NOT IMPLEMENTED
    public void DeleteGame(string name)  ///DO I want this? 
    {
        Debug.LogError("Deleting game not implemented");
    }
    
    /// <summary>
    /// Generates unique ID. Used for creation of ID of Games, Contracts, etc.
    /// </summary>
    /// <returns> New generated unique ID </returns>
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
