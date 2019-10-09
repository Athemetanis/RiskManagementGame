using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class SyncListInt : SyncList<int> { }

public class DeveloperAccountingManager : NetworkBehaviour
{
    //VARIABLES

    //STORED VALUES FOR Q0, Q1, Q2, Q3, Q4, on corresponding indexes  // Q0 are default values when the game begins. 
    private SyncListInt beginningCashBalanceQuarters = new SyncListInt() { 0 };
    private SyncListInt revenueQuarters = new SyncListInt() { 0 };
    private SyncListInt salariesQuarters = new SyncListInt() { 0 };
    private SyncListInt programmersSalariesQuarters = new SyncListInt() { 0 };
    private SyncListInt uiSpecialistsSalariesQuarters = new SyncListInt() { 0 };
    private SyncListInt integrabilitySpecialistsSalariesQuarters = new SyncListInt() { 0 };
    private SyncListInt riskSharingFeesPaidQuarters = new SyncListInt() { 0 };
    private SyncListInt marketingResearchQuarters = new SyncListInt() { 0 };
    private SyncListInt borrowEmergencyLoanQuarters = new SyncListInt() { 0 };
    private SyncListInt repayEmergencyLoanQuarters = new SyncListInt() { 0 };
    private SyncListInt endCashBalanceQuarters = new SyncListInt() { 2000000 };

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

    private DeveloperAccountingUIComponentHandler DeveloperAccountingUIHandlerCurrent;

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

    public void SetCurrentDeveloperAccountingUIHandler(DeveloperAccountingUIComponentHandler currentDeveloperAccountingUIHandler) { this.DeveloperAccountingUIHandlerCurrent = currentDeveloperAccountingUIHandler; }

    // Start is called before the first frame update
    public override void OnStartServer()
    {
        gameID = this.gameObject.GetComponent<PlayerData>().GetGameID();
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        humanResourcesManager = this.gameObject.GetComponent<HumanResourcesManager>();
        contractManager = this.gameObject.GetComponent<ContractManager>();
        LoadQuarterData(currentQuarter);
    }

    public override void OnStartClient()
    {
        humanResourcesManager = this.gameObject.GetComponent<HumanResourcesManager>();
        contractManager = this.gameObject.GetComponent<ContractManager>();
    }

    public override void OnStartAuthority()
    {
        UpdateSalaries();
        UpdateRevenue();
    }

    //METHODS
    [Server]
    public void SetupDefaultValues()
    {
        beginningCashBalanceQuarters[0] = 0;
        revenueQuarters[0] = 0;
        salariesQuarters[0] = 0;
        programmersSalariesQuarters[0] = 0;
        uiSpecialistsSalariesQuarters[0] = 0;
        integrabilitySpecialistsSalariesQuarters[0] = 0;
        riskSharingFeesPaidQuarters[0] = 0;
        marketingResearchQuarters[0] = 0;
        borrowEmergencyLoanQuarters[0] = 0;
        repayEmergencyLoanQuarters[0] = 0;
        endCashBalanceQuarters[0] = 2000000;
    }

    [Server]
    public void LoadQuarterData(int quarter)  //loading only values that are transfered to the next quarter  --------SERVER--------
    {
        if (endCashBalanceQuarters.Count != quarter) //true = player joined already running game - skipped some quarters
        {
            for (int i = endCashBalanceQuarters.Count + 1; i < quarter; i++)  //values for skipped quarters are generated
            {
                beginningCashBalanceQuarters[i] = endCashBalanceQuarters[i - 1];
                revenueQuarters[i] = 0;
                salariesQuarters[i] = salariesQuarters[i - 1];
                programmersSalariesQuarters[i] = programmersSalariesQuarters[i - 1];
                uiSpecialistsSalariesQuarters[i] = uiSpecialistsSalariesQuarters[i - 1];
                integrabilitySpecialistsSalariesQuarters[i] = integrabilitySpecialistsSalariesQuarters[i - 1];
                riskSharingFeesPaidQuarters[i] = 0;
                marketingResearchQuarters[i] = 0;
                borrowEmergencyLoanQuarters[i] = 0;
                repayEmergencyLoanQuarters[i] = 0;
                endCashBalanceQuarters[i] = beginningCashBalanceQuarters[i] - salariesQuarters[i];
            }
        }
        beginningCashBalance = endCashBalanceQuarters[quarter - 1]; 
    }

    public (int beginningCashBalance, int revenue, int salaries, int programmersSalaries, int uiSpecialistsSalaries, int integrabilitySpecialistsSalaries, int marketingResearch, int borrowEmergencyLoan, int repayEmergencyLoan, int endCashBalance) GetCorrecpondingQuarterData(int correspondingQuarter)
    {
        return (beginningCashBalanceQuarters[correspondingQuarter], revenueQuarters[correspondingQuarter], salariesQuarters[correspondingQuarter], programmersSalariesQuarters[correspondingQuarter], uiSpecialistsSalariesQuarters[correspondingQuarter], integrabilitySpecialistsSalariesQuarters[correspondingQuarter], marketingResearchQuarters[correspondingQuarter],borrowEmergencyLoanQuarters[correspondingQuarter],repayEmergencyLoanQuarters[correspondingQuarter], endCashBalanceQuarters[correspondingQuarter]);
    }

    public void UpdateSalaries()
    {
        if (hasAuthority)
        {
           CmdUpdateSalaries();
        }
    }

    [Command]
    public void CmdUpdateSalaries()
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

