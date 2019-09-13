using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DeveloperAccountingManager : NetworkBehaviour
{
    //VARIABLES
    [SyncVar (hook = "OnChangeBeginningCashBalance")]
    private int beginningCashBalance;
    [SyncVar (hook = "OnChangeRevenue")]
    private int revenue;
    [SyncVar(hook = "OnChangeSalaries")]
    private int salaries;
    [SyncVar(hook ="OnChangeProgrammersSalaries")]
    private int programmersSalaries;
    [SyncVar(hook ="OnChangeUISpecialistsSalaries")]
    private int uiSpecialistsSalaries;
    [SyncVar(hook ="OnChangeIntegrabilitySpecialistsSalaries")]
    private int integrabilitySpecialistsSalaries;
    [SyncVar (hook = "OnChangeEndCashBalance")]
    private int endCashBalance;

    private HumanResourcesManager humanResourcesManager;
    private ContractManager contractManager;
    private DeveloperAccountingUIHandler developerAccountingUIHandler;

    //GETTERS & SETTERS
    public int GetBeginningCashBalance() { return beginningCashBalance; }
    public int GetRevenue() { return revenue; }
    public int GetSalaries() { return salaries; }
    public int GetProgrammersSalaries() { return programmersSalaries; }
    public int GetUISpecialistsSalaries() { return uiSpecialistsSalaries; }
    public int GetIntegrabilitySpecialistsSalaries() { return integrabilitySpecialistsSalaries; }
    public int GetEndCashBalance() { return endCashBalance; }
    public void SetDeveloperAccountingUIHandler(DeveloperAccountingUIHandler developerAccountingUIHandler) { this.developerAccountingUIHandler = developerAccountingUIHandler; }

    // Start is called before the first frame update
   
    public override void OnStartServer()
    {
        humanResourcesManager = this.gameObject.GetComponent<HumanResourcesManager>();
        contractManager = this.gameObject.GetComponent<ContractManager>();
        beginningCashBalance = 1000000;

    }

    public override void OnStartClient()
    {
        humanResourcesManager = this.gameObject.GetComponent<HumanResourcesManager>();
        contractManager = this.gameObject.GetComponent<ContractManager>();
    }

    public override void OnStartAuthority()
    {
        ComputeSalaries();
    }

    //METHODS
    public void ComputeSalaries()
    {
        if (hasAuthority)
        {
           CmdComputeSalaries();
            
        }
    }

    [Command]
    public void CmdComputeSalaries()
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
    }

    //--------------HOOKS-----------------------------
    public void OnChangeBeginningCashBalance(int beginningCashBalance)
    {
        this.beginningCashBalance = beginningCashBalance;
        if (developerAccountingUIHandler != null)
        {
            developerAccountingUIHandler.UpdateBeginingCashBalanceText(this.beginningCashBalance);
        }
    }
    public void OnChangeRevenue(int revenue)
    {
        this.revenue = revenue;
        if (developerAccountingUIHandler != null)
        {
            developerAccountingUIHandler.UpdateBeginingCashBalanceText(this.beginningCashBalance);
        }
    }
    public void OnChangeSalaries(int salaries)
    {
        this.salaries = salaries;
        if (developerAccountingUIHandler != null)
        {
            developerAccountingUIHandler.UpdateSalariesText(this.salaries);
        }
    }
    public void OnChangeEndCashBalance(int endCashBalance)
    {
        this.endCashBalance = endCashBalance;
        if (developerAccountingUIHandler != null)
        {
            developerAccountingUIHandler.UpdateEndCashBalanceText(this.endCashBalance);
        }
    }

    public void OnChangeProgrammersSalaries(int programmersSalaries)
    {
        this.programmersSalaries = programmersSalaries;
        if (developerAccountingUIHandler != null)
        {
            developerAccountingUIHandler.UpdateProgrammersSalariesText(this.programmersSalaries);
            developerAccountingUIHandler.UpdateSalariesText(this.salaries);
        }
    }

    public void OnChangeUISpecialistsSalaries(int uiSpecialistsSalaries)
    {
        this.uiSpecialistsSalaries = uiSpecialistsSalaries;
        if (developerAccountingUIHandler != null)
        {
            developerAccountingUIHandler.UpdateUISpecialistsSalariesText(this.uiSpecialistsSalaries);
            developerAccountingUIHandler.UpdateSalariesText(this.salaries);
        }
    }

    public void OnChangeIntegrabilitySpecialistsSalaries(int integrabilitySpecialistsSalaries)
    {
        this.integrabilitySpecialistsSalaries = integrabilitySpecialistsSalaries;
        if (developerAccountingUIHandler != null)
        {
            developerAccountingUIHandler.UpdateIntegrabilitySpecialistsSalariesText(this.integrabilitySpecialistsSalaries);
            developerAccountingUIHandler.UpdateSalariesText(this.salaries);
        }
    }


}
