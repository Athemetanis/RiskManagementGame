using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomersManager : NetworkBehaviour
{
    //VARIABLES

    //-----history storage----STORED VALUES FOR Q0, Q1, Q2, Q3, Q4, on corresponding indexes  // Q0 are default values when the game begins. 
    private SyncListInt endEnterpriseCustomersQ = new SyncListInt() { };
    private SyncListInt endBusinessCustomersQ = new SyncListInt() { };
    private SyncListInt endIndividualsCutomersQ = new SyncListInt() { };

    //----current values
    [SyncVar(hook = "OnChangeBeginningEneterpriseCustomers")]
    private int beginningEnterpriseCustomers;
    [SyncVar(hook = "OnChangeBeginningBusinessCustomers")]
    private int beginningBusinessCustomers;
    [SyncVar(hook = "OnChangeBeginningIndividualCustomers")]
    private int beginningIndividualsCutomers;

    [SyncVar(hook = "OnChangeEnterpriseCustomerAddDuringQ")]
    private int enterpriseCustomersAddDuringQ;
    [SyncVar(hook = "OnChangeBusinessCustomerAddDuringQ")]
    private int businessCustomersAddDuringQ;
    [SyncVar(hook = "OnChangeIndividualCustomerAddDuringQ")]
    private int individualCustomersAddDuringQ;

    [SyncVar(hook = "OnChangeEnterpriseCustomerAddEndQ")]
    private int enterpriseCustomersAddEndQ;
    [SyncVar(hook = "OnChangeBusinessCustomerAddEndQ")]
    private int businessCustomersAddEndQ;
    [SyncVar(hook = "OnChangeIndividualCustomerAddEndQ")]
    private int individualCustomersAddEndQ;

    [SyncVar(hook = "OnChangeEndEneterpriseCustomers")]
    private int endEnterpriseCustomers;
    [SyncVar(hook = "OnChangeEndBusinessCustomers")]
    private int endBusinessCustomers;
    [SyncVar(hook = "OnChangeEndIndividualCustomers")]
    private int endIndividualsCutomers;

    [SyncVar(hook = "OnChangeEnterpriseCustomersAdvLoss")]
    private int enterpriseCustomersAdvLoss;
    [SyncVar(hook = "OnChangeBusinessCustomersAdvLoss")]
    private int businessCustomersAdvLoss;
    [SyncVar(hook = "OnChangeIndividualCustomersAdvLoss")]
    private int individualCustomersAdvLoss;
    [SyncVar(hook = "OnChangeEnterpriseCustomersAdvAdd")]
    private int enterpriseCustomersAdvAdd;
    [SyncVar(hook = "OnChangeBusinessCustomersAdvAdd")]
    private int businessCustomersAdvAdd;
    [SyncVar(hook = "OnChangeIndividualCustomersAdvAdd")]
    private int individualCustomersAdvAdd;

    private string gameID;
    private int currentQuarter;
    //REFERENCE
    private CustomersUIHandler customersUIHandler;
    private ContractManager contractManager;

    //GETTERS & SETTERS
    public void SetCustomersUIHandler(CustomersUIHandler customersUIHandler) { this.customersUIHandler = customersUIHandler; }

    public int GetEndEnterpriseCustomers() { return endEnterpriseCustomers; }
    public int GetEndBusinessCustomers() { return endBusinessCustomers; }
    public int GetEndIndividualCustomers() { return endIndividualsCutomers; }

    // Start is called before the first frame update
    public override void OnStartServer()
    {
        gameID = this.gameObject.GetComponent<PlayerData>().GetGameID();
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        if (endEnterpriseCustomersQ.Count == 0)
        {
            SetupDefaultValues();
        }
        LoadQuarterData(currentQuarter);
        contractManager = this.gameObject.GetComponent<ContractManager>();
    }
    public override void OnStartClient()
    {
        gameID = this.gameObject.GetComponent<PlayerData>().GetGameID();
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        contractManager = this.gameObject.GetComponent<ContractManager>();
    }

    //METHODS
    [Server]
    public void SetupDefaultValues()
    {
        endEnterpriseCustomersQ[0] = 0;
        endBusinessCustomersQ[0] = 0;
        endIndividualsCutomersQ[0] = 0;
    }
    [Server]
    public void LoadQuarterData(int quarter)
    {
        if (endBusinessCustomersQ.Count != quarter) 
        {
            for (int i = endBusinessCustomersQ.Count + 1 ; i < quarter; i++)
            {
                endEnterpriseCustomersQ[i] = endEnterpriseCustomersQ[i - 1];
                endBusinessCustomersQ[i] = endBusinessCustomersQ[i - 1];
                endIndividualsCutomersQ[i] = endIndividualsCutomersQ[i - 1];
            }
        }
        beginningEnterpriseCustomers = endEnterpriseCustomersQ[quarter - 1];
        beginningBusinessCustomers = endBusinessCustomersQ[quarter - 1];
        beginningIndividualsCutomers = endIndividualsCutomersQ[quarter - 1];

    }

    public void UpdateCustomersCount()
    {
        CmdUpdateCustomersCount();
    }
    [Command]
    public void CmdUpdateCustomersCount()
    {
        enterpriseCustomersAddDuringQ = 0;
        individualCustomersAddDuringQ = 0;
        businessCustomersAddDuringQ = 0;

        enterpriseCustomersAddEndQ = 0;
        businessCustomersAddEndQ = 0;
        individualCustomersAddEndQ = 0;
        foreach (Contract contract in contractManager.GetMyContracts().Values)
        {
            if(contract.GetContractState() == ContractState.Accepted)
            {
                int individualCustomersAll = contract.GetContractFeature().individualCustomers;
                int businessCustomersAll = contract.GetContractFeature().businessCustomers;
                int enterpriseCustomersAll = contract.GetContractFeature().enterpriseCustomers;
                int individualCustomersDuringQ = (individualCustomersAll / 60) * contract.GetContractDelivery();
                int businessCustomerDuringQ = (businessCustomersAll / 60) * contract.GetContractDelivery();
                int enterpriseCustomersDuringQ = (enterpriseCustomersAll / 60) * contract.GetContractDelivery();
                int individualCustomersEndQ = individualCustomersAll - individualCustomersDuringQ;
                int businessCustomersEndQ = businessCustomersAll - businessCustomerDuringQ;
                int enterpriseCustomersEndQ = enterpriseCustomersAll - enterpriseCustomersDuringQ;

                enterpriseCustomersAddDuringQ += enterpriseCustomersDuringQ;
                businessCustomersAddDuringQ += businessCustomerDuringQ;
                individualCustomersAddDuringQ += individualCustomersDuringQ;
                enterpriseCustomersAddEndQ = enterpriseCustomersEndQ;
                businessCustomersAddEndQ = businessCustomersEndQ;
                individualCustomersAddEndQ = individualCustomersEndQ;
            }
            endEnterpriseCustomers = beginningEnterpriseCustomers + enterpriseCustomersAddDuringQ + enterpriseCustomersAddEndQ;
            endBusinessCustomers = beginningBusinessCustomers + businessCustomersAddDuringQ + businessCustomersAddEndQ;
            endIndividualsCutomers = beginningIndividualsCutomers + individualCustomersAddDuringQ + individualCustomersAddEndQ;
        } 
    }

    public void RiseEnterpriseCustomersAddDuringQ(int count) { CmdRiseEnterpriseCustomersAddDuringQ(count); }
    public void RiseBusinessCustomersAddDuringQ(int count) { CmdRiseBusinessCustomersAddDuringQ(count); }
    public void RiseIndividualCustomersAddDuringQ(int count) { CmdRiseIndividualCustomersAddDuringQ(count); }
    public void RiseEnterpriseCustomersAddEndQ(int count) { CmdRiseEnterpriseCustomersAddEndQ(count); }
    public void RiseBusinessCustomersAddEndQ(int count) { CmdRiseBusinessCustomersAddEndQ(count); }
    public void RiseIndividualCustomersAddEndQ(int count) { CmdRiseIndividualCustomersAddEndQ(count); }
    
    public void CmdRiseEnterpriseCustomersAddDuringQ( int count)
    {
        enterpriseCustomersAddDuringQ += count;
        ComputeEndEnterpriseCutomers();
    }
    public void CmdRiseBusinessCustomersAddDuringQ(int count)
    {
        businessCustomersAddDuringQ += count;
        ComputeEndBusinessCutomers();
    }
    public void CmdRiseIndividualCustomersAddDuringQ(int count)
    {
       individualCustomersAddDuringQ += count;
        ComputeEndIndividualCustomers();
    }

    public void CmdRiseEnterpriseCustomersAddEndQ(int count)
    {
        enterpriseCustomersAddEndQ += count;
        ComputeEndEnterpriseCutomers();
    }
    public void CmdRiseBusinessCustomersAddEndQ(int count)
    {
        businessCustomersAddEndQ += count;
        ComputeEndBusinessCutomers();
    }
    public void CmdRiseIndividualCustomersAddEndQ(int count)
    {
        individualCustomersAddEndQ += count;
        ComputeEndIndividualCustomers();
    }

    [Server] //call only from server!!!
    public void ComputeEndEnterpriseCutomers() { endEnterpriseCustomers = beginningEnterpriseCustomers + enterpriseCustomersAddDuringQ + enterpriseCustomersAddEndQ; }
    [Server]
    public void ComputeEndBusinessCutomers() { endBusinessCustomers = beginningBusinessCustomers + businessCustomersAddDuringQ + businessCustomersAddEndQ;  }
    [Server]
    public void ComputeEndIndividualCustomers() { endIndividualsCutomers = beginningIndividualsCutomers + individualCustomersAddDuringQ + individualCustomersAddEndQ; }

    //HOOKS
    public void OnChangeBeginningEneterpriseCustomers(int beginningEnterpriseCustomers)
    {
        if (customersUIHandler != null)
        {
            customersUIHandler.SetBeginingEntCustomers(beginningEnterpriseCustomers);
        }
    }
    public void OnChangeBeginningBusinessCustomers(int beginningBusinessCustomers)
    {
        if (customersUIHandler != null) { customersUIHandler.SetBeginnigBusCutomers(beginningBusinessCustomers); }
    }
    public void OnChangeBeginningIndividualCustomers(int beginningIndividualCustomers)
    {
        if (customersUIHandler != null) { customersUIHandler.SetBeginingIndCustomers(beginningIndividualCustomers); }
    }

    public void OnChangeEnterpriseCustomerAddDuringQ(int enerpriseCustomersAddDuringQ)
    {
        if (customersUIHandler != null) { customersUIHandler.SetEntCustomersAddDuringQ(enterpriseCustomersAddDuringQ); }
    }
    public void OnChangeBusinessCustomerAddDuringQ(int businessCustomersAddDuringQ)
    {
        if (customersUIHandler != null) { customersUIHandler.SetBusCustomersAddDuringQ(businessCustomersAddDuringQ); }
    }
    public void OnChangeIndividualCustomerAddDuringQ(int individualCustomersAddDuringQ)
    {
        if (customersUIHandler != null) { customersUIHandler.SetIndCustomersAddDuringQ(individualCustomersAddDuringQ); }
    }

    public void OnChangeEnterpriseCustomerAddEndQ(int enterpriseCustomerAddEndQ)
    {
        if (customersUIHandler != null) { customersUIHandler.SetEntCustomersAddEndQ(enterpriseCustomerAddEndQ); }
    }
    public void OnChangeBusinessCustomerAddEndQ(int businessCustomerAddEndQ)
    {
        if (customersUIHandler != null) { customersUIHandler.SetBusCustomersAddEndQ(businessCustomerAddEndQ); }
    }
    public void OnChangeIndividualCustomerAddEndQ(int individualCustomerAddEndQ)
    {
        if (customersUIHandler != null) { customersUIHandler.SetIndCustomersAddEndQ(individualCustomerAddEndQ); }
    }

    public void OnChangeEndEneterpriseCustomers(int endEnterpriseCustomers)
    {
        if (customersUIHandler != null) { customersUIHandler.SetEndEntCustomers(endEnterpriseCustomers); }
    }
    public void OnChangeEndBusinessCustomers(int endBusinessCustomers)
    {
        if (customersUIHandler != null) { customersUIHandler.SetEndBusCutomers(endBusinessCustomers); }
    }
    public void OnChangeEndIndividualCustomers(int endIndividualCustomers)
    {
        if (customersUIHandler != null) { customersUIHandler.SetEndIndCustomers(endIndividualCustomers); }
    }

    public void OnChangeEnterpriseCustomersAdvLoss(int loss)
    {
        if(customersUIHandler != null)
        {
            customersUIHandler.SetEntCostumersAdvLoss(loss);
        }
    }
    public void OnChangeBusinessCustomersAdvLoss(int loss)
    {
        if (customersUIHandler != null)
        {
            customersUIHandler.SetBusCustomersAdvLoss(loss);
        }
    }
    public void OnChangeIndividualCustomersAdvLoss(int loss)
    {
        if (customersUIHandler != null)
        {
            customersUIHandler.SetIndCustomersAdvLoss(loss);
        }
    }
    public void OnChangeEnterpriseCustomersAdvAdd(int add)
    {
        if (customersUIHandler != null)
        {
            customersUIHandler.SetEntCustomerAdvAdd(add);
        }
    }
    public void OnChangeBusinessCustomersAdvAdd(int add)
    {
        if (customersUIHandler != null)
        {
            customersUIHandler.SetBusCustomersAdvAdd(add);
        }
    }
    public void OnChangeIndividualCustomersAdvAdd(int add)
    {
        if (customersUIHandler != null)
        {
            customersUIHandler.SetIndCustomersAdvAdd(add);
        }
    }


}
