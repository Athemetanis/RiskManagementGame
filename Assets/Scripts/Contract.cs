using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum ContractState { Proposal, InNegotiations, Final, Accepted, Rejected, Done }


public class Contract : NetworkBehaviour {

    //VARIABLES
    [SyncVar]
    private string contractID;
    [SyncVar]
    private string gameID;
    [SyncVar]
    private string developerID;
    [SyncVar]
    private string providerID;
    [SyncVar]
    private Feature feature;

    [SyncVar(hook = "OnChangeState")]
    private ContractState state;
    [SyncVar(hook = "OnChangeDelivery")]
    private int delivery;                    //delivery of feature in days
    [SyncVar(hook = "OnChangePrice")]
    private int price;
    [SyncVar(hook = "OnChangeTurn")] 
    private int turn;                       //if ODD - developer's turn    if EVEN - provider's turn
    
    private SyncListString historyList = new SyncListString();

    //NONSYNCED VAR
    private ContractManager contractManager;
    //GETTERS & SETTERS

    public void SetDeveloperID(string developerID) { this.developerID = developerID; }
    public string GetDeveloperID() { return developerID; }
    public void SetProviderID(string providerID) { this.providerID = providerID; }
    public string GetProviderID() { return providerID; }
    public void SetContractId(string contractID) { this.contractID = contractID; }
    public string GetContractID() { return contractID; }
    public void SetState(ContractState state) { this.state = state; }
    public ContractState GetState() { return state; }
    public void SetDelivery(int delivery) { this.delivery = delivery; }
    public int GetDelivery() { return delivery; }
    public void SetPrice(int price) { this.price = price; }
    public int GetPrice() { return price; }
    public void SetHistory() { }
    public SyncListString GetHistroy() { return historyList; }

    public void SetContractManager(ContractManager contractManager) { this.contractManager = contractManager; }


    ///pri starte by sa mala pridať do listu developerovi a providerovi     //pri vytvorení developerom ju normálne hneď pridám do jeho listu zmlúv? 
    //nie...tento objekt sa instanciuje na serveri? Aby sa mohli premenné potom po sieti synchronizovať ?????
    private void Start()
    {

        GameHandler.allPlayers[gameID][developerID].GetComponent<ContractManager>().TryToAddContractToMyContracts(contractID, this);
        GameHandler.allPlayers[gameID][providerID].GetComponent<ContractManager>().TryToAddContractToMyContracts(contractID, this);
          
    }
   

    //HOOKS
    
    public void OnChangePrice(int price)
    {
        historyList.Add("Delivery changed from " + this.price + " to " + price);
        this.price = price;
    }       
    public void OnChangeDelivery(int delivery)
    {
        historyList.Add("Delivery changed from " + this.delivery + " to " + delivery);
        this.delivery = delivery;
    }
    public void OnChangeTurn(int turn) //ak som na rade ja, vyhoĎ mi upozornenie
    {   
        historyList.Add("-------------" + turn + "-------------");
        this.turn = turn;
        contractManager.SetNotice(this.turn);
    }
    public void OnChangeState(ContractState state)  //najprv to zmenim v gui -klik send -  trigger zmenu hodnot na serveri - aktualizuje sa obom hračom gui ak existuje - ak je moje kolo - upozornenie!
    {   
        historyList.Add("State changed from " + this.state + " to " + state);
        this.state = state;
    }

    //METHODS
    
}
