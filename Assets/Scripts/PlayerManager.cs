using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Mirror;


public class PlayerManager : NetworkBehaviour {

    //This class manages player objects after player is conncted/disconnected
    //creates objects for player, saves/deletes references of players objects, 


    //VARIABLES
    [SyncVar(hook = "OnChangePlayerID")]
    private string playerID;
    [SyncVar(hook = "OnChangeGameID")]
    private string playerGameID;

    public GameObject authenticationPrefab;
    private GameObject authentication;


    public GameObject playerPrefab;
    private GameObject myPlayerObject;
    private PlayerData myPlayerData;

   

    // Use this for initialization
    void Start()
    {
        if (isLocalPlayer)
        {
            GameHandler.singleton.SetLocalPlayer(this);
        }


        Debug.Log("Reseting player connection default variables");
        playerID = "";
        //this.SetPlayerGameID("3");

        if (isLocalPlayer == false)
        {
            return;
        }

        authentication = Instantiate(authenticationPrefab);
        authentication.GetComponent<AuthenticationManager>().SetPlayerManager(this);
        authentication.SetActive(true);


    }

    // Update is called once per frame
    void Update() {

    }


    //METHODS
    public void SetPlayerID(string playerID)
    {
        if (!isServer)
        {
            this.playerID = playerID;
            CmdSetPlayerID(playerID);
            return;
        }
        CmdSetPlayerID(playerID);
    }

    [Command]
    public void CmdSetPlayerID(string playerID)
    {
        Debug.Log("playerID " + playerID);
        this.playerID = playerID;
    }

    public void SetPlayerGameID(string gameID)
    {
        
        if (!isServer)
        {
            this.playerGameID = gameID;
            CmdSetPlayerGameID(gameID);
            Debug.Log("gameID " + playerGameID);
            return;
        }
        CmdSetPlayerGameID(gameID);
    }

    [Command]
    public void CmdSetPlayerGameID(string gameID)
    {
        
        this.playerGameID = gameID;
        Debug.Log("CmdgameID " + playerGameID);
    }


    

    //Hooks 
    public void OnChangeGameID(string gameID)
    {
        playerGameID = gameID;
        if(isLocalPlayer == false)
        {
            return;
        }
        CmdGetPlayerObject();
    }
    
    public void OnChangePlayerID(string playerID)  ///This function is called only on client after playerID was changed on server. 
    {

        this.playerID = playerID;
        if (isLocalPlayer == false)  ///if I am not local client I dont care about assigning or removing player object which contains data for player  
        {
            return;
        }
        if (playerID == null || playerID.Equals(""))   /// true if I signed out
        {
            CmdRemoveAuthority();
            myPlayerObject = null;
            myPlayerData = null;
            return;

        }
     
        Debug.Log("onChnagePlayerId " + this.playerID);

        //CmdGetPlayerObject();
        GameHandler.singleton.GenerateGamesListUI();
    }

    [Command]
    public void CmdGetAdminObject()
    {

    }  
    
    [Command]
    public void CmdGetPlayerObject()
    {
        Debug.Log("trying to find my playerdata object" + playerGameID);

        if (GameHandler.allPlayers.ContainsKey(playerGameID) && GameHandler.allPlayers[playerGameID].ContainsKey(playerID))
        {
            PlayerData playerData = GameHandler.allPlayers[playerGameID][playerID];
            playerData.gameObject.GetComponent<NetworkIdentity>().AssignClientAuthority(this.gameObject.GetComponent<NetworkIdentity>().connectionToClient);
            myPlayerData = playerData;
            Debug.Log("myPlayerDataObject was found");
            return;
        }
        else
        {
            Debug.Log("my object was not found, it must be created");
            CmdCreatePlayerData();
        }
    }

    [Command]
    public void CmdCreatePlayerData()
    {
        myPlayerObject = Instantiate(playerPrefab);
        myPlayerData = myPlayerObject.GetComponent<PlayerData>();
        myPlayerData.SetGameID(playerGameID);
        Debug.Log("SERVER: gameID for my new playerdata set to: " + playerGameID);
        myPlayerData.SetPlayerID(playerID);
        Debug.Log("SERVER: playerID for my new playerdata set to: " + playerID);
        myPlayerObject.SetActive(true);

        NetworkServer.SpawnWithClientAuthority(myPlayerObject, gameObject);
        Debug.Log("new playerdataobject spawned with gameID:" + myPlayerData.GetGameID() + " & playerID: " + myPlayerData.GetPlayerID()); 
    }

    [Command]
    public void CmdRemoveAuthority()
    {
        if (myPlayerData != null)
        {
            myPlayerObject.GetComponent<NetworkIdentity>().RemoveClientAuthority(this.gameObject.GetComponent<NetworkIdentity>().connectionToClient);
        }
    }











    /// <summary>
    /// ----------------------------------------------OLD------------------------------------
    /// </summary>
    /// <param name="name"></param>
    public void SetPlayerDataFirmName(string name)
    {
        GameHandler.allPlayers[playerGameID][playerID].SetFirmName(name);
        CmdSetPlayerDataFirmName(name);
    }

    [Command]
    private void CmdSetPlayerDataFirmName(string name)
    {
        GameHandler.allPlayers[playerGameID][playerID].SetFirmName(name);
    }


     /*public void CreatePlayerObject(string playerID)
    {
        playerObject = Instantiate(playerPrefab);
        player = playerObject.GetComponent<PlayerData>();
        playerData.SetPlayerID(playerID);
        path = Application.persistentDataPath + "/" + player.GetPlayerID() + ".player";
        Debug.Log(path);
    
    }*/

    /* void SerializePlayerData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);
        formatter.Serialize(stream, data);
        stream.Close();
               
    }*/

    /*public void DeserializeplayerData()
    {
        if (File.Exists(path))
        {
            PlayerData data;
            FileStream stream = new FileStream(path, FileMode.Open);

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                
                

                data = formatter.Deserialize(stream) as PlayerData;
                player.LoadPlayer(data);

            }
            finally
            {
                stream.Close();
                
            }

        }
        Debug.Log("file fo deserialization not found");
        return;
        

    }*/




}
