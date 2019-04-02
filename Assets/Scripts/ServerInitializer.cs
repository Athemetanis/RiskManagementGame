using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ServerInitializer : NetworkBehaviour {

    public GameObject GameHandlerPrefab;


	// Use this for initialization
	void Start ()
    { 

        GameObject gameHandler = Instantiate(GameHandlerPrefab);

        NetworkServer.Spawn(gameHandler);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
  
    [Command]
    public void CmdSpawnNetworkManager()
    {
        GameObject gameHandler = Instantiate(GameHandlerPrefab);

        NetworkServer.Spawn(gameHandler);
    }
}
