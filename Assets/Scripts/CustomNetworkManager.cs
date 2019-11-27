using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class CustomNetworkManager : NetworkManager {


    public override void OnServerDisconnect(NetworkConnection conn)
    {
        uint[] clientObjects = new uint[conn.clientOwnedObjects.Count];
        //Debug.Log(conn.clientOwnedObjects.Count);

        conn.clientOwnedObjects.CopyTo(clientObjects);

        foreach (uint objId in clientObjects)
        {
            NetworkIdentity myObject = NetworkIdentity.spawned[objId];
            if (myObject.gameObject.tag == "PlayerData")
            {
                myObject.RemoveClientAuthority(conn);
            }
        }

        NetworkServer.DestroyPlayerForConnection(conn);
    }


    public void StartMyServer()
    {
        Debug.Log("STARTING SERVER  ");
        base.StartServer();
    }



}
