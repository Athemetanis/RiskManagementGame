using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class GameHandler : NetworkBehaviour {

    /// <summary>
    /// This class creates/deletes games, shows list of games
    /// loads/saves data on server
    /// </summary>
    public GameObject gamePrefab;
    public GameObject gameUIPrefab;
    public GameObject gameListUIPrefab;
    public GameObject gamePasswordVerificatorPrefab;

    private GameObject gameListUIContent;
    private PlayerManager localPlayer;

    private GameObject gameListUIGameObject;

    public static GameHandler singleton;



    // This List contains all Game objects
    public static Dictionary<string, GameData> allGames = new Dictionary<string, GameData>();
    // This List contains all GameUI objects
    public static Dictionary<string, GameObject> allGamesUI = new Dictionary<string, GameObject>();

    // This list holds data about all games and all their players
    //----------------------<gameID           playerID, PlayerData reference>------------------------------------------------------------//
    public static Dictionary<string, Dictionary<string, PlayerData>> allPlayers = new Dictionary<string, Dictionary<string, PlayerData>>();
    public static int count;


    //GETTERS & SETTERS
    public void SetLocalPlayer(PlayerManager localPlayer){ this.localPlayer = localPlayer; }
    public PlayerManager GetLocalPlayer() { return localPlayer; }

    private void Awake()
    {
        singleton = this;
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
    {
        CreateGame("DEMO2", "666");

        Debug.Log("Game Instatinated2");
        GameObject game = Instantiate(gamePrefab);
        GameData gameData = game.GetComponent<GameData>();

        gameData.SetGameID("3");
        gameData.SetGameName("demo");
        gameData.SetPassoword("666");

        //GameObject gameUI = Instantiate(GameUIPrefab);

        allGames.Add("3", gameData);

        //allGamesUI.Add("3", null);
        Debug.Log("Game Instatinated2");
        game.SetActive(true);


    }


    //This method generates visual representation of list of games
    public void GenerateGamesListUI()
    {   
        //created list of game UI
        gameListUIGameObject = Instantiate(gameListUIPrefab);
        gameListUIContent = gameListUIGameObject.transform.Find("GameScrolList/GameListViewport/GameListContent").gameObject;

        //adding exising games into UI representation
        foreach(GameData gameData in allGames.Values)
        {
            GameObject gameUI = Instantiate(gameUIPrefab);
            gameUI.transform.SetParent(gameListUIContent.transform, false);
            gameData.SetGameUIHandler(gameUI.GetComponent<GameUIHandler>());
            gameData.GameUIUpdateAll();
        }
    } 
    public void DestroyGameListUI()
    {
        Destroy(gameListUIGameObject);
    }

    public void GenerateGamePasswordVerificator(GameObject gameID)
    {
        GameObject GamePasswordVerificator = Instantiate(gamePasswordVerificatorPrefab);
        GamePasswordVerificator.GetComponent<GamePasswordVerification>().SetGameData(GameHandler.allGames[gameID.GetComponent<Text>().text].GetComponent<GameData>());
    }
       
    public void InstructorGenerateGamesListUI()
    {
        //to do -  other GameUIPrefab or whole game List??? 
        //how to create game? 

    }

    public void CreateGame(string name, string password)
    {
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
        }
    }

    public void DeleteGame(string name)  ///DO I want this? 
    {

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
