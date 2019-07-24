using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProductManager : NetworkBehaviour
{
    [SyncVar(hook = "OnChangeFunctionality")]
    public int functionality;
    [SyncVar(hook = "OnChangeUserFriendliness")]
    public int userFriendliness;
    [SyncVar(hook = "OnChangeIntergrability")]
    public int integrability;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnChangeFunctionality(int functionality)
    {

    }

    public void OnChangeUserFriendliness(int userFunctionality)
    {
       
    }

    public void OnChangeIntergrability(int intergrability)
    {

    }

}
