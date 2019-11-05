using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EventManager : NetworkBehaviour
{

    private string gameID;
    private PlayerData playerData;
    private int currentQuarter;

    private HumanResourcesManager humanResourcesManager;
    private ProviderAccountingManager providerAccountingManger;
    private CustomersManager customersManager;

    private void Start() { }

    public override void OnStartServer()
    {
        playerData = this.gameObject.GetComponent<PlayerData>();
        gameID = this.gameObject.GetComponent<PlayerData>().GetGameID();
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        if(playerData.GetPlayerRole() == PlayerRoles.Developer)
        {
            humanResourcesManager = this.gameObject.GetComponent<HumanResourcesManager>();
        }
        if (playerData.GetPlayerRole() == PlayerRoles.Provider)
        {
            providerAccountingManger = this.gameObject.GetComponent<ProviderAccountingManager>();
            customersManager = this.gameObject.GetComponent<CustomersManager>();
        }
    }

    public override void OnStartClient()
    {
        playerData = this.gameObject.GetComponent<PlayerData>();
        gameID = this.gameObject.GetComponent<PlayerData>().GetGameID();
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        if (playerData.GetPlayerRole() == PlayerRoles.Developer)
        {
            humanResourcesManager = this.gameObject.GetComponent<HumanResourcesManager>();
        }
        if (playerData.GetPlayerRole() == PlayerRoles.Provider)
        {
            providerAccountingManger = this.gameObject.GetComponent<ProviderAccountingManager>();
        }
    }

    public void InvokeGameEvent()
    {

        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        Debug.Log("InvokeGameEvent" );
        if (PlayerRoles.Developer == playerData.GetPlayerRole()  && currentQuarter == 2)
        {
            DeveloperQ2EmployeeLossEvent();
        }
        if (PlayerRoles.Provider == playerData.GetPlayerRole() && currentQuarter == 3)
        {
            ProviderQ3AdditionalExpensesEvent();
        }
    }

    [Server]
    public void DeveloperQ2EmployeeLossEvent()
    {
        Debug.Log("EmployeesLossEvent");
        //programmers
        if(humanResourcesManager.GetProgrammerSalaryPerQurter() < 2000)  //15% loss
        {
            int programmersCurrentCount = humanResourcesManager.GetProgrammersCount();
            int programmersLoss = (int)System.Math.Round(((float)programmersCurrentCount * 0.15), System.MidpointRounding.AwayFromZero);
            if(programmersLoss == 0)
            {
                programmersLoss = 1;
            }
            humanResourcesManager.SetProgrammersCount(programmersCurrentCount - programmersLoss);        }
        else if (humanResourcesManager.GetProgrammerSalaryPerQurter() >= 2000 && humanResourcesManager.GetProgrammerSalaryPerQurter() < 2500) // 10% loss
        {
            int programmersCurrentCount = humanResourcesManager.GetProgrammersCount();
            int programmersLoss = (int)System.Math.Round(((float)programmersCurrentCount * 0.1), System.MidpointRounding.AwayFromZero);
            if (programmersLoss == 0)
            {
                programmersLoss = 1;
            }
            humanResourcesManager.SetProgrammersCount(programmersCurrentCount - programmersLoss);
        }
        else if (humanResourcesManager.GetProgrammerSalaryPerQurter() >= 2500) //5% loss
        {
            int programmersCurrentCount = humanResourcesManager.GetProgrammersCount();
            int programmersLoss = (int)System.Math.Round(((float)programmersCurrentCount * 0.05), System.MidpointRounding.AwayFromZero);
            humanResourcesManager.SetProgrammersCount(programmersCurrentCount - programmersLoss);
        }
        //integrabilitySpecialsts
        if (humanResourcesManager.GetIntegrabilitySpecialistSalaryPerQuarter() < 3000)
        {
            int integrabilitySpecialistsCurrentCount = humanResourcesManager.GetProgrammersCount();
            int integrabilitySpecialistsLoss = (int)System.Math.Round(((float)integrabilitySpecialistsCurrentCount * 0.15), System.MidpointRounding.AwayFromZero);
            if (integrabilitySpecialistsLoss == 0)
            {
                integrabilitySpecialistsLoss = 1;
            }
            humanResourcesManager.SetProgrammersCount(integrabilitySpecialistsCurrentCount - integrabilitySpecialistsLoss);
        }
        else if (humanResourcesManager.GetIntegrabilitySpecialistSalaryPerQuarter() >= 3000 && humanResourcesManager.GetIntegrabilitySpecialistSalaryPerQuarter() < 3500)
        {
            int integrabilitySpecialistsCurrentCount = humanResourcesManager.GetProgrammersCount();
            int integrabilitySpecialistsLoss = (int)System.Math.Round(((float)integrabilitySpecialistsCurrentCount * 0.1), System.MidpointRounding.AwayFromZero);
            if (integrabilitySpecialistsLoss == 0)
            {
                integrabilitySpecialistsLoss = 1;
            }
            humanResourcesManager.SetProgrammersCount(integrabilitySpecialistsCurrentCount - integrabilitySpecialistsLoss);
        }
        else if (humanResourcesManager.GetIntegrabilitySpecialistSalaryPerQuarter() >= 3500)
        {
            int integrabilitySpecialistsCurrentCount = humanResourcesManager.GetProgrammersCount();
            int integrabilitySpecialistsLoss = (int)System.Math.Round(((float)integrabilitySpecialistsCurrentCount * 0.05), System.MidpointRounding.AwayFromZero);
            humanResourcesManager.SetProgrammersCount(integrabilitySpecialistsCurrentCount - integrabilitySpecialistsLoss);
        }
        //uiSpecialists
        if (humanResourcesManager.GetUISpecialistSalaryPerQuarter() < 3000)
        {
            int uiSpecialistsCurrentCount = humanResourcesManager.GetProgrammersCount();
            int uiSpecialistsLoss = (int)System.Math.Round(((float)uiSpecialistsCurrentCount * 0.15), System.MidpointRounding.AwayFromZero);
            if (uiSpecialistsLoss == 0)
            {
                uiSpecialistsLoss = 1;
            }
            humanResourcesManager.SetProgrammersCount(uiSpecialistsCurrentCount - uiSpecialistsLoss);
        }
        else if (humanResourcesManager.GetUISpecialistSalaryPerQuarter() >= 3000 && humanResourcesManager.GetUISpecialistSalaryPerQuarter() < 3500)
        {
            int uiSpecialistsCurrentCount = humanResourcesManager.GetProgrammersCount();
            int uiSpecialistsLoss = (int)System.Math.Round(((float)uiSpecialistsCurrentCount * 0.15), System.MidpointRounding.AwayFromZero);
            if (uiSpecialistsLoss == 0)
            {
                uiSpecialistsLoss = 1;
            }
            humanResourcesManager.SetProgrammersCount(uiSpecialistsCurrentCount - uiSpecialistsLoss);
        }
        else if (humanResourcesManager.GetUISpecialistSalaryPerQuarter() >= 3500)
        {
            int uiSpecialistsCurrentCount = humanResourcesManager.GetProgrammersCount();
            int uiSpecialistsLoss = (int)System.Math.Round(((float)uiSpecialistsCurrentCount * 0.15), System.MidpointRounding.AwayFromZero);
            humanResourcesManager.SetProgrammersCount(uiSpecialistsCurrentCount - uiSpecialistsLoss);
        }
    }

    [Server]
    public void ProviderQ3AdditionalExpensesEvent()
    {
        Debug.Log("AdditionalExpenses Event");
        int enterpriseCustomers = customersManager.GetEndEnterpriseCustomers();
        int businessCustomers = customersManager.GetEndBusinessCustomers();
        int individualCustomers= customersManager.GetEndIndividualCustomers();

        int allCustomers = enterpriseCustomers + businessCustomers + individualCustomers; 

        if (allCustomers < 1000)
        {
            providerAccountingManger.SetAdditionalExpenses(10000);

        }
        else if(allCustomers >= 1000  && allCustomers < 10000)
        {
            providerAccountingManger.SetAdditionalExpenses(50000);
        }
        else if(allCustomers >= 10000)
        {
            providerAccountingManger.SetAdditionalExpenses(100000);
        }
    }


}
