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

    public Feature(string name, int functionality, int userfriendliness, int integration, int timeCost)
    {
        this.nameID = name;
        this.functionality = functionality;
        this.userfriendliness = userfriendliness;
        this.integration = integration;
        this.timeCost = timeCost;
    }

}
public class FeatureManager : NetworkBehaviour
{
    //VARIABLES
    public class SyncDictionaryFeatures : SyncDictionary<string, Feature> { }

    private FeatureUIHandler featureUIHandler;
    private ContractManager contractManager;
    private ContractUIHandler contractUIHandler;

    private SyncDictionaryFeatures allFeatures = new SyncDictionaryFeatures();
    private SyncDictionaryFeatures availableFeatures;
    private SyncDictionaryFeatures outsourcedFeatures = new SyncDictionaryFeatures();
    private SyncDictionaryFeatures inDevelopmentFeatures = new SyncDictionaryFeatures();
    private SyncDictionaryFeatures doneFeatures = new SyncDictionaryFeatures();
    private SyncListString featuresForProposal = new SyncListString();

    //GETTERS & SETTERS
    public void SetFeatureUIHandler(FeatureUIHandler featureUIHandler) { this.featureUIHandler = featureUIHandler; }
    public void SetContractUIHandler(ContractUIHandler contractUIHandler) { this.contractUIHandler = contractUIHandler; }
    public void SetContractManager(ContractManager contractManager) { this.contractManager = contractManager; }
    public SyncDictionaryFeatures GetAllFeatures() { return allFeatures; }
    public SyncDictionaryFeatures GetAvailableFeatures() { return availableFeatures; }
    public SyncDictionaryFeatures GetOutsourcedFeatures() { return outsourcedFeatures; }
    public SyncDictionaryFeatures GetInDevelopmentFeatures() { return inDevelopmentFeatures; }
    public SyncDictionaryFeatures GetDoneFeatures() { return doneFeatures; }


    //METHODS  
    public override void OnStartServer() //list is then synchronized on clients
    {
        allFeatures.Add("feature1", new Feature("feature1", 10, 0, 0, 50));
        allFeatures.Add("feature2", new Feature("feature2", 0, 5, 0, 20));
        allFeatures.Add("feature3", new Feature("feature3", 0, 0, 8, 35));
        allFeatures.Add("feature4", new Feature("feature4", 10, 0, 0, 50));
        allFeatures.Add("feature5", new Feature("feature5", 0, 5, 0, 20));
        allFeatures.Add("feature6", new Feature("feature6", 0, 0, 8, 35));

        
        foreach(KeyValuePair<string, Feature> feature in allFeatures)
        {
            availableFeatures.Add(feature);
        }


        Debug.Log(availableFeatures.Count);
        Debug.Log(outsourcedFeatures.Count);
    }

    public override void OnStartClient()
    {
        availableFeatures.Callback += OnChangeFeatureAvailable;
        outsourcedFeatures.Callback += OnChangeFeatureOutsourced;
       // Debug.Log("feature manager spusteny na klientovi, pokus o feature list");
      //  OnChangeFeatureAvailable(SyncDictionary<string, Feature>.Operation.OP_ADD, "nove kluc", new Feature("feature1", 10, 0, 0, 50));
     
    }

    public void OnChangeFeatureAvailable(SyncDictionaryFeatures.Operation op, string key, Feature feature)
    {
        Debug.Log("List dostupnych feature sa zmenil - teraz vygenerovat nove UI");
        Debug.Log("pocet features teraz je: " + availableFeatures.Count);
        if (featureUIHandler != null)
        {
            featureUIHandler.UpdateAvailableFeatureUIList();
        }
        else
        {
            Debug.Log("ale featere handler neexistuje!!!!!");
        }
    }

    public void OnChangeFeatureOutsourced(SyncDictionaryFeatures.Operation op, string key, Feature feature) //here update all gui elements if they exist
    {
        if(featureUIHandler != null)
        {
            featureUIHandler.UpdateOutsourcedFeatureUIList();
            featureUIHandler.UpdateAvailableFeatureUIList();
        }
        if(contractUIHandler != null)
        {
            contractUIHandler.UpdateFeatureDropdownOptions(new List<string>(outsourcedFeatures.Keys));
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
