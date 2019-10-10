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
    private SyncListInt marketingResearchQuarters = new SyncListInt() { };
    private SyncListInt borrowEmergencyLoanQuarters = new SyncListInt() { };
    private SyncListInt repayEmergencyLoanQuarters = new SyncListInt() { };
    private SyncListInt endCashBalanceQuarters = new SyncListInt() { };

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
    [SyncVar(hook = "OnChangeMarketingResearch")]
    private int marketingResearch;
    [SyncVar(hook = "OnChangeBorrowEmergencyLoan")]
    private int borrowEmergencyLoan;
    [SyncVar(hook = "OnChangeRepayEmergencyLoan")]
    private int repayEmergencyLoan;
    [SyncVar(hook = "OnChangeEndCashBalance")]
    private int endCashBalance;

    //REFERNCES
    private string gameID;
    private int currentQuarter;

    private HumanResourcesManager humanResourcesManager;
    private ContractManager contractManager;

    private DeveloperAccountingUIComponentHandler developerAccountingUIHandlerQ1;
    private DeveloperAccountingUIComponentHandler developerAccountingUIHandlerQ2;
    private DeveloperAccountingUIComponentHandler developerAccountingUIHandlerQ3;
    private DeveloperAccountingUIComponentHandler developerAccountingUIHandlerQ4;

    private DeveloperAccountingUIComponentHandler developerAccountingUIHandlerCurrent;

    //GETTERS & SETTERS
    public int GetBeginningCashBalance() { return beginningCashBalance; }
    public int GetRevenue() { return revenue; }
    public int GetSalaries() { return salaries; }
    public int GetProgrammersSalaries() { return programmersSalaries; }
    public int GetUISpecialistsSalaries() { return uiSpecialistsSalaries; }
    public int GetIntegrabilitySpecialistsSalaries() { return integrabilitySpecialistsSalaries; }
    public int GetRishSharingFeesPaid() { return riskSharingFeesPaid; }
    public int GetMarketingResearch() { return marketingResearch; }
    public int GetBorrowEmergencyLoan() { return borrowEmergencyLoan; }
    public int GetRepayEmergencyLoan() { return repayEmergencyLoan; }
    public int GetEndCashBalance() { return endCashBalance; }
    public void SetDeveloperAccountingUIHandlerQ1(DeveloperAccountingUIComponentHandler developerAccountingUIHandler) { this.developerAccountingUIHandlerQ1 = developerAccountingUIHandler; }
    public void SetDeveloperAccountingUIHandlerQ2(DeveloperAccountingUIComponentHandler developerAccountingUIHandler) { this.developerAccountingUIHandlerQ2 = developerAccountingUIHandler; }
    public void SetDeveloperAccountingUIHandlerQ3(DeveloperAccountingUIComponentHandler developerAccountingUIHandler) { this.developerAccountingUIHandlerQ3 = developerAccountingUIHandler; }
    public void SetDeveloperAccountingUIHandlerQ4(DeveloperAccountingUIComponentHandler developerAccountingUIHandler) { this.developerAccountingUIHandlerQ4 = developerAccountingUIHandler; }

    public void SetCurrentDeveloperAccountingUIHandler(DeveloperAccountingUIComponentHandler currentDeveloperAccountingUIHandler) { this.developerAccountingUIHandlerCurrent = currentDeveloperAccountingUIHandler; }

    // Start is called before the first frame update
    public override void OnStartServer()
    {
        gameID = this.gameObject.GetComponent<PlayerData>().GetGameID();
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        humanResourcesManager = this.gameObject.GetComponent<HumanResourcesManager>();
        contractManager = this.gameObject.GetComponent<ContractManager>();

        if(beginningCashBalanceQuarters.Count == 0)
        {
            SetupDefaultValues();
        }
        LoadQuarterData(currentQuarter);
        UpdateSalariesServer();
        UpdateRevenueServer();
    }

    public override void OnStartClient()
    {
        humanResourcesManager = this.gameObject.GetComponent<HumanResourcesManager>();
        contractManager = this.gameObject.GetComponent<ContractManager>();
    }

    /*public override void OnStartAuthority()
    {
        UpdateSalaries();
        UpdateRevenue();
    }*/

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
        marketingResearchQuarters.Insert(0, 0);
        borrowEmergencyLoanQuarters.Insert(0, 0);
        repayEmergencyLoanQuarters.Insert(0, 0);
        endCashBalanceQuarters.Insert(0, 2000000);
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
                marketingResearchQuarters.Insert(i, 0);
                borrowEmergencyLoanQuarters.Insert(i, 0);
                repayEmergencyLoanQuarters.Insert(i, 0);
                endCashBalanceQuarters.Insert(i, beginningCashBalanceQuarters[i] - salariesQuarters[i]);
            }
        }
        beginningCashBalance = endCashBalanceQuarters[quarter - 1]; 
    }

    public (int beginningCashBalance, int revenue, int salaries, int programmersSalaries, int uiSpecialistsSalaries, int integrabilitySpecialistsSalaries, int riskSharingFeePaid, int marketingResearch, int borrowEmergencyLoan, int repayEmergencyLoan, int endCashBalance) GetCorrecpondingQuarterData(int correspondingQuarter)
    {
        return (beginningCashBalanceQuarters[correspondingQuarter], revenueQuarters[correspondingQuarter], salariesQuarters[correspondingQuarter], programmersSalariesQuarters[correspondingQuarter], uiSpecialistsSalariesQuarters[correspondingQuarter], integrabilitySpecialistsSalariesQuarters[correspondingQuarter], riskSharingFeesPaidQuarters[currentQuarter], marketingResearchQuarters[correspondingQuarter],borrowEmergencyLoanQuarters[correspondingQuarter],repayEmergencyLoanQuarters[correspondingQuarter], endCashBalanceQuarters[correspondingQuarter]);
    }

    [Server]
    public void UpdateSalariesServer()
    {
        int programmers = humanResourcesManager.GetProgrammersCount();
        int programmerSalary = humanResourcesManager.GetProgrammerSalary();
        int uiSpecialists = humanResourcesManager.GetUISPecialistsCount();
        int uiSpecialistSalary = humanResourcesManager.GetUISpecialistSalary();
        int integrabilitySpecialists = humanResourcesManager.GetIntegrabilitySpecialistsCount();
        int integrabilitySpecialistSalary = humanResourcesManager.GetIntegrabilitySpecialistSalary();


        programmersSalaries = programmers * programmerSalary;
        uiSpecialistsSalaries = uiSpecialists * uiSpecialistSalary;
        integrabilitySpecialistsSalaries = integrabilitySpecialists * integrabilitySpecialistSalary;
        salaries = programmersSalaries + uiSpecialistsSalaries + integrabilitySpecialistsSalaries;

        endCashBalance = ComputeEndCashBalance();
    }
    [Server]
    public void UpdateRevenueServer()
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

        endCashBalance = ComputeEndCashBalance();
    }
    
    public int ComputeEndCashBalance()
    {
        int endCash = 0;
        endCash = beginningCashBalance - salaries + revenue - riskSharingFeesPaid - marketingResearch;
        return endCash;
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


    // NEXT QUARTER EVALUATION METHODS...
    public void MoveToNextQuarter()
    {
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        CmdSaveCurrentQuarterData(currentQuarter);
        CmdSetNewReferences();
        CmdUpdateCurrentQuarterData();
        
    }

    [Command]
    public void CmdSaveCurrentQuarterData(int currentQuarter)
    {

        beginningCashBalanceQuarters.Insert(currentQuarter, beginningCashBalance);
        revenueQuarters.Insert(currentQuarter, revenue);
        salariesQuarters.Insert(currentQuarter, salaries);
        programmersSalariesQuarters.Insert(currentQuarter, programmersSalaries);
        uiSpecialistsSalariesQuarters.Insert(currentQuarter, uiSpecialistsSalaries);
        integrabilitySpecialistsSalariesQuarters.Insert(currentQuarter, integrabilitySpecialistsSalaries);
        riskSharingFeesPaidQuarters.Insert(currentQuarter, riskSharingFeesPaid);
        marketingResearchQuarters.Insert(currentQuarter, marketingResearch);
        borrowEmergencyLoanQuarters.Insert(currentQuarter, borrowEmergencyLoan);
        repayEmergencyLoanQuarters.Insert(currentQuarter, repayEmergencyLoan);
        endCashBalanceQuarters.Insert(currentQuarter, endCashBalance);
    }

    [Command]
    public void CmdSetNewReferences()
    {
        RpcSetNewReferences();
    }
    [ClientRpc]
    public void RpcSetNewReferences()
    {
        if(developerAccountingUIHandlerCurrent != null)
        {   
            if(currentQuarter + 1 == 2)
            {
                developerAccountingUIHandlerCurrent = developerAccountingUIHandlerQ2;
                developerAccountingUIHandlerQ2.gameObject.SetActive(true);
            }
            if (currentQuarter + 1 == 3)
            {
                developerAccountingUIHandlerCurrent = developerAccountingUIHandlerQ3;
                developerAccountingUIHandlerQ3.gameObject.SetActive(true);
            }
            if (currentQuarter + 1 == 4)
            {
                developerAccountingUIHandlerCurrent = developerAccountingUIHandlerQ4;
                developerAccountingUIHandlerQ4.gameObject.SetActive(true);
            }
        }
    }

    [Command]
    public void CmdUpdateCurrentQuarterData()
    {
        beginningCashBalance = endCashBalance;

        //+ prepocitat nanovo zvysok - nove revnue lebo nemam kontrakt, nove salaries lebo sa moho zmenit pocet zamestnancov, .... 
        if(currentQuarter + 1 == 2)
        {
            developerAccountingUIHandlerCurrent = developerAccountingUIHandlerQ2;
        }
        if (currentQuarter + 1 == 3)
        {
            developerAccountingUIHandlerCurrent = developerAccountingUIHandlerQ3;
        }
        if (currentQuarter + 1 == 4)
        {
            developerAccountingUIHandlerCurrent = developerAccountingUIHandlerQ4;
        }

    }

   

}
