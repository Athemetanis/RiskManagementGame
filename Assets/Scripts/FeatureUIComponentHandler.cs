using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeatureUIComponentHandler : MonoBehaviour
{   
    //VARIABLES
    private FeatureUIHandler featureUIHandler;

    public Text nameIDText;
    public Text functionalityText;
    public Text integrationText;
    public Text userExperienceText;
    public Text timeCostsText;
    public Toggle checkedForOutsourcing;


    //METHODS
    public void SetFeatureUIHandler(FeatureUIHandler featureUIHandler) { this.featureUIHandler = featureUIHandler; }
    public void SetNameIDText(string nameIDtext) { this.nameIDText.text = nameIDtext; }
    public void SetFunctionalityText(string functionalityText) { this.functionalityText.text = functionalityText; }
    public void SetIntegrationText(string integrationText) { this.integrationText.text = integrationText; }
    public void SetUserExperienceText(string userExperienceText) { this.userExperienceText.text = userExperienceText; }
    public void SetTimeCostsText(string timeCosts) { this.timeCostsText.text = timeCosts; }
    public void SetCheckedForOutsourcing(bool checkOutsourcing) { this.checkedForOutsourcing.isOn = checkOutsourcing; }

    public void SetFeatureForOutsourcing(bool setOutsourcing)
    {
        if (setOutsourcing)
        {
            featureUIHandler.AddFeatureForOutsourcing(nameIDText.text.ToString());
        }
        else
        {
            featureUIHandler.RemoveFeatureForOutsourcing(nameIDText.text.ToString());
        }
    }
}
