using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public struct Feature 
{
    public string nameID;
    public int functionality;
    public int userfriendliness;
    public int integration;
    public int timeCost;
    public bool isDone;

    public Feature(string name, int functionality, int userfriendliness, int integration, int timeCost,  bool isDone)
    {
        this.nameID = name;
        this.functionality = functionality;
        this.userfriendliness = userfriendliness;
        this.integration = integration;
        this.timeCost = timeCost;
        this.isDone = isDone;
    }

}
public class FeatureManager : NetworkBehaviour
{
    //VARIABLES
    public class SyncDictionaryFeatures : SyncDictionary<string, Feature> { }

    private FeatureUIHandler featureUIHandler;

    private SyncDictionaryFeatures outsourcedFeatures = new SyncDictionaryFeatures();
    private SyncDictionaryFeatures allFeatures = new SyncDictionaryFeatures();
    private SyncDictionaryFeatures doneFeatures = new SyncDictionaryFeatures();
    private SyncListString featuresForProposal = new SyncListString();

    //GETTERS & SETTERS

    public void SetFeatureUIHandler(FeatureUIHandler featureUIHandler) { this.featureUIHandler = featureUIHandler; }
    public SyncDictionaryFeatures GetAllFeatures() { return allFeatures; }
    public SyncDictionaryFeatures GetOutsourcedFeatures() { return outsourcedFeatures; }
    public SyncDictionaryFeatures GetDoneFeatures() { return doneFeatures; }


    //METHODS  
    public override void OnStartServer() //list is then synchronized on clients
    {
        allFeatures.Add("feature1", new Feature("feature1", 10, 0, 0, 50, false));
        allFeatures.Add("feature2", new Feature("feature2", 0, 5, 0, 20, false));
        allFeatures.Add("feature3", new Feature("feature3", 0, 0, 8, 35, false));
        allFeatures.Add("feature4", new Feature("feature4", 10, 0, 0, 50, false));
        allFeatures.Add("feature5", new Feature("feature5", 0, 5, 0, 20, false));
        allFeatures.Add("feature6", new Feature("feature6", 0, 0, 8, 35, false));


    }

    public override void OnStartClient()
    {

        outsourcedFeatures.Callback += OnChangeFeatureOutsourced;

    }


    public void OnChangeFeatureOutsourced(SyncDictionaryFeatures.Operation op, string key, Feature feature) //here update all gui elements if they exist
    {
        if(featureUIHandler != null)
        {
            featureUIHandler.UpdateOutsourcedFeatureUIList();
        }
    }

    public void AddFeatureForOutsourcing(string name)
    {
        CmdAddFeatureForOutsourcing(name);
    }

    [Command]
    public void CmdAddFeatureForOutsourcing(string name)
    {
        if (outsourcedFeatures.ContainsKey(name) == false)
        {
            outsourcedFeatures.Add(allFeatures[name].nameID, allFeatures[name]);
        }
        if (featuresForProposal.Contains(name) == false)
        {
            featuresForProposal.Add(name);
        }
    }

    public void RemoveFeatureForOutsourcing(string name)
    {
        CmdRemoveFeatureForOutsourcing(name);
    }
    
    [Command]
    public void CmdRemoveFeatureForOutsourcing(string name)
    {
        if (outsourcedFeatures.ContainsKey(name))
        {
            outsourcedFeatures.Remove(allFeatures[name].nameID);
        }
        if (featuresForProposal.Contains(name))
        {
            featuresForProposal.Remove(name);
        }
           
    }



}
