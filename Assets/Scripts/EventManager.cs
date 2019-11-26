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
        //currentQuarter = GameHandler.allGames[gameID].GetGameRound();
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
        if(humanResourcesManager.GetProgrammerSalaryPerMonth() < 2000)  //15% loss
        {   
            int programmersCurrentCount = humanResourcesManager.GetProgrammersCount();
            int programmersLoss = (int)System.Math.Round(((float)programmersCurrentCount * 0.15), System.MidpointRounding.AwayFromZero);
            if(programmersLoss == 0 && programmersCurrentCount != 0)
            {
                programmersLoss = 1;
            }
            humanResourcesManager.SetProgrammersCount(programmersCurrentCount - programmersLoss);
            Debug.Log("programmersLoss: " + programmersLoss);
        }
        else if (humanResourcesManager.GetProgrammerSalaryPerMonth() >= 2000 && humanResourcesManager.GetProgrammerSalaryPerMonth() < 2500) // 10% loss
        {
            int programmersCurrentCount = humanResourcesManager.GetProgrammersCount();
            int programmersLoss = (int)System.Math.Round(((float)programmersCurrentCount * 0.1), System.MidpointRounding.AwayFromZero);
            if (programmersLoss == 0 && programmersCurrentCount != 0)
            {
                programmersLoss = 1;
            }
            humanResourcesManager.SetProgrammersCount(programmersCurrentCount - programmersLoss);
            Debug.Log("programmersLoss: " + programmersLoss);
        }
        else if (humanResourcesManager.GetProgrammerSalaryPerMonth() >= 2500) //5% loss
        {
            int programmersCurrentCount = humanResourcesManager.GetProgrammersCount();
            int programmersLoss = (int)System.Math.Round(((float)programmersCurrentCount * 0.05), System.MidpointRounding.AwayFromZero);
            humanResourcesManager.SetProgrammersCount(programmersCurrentCount - programmersLoss);
            Debug.Log("programmersLoss: " + programmersLoss);
        }


        //integrabilitySpecialsts------------------------------------------------------------------------------------------------------
        if (humanResourcesManager.GetIntegrabilitySpecialistSalaryPerMonth() < 3000)
        {
            int integrabilitySpecialistsCurrentCount = humanResourcesManager.GetIntegrabilitySpecialistsCount();
            int integrabilitySpecialistsLoss = (int)System.Math.Round(((float)integrabilitySpecialistsCurrentCount * 0.15), System.MidpointRounding.AwayFromZero);
            if (integrabilitySpecialistsLoss == 0 && integrabilitySpecialistsCurrentCount != 0)
            {
                integrabilitySpecialistsLoss = 1;
            }
            humanResourcesManager.SetIntegrabilitySpecialistsCount(integrabilitySpecialistsCurrentCount - integrabilitySpecialistsLoss);
            Debug.Log("integrabilitySpecialistsLoss: " + integrabilitySpecialistsLoss);
        }
        else if (humanResourcesManager.GetIntegrabilitySpecialistSalaryPerMonth() >= 3000 && humanResourcesManager.GetIntegrabilitySpecialistSalaryPerMonth() < 3500)
        {
            int integrabilitySpecialistsCurrentCount = humanResourcesManager.GetIntegrabilitySpecialistsCount();
            int integrabilitySpecialistsLoss = (int)System.Math.Round(((float)integrabilitySpecialistsCurrentCount * 0.1), System.MidpointRounding.AwayFromZero);
            if (integrabilitySpecialistsLoss == 0 && integrabilitySpecialistsCurrentCount != 0)
            {
                integrabilitySpecialistsLoss = 1;
            }
            humanResourcesManager.SetIntegrabilitySpecialistsCount(integrabilitySpecialistsCurrentCount - integrabilitySpecialistsLoss);
            Debug.Log("integrabilitySpecialistsLoss: " + integrabilitySpecialistsLoss);
        }
        else if (humanResourcesManager.GetIntegrabilitySpecialistSalaryPerMonth() >= 3500)
        {
            int integrabilitySpecialistsCurrentCount = humanResourcesManager.GetIntegrabilitySpecialistsCount();
            int integrabilitySpecialistsLoss = (int)System.Math.Round(((float)integrabilitySpecialistsCurrentCount * 0.05), System.MidpointRounding.AwayFromZero);
            humanResourcesManager.SetIntegrabilitySpecialistsCount(integrabilitySpecialistsCurrentCount - integrabilitySpecialistsLoss);
            Debug.Log("integrabilitySpecialistsLoss: " + integrabilitySpecialistsLoss);
        }
        //uiSpecialists-----------------------------------------------------------------------------------------------------------------
        if (humanResourcesManager.GetUISpecialistSalaryPerMonth() < 3000)
        {
            int uiSpecialistsCurrentCount = humanResourcesManager.GetUISPecialistsCount();
            int uiSpecialistsLoss = (int)System.Math.Round(((float)uiSpecialistsCurrentCount * 0.15), System.MidpointRounding.AwayFromZero);
            if (uiSpecialistsLoss == 0 && uiSpecialistsCurrentCount != 0)
            {
                uiSpecialistsLoss = 1;
            }
            humanResourcesManager.SetUISpecialistsCount(uiSpecialistsCurrentCount - uiSpecialistsLoss);
            Debug.Log("uiSpecialistsLoss: " + uiSpecialistsLoss);
        }
        else if (humanResourcesManager.GetUISpecialistSalaryPerMonth() >= 3000 && humanResourcesManager.GetUISpecialistSalaryPerMonth() < 3500)
        {
            int uiSpecialistsCurrentCount = humanResourcesManager.GetUISPecialistsCount();
            int uiSpecialistsLoss = (int)System.Math.Round(((float)uiSpecialistsCurrentCount * 0.15), System.MidpointRounding.AwayFromZero);
            if (uiSpecialistsLoss == 0 && uiSpecialistsCurrentCount != 0)
            {
                uiSpecialistsLoss = 1;
            }
            humanResourcesManager.SetUISpecialistsCount(uiSpecialistsCurrentCount - uiSpecialistsLoss);
            Debug.Log("uiSpecialistsLoss: " + uiSpecialistsLoss);
        }
        else if (humanResourcesManager.GetUISpecialistSalaryPerMonth() >= 3500)
        {
            int uiSpecialistsCurrentCount = humanResourcesManager.GetUISPecialistsCount();
            int uiSpecialistsLoss = (int)System.Math.Round(((float)uiSpecialistsCurrentCount * 0.15), System.MidpointRounding.AwayFromZero);
            humanResourcesManager.SetUISpecialistsCount(uiSpecialistsCurrentCount - uiSpecialistsLoss);
            Debug.Log("uiSpecialistsLoss: " + uiSpecialistsLoss);
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
