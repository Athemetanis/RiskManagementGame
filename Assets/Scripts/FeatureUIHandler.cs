using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeatureUIHandler : MonoBehaviour
{

    public GameObject featureUIPrefab;
    //public GameObject allFeatureListContent;
    public GameObject availableFeatureListContent;
    public GameObject outsourcedFeatureListContent;
    //public Dropdown proposalFeatures;

    //public GameObject featurePrefab;
    
    private FeatureManager featureManager;

    // Awake = syncvar not synced, called before start; dont use - not sutable for networking!
    private void Awake()
    {  }

    private void Start()
    {
        featureManager = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject().GetComponent<FeatureManager>();
        featureManager.SetFeatureUIHandler(this);
        GenerateAvailableFeatureUIList();
        GenerateOutSourcedFeatureUIList();
        //GenerateDropdownOptions(); - -NAHRAĎ!!!!!!!!!!!!!!!!!!!!!!!!!!

    }

    /*public void GenerateDropdownOptions()
    {
        proposalFeatures.ClearOptions();
        List<string> options = new List<string>(featureManager.GetOutsourcedFeatures().Keys);
        proposalFeatures.AddOptions(options);
    }

    public void UpdateDropdownOptions()
    {
        proposalFeatures.ClearOptions();
        GenerateDropdownOptions();
    }*/


    /*public void GenerateAllFeatureUIList()
    {
        foreach( KeyValuePair<string, Feature> feature in featureManager.GetAllFeatures())
        {
            GameObject featureUI = Instantiate(featureUIPrefab);
            featureUI.transform.SetParent(allFeatureListContent.transform, false);
            FeatureUIComponentHandler featureUIComponent = featureUI.GetComponent<FeatureUIComponentHandler>();
            featureUIComponent.SetNameIDText(feature.Value.nameID);
            featureUIComponent.SetFunctionalityText(feature.Value.functionality.ToString());
            featureUIComponent.SetIntegrationText(feature.Value.integration.ToString());
            featureUIComponent.SetUserExperienceText(feature.Value.userfriendliness.ToString());
            featureUIComponent.SetTimeCostsText(feature.Value.timeCost.ToString());
            featureUIComponent.SetFeatureUIHandler(this);
            featureUI.SetActive(true);

        }
    }*/

    public void GenerateAvailableFeatureUIList()
    {
        Debug.Log("generating available feature list");
        Debug.Log("pocet feature na clientovi: " + featureManager.GetAvailableFeatures().Count);
        foreach (KeyValuePair<string, Feature> feature in featureManager.GetAvailableFeatures())
        {
            Debug.Log("vygenerovana nova featura");
            GameObject featureUI = Instantiate(featureUIPrefab);
            featureUI.transform.SetParent(availableFeatureListContent.transform, false);
            FeatureUIComponentHandler featureUIComponent = featureUI.GetComponent<FeatureUIComponentHandler>();
            featureUIComponent.SetNameIDText(feature.Value.nameID);
            featureUIComponent.SetFunctionalityText(feature.Value.functionality.ToString());
            featureUIComponent.SetIntegrationText(feature.Value.integration.ToString());
            featureUIComponent.SetUserExperienceText(feature.Value.userfriendliness.ToString());
            featureUIComponent.SetTimeCostsText(feature.Value.timeCost.ToString());
            featureUIComponent.SetFeatureUIHandler(this);
            if (featureManager.GetOutsourcedFeatures().Contains(feature) == true)
            {
                featureUIComponent.SetCheckedForOutsourcing(true);
            }
            else
            {
                featureUIComponent.SetCheckedForOutsourcing(false);
            }
            featureUI.SetActive(true);
        }
    }

    public void UpdateAvailableFeatureUIList()
    {
        foreach (Transform child in availableFeatureListContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        GenerateAvailableFeatureUIList();
    }

    public void GenerateOutSourcedFeatureUIList()
    {
        foreach (KeyValuePair<string, Feature> feature in featureManager.GetOutsourcedFeatures())
        {
            GameObject featureUI = Instantiate(featureUIPrefab);
            featureUI.transform.SetParent(outsourcedFeatureListContent.transform, false);
            FeatureUIComponentHandler featureUIComponent = featureUI.GetComponent<FeatureUIComponentHandler>();
            featureUIComponent.SetNameIDText(feature.Value.nameID);
            featureUIComponent.SetFunctionalityText(feature.Value.functionality.ToString());
            featureUIComponent.SetIntegrationText(feature.Value.integration.ToString());
            featureUIComponent.SetUserExperienceText(feature.Value.userfriendliness.ToString());
            featureUIComponent.SetTimeCostsText(feature.Value.timeCost.ToString());
            featureUIComponent.SetFeatureUIHandler(this);
            featureUIComponent.SetCheckedForOutsourcing(true);
            featureUI.SetActive(true);
        }
    }

    public void UpdateOutsourcedFeatureUIList()
    {
        foreach (Transform child in outsourcedFeatureListContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        GenerateOutSourcedFeatureUIList();
    }

    public void AddFeatureForOutsourcing(string name)
    {
        featureManager.AddFeatureForOutsourcing(name);
    }

    public void RemoveFeatureForOutsourcing(string name)
    {
        featureManager.RemoveFeatureForOutsourcing(name);
    }
}
