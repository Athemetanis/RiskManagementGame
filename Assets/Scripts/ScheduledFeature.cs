using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduledFeature
{
    private string order;
    private string contractID;
    private string providerFirmID;
    private ContractState contractState;
    private Feature feature;
    private List<Vector3> graphPoints;
    private int[] graphDays;
    private int developmentTime;
    private int deliveryTime;

    //GETTERS & SETTERS
    public void SetOrder(string priority) { this.order = priority; }
    public string GetOrder() { return order; }
    //public void SetContractID(string contractID) { this.contractID = contractID; }    
    public string GetContractID() { return contractID; }
    //public void SetProviderFirmID(string providerFirmID) { this.providerFirmID = providerFirmID; }
    public string GetProviderFirmID() { return providerFirmID; }
    public ContractState GetContractState() { return contractState; }
    public void SetContractState(ContractState contractState) { this.contractState = contractState; }
    public Feature GetFeature() { return feature; }
    public void SetDeliveryTime(int deliveryTime) { this.deliveryTime = deliveryTime; }
    public void SetDevelopmentTime(int developmentTime) { this.developmentTime = developmentTime; }
    public int GetDevelopmentTime() { return developmentTime; }
    public int GetDeliveryTime() { return deliveryTime; }
    public List<Vector3> GetGraphPoints(){return graphPoints; }
    public void SetGraphPoints(Vector3[] graphPoints) { this.graphPoints = new List<Vector3>(graphPoints); }
    public void SetGraphDays(int[] startingGraphDay) { this.graphDays = startingGraphDay; }
    public int[] GetGraphDays() { return graphDays; }

    //CONSTRUCTOR
    public ScheduledFeature( string contractID, string providerFirm, ContractState contractState, Feature feature)
    {
        order = "none";
        this.contractID = contractID;
        this.providerFirmID = providerFirm;
        this.contractState = contractState;
        this.feature = feature;
        graphPoints = new List<Vector3>();
        graphDays = new int[11] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } ;
        developmentTime = 1;
        deliveryTime = 0;
    }

    public ScheduledFeature(string order, string contractID, string providerFirmID, ContractState contractState, Feature feature, Vector3[] graphPointsArray, int[] graphDays, int developmentTime, int deliveryTime)
    {
        this.order = order;
        this.contractID = contractID;
        this.providerFirmID = providerFirmID;
        this.contractState = contractState;
        this.feature = feature;
        graphPoints = new List<Vector3>(graphPointsArray);
        this.graphDays = graphDays;
        this.developmentTime = developmentTime;
        this.deliveryTime = deliveryTime;
    }
       
}
