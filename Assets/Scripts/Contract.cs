using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum ContractState { Proposal, InNegotiations, Final, Accepted, Rejected, Done }


public class Contract : NetworkBehaviour {

    //VARIABLES

    [SyncVar(hook = "OnChangeDeveloperID")]
    private string developerID;
    [SyncVar]
    private string providerID;
    [SyncVar]
    private ContractState state;
    [SyncVar]
    private int delivery;           //delivery of feature in days
    [SyncVar]
    private int price;
    [SyncVar]
    //private string[] history = new string[0] ;       //history of changes


    private SyncListString historyList = new SyncListString();

    //GETTERS & SETTERS

    public void SetDeveloperID() { }
    public string GetDeveloperID() { return developerID; }
    public void SetProviderID() { }
    public string GetProviderID() { return providerID; }
    public void SetState() { }
    public ContractState GetState() { return state; }
    public void SetDelivery() { }
    public int GetDelivery() { return delivery; }
    public void SetPrice() { }
    public int GetPrice() { return price; }
    //public void SetHistory() { }
    public SyncListString GetHistroy() { return historyList; }

    
    ///pri starte by sa mala pridať do listu developerovi a providerovi     //pri vytvorení developerom ju normálne hneď pridám do jeho listu zmlúv? 
    //nie...tento objekt sa instanciuje na serveri? Aby sa mohli premenné potom po sieti synchronizovať ?????
    private void Start()  
    {
        
    }
   
    //HOOKS
    public void OnChangeDeveloperID(string developerID) { }
    public void OnChangeProviderID(string providerID) { }
    public void OnChangeState(ContractState state) { }
    public void OnChangeDelivery(int delivery) { }
    public void OnChangePrice(int price) { }

    //METHODS
    public void AddHistory(string changes)
    {
        historyList.Add(changes);
    }

}
