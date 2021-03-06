using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Mirror;


public class ConnectionManager : NetworkBehaviour {

    //This class manages player objects after player is conncted/disconnected
    //creates objects for player, saves/deletes references of players objects, 
    //creates/destroys local gui for player


    //VARIABLES
    [SyncVar(hook = "OnChangePlayerID")]
    private string playerFirebaseID = "";
    [SyncVar]      //<-remove this, this variable contains playerDataID  == (playerFirebaseID + playerGameID) 
    private string playerID = "";
    [SyncVar(hook = "OnChangeGameID")]
    private string playerGameID;

    public GameObject authenticationPrefab;
    private GameObject authentication;

    //--INSTRUCTOR
    public GameObject instructorPrefab;
    public GameObject instructorUIPrefab;       

    //---PLAYER
    public GameObject playerDeveloperPrefab;
    public GameObject playerDeveloperUIPrefab;
    public GameObject playerProviderPrefab;
    public GameObject playerProviderUIprefab;

    private GameObject myPlayerObject;
    private GameObject myPlayerUIObject;
    private PlayerManager myPlayerData;

    //GETTERS & SETTERS
    public PlayerManager GetMyPlayerData() { return myPlayerData; }
    public GameObject GetMyPlayerObject() { return myPlayerObject; }
    public GameObject GetMyPlayerUIObject() { return myPlayerUIObject; }




