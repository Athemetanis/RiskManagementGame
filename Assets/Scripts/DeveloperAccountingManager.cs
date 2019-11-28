using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

//public class SyncListInt : SyncList<int> { }

public class DeveloperAccountingManager : NetworkBehaviour
{
    //VARIABLES

    //STORED VALUES FOR Q0, Q1, Q2, Q3, Q4, on corresponding indexes  // Q0 are default values when the game begins. 
    private SyncListInt beginningCashBalanceQuarters = new SyncListInt() { };
    private SyncListInt revenueQuarters = new SyncListInt() { };
    private SyncListInt salariesQuarters = new SyncListInt() { };
    private SyncListInt programmersSalariesQuarters = new SyncListInt() { };
    private SyncListInt uiSpecialistsSalariesQuarters = new SyncListInt() { };
    private SyncListInt integrabilitySpecialistsSalariesQuarters = new SyncListInt() { };
    private SyncListInt riskSharingFeesPaidQuarters = new SyncListInt() { };
    private SyncListInt terminationFeePaidQuarters = new SyncListInt() { };
    private SyncListInt marketingResearchQuarters = new SyncListInt() { };

    private SyncListInt endCashBalanceQuarters = new SyncListInt() { };

    private SyncListInt borrowEmergencyLoanQuarters = new SyncListInt() { };
    private SyncListInt repayEmergencyLoanQuarters = new SyncListInt() { };

    /*
    private SyncListInt emergencyLoanBaseQuarters = new SyncListInt() { };
    private SyncListInt emergencyLoanInterestsQuarters = new SyncListInt() { };*/

    [SyncVar(hook = "OnChangeHistorySaved")]
    private bool historySaved;
    //CURRENT VALUES
    [SyncVar(hook = "OnChangeBeginningCashBalance")]
    private int beginningCashBalance;
    [SyncVar(hook = "OnChangeRevenue")]
    private int revenue;
    [SyncVar(hook = "OnChangeSalaries")]
    private int salaries;
    [SyncVar(hook = "OnChangeProgrammersSalaries")]
    private int programmersSalaries;
    [SyncVar(hook = "OnChangeUISpecialistsSalaries")]
    private int uiSpecialistsSalaries;
    [SyncVar(hook = "OnChangeIntegrabilitySpecialistsSalaries")]
    private int integrabilitySpecialistsSalaries;
    [SyncVar(hook = "OnChangeRiskSharingFeesPaid")]
    private int riskSharingFeesPaid;
    [SyncVar(hook = "OnChangeTerminationFeePaid")]
    private int terminationFeePaid;
    [SyncVar(hook = "OnChangeMarketingResearch")]
    private int marketingResearch;

    [SyncVar(hook = "OnChangeEndCashBalance")]
    private int endCashBalance;

    [SyncVar(hook = "OnChangeBorrowEmergencyLoan")]
    private int borrowEmergencyLoan;
    [SyncVar(hook = "OnChangeRepayEmergencyLoan")]
    private int repayEmergencyLoan;

    /*
    [SyncVar(hook = "OnChangeEmergencyLoanBase")]
    private int emergencyLoanBase;
    [SyncVar(hook = "OnChangeEmergencyLoanInterests")]
    private int emergencyLoanInterests;
    */



    //REFERNCES
    private string gameID;
    private int currentQuarter;

    private HumanResourcesManager humanResourcesManager;
    private ContractManager contractManager;
    private PlayerData playerData;

    private DeveloperAccountingUIHandler developerAccountingUIHandler;
    private DeveloperAccountingUIComponentHandler developerAccountingUIHandlerCurrent;

