using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeveloperAccountingUIHandler : MonoBehaviour
{
    //VARIABLES
    public Text beginningCashBalanceText;
    public Text revenueText;
    public Text salariesText;
    public Text programmersSalariesText;
    public Text uiSpecialistsSalariesText;
    public Text integrabilitySpecialistsSalariesText;
    public Text endCashBalanceText;

    private GameObject myPlayerDataObject;
    private DeveloperAccountingManager developerAccountingManager;
    private HumanResourcesManager humanResourcesManager;

    // Start is called before the first frame update
    void Start()
    {
        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        developerAccountingManager = myPlayerDataObject.GetComponent<DeveloperAccountingManager>();
        developerAccountingManager.SetDeveloperAccountingUIHandler(this);
        UpdateAllElements();

    }
    //METHODS FOR UPDATING UI ELEMENTS
    public void UpdateAllElements()
    {
        beginningCashBalanceText.text = developerAccountingManager.GetBeginningCashBalance().ToString();
        revenueText.text = developerAccountingManager.GetRevenue().ToString();
        salariesText.text = developerAccountingManager.GetSalaries().ToString();
        programmersSalariesText.text = developerAccountingManager.GetProgrammersSalaries().ToString();
        uiSpecialistsSalariesText.text = developerAccountingManager.GetUISpecialistsSalaries().ToString();
        integrabilitySpecialistsSalariesText.text = developerAccountingManager.GetIntegrabilitySpecialistsSalaries().ToString();
        endCashBalanceText.text = developerAccountingManager.GetEndCashBalance().ToString();
    }
    public void UpdateBeginingCashBalanceText(int beginingCashBalance)
    {
        beginningCashBalanceText.text = beginingCashBalance.ToString();
    }
    public void UpdateRevenueText(int revenue)
    {
        revenueText.text = revenue.ToString();  
    }
    public void UpdateEndCashBalanceText(int endCashBalance)
    {
        endCashBalanceText.text = endCashBalance.ToString();
    }
    public void UpdateSalariesText(int programmersSalaries)
    {
        salariesText.text = programmersSalaries.ToString();
    }
    public void UpdateProgrammersSalariesText(int programmersSalaries)
    {
        programmersSalariesText.text = programmersSalaries.ToString();
    }
    public void UpdateUISpecialistsSalariesText(int uiSpecialistsSalaries)
    {
        uiSpecialistsSalariesText.text = uiSpecialistsSalaries.ToString();
    }
    public void UpdateIntegrabilitySpecialistsSalariesText(int integrabilitySpecialistsSalaries)
    {
        integrabilitySpecialistsSalariesText.text = integrabilitySpecialistsSalaries.ToString();
    }
   
}
