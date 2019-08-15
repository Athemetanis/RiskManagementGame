using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProductManager : NetworkBehaviour
{   
    //VARIABLES
    [SyncVar(hook = "OnChangeFunctionality")]
    public int functionality;
    [SyncVar(hook = "OnChangeUserFriendliness")]
    public int userFriendliness;
    [SyncVar(hook = "OnChangeIntergrability")]
    public int integrability;

    private ProductUIHandler productUIHandler;

    //GETTERS & SETTERS
    public void SetProductUIHandler(ProductUIHandler productUIHandler) { this.productUIHandler = productUIHandler; }

    // Start is called before the first frame update
    void Start()
    {
        if (isServer)
        {
            functionality = 10;
            userFriendliness = 10;
            integrability = 10;
        }
    }

   

    public void OnChangeFunctionality(int functionality)
    {
        this.functionality = functionality;
        if (productUIHandler != null)
        {
            productUIHandler.UpdateUIFunctionalityText(functionality);
        }

    }

    public void OnChangeUserFriendliness(int userFriendliness)
    {
        this.userFriendliness = userFriendliness;
        if (productUIHandler != null)
        {
            productUIHandler.UpdateUIUserFrienlinessText(userFriendliness);
        }
    }

    public void OnChangeIntergrability(int integrability)
    {
        this.integrability = integrability;
        if (productUIHandler != null)
        {
            productUIHandler.UpdateUIIntegrabilityText(integrability);
        }
    }

}