    //GETTERS & SETTERS
    public int GetBeginningCashBalance() { return beginningCashBalance; }
    public int GetRevenue() { return revenue; }
    public int GetSalaries() { return salaries; }
    public int GetProgrammersSalaries() { return programmersSalaries; }
    public int GetUISpecialistsSalaries() { return uiSpecialistsSalaries; }
    public int GetIntegrabilitySpecialistsSalaries() { return integrabilitySpecialistsSalaries; }
    public int GetRishSharingFeesPaid() { return riskSharingFeesPaid; }
    public int GetTerminationFeePaid() { return terminationFeePaid; }
    public int GetMarketingResearch() { return marketingResearch; }
    public int GetBorrowEmergencyLoan() { return borrowEmergencyLoan; }
    public int GetRepayEmergencyLoan() { return repayEmergencyLoan; }
    public int GetEndCashBalance() { return endCashBalance; }

    public int GetEndCashBalanceQuarter(int quarter) { return endCashBalanceQuarters[quarter]; }

   // public int GetEmergencyLoanInterests() { return emergencyLoanInterests; }

    public void SetDeveloperAccountingUIHandler(DeveloperAccountingUIHandler developerAccountingUIHandler) { this.developerAccountingUIHandler = developerAccountingUIHandler; }
    public void SetCurrentDeveloperAccountingUIHandler(DeveloperAccountingUIComponentHandler currentDeveloperAccountingUIHandler) { this.developerAccountingUIHandlerCurrent = currentDeveloperAccountingUIHandler; }

    // Start is called before the first frame update
    public override void OnStartServer()
    {
        playerData = this.gameObject.GetComponent<PlayerData>();
        gameID = this.gameObject.GetComponent<PlayerData>().GetGameID();
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        humanResourcesManager = this.gameObject.GetComponent<HumanResourcesManager>();
        contractManager = this.gameObject.GetComponent<ContractManager>();



        if (beginningCashBalanceQuarters.Count == 0)
        {
            SetupDefaultValues();
        }
        LoadQuarterData(currentQuarter);
        UpdateSalariesServer();
        UpdateEstimatedRevenueServer();
    }

    public override void OnStartClient()
    {
        playerData = this.gameObject.GetComponent<PlayerData>();
        humanResourcesManager = this.gameObject.GetComponent<HumanResourcesManager>();
        contractManager = this.gameObject.GetComponent<ContractManager>();

        beginningCashBalanceQuarters.Callback += OnBeginningCashHisotryChanged;
        revenueQuarters.Callback += OnRevenueHistoryChanged;
        endCashBalanceQuarters.Callback += OnEndCashHistoryChanged;
    }

    public void OnBeginningCashHisotryChanged(SyncList<int>.Operation op, int index, int value)
    {
       // Debug.Log("BC history changed on index " + index + " on value " + value);
    }
    public void OnRevenueHistoryChanged(SyncList<int>.Operation op, int index, int value)
    {
     //   Debug.Log("Revenue history changed on index " + index + " on value " + value);
    }
    public void OnEndCashHistoryChanged(SyncList<int>.Operation op, int index, int value)
    {
      //  Debug.Log("EC history changed on index " + index + " on value " + value);
    }


    //METHODS
    [Server]
    public void SetupDefaultValues()
    {
        beginningCashBalanceQuarters.Insert(0, 0);
        revenueQuarters.Insert(0, 0);
        salariesQuarters.Insert(0, 0);
        programmersSalariesQuarters.Insert(0, 0);
        uiSpecialistsSalariesQuarters.Insert(0, 0);
        integrabilitySpecialistsSalariesQuarters.Insert(0, 0);
        riskSharingFeesPaidQuarters.Insert(0, 0);
        terminationFeePaidQuarters.Insert(0, 0);

        marketingResearchQuarters.Insert(0, 0);

        endCashBalanceQuarters.Insert(0, 2000000);

        borrowEmergencyLoanQuarters.Insert(0, 0);
        repayEmergencyLoanQuarters.Insert(0, 0);

        //emergencyLoanBaseQuarters.Insert(0, 0);
        //emergencyLoanInterestsQuarters.Insert(0, 0);

    }

