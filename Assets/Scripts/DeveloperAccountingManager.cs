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
    public void SetDeveloperAccountingUIHandler(DeveloperAccountingUIHandler developerAccountingUIHandler) { this.developerAccountingUIHandler = developerAccountingUIHandler; }

    // Start is called before the first frame update
    void Start()
    {
        humanResourcesManager = this.gameObject.GetComponent<HumanResourcesManager>();
        contractManager = this.gameObject.GetComponent<ContractManager>();
    }

    public void ComputeSalaries()
    {
        CmdComputeSalaries();
    }

    [Command]
    public void CmdComputeSalaries()
    {
        int programmers = humanResourcesManager.GetProgrammersCount();
        int uiSpecialists = humanResourcesManager.GetUISPecialistsCount();
        int integrabilitySpecialists = humanResourcesManager.GetIntegrabilitySpecialistsCount();
    }

    //--------------HOOKS-----------------------------
    public void OnChangeBeginningCashBalance(int beginningCashBalance)
    {
        this.beginningCashBalance = beginningCashBalance;
    }
    public void OnChangeRevenue(int revenue)
    {
        this.revenue = revenue;
    }
    public void OnChangeSalaries(int salaries)
    {
        this.salaries = salaries;
    }
    public void OnChangeEndCashBalance(int endCashBalance)
    {
        this.endCashBalance = endCashBalance;
    }

    public void OnChangeProgrammersSalaries(int programmersSalaries)
    {
        this.programmersSalaries = programmersSalaries;
    }

    public void OnChangeUISpecialistsSalaries(int uiSpecialistsSalaries)
    {
        this.uiSpecialistsSalaries = uiSpecialistsSalaries;
    }

    public void OnChangeIntegrabilitySpecialistsSalaries(int integrabilitySpecialistsSalaries)
    {
        this.integrabilitySpecialistsSalaries = integrabilitySpecialistsSalaries;
    }


}
