using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeatureUIHandler : MonoBehaviour
{
    public GameObject featureUIPrefab;
    public GameObject FeatureListContent;
    public GameObject outsourcedFeatureListContent;
    public GameObject doneFeatureListContent;

    public Toggle allFeaturesToggle;
    public Toggle availableFeaturesToggle;
    public Toggle inDevelopmentFeaturesToggle;
    public Toggle doneFeaturesToggle;
    
    private FeatureManager featureManager;

    // Awake = syncvar not synced, called before start; dont use - not suitable for networking!
    private void Awake()
    {  }

    private void Start()
    {
        featureManager = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject().GetComponent<FeatureManager>();
        featureManager.SetFeatureUIHandler(this);
        GenerateFeatureUIList();
        UpdateOutsourcedFeatureUIList();
        //GenerateDropdownOptions(); - -NAHRAĎ!!!!!!!!!!!!!!!!!!!!!!!!!!

    }

    //METHODS
    public void AddFeatureForOutsourcing(string name)
    {
        featureManager.AddFeatureForOutsourcing(name);
    }
    public void RemoveFeatureForOutsourcing(string name)
    {
        featureManager.RemoveFeatureForOutsourcing(name);
    }

    //METHODS FOR GENERATING UI ELEMENT
    public void GenerateFeatureUIList()
    {
        if (allFeaturesToggle.isOn == true)
        {
            foreach (Feature feature in featureManager.GetAllFeatures().Values)
            {
                GameObject featureUIComponent = Instantiate(featureUIPrefab);
                featureUIComponent.transform.SetParent(FeatureListContent.transform, false);
                FeatureUIComponentHandler featureUIComponentHandler = featureUIComponent.GetComponent<FeatureUIComponentHandler>();
                featureUIComponentHandler.SetupFeature(feature.nameID, feature.functionality, feature.integrability, feature.userfriendliness, feature.timeCost, feature.enterpriseCustomers, feature.businessCustomers, feature.individualCustomers);
                featureUIComponentHandler.SetFeatureUIHandler(this);
                if (featureManager.GetAvailableFeatures().ContainsKey(feature.nameID) == true)
                {
                    featureUIComponentHandler.SetStateText("Available For Outsourcing");
                    featureUIComponentHandler.SetStateColor(Color.green);
                    if (featureManager.GetOutsourcedFeatures().ContainsKey(feature.nameID) == true)
                    {
                        featureUIComponentHandler.SetCheckedForOutsourcing(true);
                    }
                    else
                    {
                        featureUIComponentHandler.SetCheckedForOutsourcing(false);
                    }
                }
                else if (featureManager.GetInDevelopmentFeatures().ContainsKey(feature.nameID) == true)
                {
                    featureUIComponentHandler.HideToggle();
                    featureUIComponentHandler.SetStateText("In Development");
                    featureUIComponentHandler.SetStateColor(Color.yellow);
                }
                else if (featureManager.GetDoneFeatures().ContainsKey(feature.nameID) == true)
                {
                    featureUIComponentHandler.HideToggle();
                    featureUIComponentHandler.SetStateText("Done");
                    featureUIComponentHandler.SetStateColor(Color.red);
                }
                featureUIComponent.SetActive(true);
            }
        }
        else if (availableFeaturesToggle.isOn == true)
        {
            foreach (Feature feature in featureManager.GetAvailableFeatures().Values)
            {
                GameObject featureUIComponent = Instantiate(featureUIPrefab);
                featureUIComponent.transform.SetParent(FeatureListContent.transform, false);
                FeatureUIComponentHandler featureUIComponentHandler = featureUIComponent.GetComponent<FeatureUIComponentHandler>();
                featureUIComponentHandler.SetupFeature(feature.nameID, feature.functionality, feature.integrability, feature.userfriendliness, feature.timeCost, feature.enterpriseCustomers, feature.businessCustomers, feature.individualCustomers);
                featureUIComponentHandler.SetFeatureUIHandler(this);
                featureUIComponentHandler.SetStateText("Available For Outsourcing");
                featureUIComponentHandler.SetStateColor(Color.green);
                if (featureManager.GetOutsourcedFeatures().ContainsKey(feature.nameID) == true)
                {
                    featureUIComponentHandler.SetCheckedForOutsourcing(true);
                }
                else
                {
                    featureUIComponentHandler.SetCheckedForOutsourcing(false);
                }
                featureUIComponent.SetActive(true);
            }
        }
        else if(inDevelopmentFeaturesToggle.isOn == true)
        {
            foreach (Feature feature in featureManager.GetInDevelopmentFeatures().Values)
            {
                GameObject featureUIComponent = Instantiate(featureUIPrefab);
                featureUIComponent.transform.SetParent(FeatureListContent.transform, false);
                FeatureUIComponentHandler featureUIComponentHandler = featureUIComponent.GetComponent<FeatureUIComponentHandler>();
                featureUIComponentHandler.SetupFeature(feature.nameID, feature.functionality, feature.integrability, feature.userfriendliness, feature.timeCost, feature.enterpriseCustomers, feature.businessCustomers, feature.individualCustomers);
                featureUIComponentHandler.SetFeatureUIHandler(this);
                featureUIComponentHandler.HideToggle();
                featureUIComponentHandler.SetStateText("In Development");
                featureUIComponentHandler.SetStateColor(Color.yellow);
                featureUIComponent.SetActive(true);
            }

        }
        else if(doneFeaturesToggle.isOn == true)
        {
            foreach (Feature feature in featureManager.GetDoneFeatures().Values)
            {
                GameObject featureUIComponent = Instantiate(featureUIPrefab);
                featureUIComponent.transform.SetParent(FeatureListContent.transform, false);
                FeatureUIComponentHandler featureUIComponentHandler = featureUIComponent.GetComponent<FeatureUIComponentHandler>();
                featureUIComponentHandler.SetupFeature(feature.nameID, feature.functionality, feature.integrability, feature.userfriendliness, feature.timeCost, feature.enterpriseCustomers, feature.businessCustomers, feature.individualCustomers);
                featureUIComponentHandler.SetFeatureUIHandler(this);
                featureUIComponentHandler.HideToggle();
                featureUIComponentHandler.HideToggle();
                featureUIComponentHandler.SetStateText("Done");
                featureUIComponentHandler.SetStateColor(Color.red);
                featureUIComponent.SetActive(true);
            }
        }

    }
    public void GenerateOutSourcedFeatureUIList()
    {
        foreach (KeyValuePair<string, Feature> feature in featureManager.GetOutsourcedFeatures())
        {
            GameObject featureUIComponent = Instantiate(featureUIPrefab);
            featureUIComponent.transform.SetParent(outsourcedFeatureListContent.transform, false);
            FeatureUIComponentHandler featureUIComponentHandler = featureUIComponent.GetComponent<FeatureUIComponentHandler>();
            featureUIComponentHandler.SetNameIDText(feature.Value.nameID);
            featureUIComponentHandler.SetFunctionalityText(feature.Value.functionality.ToString());
            featureUIComponentHandler.SetIntegrationText(feature.Value.integrability.ToString());
            featureUIComponentHandler.SetUserExperienceText(feature.Value.userfriendliness.ToString());
            featureUIComponentHandler.SetTimeCostsText(feature.Value.timeCost.ToString());
            featureUIComponentHandler.SetFeatureUIHandler(this);
            featureUIComponentHandler.SetCheckedForOutsourcing(true);
            featureUIComponent.SetActive(true);
        }
    }
    public void GenerateDoneFeatureUIList()
    {
        foreach (KeyValuePair<string, Feature> feature in featureManager.GetDoneFeatures())
        {
            GameObject featureUIComponent = Instantiate(featureUIPrefab);
            featureUIComponent.transform.SetParent(doneFeatureListContent.transform, false);
            FeatureUIComponentHandler featureUIComponentHandler = featureUIComponent.GetComponent<FeatureUIComponentHandler>();
            featureUIComponentHandler.SetNameIDText(feature.Value.nameID);
            featureUIComponentHandler.SetFunctionalityText(feature.Value.functionality.ToString());
            featureUIComponentHandler.SetIntegrationText(feature.Value.integrability.ToString());
            featureUIComponentHandler.SetUserExperienceText(feature.Value.userfriendliness.ToString());
            featureUIComponentHandler.SetTimeCostsText(feature.Value.timeCost.ToString());
            featureUIComponentHandler.SetFeatureUIHandler(this);
            featureUIComponentHandler.HideToggle();
            featureUIComponent.SetActive(true);
        }
    }

    //METHODS FOR UPDATING UI ELEMENTS
    public void UpdateFeatureUIList()
    {
        foreach (Transform child in FeatureListContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        GenerateFeatureUIList();
    }

  
    public void UpdateOutsourcedFeatureUIList()
    {
        foreach (Transform child in outsourcedFeatureListContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        GenerateOutSourcedFeatureUIList();
    }


}
