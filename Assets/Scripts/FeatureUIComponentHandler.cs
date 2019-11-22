using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FeatureUIComponentHandler : MonoBehaviour
{   
    //VARIABLES
    private FeatureUIHandler featureUIHandler;


    public TextMeshProUGUI nameIDText;
    public TextMeshProUGUI functionalityText;
    public TextMeshProUGUI integrationText;
    public TextMeshProUGUI userExperienceText;
    public TextMeshProUGUI timeCostsText;
    public Toggle checkedForOutsourcingToggle;
    public TextMeshProUGUI stateText;

    public TextMeshProUGUI enterpriseCustomersText;
    public TextMeshProUGUI businessCustomersText;
    public TextMeshProUGUI individualCustomersText;

    //GETTERS & SETTERS
    public void SetFeatureUIHandler(FeatureUIHandler featureUIHandler) { this.featureUIHandler = featureUIHandler; }
    public void SetNameIDText(string nameIDtext) { this.nameIDText.text = nameIDtext; }
    public void SetFunctionalityText(string functionalityText) { this.functionalityText.text = functionalityText; }
    public void SetIntegrationText(string integrationText) { this.integrationText.text = integrationText; }
    public void SetUserExperienceText(string userExperienceText) { this.userExperienceText.text = userExperienceText; }
    public void SetTimeCostsText(string timeCosts) { timeCostsText.text = timeCosts; }
    public void SetCheckedForOutsourcing(bool checkOutsourcing) { checkedForOutsourcingToggle.isOn = checkOutsourcing ; }
    public void SetEnterpriseCustomers(string enterpriseCustomersText) { this.enterpriseCustomersText.text = enterpriseCustomersText; }
    public void SetBusinessCustomers(string businessCustomersText) { this.businessCustomersText.text = businessCustomersText; }
    public void SetIndividualsCustomers(string individualCustomersText) { this.individualCustomersText.text = individualCustomersText; }


    //METHODS
    public void SetupFeature(string nameID, int functionality, int integrability, int userInterface, int timeCost, int enterpriseCustomers, int businessCustomers, int individualCustomers)
    {
        nameIDText.text = nameID;
        functionalityText.text = functionality.ToString();
        integrationText.text = integrability.ToString();
        userExperienceText.text = userInterface.ToString();
        timeCostsText.text = timeCost.ToString();
        enterpriseCustomersText.text = enterpriseCustomers.ToString();
        businessCustomersText.text = businessCustomers.ToString();
        individualCustomersText.text = individualCustomers.ToString();
    }

    public void SetFeatureForOutsourcing(bool setOutsourcing)
    {

        if (setOutsourcing)
        {
            Debug.Log("GUI CHECK: pridavam feature do outsourced " + nameIDText.text );
            featureUIHandler.AddFeatureForOutsourcing(nameIDText.text.ToString());
        }
        else
        {
            featureUIHandler.RemoveFeatureForOutsourcing(nameIDText.text.ToString());
        }
    }
    public void HideToggle()
    {
        checkedForOutsourcingToggle.gameObject.SetActive(false);
    }
    public void SetStateText(string state)
    {
        stateText.text = state;
    }
    public void SetStateColor(Color color)
    {
        stateText.color = color;
    }

    public void DisabeOutsourcingToggleInteraction()
    {
        checkedForOutsourcingToggle.interactable = false;
    }
    

}
