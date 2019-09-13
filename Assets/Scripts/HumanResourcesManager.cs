using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HumanResourcesManager : NetworkBehaviour
{
    //VARIABLES
    [SyncVar(hook = "OnChangeProgrammersCount")]
    private int programmersCurrentCount;
    [SyncVar(hook = "OnChangeUISpecialistsCount")]
    private int userInterfaceSpecialistsCurrentCount;
    [SyncVar(hook = "OnChangeIntegrabilitySpecialistsCount")]
    private int integrabilitySpecialistsCurrentCount;
    [SyncVar(hook = "OnChangeProgrammersAvailableCount")]
    private int programmersAvailableCount;
    [SyncVar(hook = "OnChangeSpecialistsAvailableCount")]
    private int specialistsAvailableCount;
    [SyncVar(hook = "OnChangeHireProgrammersCount")]
    private int hireProgrammersCount;
    [SyncVar(hook = "OnChangeHireUISpecialistsCount")]
    private int hireUISpecialistsCount;
    [SyncVar(hook = "OnChangeHireIntegrabilitySpecialistsCount")]
    private int hireIntegrabilitySpecialistsCount;

    [SyncVar(hook = "OnChangeProgrammerSalary")]
    private int programmerSalary;
    [SyncVar(hook = "OnChangeUISpecialistSalary")]
    private int uiSpecialistSalary;
    [SyncVar(hook = "OnChangeIntegrabilitySpecialistSalary")]
    private int integrabilitySpecialistSalary;




    [SyncVar(hook = "OnChangeQASpecialistsCount")]
    private int qualityAssuranceSpecialistsCurrentCount;

    private HumanResourcesUIHandler humanResourcesUIHandler;
    private DeveloperAccountingManager developerAccountingManager;
    private ScheduleManager scheduleManager;

    //GETTERS & SETTERS
    public void SetProgrammersCount(int count) { programmersCurrentCount = count; }
    public int GetProgrammersCount() { return programmersCurrentCount; }
    public void SetUISpecialistsCount(int count) { userInterfaceSpecialistsCurrentCount = count; }
    public int GetUISPecialistsCount() { return userInterfaceSpecialistsCurrentCount; }
    public void SetIntegrabilitySpecialistsCount(int count) { integrabilitySpecialistsCurrentCount = count; }
    public int GetIntegrabilitySpecialistsCount() { return integrabilitySpecialistsCurrentCount; }
    public void SetQASpecialistsCount(int count) { qualityAssuranceSpecialistsCurrentCount = count; }
    public int GetQASpecialistsCount() { return qualityAssuranceSpecialistsCurrentCount; }
    public void SetHumanResourcesUIHandler(HumanResourcesUIHandler humanResourcesUIHandler) { this.humanResourcesUIHandler = humanResourcesUIHandler; }
    public int GetProgrammerSalary() { return programmerSalary; }
    public int GetUISpecialistSalary() { return uiSpecialistSalary; }
    public int GetIntegrabilitySpecialistSalary() { return integrabilitySpecialistSalary; }
    public int GetProgrammersAvailableCount() { return programmersAvailableCount; }
    public int GetSpecialistsAvailableCount() { return specialistsAvailableCount; }

    private void Awake()
    {
       developerAccountingManager = this.gameObject.GetComponent<DeveloperAccountingManager>();
        /*Debug.LogWarning("AWAKE snazim sa ziskat developerAccounting Manager s vysledkom: " );
        Debug.LogWarning(developerAccountingManager == null);
        */
        
    }
    public override void OnStartServer()
    {
        scheduleManager = this.gameObject.GetComponent<ScheduleManager>();
        developerAccountingManager = this.gameObject.GetComponent<DeveloperAccountingManager>();
        programmersCurrentCount = 10;
        userInterfaceSpecialistsCurrentCount = 1;
        integrabilitySpecialistsCurrentCount = 1;
        programmersAvailableCount = 5;
        specialistsAvailableCount = 2;
        programmerSalary = 30000;
        uiSpecialistSalary = 40000;
        integrabilitySpecialistSalary = 40000;

    }

    public override void OnStartClient()
    {
        scheduleManager = this.gameObject.GetComponent<ScheduleManager>();
        developerAccountingManager = this.gameObject.GetComponent<DeveloperAccountingManager>();
    }
   

    public void AddProgramer()
    {
        CmdAddProgrammer();
    }
    public void AddUISpecialist()
    {
        CmdAddUISpecialist();
    }
    public void AddIntegrabilitySpecialist()
    {
        CmdAddIntegrabilitySpecialist();
    }
    public void SubstractProgrammer()
    {
        CmdSubstractProgrammer();
    }
    public void SubstractUISpecialist()
    {
        CmdSubstractUISpecialist();
    }
    public void SubstarctIntegrabilitySpecialist()
    {
        CmdSubstractIntegrabilitySpecialist();
    }

    [Command]
    public void CmdAddProgrammer()
    {
        if (programmersAvailableCount > 0)
        {
            programmersCurrentCount++;
            programmersAvailableCount--;
        }
    }
    [Command]
    public void CmdSubstractProgrammer()
    { if (programmersCurrentCount > 0)
        {
            programmersCurrentCount--;
            programmersAvailableCount++;
        }
    }
    [Command]
    public void CmdAddUISpecialist()
    {
        if (specialistsAvailableCount > 0)
        {
            userInterfaceSpecialistsCurrentCount++;
            specialistsAvailableCount--;
        }
    }
    [Command]
    public void CmdAddIntegrabilitySpecialist()
    {
        if (specialistsAvailableCount > 0)
        {
            integrabilitySpecialistsCurrentCount++;
            specialistsAvailableCount--;
        }
    }
    [Command]
    public void CmdSubstractUISpecialist()
    {
        if (userInterfaceSpecialistsCurrentCount > 0)
        {
            userInterfaceSpecialistsCurrentCount--;
            specialistsAvailableCount++;
        }
    }
    [Command]
    public void CmdSubstractIntegrabilitySpecialist()
    {
        if (integrabilitySpecialistsCurrentCount > 0)
        {
            integrabilitySpecialistsCurrentCount--;
            specialistsAvailableCount++;
        }
    }

    public void HireProgrammersNextQuarter(int hireProgrammersCount)
    {
        this.hireProgrammersCount = hireProgrammersCount;
    }
    public void HireUISPecialistsNextQuarter(int hireUISpecialistsCount)
    {
        this.hireUISpecialistsCount = hireUISpecialistsCount;
    }
    public void HireIntegrabilitySpecialistsNextQuarter(int hireIntegrabilitySpecialistsCount)
    {
        this.hireIntegrabilitySpecialistsCount = hireIntegrabilitySpecialistsCount;
    }

    public void ChangeProgrammmerSalary(int programmerSalary) { CmdChangeProgrammerSalary(programmerSalary); }
    public void ChangeUISpecialistSalary(int uiSpecialistSalary) { CmdChangeUISpecialistSalary(uiSpecialistSalary); }
    public void ChangeIntegrabilitySpecialistSalary(int integrabilitySpecialistSalary) { CmdChangeIntegrabilitySpecialistSalary(integrabilitySpecialistSalary); }

    [Command]
    public void CmdChangeProgrammerSalary(int programmerSalary) { this.programmerSalary = programmerSalary; }
    [Command]
    public void CmdChangeUISpecialistSalary(int uiSpecialistSalary) { this.uiSpecialistSalary = uiSpecialistSalary; }
    [Command]
    public void CmdChangeIntegrabilitySpecialistSalary(int integrabilitySpecialistSalary) { this.integrabilitySpecialistSalary = integrabilitySpecialistSalary; }


    //-----------HOOKS--------------
    public void OnChangeProgrammersCount(int programmersCount)
    {
        this.programmersCurrentCount = programmersCount;
        developerAccountingManager.ComputeSalaries();
        if (this.humanResourcesUIHandler != null)
        {
            humanResourcesUIHandler.UpdateProgramersCurrentCountText(this.programmersCurrentCount);
        }
        if(this.scheduleManager != null)
        {
            scheduleManager.UpdateAllFeatureGraphs();
        }
    }
    public void OnChangeUISpecialistsCount(int UISpecialistsCount)
    {
        this.userInterfaceSpecialistsCurrentCount = UISpecialistsCount;
        developerAccountingManager.ComputeSalaries();
        if (this.humanResourcesUIHandler != null)
        {
            humanResourcesUIHandler.UpdateUserInterfaceSpecialistsCurrentCountText(this.userInterfaceSpecialistsCurrentCount);
        }
        if (this.scheduleManager != null)
        {
            scheduleManager.UpdateAllFeatureGraphs();
        }
    }
    public void OnChangeIntegrabilitySpecialistsCount(int integrabilitySpecialistsCount)
    {
        this.integrabilitySpecialistsCurrentCount = integrabilitySpecialistsCount;
        developerAccountingManager.ComputeSalaries();
        if (this.humanResourcesUIHandler != null)
        {
            humanResourcesUIHandler.UpdateIntegrabilitySpecialistsCurrentCountText(this.integrabilitySpecialistsCurrentCount);
        }
        if (this.scheduleManager != null)
        {
            scheduleManager.UpdateAllFeatureGraphs();
        }
    }
    public void OnChangeProgrammersAvailableCount(int programmersAvailableCount)
    {
        this.programmersAvailableCount = programmersAvailableCount;
        if (this.humanResourcesUIHandler != null)
        {
            humanResourcesUIHandler.UpdateProgramersAvailableCountText(this.programmersAvailableCount);
        }
    }
    public void OnChangeSpecialistsAvailableCount(int specialistsAvailableCount)
    {
        this.specialistsAvailableCount = specialistsAvailableCount;
        if (this.humanResourcesUIHandler != null)
        {
            humanResourcesUIHandler.UpdateSpecialistsAvailableCountText(this.specialistsAvailableCount);
        }
    }
    public void OnChangeQASpecialistsCount(int qualityAssuranceSpecialistsCount)
    {
        this.qualityAssuranceSpecialistsCurrentCount = qualityAssuranceSpecialistsCount;
        if (this.humanResourcesUIHandler != null)
        {
            if (this.qualityAssuranceSpecialistsCurrentCount == 0 )
            {


            }
        }
    }
    public void OnChangeHireProgrammersCount(int hireProgrammersCount)
    {
        this.hireProgrammersCount = hireProgrammersCount;
        if(humanResourcesUIHandler != null)
        {
            humanResourcesUIHandler.UpdateHireProgrammersCount(this.hireProgrammersCount);
        }
    }
    public void OnChangeHireUISpecialistsCount(int hireUISpecialistsCount)
    {
        this.hireUISpecialistsCount = hireUISpecialistsCount;
        if (humanResourcesUIHandler != null)
        {
            humanResourcesUIHandler.UpdateHireUISpecialistsCount(this.hireUISpecialistsCount);
        }
    }
    public void OnChangeHireIntegrabilitySpecialistsCount(int hireIntegrabilitySpecialistsCount)
    {
        this.hireIntegrabilitySpecialistsCount = hireIntegrabilitySpecialistsCount;
        if (humanResourcesUIHandler != null)
        {
            humanResourcesUIHandler.UpdateHireIntegrabilitySpecialistsCount(this.hireIntegrabilitySpecialistsCount);
        }
    }
    public void OnChangeProgrammerSalary (int programmerSalary)
    {
        this.programmerSalary = programmerSalary;
        developerAccountingManager.ComputeSalaries();
        if(humanResourcesUIHandler != null )
        {
            humanResourcesUIHandler.UpdateProgrammerSalarySlider(this.programmerSalary);
        }
    }
    public void OnChangeUISpecialistSalary(int uiSpecialistSalary)
    {
        this.uiSpecialistSalary = uiSpecialistSalary;
        developerAccountingManager.ComputeSalaries();
        if (humanResourcesUIHandler != null)
        {
            humanResourcesUIHandler.UpdateUISpecialistSalarySlider(this.uiSpecialistSalary);
        }
    }
    public void OnChangeIntegrabilitySpecialistSalary(int integrabilitySpecialistSalary)
    {
        this.integrabilitySpecialistSalary = integrabilitySpecialistSalary;
        developerAccountingManager.ComputeSalaries();
        if (humanResourcesUIHandler != null)
        {
            humanResourcesUIHandler.UpdateIntegrabilitySpecialistSalarySlider(this.integrabilitySpecialistSalary);
        }

    }



}