    // Use this for initialization
    void Start()
    {
        if (isLocalPlayer)
        {
            GameHandler.singleton.SetLocalPlayer(this);
            Debug.Log("Local player was set in PlayerManager");
        }

        //Debug.Log("Reseting player connection default variables");
        //playerID = "";
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

    //HOOKS
    public void OnChangeGameID(string gameID)
    {
        playerID = playerFirebaseID + gameID;
        playerGameID = gameID;
        if (isLocalPlayer == false)
        {
            return;
        }
        if (playerGameID == "" || playerGameID.Equals(""))
        {
            CmdRemoveAuthority();
            return;
        }
        if(this.playerFirebaseID == "rojDP7X6ujdmd9jeUKIqGwyLNmG2" && playerGameID != "")
        {
            
            CreateInstructorUI();
            return;
        }
        if (this.playerFirebaseID == "rojDP7X6ujdmd9jeUKIqGwyLNmG2" && playerGameID == "")
        {
            GameHandler.singleton.GenerateGamesListUIForInstructor();
            return;
        }
        GetPlayerObject();
    }

    

    public void OnChangePlayerID(string playerFirebaseID)  ///This function is called only on client after playerID was changed on server. 
    {

        this.playerFirebaseID = playerFirebaseID;
        if (isLocalPlayer == false)  ///if I am not local client I dont care about assigning or removing player object which contains data for player  
        {
            return;
        }
        if (playerFirebaseID == null || playerFirebaseID.Equals(""))   /// true if I signed out
        {   
            if(myPlayerData != null)
            {   
                if(myPlayerData.GetPlayerUI() != null)
                {
                    Destroy(myPlayerData.GetPlayerUI());
                }                
            }
            GameHandler.singleton.DestroyGameListUI();
            //CmdRemoveAuthority();
            myPlayerObject = null;
            myPlayerData = null;

            InstructorManager instructor = GameHandler.singleton.GetInstructor();
            if ( instructor != null)
            {
              if(instructor.GetMyInstructorUIObject() != null)
              {
                    Destroy(instructor.GetMyInstructorUIObject());
              }
                Destroy(instructor.gameObject);
            }
            
            return;

        }
        if (this.playerFirebaseID == "rojDP7X6ujdmd9jeUKIqGwyLNmG2")
        {
            
            CmdCreateInstructorObject();
            return;
        }

        Debug.Log("onChnagePlayerId " + this.playerFirebaseID);
        //CmdGetPlayerObject();
        GameHandler.singleton.GenerateGamesListUI();
    }

    //METHODS

    public void SetPlayerFirebaseID(string playerFirebaseID)
    {
        if (!isServer)
        {
            this.playerFirebaseID = playerFirebaseID;
            CmdSetPlayerFirebaseID(playerFirebaseID);
            return;
        }
        CmdSetPlayerFirebaseID(playerFirebaseID);
    }

    [Command]
    public void CmdSetPlayerFirebaseID(string playerFirebaseID)
    {
        Debug.Log("playerFireBaseID " + playerFirebaseID);
        this.playerFirebaseID = playerFirebaseID;
    }

    public void SetPlayerGameID(string gameID)
    {
        GameHandler.singleton.DestroyGameListUI();
        if (!isServer)
        {
            playerID = playerFirebaseID + gameID;
            Debug.Log("playerID " + playerID);
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
        playerID = playerFirebaseID + gameID;
        //Debug.Log("playerID " + playerID);
        this.playerGameID = gameID;
        //Debug.Log("CmdgameID " + playerGameID);

    }

    [Command]
    public void CmdCreateInstructorObject()
    {
        GameObject myInstructorObject = Instantiate(instructorPrefab);

        NetworkServer.SpawnWithClientAuthority(myInstructorObject, this.gameObject);
    }


    public void GetPlayerObject()
    {
        Debug.Log("trying to find my playerdata object " + playerID);


        //BAD THINGS HAPPEN HERE WHEN STOPING CLIENT AND CONNECTING AGAIN 

        // Debug.Log(GameHandler.allPlayers[playerGameID].ContainsKey(playerID));
        // if(GameHandler.allPlayers[playerGameID].ContainsKey(playerID))   // true
        // {
        //      Debug.Log(GameHandler.allPlayers[playerGameID][playerID];   //null  -how it is possible? bad sync? 
        //}



        if (GameHandler.allPlayers.ContainsKey(playerGameID) && GameHandler.allPlayers[playerGameID].ContainsKey(playerID))
        {
            PlayerManager playerData = GameHandler.allPlayers[playerGameID][playerID].GetComponent<PlayerManager>();
            Debug.Log(playerData == null);
            //playerData.gameObject.GetComponent<NetworkIdentity>().AssignClientAuthority(this.gameObject.GetComponent<NetworkIdentity>().connectionToClient);
            CmdAssignClientAuthority();
            myPlayerObject = GameHandler.allPlayers[playerGameID][playerID];
            myPlayerData = playerData;
            Debug.Log("myPlayerDataObject was found");
            CreatePlayerUI();
            return;
        }
        else
        {
            Debug.Log("my object was not found, it must be created");
            CmdCreatePlayerData();
        }
    }

    [ClientRpc]
    public void RpcGetPlayerObject()
    {
        if (isLocalPlayer == false)
        {
            return;
        }
        Debug.Log("trying to find my playerdata object" + playerID);
        //Debug.Log(GameHandler.allPlayers.ContainsKey(playerGameID));
        //Debug.Log(GameHandler.allPlayers[playerGameID].ContainsKey(playerID));

        if (GameHandler.allPlayers.ContainsKey(playerGameID) && GameHandler.allPlayers[playerGameID].ContainsKey(playerID))
        {
            PlayerManager playerData = GameHandler.allPlayers[playerGameID][playerID].GetComponent<PlayerManager>();
            //playerData.gameObject.GetComponent<NetworkIdentity>().AssignClientAuthority(this.gameObject.GetComponent<NetworkIdentity>().connectionToClient);
            CmdAssignClientAuthority();
            myPlayerData = playerData;
            myPlayerObject = GameHandler.allPlayers[playerGameID][playerID];
            Debug.Log("myPlayerDataObject was found");
            CreatePlayerUI();
            return;
        }
    }


    [Command]
    public void CmdAssignClientAuthority()
    {
        Debug.Log("assigning client authority on my player data object");
        PlayerManager playerData = GameHandler.allPlayers[playerGameID][playerID].GetComponent<PlayerManager>();
        playerData.gameObject.GetComponent<NetworkIdentity>().AssignClientAuthority(this.gameObject.GetComponent<NetworkIdentity>().connectionToClient);

    }


    [Command]
    public void CmdRemoveAuthority()
    {
        if (myPlayerData != null)
        {
            myPlayerObject.GetComponent<NetworkIdentity>().RemoveClientAuthority(this.gameObject.GetComponent<NetworkIdentity>().connectionToClient);
        }
    }


    [Command]
    public void CmdCreatePlayerData()
    {
        GameData myGame = GameHandler.allGames[playerGameID];
        if (myGame.GetDevelopersCount() > myGame.GetProvidersCount() || myGame.GetProvidersCount() == myGame.GetDevelopersCount())
        {
            myPlayerObject = Instantiate(playerProviderPrefab);

            myPlayerData = myPlayerObject.GetComponent<PlayerManager>();
            myPlayerData.SetPlayerRole(PlayerRoles.Provider);
        }
        else
        {
            myPlayerObject = Instantiate(playerDeveloperPrefab);
            myPlayerData = myPlayerObject.GetComponent<PlayerManager>();
            myPlayerData.SetPlayerRole(PlayerRoles.Developer);
        }

        //myPlayerObject = Instantiate(playerPrefab);
        //myPlayerData = myPlayerObject.GetComponent<PlayerData>();
        myPlayerData.SetGameID(playerGameID);
        Debug.Log("SERVER: gameID for my new playerdata set to: " + playerGameID);
        myPlayerData.SetPlayerID(playerID);
        Debug.Log("SERVER: playerID for my new playerdata set to: " + playerID);
        myPlayerObject.SetActive(true);
        NetworkServer.Spawn(myPlayerObject);
        //NetworkServer.SpawnWithClientAuthority(myPlayerObject, gameObject);
        Debug.Log("new playerdataobject spawned with gameID:" + myPlayerData.GetGameID() + " & playerID: " + myPlayerData.GetPlayerID());

        StartCoroutine(Function1());
    }
   
    private IEnumerator Function1 ()
    {
        yield return new WaitForSeconds(1);
        RpcGetPlayerObject();
    }

    public void CreatePlayerUI()
    {
        Debug.Log("Creating UI");
        if (myPlayerData.GetPlayerRole() == PlayerRoles.Developer)
        {
            myPlayerUIObject = Instantiate(playerDeveloperUIPrefab);
            myPlayerData.SetPlayerUI(myPlayerUIObject);
        }
        else
        {
            myPlayerUIObject = Instantiate(playerProviderUIprefab);
            myPlayerData.SetPlayerUI(myPlayerUIObject);
        }
    }

    /*public void DestroyPlayerUI()
    {   
        if(myPlayerData.GetPlayerUI() != null)
        {
            Destroy(myPlayerData.GetPlayerUI());
        }
  
        
    }*/
     


    [Client]
    public void CreateInstructorUI()
    {
        GameObject myInstructorUIObject = Instantiate(instructorUIPrefab);
        GameHandler.singleton.GetInstructor().SetMyInstructorUIObject(myInstructorUIObject);
        GameHandler.singleton.GetInstructor().SetGameID(playerGameID);
        myInstructorUIObject.GetComponent<InstructorGameInfoUIHandler>().SetInstructorManager(GameHandler.singleton.GetInstructor());
        myInstructorUIObject.GetComponent<InstructorGameInfoUIHandler>().SetGameID(playerGameID);
        myInstructorUIObject.gameObject.SetActive(true);
    }



    /// <summary>
    /// ----------------------------------------------OLD------------------------------------
    /// </summary>
    /// <param name="name"></param>
    /*

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
     */


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