    [Server]
    public void LoadQuarterData(int quarter)  //loading only values that are transfered to the next quarter  --------SERVER--------
    {
        if (endCashBalanceQuarters.Count != quarter) //true = player joined already running game - skipped some quarters
        {
            for (int i = endCashBalanceQuarters.Count + 1; i < quarter; i++)  //values for skipped quarters are generated
            {
                beginningCashBalanceQuarters.Insert( i ,endCashBalanceQuarters[i - 1]);
                revenueQuarters.Insert(i, 0);
                salariesQuarters.Insert(i, salariesQuarters[i - 1]);
                programmersSalariesQuarters.Insert(i, programmersSalariesQuarters[i - 1]);
                uiSpecialistsSalariesQuarters.Insert(i, uiSpecialistsSalariesQuarters[i - 1]);
                integrabilitySpecialistsSalariesQuarters.Insert(i, integrabilitySpecialistsSalariesQuarters[i - 1]);
                riskSharingFeesPaidQuarters.Insert(i, 0);
                terminationFeePaidQuarters.Insert(i, 0);

                marketingResearchQuarters.Insert(i, 0);

                repayEmergencyLoanQuarters.Insert(i, 0);

                endCashBalanceQuarters.Insert(i, beginningCashBalanceQuarters[i] - salariesQuarters[i]);


                borrowEmergencyLoanQuarters.Insert(i, 0);
                repayEmergencyLoanQuarters.Insert(i, 0);

                // emergencyLoanBaseQuarters.Insert(i, emergencyLoanBaseQuarters[i - 1] + emergencyLoanInterestsQuarters[i - 1]);
                // emergencyLoanInterestsQuarters.Insert(i, (int)System.Math.Round(((float)emergencyLoanBaseQuarters[i] * 0.1), System.MidpointRounding.AwayFromZero));

                // borrowEmergencyLoanQuarters.Insert(i, emergencyLoanBaseQuarters[i] + emergencyLoanInterestsQuarters[i]);


            }
        }

        beginningCashBalance = endCashBalanceQuarters[quarter - 1];
        marketingResearch = marketingResearchQuarters[quarter - 1];

        /*if(repayEmergencyLoanQuarters[quarter - 1] > 0)
        {
            emergencyLoanBase = borrowEmergencyLoanQuarters[quarter - 1] - repayEmergencyLoanQuarters[quarter - 1];
        }
        else
        {
            emergencyLoanBase = borrowEmergencyLoanQuarters[quarter - 1];
        }*/

        /* if (beginningCashBalance < 0)
         {
             emergencyLoanBase += beginningCashBalance;
             emergencyLoanInterests = (int)System.Math.Round(((float)emergencyLoanBase * 0.1), System.MidpointRounding.AwayFromZero);
             borrowEmergencyLoan = emergencyLoanBase + emergencyLoanInterests ;
             beginningCashBalance = 0;
         }*/

        repayEmergencyLoan = 0;

        UpdateEstimatedRevenueServer();
        UpdateSalariesServer();





        ComputeEndCashBalance();
        historySaved = false;
    }

    /*public (int beginningCashBalance, int revenue, int salaries, int programmersSalaries, int uiSpecialistsSalaries, int integrabilitySpecialistsSalaries, int riskSharingFeePaid, int terminationFeePaid, int marketingResearch, int borrowEmergencyLoan, int repayEmergencyLoan, int endCashBalance, int emergencyLoanInterests) GetCorrecpondingQuarterData(int correspondingQuarter)
    {
        Debug.Log(currentQuarter);
        return (beginningCashBalanceQuarters[correspondingQuarter], revenueQuarters[correspondingQuarter], salariesQuarters[correspondingQuarter], programmersSalariesQuarters[correspondingQuarter], uiSpecialistsSalariesQuarters[correspondingQuarter], integrabilitySpecialistsSalariesQuarters[correspondingQuarter], riskSharingFeesPaidQuarters[correspondingQuarter], terminationFeePaidQuarters[correspondingQuarter], marketingResearchQuarters[correspondingQuarter],borrowEmergencyLoanQuarters[correspondingQuarter],repayEmergencyLoanQuarters[correspondingQuarter], endCashBalanceQuarters[correspondingQuarter], emergencyLoanInterestsQuarters[correspondingQuarter]);
    }*/

