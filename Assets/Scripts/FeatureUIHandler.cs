using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeatureUIHandler : MonoBehaviour
{
    public GameObject featureUIPrefab;
    public GameObject availableFeatureListContent;
    public GameObject outsourcedFeatureListContent;
    
    private FeatureManager featureManager;

    // Awake = syncvar not synced, called before start; dont use - not suitable for networking!
    private void Awake()
    {  }

    private void Start()
    {
        featureManager = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject().GetComponent<FeatureManager>();
        featureManager.SetFeatureUIHandler(this);
        GenerateAvailableFeatureUIList();
        UpdateOutsourcedFeatureUIList();
        //GenerateDropdownOptions(); - -NAHRAĎ!!!!!!!!!!!!!!!!!!!!!!!!!!

    }

    public void GenerateAvailableFeatureUIList()
    {
        foreach (KeyValuePair<string, Feature> feature in featureManager.GetAvailableFeatures())
        {
            GameObject featureUIComponent = Instantiate(featureUIPrefab);
            featureUIComponent.transform.SetParent(availableFeatureListContent.transform, false);
            FeatureUIComponentHandler featureUIComponentHandler = featureUIComponent.GetComponent<FeatureUIComponentHandler>();
            featureUIComponentHandler.SetNameIDText(feature.Value.nameID);
            featureUIComponentHandler.SetFunctionalityText(feature.Value.functionality.ToString());
            featureUIComponentHandler.SetIntegrationText(feature.Value.integrability.ToString());
            featureUIComponentHandler.SetUserExperienceText(feature.Value.userfriendliness.ToString());
            featureUIComponentHandler.SetTimeCostsText(feature.Value.timeCost.ToString());
            featureUIComponentHandler.SetFeatureUIHandler(this);
            if (featureManager.GetOutsourcedFeatures().Contains(feature) == true)
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
            GameObject featureUIComponent = Instantiate(featureUIPrefab);
            featureUIComponent.transform.SetParent(outsourcedFeatureListContent.transform, false);
            FeatureUIComponentHandler featureUIComponentHnadler = featureUIComponent.GetComponent<FeatureUIComponentHandler>();
            featureUIComponentHnadler.SetNameIDText(feature.Value.nameID);
            featureUIComponentHnadler.SetFunctionalityText(feature.Value.functionality.ToString());
            featureUIComponentHnadler.SetIntegrationText(feature.Value.integrability.ToString());
            featureUIComponentHnadler.SetUserExperienceText(feature.Value.userfriendliness.ToString());
            featureUIComponentHnadler.SetTimeCostsText(feature.Value.timeCost.ToString());
            featureUIComponentHnadler.SetFeatureUIHandler(this);
            featureUIComponentHnadler.SetCheckedForOutsourcing(true);
            featureUIComponent.SetActive(true);
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
