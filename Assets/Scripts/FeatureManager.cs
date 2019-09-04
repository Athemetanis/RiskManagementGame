using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public struct Feature 
{
    public string nameID;
    public int functionality;
    public int userfriendliness;
    public int integrability;
    public int timeCost;
    public int difficulty;

    public Feature(string name, int functionality, int userfriendliness, int integrability, int timeCost, int difficulty)
    {
        this.nameID = name;
        this.functionality = functionality;
        this.userfriendliness = userfriendliness;
        this.integrability = integrability;
        this.timeCost = timeCost;
        this.difficulty = difficulty;
    }

    public override bool Equals(object obj)
    {                   
        if (Object.ReferenceEquals(obj, null))
        {
            return false;
        }
        if (Object.ReferenceEquals(this, obj))
        {
            return true;
        }
        if (this.GetType() != obj.GetType())
        {
            return false;
        }
        if(obj is Feature)
        {
            Feature f = (Feature)obj;
            return (this.nameID == f.nameID) && (this.functionality == f.functionality) && (this.userfriendliness == f.userfriendliness) && (this.integrability == f.integrability) && (this.timeCost == f.timeCost) && (this.difficulty == f.difficulty);
        }
        else
        {
            return false;
        }
    }

    public bool Equals(Feature f)
    {
        if (Object.ReferenceEquals(f, null))
        {
            return false;
        }
        if (Object.ReferenceEquals(this, f))
        {
            return true;
        }
        if (this.GetType() != f.GetType())
        {
            return false;
        }
        return (this.nameID == f.nameID) && (this.functionality == f.functionality) && (this.userfriendliness == f.userfriendliness) && (this.integrability == f.integrability) && (this.timeCost == f.timeCost) && (this.difficulty == f.difficulty);
    }

    public override int GetHashCode()
    {
        var hashCode = 1047090843;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(nameID);
        hashCode = hashCode * -1521134295 + functionality.GetHashCode();
        hashCode = hashCode * -1521134295 + userfriendliness.GetHashCode();
        hashCode = hashCode * -1521134295 + integrability.GetHashCode();
        hashCode = hashCode * -1521134295 + timeCost.GetHashCode();
        hashCode = hashCode * -1521134295 + difficulty.GetHashCode();
        return hashCode;
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
    private SyncDictionaryFeatures availableFeatures = new SyncDictionaryFeatures();
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
    public override void OnStartServer() //lists are then synchronized on clients
    {
        allFeatures.Add("feature1", new Feature("feature1", 10, 0, 0, 150, 1));
        allFeatures.Add("feature2", new Feature("feature2", 1, 8, 1, 250, 1));
        allFeatures.Add("feature3", new Feature("feature3", 0, 0, 8, 200, 1));
        allFeatures.Add("feature4", new Feature("feature4", 10, 0, 2, 170, 1));
        allFeatures.Add("feature5", new Feature("feature5", 0, 5, 2, 190, 1));
        allFeatures.Add("feature6", new Feature("feature6", 0, 2, 8, 270, 1));
      
        foreach(KeyValuePair<string, Feature> feature in allFeatures)
        {
            availableFeatures.Add(feature);
        }
    }

    public override void OnStartClient()
    {
        availableFeatures.Callback += OnChangeFeatureAvailable;
        outsourcedFeatures.Callback += OnChangeFeatureOutsourced;
           
    }

    public void OnChangeFeatureAvailable(SyncDictionaryFeatures.Operation op, string key, Feature feature)
    {
        if (featureUIHandler != null)
        {
            featureUIHandler.UpdateAvailableFeatureUIList();
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
