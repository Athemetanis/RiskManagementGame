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
    private int beginningIndividualCutomers;

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

   /* [SyncVar(hook = "OnChangeEnterpriseCustomersPriceLoss")]
    private int enterpriseCustomersPriceLoss;
    [SyncVar(hook = "OnChangeBusinessCustomersPriceLoss")]
    private int businessCustomersPriceLoss;
    [SyncVar(hook = "OnChangeIndividualCustomersPriceLoss")]
    private int individualCustomersPriceLoss;*/
   
    private string gameID;
    private int currentQuarter;
    //REFERENCE
    private CustomersUIHandler customersUIHandler;
    private ContractManager contractManager;
    private ProviderAccountingManager providerAccountingmanager;
    private MarketingManager marketingManager;

    //GETTERS & SETTERS
    public void SetCustomersUIHandler(CustomersUIHandler customersUIHandler) { this.customersUIHandler = customersUIHandler; }

    public int GetBeginningEnterpriseCustomers() { return beginningEnterpriseCustomers; }
    public int GetBeginningBusinessCustomers() { return beginningBusinessCustomers; }
    public int GetBeginningIndividualCustomers() { return beginningIndividualCutomers; }

    public int GetEnterpriseCustomersDuringQ() { return enterpriseCustomersAddDuringQ; }
    public int GetBusinessCustomersDuringQ() { return businessCustomersAddDuringQ; }
    public int GetIndividualCustomersDuringQ() { return individualCustomersAddDuringQ; }

    public int GetEnterpriseCustomersEndQ() { return enterpriseCustomersAddEndQ; }
    public int GetBusinessCustomersEndQ() { return businessCustomersAddEndQ; }
    public int GetIndividualCustomersEndQ() { return individualCustomersAddEndQ; }
       
    public int GetEndEnterpriseCustomers() { return endEnterpriseCustomers; }
    public int GetEndBusinessCustomers() { return endBusinessCustomers; }
    public int GetEndIndividualCustomers() { return endIndividualsCutomers; }

    public int GetEnterpriseCustomersAdvLoss() { return enterpriseCustomersAdvLoss; }
    public int GetBusinessCustomersAdvLoss() { return businessCustomersAdvLoss; }
    public int GetIndividualCustomersAdvLoss() { return individualCustomersAdvLoss; }

    public int GetEnterpriseCustomersAdvAdd() { return enterpriseCustomersAdvAdd; }
    public int GetBusinessCustomersAdvAdd() { return businessCustomersAdvAdd; }
    public int GetIndividualCustomersAdvAdd() { return individualCustomersAdvAdd; }

    public int GetEnterpriseCustomersQ(int quarter) { return endEnterpriseCustomersQ[quarter]; }
    public int GetBusinesCustomersQ(int quarter) { return endBusinessCustomersQ[quarter]; }
    public int GetIndividualCustomersQ(int quarter) { return endIndividualsCutomersQ[quarter]; }

    // Start is called before the first frame update
    public override void OnStartServer()
    {
        Debug.Log("customersManager ON");
        gameID = this.gameObject.GetComponent<PlayerManager>().GetGameID();
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        contractManager = this.gameObject.GetComponent<ContractManager>();
        providerAccountingmanager = this.gameObject.GetComponent<ProviderAccountingManager>();
        marketingManager = this.gameObject.GetComponent<MarketingManager>(); 
        if (endEnterpriseCustomersQ.Count == 0)
        {
            SetupDefaultValues();
        }
        LoadQuarterData(currentQuarter);
        UpdateEstimatedCustomersCountServer();
        UpdateAdverisementCustomersInfluence();


    }
    private void Start()
    {
        
    }
    public override void OnStartClient()
    {
        contractManager = this.gameObject.GetComponent<ContractManager>();
        providerAccountingmanager = this.gameObject.GetComponent<ProviderAccountingManager>();
    }


    //METHODS
    [Server]
    public void SetupDefaultValues()
    {
        endEnterpriseCustomersQ.Insert(0, 0);
        endBusinessCustomersQ.Insert(0, 0);
        endIndividualsCutomersQ.Insert(0, 0);
        enterpriseCustomersAdvAdd = 0;
        businessCustomersAdvAdd = 0;
        individualCustomersAdvAdd = 0;
        enterpriseCustomersAdvLoss = 0;
        businessCustomersAdvLoss = 0;
        individualCustomersAdvLoss = 0;
    }
    [Server]
    public void LoadQuarterData(int quarter)
    {
        if (endBusinessCustomersQ.Count != quarter)
        {
            for (int i = endBusinessCustomersQ.Count + 1; i < quarter; i++)
            {
                endEnterpriseCustomersQ.Insert(i, endEnterpriseCustomersQ[i - 1]);
                endBusinessCustomersQ.Insert(i, endBusinessCustomersQ[i - 1]);
                endIndividualsCutomersQ.Insert(i, endIndividualsCutomersQ[i - 1]);
            }
        }
        beginningEnterpriseCustomers = endEnterpriseCustomersQ[quarter - 1];
        beginningBusinessCustomers = endBusinessCustomersQ[quarter - 1];
        beginningIndividualCutomers = endIndividualsCutomersQ[quarter - 1];

        UpdateEstimatedCustomersCountServer();
        UpdateAdverisementCustomersInfluence();

        enterpriseCustomersAdvAdd = 0;
        businessCustomersAdvAdd = 0;
        individualCustomersAdvAdd = 0;

          //losing 10% of cutomers
            enterpriseCustomersAdvLoss = (int) System.Math.Round((beginningEnterpriseCustomers* 0.1f), System.MidpointRounding.AwayFromZero);
            businessCustomersAdvLoss = (int) System.Math.Round((beginningBusinessCustomers* 0.1f), System.MidpointRounding.AwayFromZero);
            individualCustomersAdvLoss = (int) System.Math.Round((beginningIndividualCutomers* 0.1f), System.MidpointRounding.AwayFromZero);

        endIndividualsCutomers = ComputeEndEnterpriseCutomers();
        endBusinessCustomers = ComputeEndBusinessCutomers();
        endIndividualsCutomers = ComputeEndIndividualCustomers();
    }

    [Server]
    public void UpdateEstimatedCustomersCountServer()
    {
        enterpriseCustomersAddDuringQ = 0;
        individualCustomersAddDuringQ = 0;
        businessCustomersAddDuringQ = 0;

        enterpriseCustomersAddEndQ = 0;
        businessCustomersAddEndQ = 0;
        individualCustomersAddEndQ = 0;


        foreach (Contract contract in contractManager.GetMyContracts().Values)
        {
            if (contract.GetContractState() == ContractState.Accepted)
            {
                int individualCustomersAll = contract.GetContractFeature().individualCustomers;
                int businessCustomersAll = contract.GetContractFeature().businessCustomers;
                int enterpriseCustomersAll = contract.GetContractFeature().enterpriseCustomers;

                int individualCustomersDuringQ = (int)System.Math.Round((individualCustomersAll / 60f) * (60f - contract.GetContractDelivery()), System.MidpointRounding.AwayFromZero);
                int businessCustomerDuringQ = (int)System.Math.Round((businessCustomersAll / 60f) * (60f - contract.GetContractDelivery()), System.MidpointRounding.AwayFromZero);
                int enterpriseCustomersDuringQ = (int)System.Math.Round((enterpriseCustomersAll / 60f) * (60f - contract.GetContractDelivery()), System.MidpointRounding.AwayFromZero);

                int individualCustomersEndQ = individualCustomersAll - individualCustomersDuringQ;
                int businessCustomersEndQ = businessCustomersAll - businessCustomerDuringQ;
                int enterpriseCustomersEndQ = enterpriseCustomersAll - enterpriseCustomersDuringQ;

                enterpriseCustomersAddDuringQ += enterpriseCustomersDuringQ;
                businessCustomersAddDuringQ += businessCustomerDuringQ;
                individualCustomersAddDuringQ += individualCustomersDuringQ;
                enterpriseCustomersAddEndQ += enterpriseCustomersEndQ;
                businessCustomersAddEndQ += businessCustomersEndQ;
                individualCustomersAddEndQ += individualCustomersEndQ;
            }          
        }
        endEnterpriseCustomers = ComputeEndEnterpriseCutomers();
        endBusinessCustomers = ComputeEndBusinessCutomers();
        endIndividualsCutomers = ComputeEndIndividualCustomers();
        providerAccountingmanager.UpdateRevenueServer();
    }
    [Server]
    public void UpdateAdverisementCustomersInfluence()
    {
        int advertisementSum = 0;
        int advertisementCount = 0;
        int averageAdvertisement = 0;


        Debug.Log("DEBUG: gameid=" + gameID);

        foreach(GameObject provider in GameHandler.allGames[gameID].GetProviderList().Values)
        {
            advertisementSum += provider.GetComponent<MarketingManager>().GetAdvertismenetCoverageQuarters(currentQuarter - 1);
            advertisementCount = advertisementCount + 1;
        }

        if(advertisementCount != 0)
        {
            averageAdvertisement = advertisementSum / advertisementCount;
        }        
        if(marketingManager.GetAdvertismenetCoverageQuarters(currentQuarter - 1) > averageAdvertisement)
        {
            //gaining 10% of customers
            enterpriseCustomersAdvAdd = (int)System.Math.Round((beginningEnterpriseCustomers * 0.2f), System.MidpointRounding.AwayFromZero); 
            businessCustomersAdvAdd = (int)System.Math.Round((beginningBusinessCustomers * 0.2f), System.MidpointRounding.AwayFromZero);
            individualCustomersAdvAdd = (int)System.Math.Round((beginningIndividualCutomers * 0.2f), System.MidpointRounding.AwayFromZero);

        }
        else if (marketingManager.GetAdvertismenetCoverageQuarters(currentQuarter - 1) < averageAdvertisement)
        {
            //losing 10% of cutomers
            enterpriseCustomersAdvLoss = (int)System.Math.Round((beginningEnterpriseCustomers * 0.1f), System.MidpointRounding.AwayFromZero);
            businessCustomersAdvLoss = (int)System.Math.Round((beginningBusinessCustomers * 0.1f), System.MidpointRounding.AwayFromZero);
            individualCustomersAdvLoss = (int)System.Math.Round((beginningIndividualCutomers * 0.1f), System.MidpointRounding.AwayFromZero);
        }

        endEnterpriseCustomers = ComputeEndEnterpriseCutomers();
        endBusinessCustomers = ComputeEndBusinessCutomers();
        endIndividualsCutomers = ComputeEndIndividualCustomers();
    }
    [Server]
    public void UpdateRealCustomersCountServer()
    {
        enterpriseCustomersAddDuringQ = 0;
        individualCustomersAddDuringQ = 0;
        businessCustomersAddDuringQ = 0;

        enterpriseCustomersAddEndQ = 0;
        businessCustomersAddEndQ = 0;
        individualCustomersAddEndQ = 0;


        foreach (Contract contract in contractManager.GetMyContracts().Values)
        {
            if (contract.GetContractState() == ContractState.Completed && contract.GetTrueDeliveryTime() < 60)
            {
                Debug.Log("GetTrueDeliveryTime " + contract.GetTrueDeliveryTime());
                int individualCustomersAll = contract.GetContractFeature().individualCustomers;
                int businessCustomersAll = contract.GetContractFeature().businessCustomers;
                int enterpriseCustomersAll = contract.GetContractFeature().enterpriseCustomers;

                int individualCustomersDuringQ = (int)System.Math.Round((individualCustomersAll / 60f) * (60f - contract.GetTrueDeliveryTime()), System.MidpointRounding.AwayFromZero);
                int businessCustomerDuringQ = (int)System.Math.Round((businessCustomersAll / 60f) * (60f - contract.GetTrueDeliveryTime()), System.MidpointRounding.AwayFromZero);
                int enterpriseCustomersDuringQ = (int)System.Math.Round((enterpriseCustomersAll / 60f) * (60f - contract.GetTrueDeliveryTime()), System.MidpointRounding.AwayFromZero);

                int individualCustomersEndQ = individualCustomersAll - individualCustomersDuringQ;
                int businessCustomersEndQ = businessCustomersAll - businessCustomerDuringQ;
                int enterpriseCustomersEndQ = enterpriseCustomersAll - enterpriseCustomersDuringQ;

                enterpriseCustomersAddDuringQ += enterpriseCustomersDuringQ;
                businessCustomersAddDuringQ += businessCustomerDuringQ;
                individualCustomersAddDuringQ += individualCustomersDuringQ;
                enterpriseCustomersAddEndQ += enterpriseCustomersEndQ;
                businessCustomersAddEndQ += businessCustomersEndQ;
                individualCustomersAddEndQ += individualCustomersEndQ;
            }
            else if(contract.GetContractState() == ContractState.Completed && contract.GetTrueDeliveryTime() > 60)
            {
                enterpriseCustomersAddDuringQ = 0;
                individualCustomersAddDuringQ = 0;
                businessCustomersAddDuringQ = 0;

                enterpriseCustomersAddEndQ = contract.GetContractFeature().enterpriseCustomers;
                businessCustomersAddEndQ = contract.GetContractFeature().businessCustomers;
                individualCustomersAddEndQ = contract.GetContractFeature().individualCustomers;
            }
        }
        endEnterpriseCustomers = ComputeEndEnterpriseCutomers();
        endBusinessCustomers = ComputeEndBusinessCutomers();
        endIndividualsCutomers = ComputeEndIndividualCustomers();

        Debug.Log("updating real customers count" + endEnterpriseCustomers + "," + endBusinessCustomers + "," + endIndividualsCutomers);
    }

    /*public void RiseEnterpriseCustomersAddDuringQ(int count) { CmdRiseEnterpriseCustomersAddDuringQ(count); }
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
    }*/


    public int ComputeEndEnterpriseCutomers() { int endEnterpriseCustomersTmp = beginningEnterpriseCustomers + enterpriseCustomersAddDuringQ + enterpriseCustomersAddEndQ + enterpriseCustomersAdvAdd - enterpriseCustomersAdvLoss; return endEnterpriseCustomersTmp; }
    public int ComputeEndBusinessCutomers() { int endBusinessCustomersTmp = beginningBusinessCustomers + businessCustomersAddDuringQ + businessCustomersAddEndQ + businessCustomersAdvAdd - businessCustomersAdvLoss; return endBusinessCustomersTmp; }
    public int ComputeEndIndividualCustomers() { int endIndividualsCutomersTmp = beginningIndividualCutomers + individualCustomersAddDuringQ + individualCustomersAddEndQ + businessCustomersAdvAdd - individualCustomersAdvLoss; return endIndividualsCutomersTmp; }

    //HOOKS
    public void OnChangeBeginningEneterpriseCustomers(int beginningEnterpriseCustomers)
    {
        this.beginningEnterpriseCustomers = beginningEnterpriseCustomers;
        if (customersUIHandler != null)
        {
            customersUIHandler.SetBeginingEntCustomers(beginningEnterpriseCustomers);
        }
    }
    public void OnChangeBeginningBusinessCustomers(int beginningBusinessCustomers)
    {
        this.beginningBusinessCustomers = beginningBusinessCustomers;
        if (customersUIHandler != null) { customersUIHandler.SetBeginnigBusCutomers(beginningBusinessCustomers); }
    }
    public void OnChangeBeginningIndividualCustomers(int beginningIndividualCustomers)
    {
        this.beginningIndividualCutomers = beginningIndividualCustomers;
        if (customersUIHandler != null) { customersUIHandler.SetBeginingIndCustomers(beginningIndividualCustomers); }
    }

    public void OnChangeEnterpriseCustomerAddDuringQ(int enterpriseCustomersAddDuringQ)
    {
        this.enterpriseCustomersAddDuringQ = enterpriseCustomersAddDuringQ;
        if (customersUIHandler != null) { customersUIHandler.SetEntCustomersAddDuringQ(enterpriseCustomersAddDuringQ); }
    }
    public void OnChangeBusinessCustomerAddDuringQ(int businessCustomersAddDuringQ)
    {
        this.businessCustomersAddDuringQ = businessCustomersAddDuringQ;
        if (customersUIHandler != null) { customersUIHandler.SetBusCustomersAddDuringQ(businessCustomersAddDuringQ); }
    }
    public void OnChangeIndividualCustomerAddDuringQ(int individualCustomersAddDuringQ)
    {
        this.individualCustomersAddDuringQ = individualCustomersAddDuringQ;
        if (customersUIHandler != null) { customersUIHandler.SetIndCustomersAddDuringQ(individualCustomersAddDuringQ); }
    }

    public void OnChangeEnterpriseCustomerAddEndQ(int enterpriseCustomersAddEndQ)
    {
        this.enterpriseCustomersAddEndQ = enterpriseCustomersAddEndQ;
        if (customersUIHandler != null) { customersUIHandler.SetEntCustomersAddEndQ(enterpriseCustomersAddEndQ); }
    }
    public void OnChangeBusinessCustomerAddEndQ(int businessCustomerAddEndQ)
    {
        this.businessCustomersAddEndQ = businessCustomerAddEndQ;
        if (customersUIHandler != null) { customersUIHandler.SetBusCustomersAddEndQ(businessCustomerAddEndQ); }
    }
    public void OnChangeIndividualCustomerAddEndQ(int individualCustomerAddEndQ)
    {
        this.individualCustomersAddEndQ = individualCustomerAddEndQ;
        if (customersUIHandler != null) { customersUIHandler.SetIndCustomersAddEndQ(individualCustomerAddEndQ); }
    }

    public void OnChangeEndEneterpriseCustomers(int endEnterpriseCustomers)
    {
        this.endEnterpriseCustomers = endEnterpriseCustomers;
        if (customersUIHandler != null) { customersUIHandler.SetEndEntCustomers(endEnterpriseCustomers); }
    }
    public void OnChangeEndBusinessCustomers(int endBusinessCustomers)
    {
        this.endBusinessCustomers = endBusinessCustomers;
        if (customersUIHandler != null) { customersUIHandler.SetEndBusCutomers(endBusinessCustomers); }
    }
    public void OnChangeEndIndividualCustomers(int endIndividualCustomers)
    {
        this.endIndividualsCutomers = endIndividualCustomers;
        if (customersUIHandler != null) { customersUIHandler.SetEndIndCustomers(endIndividualCustomers); }
    }

    public void OnChangeEnterpriseCustomersAdvLoss(int loss)
    {
        this.enterpriseCustomersAdvLoss = loss;
        if(customersUIHandler != null)
        {
            customersUIHandler.SetEntCostumersAdvLoss(loss);
        }
    }
    public void OnChangeBusinessCustomersAdvLoss(int loss)
    {
        this.businessCustomersAdvLoss = loss;
        if (customersUIHandler != null)
        {
            customersUIHandler.SetBusCustomersAdvLoss(loss);
        }
    }
    public void OnChangeIndividualCustomersAdvLoss(int loss)
    {
        this.individualCustomersAdvLoss = loss;
        if (customersUIHandler != null)
        {
            customersUIHandler.SetIndCustomersAdvLoss(loss);
        }
    }
    public void OnChangeEnterpriseCustomersAdvAdd(int add)
    {
        this.enterpriseCustomersAdvAdd = add;
        if (customersUIHandler != null)
        {
            customersUIHandler.SetEntCustomerAdvAdd(add);
        }
    }
    public void OnChangeBusinessCustomersAdvAdd(int add)
    {
        this.businessCustomersAdvAdd = add;
        if (customersUIHandler != null)
        {
            customersUIHandler.SetBusCustomersAdvAdd(add);
        }
    }
    public void OnChangeIndividualCustomersAdvAdd(int add)
    {
        this.individualCustomersAdvAdd = add;
        if (customersUIHandler != null)
        {
            customersUIHandler.SetIndCustomersAdvAdd(add);
        }
    }

    // NEXT QUARTER EVALUATION METHODS...
    [Server]
    public void UpdateCurrentQuarterData()
    {
        UpdateRealCustomersCountServer();
    }

    [Server]
    public void MoveToNextQuarter()
    {
        LoadNextQuarterData(currentQuarter);
    }
    [Server]
    public void SaveCurrentQuarterData()
    {
        endEnterpriseCustomersQ.Insert(currentQuarter, endEnterpriseCustomers);
        endBusinessCustomersQ.Insert(currentQuarter, endBusinessCustomers);
        endIndividualsCutomersQ.Insert(currentQuarter, endIndividualsCutomers);

    }

    [Server]
    public void LoadNextQuarterData(int currentQuarter)
    {
        LoadQuarterData(currentQuarter + 1);
    }
}
