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
    public GameObject GamePrefab;
    public GameObject GameUIPrefab;
    public GameObject GameListUIPrefab;
    public GameObject GamePasswordVerificatorPrefab;

    private GameObject GameListUIContent;
    private Text GameNameText;
    private Text GameIDText;
    private Text GamePlayersText;
    private Text GameRoundText;

   
    private PlayerManager localPlayer;    


    public static GameHandler singleton;


    // This List contains all Game objects
    public static Dictionary<string, GameObject> allGames = new Dictionary<string, GameObject>();
    // This List contains all GameUI objects
    public static Dictionary<string, GameObject> allGamesUI = new Dictionary<string, GameObject>();



    public static string test;

    // This list holds data about all games and all their players
    //----------------------<gameID           playerID, PlayerData reference>------------------------------------------------------------//
    public static Dictionary<string, Dictionary<string, PlayerData>> allPlayers = new Dictionary<string, Dictionary<string, PlayerData>>();
    public static int count;


    //GETTERS & SETTERS
    public void SetLocalPlayer(PlayerManager localPlayer)
    {
        this.localPlayer = localPlayer;

    }

    public PlayerManager GetLocalPlayer()
    {
        return localPlayer;
    }




    private void Awake()
    {
        singleton = this;

        //creates DEMO game when server starts;
        if (isServer)
            Debug.Log("Game Instatinated2");
            GameObject game = Instantiate(GamePrefab);
            GameData gameData = game.GetComponent<GameData>();

            gameData.SetGameID("3");
            gameData.SetGameName("demo");
            gameData.SetPassoword("666");

            //GameObject gameUI = Instantiate(GameUIPrefab);

            allGames.Add("3", game);

            //allGamesUI.Add("3", null);
            Debug.Log("Game Instatinated2");
            game.SetActive(true);
            
        }

    

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(count);
    }


    //This method generates visual representation of list of games
    public void GenerateGamesListUI()
    {   
        //created list of game UI
        GameObject GameUIList = Instantiate(GameListUIPrefab);
        GameListUIContent = GameUIList.transform.Find("GameScrolList/GameListViewport/GameListContent").gameObject;

        //adding exising games into UI representation
        foreach(GameObject game in allGames.Values)
        {
            GameObject gameUI = Instantiate(GameUIPrefab);
            gameUI.transform.SetParent(GameListUIContent.transform, false);

            GameNameText = gameUI.transform.Find("NameText").gameObject.GetComponent<Text>();
            GameIDText = gameUI.transform.Find("IDText").gameObject.GetComponent<Text>();
            GamePlayersText = gameUI.transform.Find("PlayersText").gameObject.GetComponent<Text>();
            GameRoundText = gameUI.transform.Find("RoundText").gameObject.GetComponent<Text>();

            GameData gamedata = game.GetComponent<GameData>();

        
            GameNameText.text = gamedata.GetGameName();
            GameIDText.text = gamedata.GetGameID();
            GamePlayersText.text = gamedata.GetPlayersCount().ToString();
            GameRoundText.text = gamedata.GetGameRound().ToString();
        }
    } 

    public void GenerateGamePasswordVerificator(GameObject gameID)
    {   
        GameObject GamePasswordVerificator = Instantiate(GamePasswordVerificatorPrefab);
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
            GameObject game = Instantiate(GamePrefab);
            GameData gameData = game.GetComponent<GameData>();

            gameData.SetGameID(GenerateUniqueID());
            gameData.SetGameName(name);
            gameData.SetGameRound(1);
            gameData.SetPassoword(password);
            gameData.SetPlayersCount(0);
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
