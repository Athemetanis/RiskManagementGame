using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProviderAccountingManager : NetworkBehaviour
{
    //VARIABLES
    //STORED VALUES FOR Q0, Q1, Q2, Q3, Q4, on corresponding indexes  // Q0 are default values when the game begins. 
    private SyncListInt beginningCashBalanceQuarters = new SyncListInt() { };
    private SyncListInt revenueQuarters = new SyncListInt() { };
    private SyncListInt individualRevenueQuarters = new SyncListInt() { };
    private SyncListInt enterpriseRevenueQarters = new SyncListInt() { };
    private SyncListInt businessRevenueQuarters = new SyncListInt() { };
    private SyncListInt advertisementCostQuarters = new SyncListInt() { };
    private SyncListInt contractPaymentsQuarters = new SyncListInt() { };
    private SyncListInt riskSharingFeesReceivedQuarters = new SyncListInt() { };
    private SyncListInt terminationFeeReceivedQuarters = new SyncListInt() { };
    private SyncListInt marketingResearchQuarters = new SyncListInt() { };
    private SyncListInt borrowEmergencyLoanQuarters = new SyncListInt(){};
    private SyncListInt repayEmergencyLoanQuarters = new SyncListInt() { };
    private SyncListInt endCashBalanceQuarters = new SyncListInt() { };

    //CURRENT VALUES
    [SyncVar(hook = "OnChangeBeginningCashBalance")]
    private int beginningCashBalance;
    [SyncVar(hook = "OnChangeRevenue")]
    private int revenue;
    [SyncVar(hook = "OnChangeIndividualCustomersRevenue")]
    private int individualCustomersRevenue;
    [SyncVar(hook = "OnChangeBusinessCustomersRevenue")]
    private int businessCustomersRevenue;
    [SyncVar(hook = "OnChangeEnterpriseCustomersRevenue")]
    private int enterpriseCustomersRevenue;
    [SyncVar(hook = "OnChangeAdvertisementCost")]
    private int advertisementCost;
    [SyncVar(hook = "OnChangeContractPayments")]
    private int contractPayments;
    [SyncVar(hook = "OnChangeRiskSharingFeesReceived")]
    private int riskSharingFeesReceived;
    [SyncVar(hook = "OnChangeTerminationFeeReceived")]
    private int terminationFeeReceived;
    [SyncVar(hook = "OnChangeMarketingResearch")]
    private int marketingResearch;
    [SyncVar(hook = "OnChangeBorrowEmergencyLoan")]
    private int borrowEmergencyLoan;
    [SyncVar(hook = "OnChangeRepayEmergencyLoan")]
    private int repayEmergencyLoan;
    [SyncVar(hook = "OnChangeEndCashBalance")]
    private int endCashBalance;


    //RERERENCES
    private string gameID;
    private int currentQuarter;

    private ProductManager productManager;
    private ContractManager contractManager;
    private CustomersManager customersManager;
    private MarketingManager marketingManager;

    private ProviderAccountingUIComponentHandler providerAccountingUIComponentHandlerQ1;
    private ProviderAccountingUIComponentHandler providerAccountingUIComponentHandlerQ2;
    private ProviderAccountingUIComponentHandler providerAccountingUIComponentHandlerQ3;
    private ProviderAccountingUIComponentHandler providerAccountingUIComponentHandlerQ4;

    private ProviderAccountingUIComponentHandler providerAccountingUIComponentHandlerCurrent;

    //GETTERS & SETTERS
    public int GetBeginnigCashBalance() { return beginningCashBalance; }
    public int GetRevenue() { return revenue; }
    public int GetIndividualCustomersRevenue() { return individualCustomersRevenue; }
    public int GetBusinessCustomersRevenue() { return businessCustomersRevenue; }
    public int GetEnterpriseCustomersRevenue() { return enterpriseCustomersRevenue; }
    public int GetAdvertisementCost() { return advertisementCost; }
    public int GetContractPayments() { return contractPayments; }
    public int GetRishSharingFeesReceived() { return riskSharingFeesReceived; }
    public int GetTerminationFeeReceived() { return terminationFeeReceived; }
    public int GetMarketingResearch() { return marketingResearch; }
    public int GetBorrowEmergencyLoan() { return borrowEmergencyLoan; }
    public int GetRepayEmergencyLoan() { return repayEmergencyLoan; }
    public int GetEndCashBalance() { return endCashBalance; }

    public void SetProviderAccountingUIComponentHandlerQ1(ProviderAccountingUIComponentHandler providerAccountingUIComponentHandler) { this.providerAccountingUIComponentHandlerQ1 = providerAccountingUIComponentHandler; }
    public void SetProviderAccountingUIComponentHandlerQ2(ProviderAccountingUIComponentHandler providerAccountingUIComponentHandler) { this.providerAccountingUIComponentHandlerQ2 = providerAccountingUIComponentHandler; }
    public void SetProviderAccountingUIComponentHandlerQ3(ProviderAccountingUIComponentHandler providerAccountingUIComponentHandler) { this.providerAccountingUIComponentHandlerQ3 = providerAccountingUIComponentHandler; }
    public void SetProviderAccountingUIComponentHandlerQ4(ProviderAccountingUIComponentHandler providerAccountingUIComponentHandler) { this.providerAccountingUIComponentHandlerQ4 = providerAccountingUIComponentHandler; }

    public void SetCurrentProviderAccountingUIHandler(ProviderAccountingUIComponentHandler providerAccountingUIHandler) { this.providerAccountingUIComponentHandlerCurrent = providerAccountingUIHandler; }

    // Start is called before the first frame update
    public override void OnStartServer()
    {
        gameID = this.gameObject.GetComponent<PlayerData>().GetGameID();
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        productManager = this.gameObject.GetComponent<ProductManager>();
        contractManager = this.gameObject.GetComponent<ContractManager>();
        customersManager = this.gameObject.GetComponent<CustomersManager>();
        marketingManager = this.gameObject.GetComponent<MarketingManager>();
        if(beginningCashBalanceQuarters.Count == 0)
        {
            SetupDefaultValues();
        }
        LoadQuarterData(currentQuarter);
    }
    void Start()
    {
        productManager = this.gameObject.GetComponent<ProductManager>();
        contractManager = this.gameObject.GetComponent<ContractManager>();
        customersManager = this.gameObject.GetComponent<CustomersManager>();
        marketingManager = this.gameObject.GetComponent<MarketingManager>();
    }

    //METHODS
    [Server]
    public void SetupDefaultValues()
    {
        beginningCashBalanceQuarters.Insert(0, 0);
        revenueQuarters.Insert(0, 0);
        individualRevenueQuarters.Insert(0, 0);
        businessRevenueQuarters.Insert(0, 0);
        enterpriseRevenueQarters.Insert(0, 0);
        advertisementCostQuarters.Insert(0, 0);
        contractPaymentsQuarters.Insert(0, 0);
        riskSharingFeesReceivedQuarters.Insert(0, 0);
        terminationFeeReceivedQuarters.Insert(0, 0);
        marketingResearchQuarters.Insert(0, 0);
        borrowEmergencyLoanQuarters.Insert(0, 0);
        repayEmergencyLoanQuarters.Insert(0, 0);
        endCashBalanceQuarters.Insert(0, 2000000);
    }
    [Server]
    public void LoadQuarterData(int quarter)
    {
        if (endCashBalanceQuarters.Count != quarter)
        {
            for (int i = endCashBalanceQuarters.Count + 1; i < quarter; i++)
            {
                beginningCashBalanceQuarters.Insert(i, endCashBalanceQuarters[i - 1]);
                advertisementCostQuarters.Insert(i, advertisementCostQuarters[i - 1]);
                contractPaymentsQuarters.Insert(i, 0);
                terminationFeeReceivedQuarters.Insert(i, 0);
                riskSharingFeesReceivedQuarters.Insert(i, 0);
                marketingResearchQuarters.Insert(i, 0);
                borrowEmergencyLoanQuarters.Insert(i, 0);
                repayEmergencyLoanQuarters.Insert(i, 0);
                int individualCustomers = customersManager.GetIndividualCustomersQ(i - 1);
                int businessCustomers = customersManager.GetBusinesCustomersQ(i - 1);
                int enterpriseCustomers = customersManager.GetEnterpriseCustomersQ(i - 1);
                int individualPrice = marketingManager.GetIndividualsPrice();
                int businessPrice = marketingManager.GetBusinessPrice();
                int enterprisePrice = marketingManager.GetEnterprisePrice();
                individualRevenueQuarters.Insert(i, individualCustomers * individualPrice);
                businessRevenueQuarters.Insert(i, businessCustomers * businessPrice);
                enterpriseRevenueQarters.Insert(i, enterpriseCustomers * enterprisePrice);
                revenueQuarters.Insert(i, individualCustomersRevenue + businessCustomersRevenue + enterpriseCustomersRevenue);
                endCashBalanceQuarters.Insert(i, beginningCashBalanceQuarters[i] + revenueQuarters[i] - advertisementCostQuarters[i]);
            }
        }
        beginningCashBalance = endCashBalanceQuarters[quarter - 1];
        UpdateRevenueServer();
        UpdateEstimatedContractPaymentsServer();
        UpdateAdvertisementCostServer();
        ComputeEndCashBalance();
    }
    
    public (int beginningCashBalance, int revenue, int enterpriseRevenue, int businessRevenue, int individualRevenue,int advertismenentCost, int contractPayments, int riskSharingFeeReceived, int terminationFeeReceived, int marketingResearch, int borrowEmergencyLoan, int repayEmergencyLoan, int endCashBalance) GetCorrespondingQuarterData(int correspondingQuarter)
    {
        return (beginningCashBalanceQuarters[correspondingQuarter], revenueQuarters[correspondingQuarter], enterpriseRevenueQarters[correspondingQuarter], businessRevenueQuarters[correspondingQuarter], individualRevenueQuarters[correspondingQuarter], advertisementCostQuarters[correspondingQuarter], contractPaymentsQuarters[correspondingQuarter], riskSharingFeesReceivedQuarters[correspondingQuarter], terminationFeeReceivedQuarters[correspondingQuarter], marketingResearchQuarters[correspondingQuarter], borrowEmergencyLoanQuarters[correspondingQuarter], repayEmergencyLoanQuarters[correspondingQuarter], endCashBalanceQuarters[correspondingQuarter]);
    }

    /*public void UpdateRevenue()
    {
        CmdUpdateRevenue();
    }
    public void CmdUpdateRevenue() 
    {
        int beginningIndividualCustomers = customersManager.GetBeginningIndividualCustomers();
        int beginningBusinessCustomers = customersManager.GetBeginningBusinessCustomers();
        int beginningEnterpriseCustomers = customersManager.GetBeginningEnterpriseCustomers();

        int enterpriseCustomersAddDuringQ = customersManager.GetEnterpriseCustomersDuringQ();
        int businessCustomersAddDuringQ = customersManager.GetBusinessCustomersDuringQ();
        int individualCustomersAddDuringQ = customersManager.GetIndividualCustomersDuringQ();

        int individualPrice = marketingManager.GetIndividualsPrice();
        int businessPrice = marketingManager.GetBusinessPrice();
        int enterprisePrice = marketingManager.GetEnterprisePrice();

        individualCustomersRevenue = (beginningIndividualCustomers + enterpriseCustomersAddDuringQ) * individualPrice;
        businessCustomersRevenue = (beginningBusinessCustomers + businessCustomersAddDuringQ) * businessPrice;
        enterpriseCustomersRevenue = (beginningEnterpriseCustomers + enterpriseCustomersAddDuringQ) * enterprisePrice;
        revenue = individualCustomersRevenue + businessCustomersRevenue + enterpriseCustomersRevenue;

        endCashBalance = ComputeEndCashBalance();
    }*/

    [Server]
    public void UpdateRevenueServer()
    {   
        int beginningIndividualCustomers = customersManager.GetBeginningIndividualCustomers(); 
        int beginningBusinessCustomers = customersManager.GetBeginningBusinessCustomers();
        int beginningEnterpriseCustomers = customersManager.GetBeginningEnterpriseCustomers();

        int enterpriseCustomersAddDuringQ = customersManager.GetEnterpriseCustomersDuringQ();
        int businessCustomersAddDuringQ = customersManager.GetBusinessCustomersDuringQ();
        int individualCustomersAddDuringQ = customersManager.GetIndividualCustomersDuringQ();

        int enterpriseCustomersAdvLoss = customersManager.GetEnterpriseCustomersAdvLoss();
        int businessCutomersAdvLoss = customersManager.GetBusinessCustomersAdvLoss();
        int individualCustomersAdvLoss = customersManager.GetIndividualCustomersAdvLoss();

        int enterpriseCustomersAdvAdd = customersManager.GetEnterpriseCustomersAdvAdd();
        int businessCutomersAdvAdd = customersManager.GetBusinessCustomersAdvAdd();
        int individualCustomersAdvAdd = customersManager.GetIndividualCustomersAdvAdd();

        int individualPrice = marketingManager.GetIndividualsPrice();
        int businessPrice = marketingManager.GetBusinessPrice();
        int enterprisePrice = marketingManager.GetEnterprisePrice();

        individualCustomersRevenue = (beginningIndividualCustomers + individualCustomersAddDuringQ + individualCustomersAdvAdd - individualCustomersAdvLoss) * individualPrice;
        businessCustomersRevenue = (beginningBusinessCustomers + businessCustomersAddDuringQ + businessCutomersAdvAdd - businessCutomersAdvLoss) * businessPrice;
        enterpriseCustomersRevenue = (beginningEnterpriseCustomers + enterpriseCustomersAddDuringQ + enterpriseCustomersAdvAdd - enterpriseCustomersAdvLoss) * enterprisePrice;
        revenue = individualCustomersRevenue + businessCustomersRevenue + enterpriseCustomersRevenue;

        endCashBalance = ComputeEndCashBalance();
    }
    [Client]
    public void UpdateAdvertisementCost()
    {
        CmdUpdateAdvertisementCost();
    }
    [Command]
    public void CmdUpdateAdvertisementCost()
    {
        switch (marketingManager.GetAdvertisementCoverage())
        {
            case 0:
                advertisementCost = marketingManager.GetAdvertisement0Price();
                break;
            case 25:
                advertisementCost = marketingManager.GetAdvertisement25Price();
                break;
            case 50:
                advertisementCost = marketingManager.GetAdvertisement50Price();
                break;
            case 75:
                advertisementCost = marketingManager.GetAdvertisement75Price();
                break;
            case 100:
                advertisementCost = marketingManager.GetAdvertisement100Price();
                break;
        }
        endCashBalance = ComputeEndCashBalance();
    }
    [Server]
    public void UpdateAdvertisementCostServer()
    {
        switch (marketingManager.GetAdvertisementCoverage())
        {
            case 0:
                advertisementCost = marketingManager.GetAdvertisement0Price();
                break;
            case 25:
                advertisementCost = marketingManager.GetAdvertisement25Price();
                break;
            case 50:
                advertisementCost = marketingManager.GetAdvertisement50Price();
                break;
            case 75:
                advertisementCost = marketingManager.GetAdvertisement75Price();
                break;
            case 100:
                advertisementCost = marketingManager.GetAdvertisement100Price();
                break;
        }
        endCashBalance = ComputeEndCashBalance();
    }

    [Server]
    public void UpdateEstimatedContractPaymentsServer()
    {
        contractPayments = 0;
        foreach (Contract contract in contractManager.GetMyContracts().Values)
        {
            if (contract.GetContractState() == ContractState.Accepted)
            {
                contractPayments += contract.GetContractPrice();
            }
        }
        riskSharingFeesReceived = 0;
        terminationFeeReceived = 0;

    }
    [Server]
    public void UpdateRealContractPaymentsServer()
    {
        contractPayments = 0;
        riskSharingFeesReceived = 0;
        terminationFeeReceived = 0;
        foreach (Contract contract in contractManager.GetMyContracts().Values)
        {
            if (contract.GetContractState() == ContractState.Completed)
            {
                contractPayments += contract.GetContractPrice();
                riskSharingFeesReceived += contract.GetRiskSharingFeePaid();
            }
            else if (contract.GetContractState() == ContractState.Terminated)
            {
                terminationFeeReceived += contract.GetTerminationFeePaid();
            }
        }

        endCashBalance = ComputeEndCashBalance();
    }
           
    public int ComputeEndCashBalance()
    {
        int endCash = 0;
        endCash = beginningCashBalance - contractPayments - advertisementCost - marketingResearch + revenue + riskSharingFeesReceived + terminationFeeReceived;
        return endCash;
    }
    //HOOKS
    public void OnChangeBeginningCashBalance(int beginningCashBalance)
    {
        this.beginningCashBalance = beginningCashBalance;
        if(providerAccountingUIComponentHandlerCurrent != null)
        {
            providerAccountingUIComponentHandlerCurrent.UpdateBeginingCashBalanceText(this.beginningCashBalance);
        }
    }
    public void OnChangeRevenue(int revenue)
    {
        this.revenue = revenue;
        if (providerAccountingUIComponentHandlerCurrent != null)
        {
            providerAccountingUIComponentHandlerCurrent.UpdateRevenueText(this.revenue);
        }
    }
    public void OnChangeIndividualCustomersRevenue(int individualCustomersRevenue)
    {
        this.individualCustomersRevenue = individualCustomersRevenue;
        if(providerAccountingUIComponentHandlerCurrent != null)
        {
            providerAccountingUIComponentHandlerCurrent.UpdateIndividualRevenue(this.individualCustomersRevenue);
        }
    }
    public void OnChangeBusinessCustomersRevenue(int businessCustomersRevenue)
    {
        this.businessCustomersRevenue = businessCustomersRevenue;
        if (providerAccountingUIComponentHandlerCurrent != null)
        {
            providerAccountingUIComponentHandlerCurrent.UpdateBusinessRevenue(this.businessCustomersRevenue);
        }

    }
    public void OnChangeEnterpriseCustomersRevenue(int enterpriseCustomersRevenue)
    {
        this.enterpriseCustomersRevenue = enterpriseCustomersRevenue;
        if (providerAccountingUIComponentHandlerCurrent != null)
        {
            providerAccountingUIComponentHandlerCurrent.UpdateEnterpriseRevenue(this.enterpriseCustomersRevenue);
        }

    }
    public void OnChangeAdvertisementCost(int advertisementCost)
    {
        this.advertisementCost = advertisementCost;
        if (providerAccountingUIComponentHandlerCurrent != null)
        {
            providerAccountingUIComponentHandlerCurrent.UpdateAdvertisementText(this.advertisementCost);
        }
    }
    public void OnChangeContractPayments(int contractPayments)
    {
        this.contractPayments = contractPayments;
        if (providerAccountingUIComponentHandlerCurrent != null)
        {
            providerAccountingUIComponentHandlerCurrent.UpdateContractPayments(this.contractPayments);
        }

    }
    public void OnChangeRiskSharingFeesReceived (int riskSharingFeesReceived)
    {
        this.riskSharingFeesReceived = riskSharingFeesReceived;
        if (providerAccountingUIComponentHandlerCurrent != null)
        {
            providerAccountingUIComponentHandlerCurrent.UpdateRiskSharingFee(this.riskSharingFeesReceived);
        }
    }
    public void OnChangeTerminationFeeReceived(int terminationFeeReceived)
    {
        this.terminationFeeReceived = terminationFeeReceived;
        if (providerAccountingUIComponentHandlerCurrent != null)
        {
            providerAccountingUIComponentHandlerCurrent.UpdateTerminatinFeeReceived(this.terminationFeeReceived);
        }
    }
    public void OnChangeMarketingResearch(int marketingResearch)
    {
        this.marketingResearch = marketingResearch;
        if (providerAccountingUIComponentHandlerCurrent != null)
        {
            providerAccountingUIComponentHandlerCurrent.UpdateMarketingResearch(this.marketingResearch);
        }
    }
    public void OnChangeBorrowEmergencyLoan(int borrowEmergencyLoan)
    {
        this.borrowEmergencyLoan = borrowEmergencyLoan;
        if (providerAccountingUIComponentHandlerCurrent != null)
        {
            providerAccountingUIComponentHandlerCurrent.UpdateBorrowEmergencyLoan(this.borrowEmergencyLoan);
        }

    }
    public void OnChangeRepayEmergencyLoan(int repayEmergencyLoan)
    {
        this.repayEmergencyLoan = repayEmergencyLoan;
        if (providerAccountingUIComponentHandlerCurrent != null)
        {
            providerAccountingUIComponentHandlerCurrent.UpdateRepayEmergencyLoan(this.repayEmergencyLoan);
        }
    }
    public void OnChangeEndCashBalance(int endCashBalance)
    {
        this.endCashBalance = endCashBalance;
        if (providerAccountingUIComponentHandlerCurrent != null)
        {
            providerAccountingUIComponentHandlerCurrent.UpdateEndCashBalanceText(this.endCashBalance);
        }
    }

    // NEXT QUARTER EVALUATION METHODS...
    [Server]
    public void UpdateCurrentQuarterData()
    {
        UpdateRevenueServer();
        UpdateRealContractPaymentsServer();
    }

    [Server]
    public void MoveToNextQuarter()
    {
        SetNewReferences();
        LoadNextQuarterData();
    }

    [Server]
    public void SaveCurrentQuarterData()
    {
        Debug.Log("prov. Acc. saving data " + beginningCashBalance + " , " + riskSharingFeesReceived + " , " + individualCustomersRevenue + " , " + endCashBalance);
        beginningCashBalanceQuarters.Insert(currentQuarter, beginningCashBalance);
        revenueQuarters.Insert(currentQuarter, revenue);
        individualRevenueQuarters.Insert(currentQuarter, individualCustomersRevenue);
        businessRevenueQuarters.Insert(currentQuarter, businessCustomersRevenue);
        enterpriseRevenueQarters.Insert(currentQuarter, enterpriseCustomersRevenue);
        advertisementCostQuarters.Insert(currentQuarter, advertisementCost);
        contractPaymentsQuarters.Insert(currentQuarter, contractPayments);
        riskSharingFeesReceivedQuarters.Insert(currentQuarter, riskSharingFeesReceived);
        terminationFeeReceivedQuarters.Insert(currentQuarter, terminationFeeReceived);
        marketingResearchQuarters.Insert(currentQuarter, marketingResearch);
        borrowEmergencyLoanQuarters.Insert(currentQuarter, borrowEmergencyLoan);
        repayEmergencyLoanQuarters.Insert(currentQuarter, repayEmergencyLoan);
        endCashBalanceQuarters.Insert(currentQuarter, endCashBalance);
    }

    [Server]
    public void SetNewReferences()
    {
        Debug.Log("setting new references server");
        RpcSetNewReferences();
    }

    [ClientRpc]
    public void RpcSetNewReferences()
    {
        gameID = this.gameObject.GetComponent<PlayerData>().GetGameID();
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        Debug.Log("current quarter for reference" + currentQuarter);
        if (providerAccountingUIComponentHandlerCurrent != null)
        {
            Debug.Log("setting new references client");
            if (currentQuarter == 1)
            {
                Debug.Log("setting active 2. quarter");
                providerAccountingUIComponentHandlerCurrent = providerAccountingUIComponentHandlerQ2;
                providerAccountingUIComponentHandlerQ2.gameObject.SetActive(true);
                providerAccountingUIComponentHandlerQ2.UpdateAllElements();
                
            }
            if (currentQuarter == 2)
            {
                providerAccountingUIComponentHandlerCurrent = providerAccountingUIComponentHandlerQ3;
                providerAccountingUIComponentHandlerQ3.gameObject.SetActive(true);
                providerAccountingUIComponentHandlerQ3.UpdateAllElements();
            }
            if (currentQuarter == 3)
            {
                providerAccountingUIComponentHandlerCurrent = providerAccountingUIComponentHandlerQ4;
                providerAccountingUIComponentHandlerQ4.gameObject.SetActive(true);
                providerAccountingUIComponentHandlerQ4.UpdateAllElements();
            }
        }       
    }

    [Server]
    public void LoadNextQuarterData()
    {
        Debug.Log("loading new data");
        LoadQuarterData(currentQuarter + 1);
    }



}