    public (int beginningCashBalance, int revenue, int salaries, int programmersSalaries, int uiSpecialistsSalaries, int integrabilitySpecialistsSalaries, int riskSharingFeePaid, int terminationFeePaid, int marketingResearch, int borrowEmergencyLoan, int repayEmergencyLoan, int endCashBalance) GetCorrecpondingQuarterData(int correspondingQuarter)
    {
        Debug.Log(currentQuarter);
        return (beginningCashBalanceQuarters[correspondingQuarter], revenueQuarters[correspondingQuarter], salariesQuarters[correspondingQuarter], programmersSalariesQuarters[correspondingQuarter], uiSpecialistsSalariesQuarters[correspondingQuarter], integrabilitySpecialistsSalariesQuarters[correspondingQuarter], riskSharingFeesPaidQuarters[correspondingQuarter], terminationFeePaidQuarters[correspondingQuarter], marketingResearchQuarters[correspondingQuarter], borrowEmergencyLoanQuarters[correspondingQuarter], repayEmergencyLoanQuarters[correspondingQuarter], endCashBalanceQuarters[correspondingQuarter]);
    }




    [Server]
    public void UpdateSalariesServer()
    {
        int programmers = humanResourcesManager.GetProgrammersCount();
        int programmerSalary = humanResourcesManager.GetProgrammerSalaryPerQurter();
        int uiSpecialists = humanResourcesManager.GetUISPecialistsCount();
        int uiSpecialistSalary = humanResourcesManager.GetUISpecialistSalaryPerQuarter();
        int integrabilitySpecialists = humanResourcesManager.GetIntegrabilitySpecialistsCount();
        int integrabilitySpecialistSalary = humanResourcesManager.GetIntegrabilitySpecialistSalaryPerQuarter();


        programmersSalaries = programmers * programmerSalary;
        uiSpecialistsSalaries = uiSpecialists * uiSpecialistSalary;
        integrabilitySpecialistsSalaries = integrabilitySpecialists * integrabilitySpecialistSalary;
        salaries = programmersSalaries + uiSpecialistsSalaries + integrabilitySpecialistsSalaries;

        ComputeEndCashBalance();
    }
    [Server]
    public void UpdateEstimatedRevenueServer()
    {
        int contractPayments = 0;
        foreach (Contract contract in contractManager.GetMyContracts().Values)
        {
            if (contract.GetContractState() == ContractState.Accepted)
            {
                contractPayments += contract.GetContractPrice();
            }
        }
        revenue = contractPayments;
        riskSharingFeesPaid = 0;
        terminationFeePaid = 0;
        ComputeEndCashBalance();
    }
    [Server]
    public void UpdateRealRevenueServer()
    {
        int contractPayments = 0;
        terminationFeePaid = 0;
        riskSharingFeesPaid = 0;
        foreach (Contract contract in contractManager.GetMyContracts().Values)
        {
            if (contract.GetContractState() == ContractState.Completed)
            {
                contractPayments += contract.GetContractPrice();
                riskSharingFeesPaid += contract.GetRiskSharingFeePaid();
            }
            if(contract.GetContractState() == ContractState.Terminated)
            {
                terminationFeePaid += contract.GetTerminationFee();
            }
        }
        revenue = contractPayments;
        ComputeEndCashBalance();
    }

    [Server]
    public void ComputeEndCashBalance()
    {
        int endCash = 0;
        endCash = beginningCashBalance - salaries + revenue - riskSharingFeesPaid - terminationFeePaid - marketingResearch - repayEmergencyLoan;
        endCashBalance = endCash;
    }


