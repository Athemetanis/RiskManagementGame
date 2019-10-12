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

    public int individualCustomers;
    public int businessCustomers;
    public int enterpriseCustomers;

    public Feature(string name, int functionality, int integrability, int userfriendliness, int timeCost, int difficulty )
    {
        this.nameID = name;
        this.functionality = functionality;
        this.userfriendliness = userfriendliness;
        this.integrability = integrability;
        this.timeCost = timeCost;
        this.difficulty = difficulty;
        this.enterpriseCustomers = (int)System.Math.Round(((float)functionality * 0.5f), System.MidpointRounding.AwayFromZero);
        this.businessCustomers = (int)System.Math.Round(((float)integrability * 50), System.MidpointRounding.AwayFromZero);
        this.individualCustomers = (int)System.Math.Round(((float)userfriendliness * 1000), System.MidpointRounding.AwayFromZero);
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
            return (this.nameID == f.nameID) && (this.functionality == f.functionality) && (this.userfriendliness == f.userfriendliness) && (this.integrability == f.integrability) && (this.timeCost == f.timeCost) && (this.difficulty == f.difficulty) && (this.individualCustomers == f.individualCustomers) && (this.businessCustomers == f.businessCustomers) && (this.enterpriseCustomers) == (f.enterpriseCustomers);
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
        return (this.nameID == f.nameID) && (this.functionality == f.functionality) && (this.userfriendliness == f.userfriendliness) && (this.integrability == f.integrability) && (this.timeCost == f.timeCost) && (this.difficulty == f.difficulty) && (this.individualCustomers == f.individualCustomers) && (this.businessCustomers == f.businessCustomers) && (this.enterpriseCustomers) == (f.enterpriseCustomers);
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
    //private SyncListString featuresForProposal = new SyncListString();

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
    {                                          //name, func, ui, int, t, diff,  ent, bus, ind
        GenerateAllFeatures();
        if(availableFeatures.Count == 0) 
        {
            SetupDefaultValues();
        }

        
    }

    public override void OnStartClient()
    {
        availableFeatures.Callback += OnChangeFeatureAvailable;
        outsourcedFeatures.Callback += OnChangeFeatureOutsourced;
        inDevelopmentFeatures.Callback += OnChangeFeatureInDevelopmet;
        doneFeatures.Callback += OnChangeFeatureDone;
    }

    //METHODS
    [Server]
    public void GenerateAllFeatures()
    {
        allFeatures.Add("feature1", new Feature("feature1", 10, 0, 0, 150, 1));
        allFeatures.Add("feature2", new Feature("feature2", 1, 1, 8, 250, 1));
        allFeatures.Add("feature3", new Feature("feature3", 0, 8, 0, 200, 1));
        allFeatures.Add("feature4", new Feature("feature4", 10, 2, 0, 170, 1));
        allFeatures.Add("feature5", new Feature("feature5", 0, 2, 5, 190, 1));
        allFeatures.Add("feature6", new Feature("feature6", 0, 8, 2, 270, 1));
    }

    [Server]
    public void SetupDefaultValues()
    {
        foreach (KeyValuePair<string, Feature> feature in allFeatures)
        {
            availableFeatures.Add(feature);
        }
    }

    [Client]
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
    }
    [Server]
    public void RemoveFeatureForOutsourcingServer(string name)
    {
        if (outsourcedFeatures.ContainsKey(name) == true)
        {
            outsourcedFeatures.Remove(name);
        }
    }
    [Client]
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
    }
    [Client]
    public void AddFeatureInDevelopment(string name)
    {
        CmdAddFeatureInDevelopment(name);
    }
    [Command]
    public void CmdAddFeatureInDevelopment(string name)
    {
        if (inDevelopmentFeatures.ContainsKey(name) == false)
        {
            inDevelopmentFeatures.Add(allFeatures[name].nameID, allFeatures[name]);
        }

    }
    [Client]
    public void RemoveFeatureInDevelopment(string name)
    {
        CmdRemoveFeatureInDevelopmet(name);
    }
    [Command]
    public void CmdRemoveFeatureInDevelopmet(string name)
    {
        if (inDevelopmentFeatures.ContainsKey(name) == true)
        {
            inDevelopmentFeatures.Remove(name);
        }
    }
    
    [Client]
    public void AddFeatureToDone(string name)
    {
        if(doneFeatures.ContainsKey(name) == false)
        {
            doneFeatures.Add(allFeatures[name].nameID, allFeatures[name]);
        }
    }
    [Command]
    public void CmdAddFeatureToDone(string name)
    {
        if (doneFeatures.ContainsKey(name) == false)
        {
            doneFeatures.Add(allFeatures[name].nameID, allFeatures[name]);
        }
    }


    [Server]
    public void AddFeatureInDevelopmentServer(string name)  //call only from server
    {
        if (inDevelopmentFeatures.ContainsKey(name) == false)
        {
            inDevelopmentFeatures.Add(allFeatures[name].nameID, allFeatures[name]);
            UpdateAviableFeatures();
        }

    }
    [Server]
    public void RemoveFeatureInDevelopmentServer(string name)
    {
        if (inDevelopmentFeatures.ContainsKey(name) == true)
        {
            inDevelopmentFeatures.Remove(name);
        }
    }
    [Server]
    public void AddFeatureToDoneServer(string name)
    {
        if (doneFeatures.ContainsKey(name) == false)
        {
            doneFeatures.Add(allFeatures[name].nameID, allFeatures[name]);
            UpdateAviableFeatures();
        }
    }
    [Server]
    public void AddFeatureToAvailableServer(string name)
    {
        if(availableFeatures.ContainsKey(name) != true)
        {
            availableFeatures.Add(allFeatures[name].nameID, allFeatures[name]);
        }
    }

    [Client]
    public void RemoveFeatureInDevelopmentClient(string name)
    {
        if (inDevelopmentFeatures.ContainsKey(name) == true)
        {
            inDevelopmentFeatures.Remove(name);
        }
    }
    [Client]
    public void AddFeatureToDoneClient(string name)
    {
        if (doneFeatures.ContainsKey(name) == false)
        {
            doneFeatures.Add(allFeatures[name].nameID, allFeatures[name]);
            UpdateAviableFeatures();
        }
    }
    [Client]
    public void AddFeatureToAvailableClient(string name)
    {
        if (availableFeatures.ContainsKey(name) != true)
        {
            availableFeatures.Add(allFeatures[name].nameID, allFeatures[name]);
        }
    }


    
    public void UpdateAviableFeatures()
    {
        List<Feature> temp = new List<Feature>(availableFeatures.Values);
        foreach (Feature feature in temp)
        {
            if (inDevelopmentFeatures.ContainsKey(feature.nameID) || doneFeatures.ContainsKey(feature.nameID))
            {
                availableFeatures.Remove(feature.nameID);
            }
        }
    }

    /* [Client]
     public void UpdateAvailableFeatures()
     {
         CmdUpdateAvailableFeatures();
     }
     [Command]
     public void CmdUpdateAvailableFeatures()
     {
         List<Feature> temp = new List<Feature>(availableFeatures.Values);
         foreach(Feature feature in temp)
         {
             if (inDevelopmentFeatures.ContainsKey(feature.nameID) || doneFeatures.ContainsKey(feature.nameID))
             {
                 availableFeatures.Remove(feature.nameID);
             }
         }
     }
     //HOOKS
     */

    public void OnChangeFeatureAvailable(SyncDictionaryFeatures.Operation op, string key, Feature feature)
    {
        if (featureUIHandler != null)
        {
            featureUIHandler.UpdateFeatureUIList();
        }
    }
   

    public void OnChangeFeatureOutsourced(SyncDictionaryFeatures.Operation op, string key, Feature feature) //here update all gui elements if they exist
    {
        if (featureUIHandler != null)
        {
            featureUIHandler.UpdateOutsourcedFeatureUIList();
            featureUIHandler.UpdateFeatureUIList();
        }
        if (contractUIHandler != null)
        {
            contractUIHandler.UpdateFeatureDropdownOptions(new List<string>(outsourcedFeatures.Keys));
        }
    }

    public void OnChangeFeatureInDevelopmet(SyncDictionaryFeatures.Operation op, string key, Feature feature)
    {
        if (featureUIHandler != null)
        {
            featureUIHandler.UpdateFeatureUIList();
        }
    }

    public void OnChangeFeatureDone(SyncDictionaryFeatures.Operation op, string key, Feature feature)
    {
        if (featureUIHandler != null)
        {
            featureUIHandler.UpdateFeatureUIList();
        }
    }

}
