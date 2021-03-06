﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
//using System.Linq;

public enum ContractState { Proposal, InNegotiations, Final, Accepted, Rejected, Completed, Terminated }

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
    private int riskSharingFee;
    private int trueDeliveryTime;
    private int riskSharingFeePaid;
    private int terminationFee;
    private int terminationFeePaid;
  

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
    public int GetContractTurn() { return turn; }
    public void SetContractHistory(List<string> history) { this.history = history; }
    public List<string> GetContractHistory () { return history; }
    public void SetContractRiskSharingFee(int riskSharingFee) { this.riskSharingFee = riskSharingFee; }
    public int GetContractRiskSharingFee() { return riskSharingFee; }
    public void SetTrueDeliveryTime(int trueDeliveryTime) { this.trueDeliveryTime = trueDeliveryTime; }
    public int GetTrueDeliveryTime() { return trueDeliveryTime; }
    public void SetRiskSharingFeePaid(int riskSharingFeePaid) { this.riskSharingFeePaid = riskSharingFeePaid; }
    public int GetRiskSharingFeePaid() { return riskSharingFeePaid;}
    public void SeTerminationFee(int terminationFee) { this.terminationFee = terminationFee; }
    public int GetTerminationFee() { return terminationFee; }
    public int GetTerminationFeePaid() { return terminationFeePaid; }



    //CONSTRUCTOR
    public Contract(string contractID, string gameID, string providerID, string developerID, Feature feature, ContractState state, int turn, int delivery, int price, string[] history, int riskSharingFee, int trueDeliveryTime)
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
        this.riskSharingFee = riskSharingFee;
        this.trueDeliveryTime = trueDeliveryTime; //0 by creation
        riskSharingFeePaid = 0;
        terminationFee = 500000;
        terminationFeePaid = 0;
    }

    public Contract(string contractID, string gameID, string providerID, string developerID, Feature feature, ContractState state, int turn, int delivery, int price, string[] history, int riskSharingFee, int trueDeliveryTime, int riskSharingFeePaid, int terminationFeePaid)
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
        this.history = new List<string>(history);
        this.riskSharingFee = riskSharingFee;
        this.trueDeliveryTime = trueDeliveryTime; //0 by creation
        this.riskSharingFeePaid = riskSharingFeePaid;
        terminationFee = 500000;
        this.terminationFeePaid = terminationFeePaid;
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
    public void AssignTerminationFeePaid()
    {
        terminationFeePaid = terminationFee;
    }
}
