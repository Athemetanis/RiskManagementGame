﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeatureUIHandler : MonoBehaviour
{

    public GameObject featureUIPrefab;
    public GameObject allFeatureListContent;
    public GameObject outsourcedFeatureListContent;
    public Dropdown proposalFeatures;

    //public GameObject featurePrefab;

         

    private FeatureManager featureManager;

    // Start is called before the first frame update
    private void Awake()
    {
        featureManager = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject().GetComponent<FeatureManager>();
        featureManager.SetFeatureUIHandler(this);
    }

    
    private void Start()
    {
        GenerateOutSourcedFeatureUIList();
        GenerateAllFeatureUIList();

    }



    public void GenerateAllFeatureUIList()
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
    }

    public void UpdateAllFeatureUIList()
    {

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