    public void ChangeEmergencyLoanRepay(int repayEmergencyLoan)
    {
        CmdChangeEmergencyLoanRepay(repayEmergencyLoan);
    }
    [Command]
    public void CmdChangeEmergencyLoanRepay(int repayEmergencyLoan)
    {
        this.repayEmergencyLoan = repayEmergencyLoan;
        ComputeEndCashBalance();
    }


    public void ChangeMarketingResearchServer(int price)
    {
        marketingResearch = price;

        ComputeEndCashBalance();
    }


    //--------------HOOKS-----------------------------
    public void OnChangeBeginningCashBalance(int beginningCashBalance)
    {
        this.beginningCashBalance = beginningCashBalance;
        if (developerAccountingUIHandlerCurrent != null)
        {
            developerAccountingUIHandlerCurrent.UpdateBeginingCashBalanceText(this.beginningCashBalance);
        }
    }
    public void OnChangeRevenue(int revenue)
    {
        this.revenue = revenue;
        if (developerAccountingUIHandlerCurrent != null)
        {
            developerAccountingUIHandlerCurrent.UpdateRevenueText(this.revenue);
        }
    }
    public void OnChangeSalaries(int salaries)
    {
        this.salaries = salaries;
        if (developerAccountingUIHandlerCurrent != null)
        {
            developerAccountingUIHandlerCurrent.UpdateSalariesText(this.salaries);
        }
    }
    public void OnChangeEndCashBalance(int endCashBalance)
    {
        this.endCashBalance = endCashBalance;
        if (developerAccountingUIHandlerCurrent != null)
        {
            developerAccountingUIHandlerCurrent.UpdateEndCashBalanceText(this.endCashBalance);
        }
    }

    public void OnChangeProgrammersSalaries(int programmersSalaries)
    {
        this.programmersSalaries = programmersSalaries;
        if (developerAccountingUIHandlerCurrent != null)
        {
            developerAccountingUIHandlerCurrent.UpdateProgrammersSalariesText(this.programmersSalaries);
        }
    }
    public void OnChangeUISpecialistsSalaries(int uiSpecialistsSalaries)
    {
        this.uiSpecialistsSalaries = uiSpecialistsSalaries;
        if (developerAccountingUIHandlerCurrent != null)
        {
            developerAccountingUIHandlerCurrent.UpdateUISpecialistsSalariesText(this.uiSpecialistsSalaries);
        }
    }
    public void OnChangeIntegrabilitySpecialistsSalaries(int integrabilitySpecialistsSalaries)
    {
        this.integrabilitySpecialistsSalaries = integrabilitySpecialistsSalaries;
        if (developerAccountingUIHandlerCurrent != null)
        {
            developerAccountingUIHandlerCurrent.UpdateIntegrabilitySpecialistsSalariesText(this.integrabilitySpecialistsSalaries);
        }
    }

    public void OnChangeRiskSharingFeesPaid(int riskSharingFeesPaid)
    {
        this.riskSharingFeesPaid = riskSharingFeesPaid;
        if(developerAccountingUIHandlerCurrent != null)
        {
            developerAccountingUIHandlerCurrent.UpdateRiskSharingFeePaid(this.riskSharingFeesPaid);
        }
    }
    public void OnChangeTerminationFeePaid(int terminationFeePaid)
    {
        this.terminationFeePaid = terminationFeePaid;
        if (developerAccountingUIHandlerCurrent != null)
        {
            developerAccountingUIHandlerCurrent.UpdateTerminationFeepaid(terminationFeePaid);
        }

    }
    public void OnChangeMarketingResearch(int marketingResearch)
    {
        this.marketingResearch = marketingResearch;
        if(developerAccountingUIHandlerCurrent != null)
        {
            developerAccountingUIHandlerCurrent.UpdateMarketingResearch(this.marketingResearch);
        }
    }


