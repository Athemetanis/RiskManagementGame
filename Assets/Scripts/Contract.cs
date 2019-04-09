using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum ContractState { Proposal, InNegotiations, Final, Accepted, Rejected, Done }


public class Contract : NetworkBehaviour {

    //VARIABLES
    [SyncVar]
    private string gameID;
    [SyncVar]
    private string developerID;
    [SyncVar]
    private string providerID;

    [SyncVar]
    private string contractID;

    [SyncVar(hook = "OnChangeState")]
    private ContractState state;
    [SyncVar]
    private int delivery;           //delivery of feature in days
    [SyncVar]
    private int price;
    [SyncVar(hook = "OnChangeTurn")] 
    private int turn;           //if EVEN - developer's turn    if ODD - provider's turn
    /*[SyncVar]
    private string playersTurn;     //values: "your turn" || "partners turn"*/


    //private string[] history = new string[0] ;       //history of changes


    private SyncListString historyList = new SyncListString();

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
    //public void SetHistory() { }
    public SyncListString GetHistroy() { return historyList; }


    ///pri starte by sa mala pridať do listu developerovi a providerovi     //pri vytvorení developerom ju normálne hneď pridám do jeho listu zmlúv? 
    //nie...tento objekt sa instanciuje na serveri? Aby sa mohli premenné potom po sieti synchronizovať ?????
    private void Start()
    {

        GameHandler.allPlayers[gameID][developerID].GetComponent<ContractManager>().TryToAddContractToMyContracts(contractID, this);
        GameHandler.allPlayers[gameID][providerID].GetComponent<ContractManager>().TryToAddContractToMyContracts(contractID, this);
          
    }
   

    //HOOKS
    public void OnChangeDeveloperID(string developerID) { }
    public void OnChangeProviderID(string providerID) { }       //najprv to zmenim v gui -klik send -  trigger zmenu hodnot na serveri - aktualizuje sa obom hračom gui ak existuje - ak je moje kolo - upozornenie!
    public void OnChangeState(ContractState state) { }
    public void OnChangeDelivery(int delivery) { }



    public void OnChangeTurn(int turn)
    {
        //ak som na rade ja, vyhoĎ mi upozornenie
    }

    //METHODS
    public void AddHistory(string changes)
    {
        historyList.Add(changes);
    }

    public void NewTurn()
    {
        turn++;
        
    }

    public void SendChangesOfContract()
    {
        //ke+d kliknem na tlčítko mali by sa zmeny prepísať na serveri
    }

}
