using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeveloperAccountingUIComponentHandler : MonoBehaviour
{
    //VARIABLES
    public Text beginningCashBalanceText;
    public Text revenueText;
    public Text salariesText;
    public Text programmersSalariesText;
    public Text uiSpecialistsSalariesText;
    public Text integrabilitySpecialistsSalariesText;
    public Text riskSharingFeePaidText;
    public Text terminationFeePaidText;
    public Text marketingResearchText;
    public Text borrowEmergencyLoanText;
    public Text repayEmergencyLoanText;
    public Text endCashBalanceText;

    public int correspondingAccountingQuarter; //must be set in editor manually

    //REFERENCES 
    private string gameID;
    private int currentQuarter;
    private GameObject myPlayerDataObject;
    private DeveloperAccountingManager developerAccountingManager;
    //private HumanResourcesManager humanResourcesManager;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("provider accounting component start " + correspondingAccountingQuarter);
        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        gameID = myPlayerDataObject.GetComponent<PlayerData>().GetGameID();
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        developerAccountingManager = myPlayerDataObject.GetComponent<DeveloperAccountingManager>();

        if (correspondingAccountingQuarter == 1)
        {
            developerAccountingManager.SetDeveloperAccountingUIHandlerQ1(this);
        }
        if (correspondingAccountingQuarter == 2)
        {
            developerAccountingManager.SetDeveloperAccountingUIHandlerQ2(this);
        }
        if (correspondingAccountingQuarter == 3)
        {
            developerAccountingManager.SetDeveloperAccountingUIHandlerQ3(this);
        }
        if (correspondingAccountingQuarter == 4)
        {
            developerAccountingManager.SetDeveloperAccountingUIHandlerQ4(this);
        }
        if (correspondingAccountingQuarter < currentQuarter)
        {
            (int beginningCashBalance, int revenue, int salaries, int programmersSalaries, int uiSpecialistsSalaries, int integrabilitySpecialistsSalaries,int riskSharingFeePaid, int terminationFeePaid, int marketingResearch, int borrowEmergencyLoan, int repayEmergencyLoan, int endCashBalance) = developerAccountingManager.GetCorrecpondingQuarterData(correspondingAccountingQuarter);
            beginningCashBalanceText.text = beginningCashBalance.ToString("n0");
            revenueText.text = revenue.ToString("n0");
            salariesText.text = salaries.ToString("n0");
            programmersSalariesText.text = programmersSalaries.ToString("n0");
            uiSpecialistsSalariesText.text = uiSpecialistsSalaries.ToString("n0");
            integrabilitySpecialistsSalariesText.text = integrabilitySpecialistsSalaries.ToString("n0");
            riskSharingFeePaidText.text = riskSharingFeePaid.ToString("n0");
            terminationFeePaidText.text = terminationFeePaid.ToString("n0");
            marketingResearchText.text = marketingResearch.ToString("n0");
            borrowEmergencyLoanText.text = borrowEmergencyLoan.ToString("n0");
            repayEmergencyLoanText.text = repayEmergencyLoan.ToString("n0");
            endCashBalanceText.text = endCashBalance.ToString("n0");
        }
        if (correspondingAccountingQuarter == currentQuarter)
        {
            developerAccountingManager.SetCurrentDeveloperAccountingUIHandler(this);
            UpdateAllElements();
        }
        if(correspondingAccountingQuarter > currentQuarter)
        {
            this.gameObject.SetActive(false);
        }    
    }
    
    //METHODS FOR UPDATING UI ELEMENTS
    public void UpdateAllElements()
    {
        beginningCashBalanceText.text = developerAccountingManager.GetBeginningCashBalance().ToString("n0");
        revenueText.text = developerAccountingManager.GetRevenue().ToString("n0");
        salariesText.text = developerAccountingManager.GetSalaries().ToString("n0");
        programmersSalariesText.text = developerAccountingManager.GetProgrammersSalaries().ToString("n0");
        uiSpecialistsSalariesText.text = developerAccountingManager.GetUISpecialistsSalaries().ToString("n0");
        integrabilitySpecialistsSalariesText.text = developerAccountingManager.GetIntegrabilitySpecialistsSalaries().ToString("n0");
        riskSharingFeePaidText.text = developerAccountingManager.GetRishSharingFeesPaid().ToString("n0");
        terminationFeePaidText.text = developerAccountingManager.GetTerminationFeePaid().ToString("n0");
        marketingResearchText.text = developerAccountingManager.GetMarketingResearch().ToString("n0");
        borrowEmergencyLoanText.text = developerAccountingManager.GetBorrowEmergencyLoan().ToString("n0");
        repayEmergencyLoanText.text = developerAccountingManager.GetRepayEmergencyLoan().ToString("n0");
        endCashBalanceText.text = developerAccountingManager.GetEndCashBalance().ToString("n0");


    }

    public void UpdateBeginingCashBalanceText(int beginingCashBalance)
    {
        beginningCashBalanceText.text = beginingCashBalance.ToString("n0");
    }
    public void UpdateRevenueText(int revenue)
    {
        revenueText.text = revenue.ToString("n0");  
    }
    public void UpdateSalariesText(int programmersSalaries)
    {
        salariesText.text = programmersSalaries.ToString("n0");
    }
    public void UpdateProgrammersSalariesText(int programmersSalaries)
    {
        programmersSalariesText.text = programmersSalaries.ToString("n0");
    }
    public void UpdateUISpecialistsSalariesText(int uiSpecialistsSalaries)
    {
        uiSpecialistsSalariesText.text = uiSpecialistsSalaries.ToString("n0");
    }
    public void UpdateIntegrabilitySpecialistsSalariesText(int integrabilitySpecialistsSalaries)
    {
        integrabilitySpecialistsSalariesText.text = integrabilitySpecialistsSalaries.ToString("n0");
   }
    public void UpdateRiskSharingFeePaid(int riskSharingFeePaid)
    {
        riskSharingFeePaidText.text = riskSharingFeePaid.ToString("n0");
    }
    public void UpdateTerminationFeepaid(int terminationFeePaid)
    {
        terminationFeePaidText.text = terminationFeePaid.ToString("n0");
    }
    public void UpdateMarketingResearch(int marketingResearch)
    {
        marketingResearchText.text = marketingResearch.ToString("n0");
    }
    public void UpdateBorrowEmergencyLoan(int borrowEmergencyLoan)
    {
        borrowEmergencyLoanText.text = borrowEmergencyLoan.ToString("n0");
    }
    public void UpdateRepayEmergencyLoan(int repayEmergencyLoan)
    {
        repayEmergencyLoanText.text = repayEmergencyLoan.ToString("n0");
    }
    public void UpdateEndCashBalanceText(int endCashBalance)
    {
        endCashBalanceText.text = endCashBalance.ToString("n0");
    }

}
