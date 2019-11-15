using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HumanResourcesManager : NetworkBehaviour
{
    //VARIABLES
    private SyncListInt endProgrammersCountQ = new SyncListInt() { };
    private SyncListInt endIntegrabilitySpecialistsCountQ = new SyncListInt() { };
    private SyncListInt endUISpecialistsCountQ = new SyncListInt() { };

    private SyncListInt programmerSalaryQ = new SyncListInt() { };
    private SyncListInt integrabilitySpecialistSalaryQ = new SyncListInt() { };
    private SyncListInt uiSpecialistSalaryQ = new SyncListInt() { };

    [SyncVar(hook = "OnChangeProgrammersCount")]
    private int programmersCurrentCount;
    [SyncVar(hook = "OnChangeUISpecialistsCount")]
    private int userInterfaceSpecialistsCurrentCount;
    [SyncVar(hook = "OnChangeIntegrabilitySpecialistsCount")]
    private int integrabilitySpecialistsCurrentCount;

    [SyncVar(hook = "OnChangeProgrammersAvailableCount")]
    private int programmersAvailableCount;
    [SyncVar(hook = "OnChangeIntegrabilitySpecialistAvailableCount")]
    private int integrabilitySpecialistsAvailableCount;
    [SyncVar(hook = "OnChangeUISpecialistAvailableCount")]
    private int userInterfaceSpecialistsAvailableCount;

    [SyncVar(hook = "OnChangeHireProgrammersCount")]
    private int hireProgrammersCount;
    [SyncVar(hook = "OnChangeHireUISpecialistsCount")]
    private int hireUISpecialistsCount;
    [SyncVar(hook = "OnChangeHireIntegrabilitySpecialistsCount")]
    private int hireIntegrabilitySpecialistsCount;

    [SyncVar(hook = "OnChangeProgrammerSalaryQuarter")]
    private int programmerSalaryPerQuarter;
    [SyncVar(hook = "OnChangeUISpecialistSalaryQuarter")]
    private int uiSpecialistSalaryPerQuarter;
    [SyncVar(hook = "OnChangeIntegrabilitySpecialistSalaryQuarter")]
    private int integrabilitySpecialistSalaryPerQuarter;

    [SyncVar(hook = "OnChangeProgrammerSalaryMonth")]
    private int programmerSalaryPerMonth;
    [SyncVar(hook = "OnChangeUISpecialistSalaryMonth")]
    private int uiSpecialistSalaryPerMonth;
    [SyncVar(hook = "OnChangeIntegrabilitySpecialistSalaryMonth")]
    private int integrabilitySpecialistSalaryPerMonth;


    private string gameID;
    private int currentQuarter;
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
    public void SetHumanResourcesUIHandler(HumanResourcesUIHandler humanResourcesUIHandler) { this.humanResourcesUIHandler = humanResourcesUIHandler; }

    public int GetProgrammerSalaryPerQurter() { return programmerSalaryPerQuarter; }
    public int GetUISpecialistSalaryPerQuarter() { return uiSpecialistSalaryPerQuarter; }
    public int GetIntegrabilitySpecialistSalaryPerQuarter() { return integrabilitySpecialistSalaryPerQuarter; }

    public int GetProgrammerSalaryPerMonth() { return programmerSalaryPerMonth; }
    public int GetUISpecialistSalaryPerMonth() { return uiSpecialistSalaryPerMonth; }
    public int GetIntegrabilitySpecialistSalaryPerMonth() { return integrabilitySpecialistSalaryPerMonth; }

    public int GetProgrammersAvailableCount() { return programmersAvailableCount; }
    public int GetUISpecialsitsAvailableCount() { return userInterfaceSpecialistsAvailableCount; }
    public int GetIntegrabilitySpecialistsAvailableCount() { return integrabilitySpecialistsAvailableCount; }

    public int GetEmployeesCountQuater(int quater) { return endProgrammersCountQ[quater] + endUISpecialistsCountQ[quater] + endIntegrabilitySpecialistsCountQ[quater]; }
    public int GetProgrammersCountQuarter(int quarter) { return endProgrammersCountQ[quarter]; }
    public int GetIntegrabilitySpecialistsCountQuarter(int quarter) { return endIntegrabilitySpecialistsCountQ[quarter]; }
    public int GetUISpecialistsCountQuarter(int quarter) { return endUISpecialistsCountQ[quarter]; }


    public int GetProgrammersSalaryQuarter(int quarter) { return programmerSalaryQ[quarter]; }
    public int GetIntegrabilitySpecialistSalaryQuarter(int quarter) { return integrabilitySpecialistSalaryQ[quarter]; }
    public int GetUISpecialistSalaryQuarter(int quarter) { return uiSpecialistSalaryQ[quarter]; }

    private void Awake()
    {

    }

    public override void OnStartServer()
    {
        gameID = this.gameObject.GetComponent<PlayerData>().GetGameID();
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        developerAccountingManager = this.gameObject.GetComponent<DeveloperAccountingManager>();
        scheduleManager = this.gameObject.GetComponent<ScheduleManager>();
        developerAccountingManager = this.gameObject.GetComponent<DeveloperAccountingManager>();

        if (integrabilitySpecialistsAvailableCount == 0)
        {
            SetupDefaultValues();
        }
        LoadQuarterData(currentQuarter);
    }

    public override void OnStartClient()
    {
        scheduleManager = this.gameObject.GetComponent<ScheduleManager>();
        developerAccountingManager = this.gameObject.GetComponent<DeveloperAccountingManager>();
    }
    [Server]
    public void SetupDefaultValues()
    {
        endProgrammersCountQ.Insert(0, 0);
        endIntegrabilitySpecialistsCountQ.Insert(0, 0);
        endUISpecialistsCountQ.Insert(0, 0);

        programmersCurrentCount = 0;
        userInterfaceSpecialistsCurrentCount = 0;
        integrabilitySpecialistsCurrentCount = 0;
        programmersAvailableCount = 10;
        userInterfaceSpecialistsAvailableCount = 7;
        integrabilitySpecialistsAvailableCount = 7;
        hireProgrammersCount = 0;
        hireIntegrabilitySpecialistsCount = 0;
        hireUISpecialistsCount = 0;

        programmerSalaryPerMonth = 2000;
        uiSpecialistSalaryPerMonth = 3000;
        integrabilitySpecialistSalaryPerMonth = 3000;

        programmerSalaryPerQuarter = programmerSalaryPerMonth * 3;
        uiSpecialistSalaryPerQuarter = uiSpecialistSalaryPerMonth * 3;
        integrabilitySpecialistSalaryPerQuarter = integrabilitySpecialistSalaryPerMonth * 3;

        programmerSalaryQ.Insert(0, 0);
        integrabilitySpecialistSalaryQ.Insert(0, 0);
        uiSpecialistSalaryQ.Insert(0, 0);
    }

    [Server]
    public void LoadQuarterData(int quarter)
    {
        if(endProgrammersCountQ.Count != currentQuarter)
        {
            for(int i = endProgrammersCountQ.Count +1; i < quarter; i++)
            {
                endProgrammersCountQ.Insert(i, endProgrammersCountQ[i - 1]);
                endIntegrabilitySpecialistsCountQ.Insert(i, endIntegrabilitySpecialistsCountQ[i-1]);
                endUISpecialistsCountQ.Insert(i, endUISpecialistsCountQ[i - 1]);

                programmerSalaryQ.Insert(i, programmerSalaryQ[i-1]);
                integrabilitySpecialistSalaryQ.Insert(i, integrabilitySpecialistSalaryQ[i-1]);
                uiSpecialistSalaryQ.Insert(i, uiSpecialistSalaryQ[i-1]);
            }
        }
        programmersCurrentCount = endProgrammersCountQ[quarter - 1];
        integrabilitySpecialistsCurrentCount = endIntegrabilitySpecialistsCountQ[quarter - 1];
        userInterfaceSpecialistsCurrentCount = endUISpecialistsCountQ[quarter - 1];
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
            developerAccountingManager.UpdateSalariesServer();
        }
    }
    [Command]
    public void CmdSubstractProgrammer()
    { if (programmersCurrentCount > 0)
        {
            programmersCurrentCount--;
            programmersAvailableCount++;
            developerAccountingManager.UpdateSalariesServer();
        }
    }
    [Command]
    public void CmdAddUISpecialist()
    {
        if (userInterfaceSpecialistsAvailableCount > 0)
        {
            userInterfaceSpecialistsCurrentCount++;
            userInterfaceSpecialistsAvailableCount--;
            developerAccountingManager.UpdateSalariesServer();
        }
    }
    [Command]
    public void CmdAddIntegrabilitySpecialist()
    {
        if (integrabilitySpecialistsAvailableCount > 0)
        {
            integrabilitySpecialistsCurrentCount++;
            integrabilitySpecialistsAvailableCount--;
            developerAccountingManager.UpdateSalariesServer();
        }
    }
    [Command]
    public void CmdSubstractUISpecialist()
    {
        if (userInterfaceSpecialistsCurrentCount > 0)
        {
            userInterfaceSpecialistsCurrentCount--;
            userInterfaceSpecialistsAvailableCount++;
            developerAccountingManager.UpdateSalariesServer();
        }
    }
    [Command]
    public void CmdSubstractIntegrabilitySpecialist()
    {
        if (integrabilitySpecialistsCurrentCount > 0)
        {
            integrabilitySpecialistsCurrentCount--;
            integrabilitySpecialistsAvailableCount++;
            developerAccountingManager.UpdateSalariesServer();
        }
    }

    public void HireProgrammersNextQuarter(int hireProgrammersCount)
    {
        CmdHireProgramersNextQuarter(hireProgrammersCount);
    }
    public void HireUISPecialistsNextQuarter(int hireUISpecialistsCount)
    {
        CmdHireUISpecialistsNextQuarter(hireUISpecialistsCount);
    }
    public void HireIntegrabilitySpecialistsNextQuarter(int hireIntegrabilitySpecialistsCount)
    {
        CmdHireIntegrabilitySpecialistsNextQuarter(hireIntegrabilitySpecialistsCount);
    }
    [Command]
    public void CmdHireProgramersNextQuarter(int hireProgrammersCount)
    {
        this.hireProgrammersCount = hireProgrammersCount;
    }
    [Command]
    public void CmdHireUISpecialistsNextQuarter(int hireUISpecialistsCount)
    {
        this.hireUISpecialistsCount = hireUISpecialistsCount;
    }
    [Command]
    public void CmdHireIntegrabilitySpecialistsNextQuarter(int hireIntegrabilitySpecialistsCount)
    {
        this.hireIntegrabilitySpecialistsCount = hireIntegrabilitySpecialistsCount;
    }


    public void ChangeProgrammmerSalaryMonth(int programmerSalary) { CmdChangeProgrammerSalary(programmerSalary); }
    public void ChangeUISpecialistSalaryMonth(int uiSpecialistSalary) { CmdChangeUISpecialistSalary(uiSpecialistSalary); }
    public void ChangeIntegrabilitySpecialistSalaryMonth(int integrabilitySpecialistSalary) { CmdChangeIntegrabilitySpecialistSalary(integrabilitySpecialistSalary); }
    [Command]
    public void CmdChangeProgrammerSalary(int programmerSalary)
    {
        programmerSalaryPerMonth = programmerSalary;
        programmerSalaryPerQuarter = programmerSalaryPerMonth * 3;
        developerAccountingManager.UpdateSalariesServer();
    }
    [Command]
    public void CmdChangeUISpecialistSalary(int uiSpecialistSalary)
    {
        uiSpecialistSalaryPerMonth = uiSpecialistSalary;
        uiSpecialistSalaryPerQuarter = uiSpecialistSalaryPerMonth * 3;
        developerAccountingManager.UpdateSalariesServer();
    }
    [Command]
    public void CmdChangeIntegrabilitySpecialistSalary(int integrabilitySpecialistSalary)
    {
        integrabilitySpecialistSalaryPerMonth = integrabilitySpecialistSalary;
        integrabilitySpecialistSalaryPerQuarter = integrabilitySpecialistSalaryPerMonth * 3;
        developerAccountingManager.UpdateSalariesServer();
    }


    //-----------HOOKS--------------
    public void OnChangeProgrammersCount(int programmersCount)
    {
        this.programmersCurrentCount = programmersCount;
        if (this.humanResourcesUIHandler != null)
        {
            humanResourcesUIHandler.UpdateProgramersCurrentCountText(this.programmersCurrentCount);
        }
        if (this.scheduleManager != null)
        {
            scheduleManager.UpdateAllFeatureGraphs();
        }
    }
    public void OnChangeUISpecialistsCount(int UISpecialistsCount)
    {
        this.userInterfaceSpecialistsCurrentCount = UISpecialistsCount;
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
    public void OnChangeIntegrabilitySpecialistAvailableCount(int integrabilitySpecialistsAvailableCount)
    {
        this.integrabilitySpecialistsAvailableCount = integrabilitySpecialistsAvailableCount;
        if (this.humanResourcesUIHandler != null)
        {
            humanResourcesUIHandler.UpdateIntegrabilitySpecialistsAvailableCountText(this.integrabilitySpecialistsAvailableCount);
        }
    }
    public void OnChangeUISpecialistAvailableCount(int uiSpecialistsAvailableCount)
    {
        this.userInterfaceSpecialistsAvailableCount = uiSpecialistsAvailableCount;
        if (this.humanResourcesUIHandler != null)
        {
            humanResourcesUIHandler.UpdateUISpecialistsAvailableCountText(this.userInterfaceSpecialistsAvailableCount);
        }
    }

    public void OnChangeHireProgrammersCount(int hireProgrammersCount)
    {
        this.hireProgrammersCount = hireProgrammersCount;
        if (humanResourcesUIHandler != null)
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

    public void OnChangeProgrammerSalaryQuarter(int programmerSalary)
    {
        this.programmerSalaryPerQuarter = programmerSalary;
    }
    public void OnChangeUISpecialistSalaryQuarter(int uiSpecialistSalary)
    {
        this.uiSpecialistSalaryPerQuarter = uiSpecialistSalary;
    }
    public void OnChangeIntegrabilitySpecialistSalaryQuarter(int integrabilitySpecialistSalary)
    {
        this.integrabilitySpecialistSalaryPerQuarter = integrabilitySpecialistSalary;
    }

    public void OnChangeProgrammerSalaryMonth(int programmerSalary)
    {
        this.programmerSalaryPerMonth = programmerSalary;
        if (humanResourcesUIHandler != null)
        {
            humanResourcesUIHandler.UpdateProgrammerSalarySlider(this.programmerSalaryPerMonth);

        }
    }
    public void OnChangeUISpecialistSalaryMonth(int uiSpecialistSalary)
    {
        this.uiSpecialistSalaryPerMonth = uiSpecialistSalary;
        if (humanResourcesUIHandler != null)
        {
            humanResourcesUIHandler.UpdateUISpecialistSalarySlider(this.uiSpecialistSalaryPerMonth);
        }
    }
    public void OnChangeIntegrabilitySpecialistSalaryMonth(int integrabilitySpecialistSalary)
    {
        this.integrabilitySpecialistSalaryPerMonth = integrabilitySpecialistSalary;
        if (humanResourcesUIHandler != null)
        {
            humanResourcesUIHandler.UpdateIntegrabilitySpecialistSalarySlider(this.integrabilitySpecialistSalaryPerMonth);
        }
    }


    [Server]
    public void MoveToNextQuarter()
    {

        LoadNextQuarterData(currentQuarter);

    }

    [Server]
    public void SaveCurrentQuarterData()
    {
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();

        endProgrammersCountQ.Insert(currentQuarter, programmersCurrentCount + hireProgrammersCount);
        endIntegrabilitySpecialistsCountQ.Insert(currentQuarter, integrabilitySpecialistsCurrentCount + hireIntegrabilitySpecialistsCount);
        endUISpecialistsCountQ.Insert(currentQuarter, userInterfaceSpecialistsCurrentCount + hireUISpecialistsCount);
        programmerSalaryQ.Insert(currentQuarter, programmerSalaryPerQuarter);
        integrabilitySpecialistSalaryQ.Insert(currentQuarter, integrabilitySpecialistSalaryPerQuarter);
        uiSpecialistSalaryQ.Insert(currentQuarter, uiSpecialistSalaryPerQuarter);
    }

    [Server]
    public void LoadNextQuarterData(int currentQuarter)
    {
        LoadQuarterData(currentQuarter + 1);
    }

    private void Start()
    {

    }
}

