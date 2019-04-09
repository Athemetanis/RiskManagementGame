using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Feature 
{
    public string name;

    public int functionality;
    public int userfriendliness;
    public int integration;
    public int timeCost;
    public bool checkedForOutsourcing;

    public Feature(string name, int functionality, int userfriendliness, int integration, int timeCost, bool checkedForOutsourcing)
    {
        this.name = name;
        this.functionality = functionality;
        this.userfriendliness = userfriendliness;
        this.integration = integration;
        this.timeCost = timeCost;
        this.checkedForOutsourcing = checkedForOutsourcing;
    }

}
public class StructManager : MonoBehaviour
{
    public List<Feature> listOfFeatures = new List<Feature>();

    private bool feature1Check = false;
    private bool feature2Check = false;
    private bool feature3Check = false;

    private void Start()
    {
        listOfFeatures.Add(new Feature("feature1", 10, 0, 0, 50, false));
        listOfFeatures.Add(new Feature("feature2", 0, 5, 0, 20, false));
        listOfFeatures.Add(new Feature("feature3", 10, 0, 8, 35, false));
    }

    private void Update()
    {
        foreach (Feature feature in listOfFeatures)
        {
            if (feature.checkedForOutsourcing)
            {

            }


        }
    }






}