    public void OnChangeBorrowEmergencyLoan(int borrowEmergencyLoan)
    {
        this.borrowEmergencyLoan = borrowEmergencyLoan;
        if (developerAccountingUIHandlerCurrent != null)
        {
            developerAccountingUIHandlerCurrent.UpdateBorrowEmergencyLoan(this.borrowEmergencyLoan);
        }
    }
    public void OnChangeRepayEmergencyLoan(int repayEmergencyLoan)
    {
        this.repayEmergencyLoan = repayEmergencyLoan;
        if (developerAccountingUIHandlerCurrent != null)
        {
            developerAccountingUIHandlerCurrent.UpdateRepayEmergencyLoan(this.repayEmergencyLoan);
        }
    }

    /*public void OnChangeEmergencyLoanInterests(int emergencyLoanInterests)
    {
        this.emergencyLoanInterests = emergencyLoanInterests;
        if(developerAccountingUIHandlerCurrent != null)
        {
            developerAccountingUIHandlerCurrent.UpdateEmergencyLoanInterests(emergencyLoanInterests);
        }
    }
    public void OnChangeEmergencyLoanBase(int emergencyLoanBase)
    {
        this.emergencyLoanBase = emergencyLoanBase;
        if (developerAccountingUIHandlerCurrent != null)
        {
            //not implementeed yet
        }
    }*/

    public void OnChangeHistorySaved(bool historySaved)
    {
        Debug.Log("history changed on " + historySaved);
        if (historySaved  && hasAuthority)
        {
            playerData.CmdMoveToNextQuarter();
        }
    }

    // NEXT QUARTER EVALUATION METHODS...

    [Server]
    public void UpdateCurrentQuarterDataServer()
    {
        UpdateRealRevenueServer();
        UpdateSalariesServer();
    }

    [Server]
    public void MoveToNextQuarter()
    {
        SetNewReferencesServer();
        LoadNextQuarterData(currentQuarter);
    }
        
    [Server]
    public void SaveCurrentQuarterDataServer()
    {
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();

        Debug.Log("prov. Acc. saving data " + beginningCashBalance + " , " + riskSharingFeesPaid + " , " + uiSpecialistsSalaries + " , " + endCashBalance);
        beginningCashBalanceQuarters.Insert(currentQuarter, beginningCashBalance);
        revenueQuarters.Insert(currentQuarter, revenue);
        salariesQuarters.Insert(currentQuarter, salaries);
        programmersSalariesQuarters.Insert(currentQuarter, programmersSalaries);
        uiSpecialistsSalariesQuarters.Insert(currentQuarter, uiSpecialistsSalaries);
        integrabilitySpecialistsSalariesQuarters.Insert(currentQuarter, integrabilitySpecialistsSalaries);
        riskSharingFeesPaidQuarters.Insert(currentQuarter, riskSharingFeesPaid);
        terminationFeePaidQuarters.Insert(currentQuarter, terminationFeePaid);

        marketingResearchQuarters.Insert(currentQuarter, marketingResearch);

        repayEmergencyLoanQuarters.Insert(currentQuarter, repayEmergencyLoan);
        borrowEmergencyLoanQuarters.Insert(currentQuarter, borrowEmergencyLoan);
       // emergencyLoanBaseQuarters.Insert(currentQuarter, emergencyLoanBase);
        //emergencyLoanInterestsQuarters.Insert(currentQuarter, emergencyLoanInterests);

        endCashBalanceQuarters.Insert(currentQuarter, endCashBalance);
        historySaved = true;
    }

    [Server]
    public void SetNewReferencesServer()
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
        if (developerAccountingUIHandler != null)
        {
            developerAccountingUIHandler.SetReferences(currentQuarter + 1);
        }
    }

    [Server]
    public void LoadNextQuarterData(int currentQuarter)
    {
        Debug.Log("loading new data");
        LoadQuarterData(currentQuarter + 1);

    }

    private void Start()
    {

    }
}
