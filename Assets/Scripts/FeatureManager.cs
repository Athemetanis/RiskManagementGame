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
    public bool checkedForOutsourcing;
    public bool isDone;

    public Feature(string name, int functionality, int userfriendliness, int integration, int timeCost, bool checkedForOutsourcing, bool isDone)
    {
        this.nameID = name;
        this.functionality = functionality;
        this.userfriendliness = userfriendliness;
        this.integration = integration;
        this.timeCost = timeCost;
        this.checkedForOutsourcing = checkedForOutsourcing;
        this.isDone = isDone;
    }

}
public class FeatureManager : NetworkBehaviour
{
    private SyncListSTRUCT<Feature> listOfFeatures = new SyncListSTRUCT<Feature>();
    
    private void Start()
    {
             
    }

    public override void OnStartServer() //list is then synchronized on clients
    {
        listOfFeatures.Add(new Feature("feature1", 10, 0, 0, 50, false, false));
        listOfFeatures.Add(new Feature("feature2", 0, 5, 0, 20, false, false));
        listOfFeatures.Add(new Feature("feature3", 10, 0, 8, 35, false, false));
        listOfFeatures.Add(new Feature("feature4", 10, 0, 0, 50, false, false));
        listOfFeatures.Add(new Feature("feature5", 0, 5, 0, 20, false, false));
        listOfFeatures.Add(new Feature("feature6", 10, 0, 8, 35, false, false));
    }

    private void Update()
    {
        
    }


}