    public void UpdateRevenue()
    {
        CmdUpdateRevenue();
    }
    [Command]
    public void CmdUpdateRevenue()
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
        if (DeveloperAccountingUIHandlerCurrent != null)
        {
            DeveloperAccountingUIHandlerCurrent.UpdateBeginingCashBalanceText(this.beginningCashBalance);
        }
    }
    public void OnChangeRevenue(int revenue)
    {
        this.revenue = revenue;
        if (DeveloperAccountingUIHandlerCurrent != null)
        {
            DeveloperAccountingUIHandlerCurrent.UpdateRevenueText(this.revenue);
        }
    }
    public void OnChangeSalaries(int salaries)
    {
        this.salaries = salaries;
        if (DeveloperAccountingUIHandlerCurrent != null)
        {
            DeveloperAccountingUIHandlerCurrent.UpdateSalariesText(this.salaries);
        }
    }
    public void OnChangeEndCashBalance(int endCashBalance)
    {
        this.endCashBalance = endCashBalance;
        if (DeveloperAccountingUIHandlerCurrent != null)
        {
            DeveloperAccountingUIHandlerCurrent.UpdateEndCashBalanceText(this.endCashBalance);
        }
    }

    public void OnChangeProgrammersSalaries(int programmersSalaries)
    {
        this.programmersSalaries = programmersSalaries;
        if (DeveloperAccountingUIHandlerCurrent != null)
        {
            DeveloperAccountingUIHandlerCurrent.UpdateProgrammersSalariesText(this.programmersSalaries);
            DeveloperAccountingUIHandlerCurrent.UpdateSalariesText(this.salaries);
        }
    }
    public void OnChangeUISpecialistsSalaries(int uiSpecialistsSalaries)
    {
        this.uiSpecialistsSalaries = uiSpecialistsSalaries;
        if (DeveloperAccountingUIHandlerCurrent != null)
        {
            DeveloperAccountingUIHandlerCurrent.UpdateUISpecialistsSalariesText(this.uiSpecialistsSalaries);
            DeveloperAccountingUIHandlerCurrent.UpdateSalariesText(this.salaries);
        }
    }
    public void OnChangeIntegrabilitySpecialistsSalaries(int integrabilitySpecialistsSalaries)
    {
        this.integrabilitySpecialistsSalaries = integrabilitySpecialistsSalaries;
        if (DeveloperAccountingUIHandlerCurrent != null)
        {
            DeveloperAccountingUIHandlerCurrent.UpdateIntegrabilitySpecialistsSalariesText(this.integrabilitySpecialistsSalaries);
            DeveloperAccountingUIHandlerCurrent.UpdateSalariesText(this.salaries);
        }
    }

    public void OnChangeRiskSharingFeesPaid(int riskSharingFeesPaid)
    {
        this.riskSharingFeesPaid = riskSharingFeesPaid;
    }
    public void OnChangeMarketingResearch(int marketingResearch)
    {
        this.marketingResearch = marketingResearch;
    }
    public void OnChangeBorrowEmergencyLoan(int borrowEmergencyLoan)
    {
        this.borrowEmergencyLoan = borrowEmergencyLoan;
    }
    public void OnChangeRepayEmergencyLoan(int repayEmergencyLoan)
    {
        this.repayEmergencyLoan = repayEmergencyLoan;
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

        beginningCashBalanceQuarters[currentQuarter] = beginningCashBalance;
        revenueQuarters[currentQuarter] = revenue;
        salariesQuarters[currentQuarter] = salaries;
        programmersSalariesQuarters[currentQuarter] = programmersSalaries;
        uiSpecialistsSalariesQuarters[currentQuarter] = uiSpecialistsSalaries;
        integrabilitySpecialistsSalariesQuarters[currentQuarter] = integrabilitySpecialistsSalaries;
        riskSharingFeesPaidQuarters[currentQuarter] = riskSharingFeesPaid;
        marketingResearchQuarters[currentQuarter] = marketingResearch;
        borrowEmergencyLoanQuarters[currentQuarter] = borrowEmergencyLoan;
        repayEmergencyLoanQuarters[currentQuarter] = repayEmergencyLoan;
        endCashBalanceQuarters[currentQuarter] = endCashBalance;
    }

    [Command]
    public void CmdSetNewReferences()
    {
        RpcSetNewReferences();
    }
    [ClientRpc]
    public void RpcSetNewReferences()
    {
        if(DeveloperAccountingUIHandlerCurrent != null)
        {   
            if(currentQuarter + 1 == 2)
            {
                DeveloperAccountingUIHandlerCurrent = developerAccountingUIHandlerQ2;
                developerAccountingUIHandlerQ2.gameObject.SetActive(true);
            }
            if (currentQuarter + 1 == 3)
            {
                DeveloperAccountingUIHandlerCurrent = developerAccountingUIHandlerQ3;
                developerAccountingUIHandlerQ3.gameObject.SetActive(true);
            }
            if (currentQuarter + 1 == 4)
            {
                DeveloperAccountingUIHandlerCurrent = developerAccountingUIHandlerQ4;
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
            DeveloperAccountingUIHandlerCurrent = developerAccountingUIHandlerQ2;
        }
        if (currentQuarter + 1 == 3)
        {
            DeveloperAccountingUIHandlerCurrent = developerAccountingUIHandlerQ3;
        }
        if (currentQuarter + 1 == 4)
        {
            DeveloperAccountingUIHandlerCurrent = developerAccountingUIHandlerQ4;
        }

    }

   

}
