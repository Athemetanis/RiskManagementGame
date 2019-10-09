using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProductManager : NetworkBehaviour
{   

    private SyncListInt functionalityQ;
    private SyncListInt userFriendlinessQ;
    private SyncListInt integrabilityQ;

    //VARIABLES
    [SyncVar(hook = "OnChangeFunctionality")]
    private int functionality;
    [SyncVar(hook = "OnChangeUserFriendliness")]
    private int userFriendliness;
    [SyncVar(hook = "OnChangeIntergrability")]
    private int integrability;

   
    //REFERENCES
    private ProductUIHandler productUIHandler;

    //GETTERS & SETTERS
    public void SetProductUIHandler(ProductUIHandler productUIHandler) { this.productUIHandler = productUIHandler; }

    public override void OnStartServer()
    {
       if(functionality == 0)
        {
            SetupDefaultValues();
        }
    }

    [Server]
    public void SetupDefaultValues()
    {
        functionality = 10;
        userFriendliness = 10;
        integrability = 10;
    }
    
    //Hooks
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
