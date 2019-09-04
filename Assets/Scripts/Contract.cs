using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
//using System.Linq;

public enum ContractState { Proposal, InNegotiations, Final, Accepted, Rejected, Done }

public class Contract
{   
    //VARIABLES
    private string contractID;
    private string gameID;
    private string developerID;
    private string providerID;
    private Feature feature;
    private ContractState state;
    private int delivery;
    private int price;
    private int turn;    // if ODD - developer's turn    if EVEN - provider's turn
    private List<string> history;
  

    //GETTERS & SETTERS
    public void SetContractId(string contractID) { this.contractID = contractID; }
    public string GetContractID() { return contractID; }
    public void SetContractGameID(string gameID) { this.gameID = gameID; }
    public string GetContractGameID() { return gameID; }
    public void SetDeveloperID(string developerID) { this.developerID = developerID; }
    public string GetDeveloperID() { return developerID; }
    public void SetProviderID(string providerID) { this.providerID = providerID; }
    public string GetProviderID() { return providerID; }
    public void SetContractFeature(Feature feature) { this.feature = feature; }
    public Feature GetContractFeature() { return feature; }
    public void SetContractState(ContractState state) { this.state = state; }
    public ContractState GetContractState() { return state; }
    public void SetContractDelivery(int delivery) { this.delivery = delivery; }
    public int GetContractDelivery() { return delivery; }
    public void SetContractPrice(int price){ this.price = price; }
    public int GetContractPrice() { return price; }
    public void SetContractTurn(int turn) { this.turn = turn; }
    public int GetContractTrun() { return turn; }
    public void SetContractHistory(List<string> history) { this.history = history; }
    public List<string> GetContractHistory () { return history; }


    //CONSTRUCTOR
    public Contract(string contractID, string gameID, string providerID, string developerID, Feature feature, ContractState state, int turn, int delivery, int price, string[] history)
    {
        this.contractID = contractID;
        this.gameID = gameID;
        this.providerID = providerID;
        this.developerID = developerID;
        this.feature = feature;
        this.state = state;
        this.turn = turn;
        this.delivery = delivery;
        this.price = price;
        this.history = new List<string> (history);
       
    }

    //METHODS
    public void AddHistoryRecord(string record)
    {
        history.Add(record);
    }

    public void MoveTurn()
    {
        turn = turn + 1;
    }
}
