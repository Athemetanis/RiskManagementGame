using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProductManager : NetworkBehaviour
{

    private SyncListInt functionalityQ =  new SyncListInt() { };
    private SyncListInt userFriendlinessQ = new SyncListInt() { };
    private SyncListInt integrabilityQ = new SyncListInt() { };

    //VARIABLES
    [SyncVar(hook = "OnChangeFunctionality")]
    private int functionality;
    [SyncVar(hook = "OnChangeUserFriendliness")]
    private int userFriendliness;
    [SyncVar(hook = "OnChangeIntergrability")]
    private int integrability;

    private string gameID;
    private int currentQuarter;
    //REFERENCES
    private ProductUIHandler productUIHandler;
    private ContractManager contractManager;
    private FeatureManager featureManager;

    //GETTERS & SETTERS
    public void SetProductUIHandler(ProductUIHandler productUIHandler) { this.productUIHandler = productUIHandler; }
    public int GetFunctionality() { return functionality; }
    public int GetIntegrability() { return integrability; }
    public int GetUserFrienliness() { return userFriendliness; }

    public int GetFunctionalityQuarter(int quarter) { return functionalityQ[quarter]; }
    public int GetIntegrabilityQuarter(int quarter) { return integrabilityQ[quarter]; }
    public int GetUserFriendlinessQuarter(int quarter) { return userFriendlinessQ[quarter]; }

    public override void OnStartServer()
    {
        gameID = this.gameObject.GetComponent<PlayerManager>().GetGameID();
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        contractManager = this.gameObject.GetComponent<ContractManager>();
        featureManager = this.gameObject.GetComponent<FeatureManager>();
        
        if (functionality == 0)
        {
            SetupDefaultValues();
        }
        LoadQuarterData(currentQuarter);

    }

    [Server]
    public void SetupDefaultValues()
    {
        functionalityQ.Insert(0, 10);
        userFriendlinessQ.Insert(0, 10);
        integrabilityQ.Insert(0, 10);

    }
    [Server]
    public void LoadQuarterData(int quarter)
    {
        if(functionalityQ.Count != quarter)
        {
            for(int i  = functionalityQ.Count; i < quarter; i++)
            {
                functionalityQ.Insert(i, functionalityQ[i - 1]);
                userFriendlinessQ.Insert(i, userFriendlinessQ[i - 1]);
                integrabilityQ.Insert(i, integrabilityQ[i - 1]);
            }

        }
        functionality = functionalityQ[quarter - 1];
        integrability = integrabilityQ[quarter - 1];
        userFriendliness = userFriendlinessQ[quarter - 1];
    }


    [Server]
    public void ComputeProductParameters()
    {
        functionality = 10;
        integrability = 10;
        userFriendliness = 10;
        Debug.Log("Computing ProductParameters from done features: " + featureManager.GetDoneFeatures().Count);
        foreach (Feature feature in featureManager.GetDoneFeatures().Values)
        {
            functionality += feature.functionality;
            integrability += feature.integrability;
            userFriendliness += feature.userfriendliness;
        }
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
    // NEXT QUARTER EVALUATION METHODS...
    [Server]
    public void UpdateCurrentQuarterData()
    {        
        ComputeProductParameters();
    }
    
    [Server]
    public void MoveToNextQuarter()
    {
        LoadCurrentQuarterData(currentQuarter);
    }

    [Server]
    public void SaveCurrentQuarterData()
    {
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();

        functionalityQ.Insert(currentQuarter, functionality);
        integrabilityQ.Insert(currentQuarter, integrability);
        userFriendlinessQ.Insert(currentQuarter, userFriendliness);

    }

    public void LoadCurrentQuarterData(int curentQuarter)
    {
        LoadQuarterData(currentQuarter + 1);
    }

    private void Start()
    {

    }
}
