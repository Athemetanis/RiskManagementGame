using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractUIComponentHandler : MonoBehaviour
{   
    //VARIABLES
    public Text partnersNameText;
    public Text contractIDText;
    public Text playersTurnText;
    public Text statusText;
    public Button editButton;
    public Button detailButton;

    private ContractUIHandler contractUIHandler;

    //GETTERS & SETTERS
    public void SetPartnersNameText(string partnersName) { partnersNameText.text = partnersName; }
    public void SetContractIDText(string contractID) { contractIDText.text = contractID; }
    public void SetPlayerTurnText(string playerTurn) { playersTurnText.text = playerTurn; }
    public void SetStatus(string status) { statusText.text = status; }
    public void SetContractUIHandler(ContractUIHandler contractUIHandler){ this.contractUIHandler = contractUIHandler; }
    
    //Methods
    public void ChangeEditButtonText(string newText)
    {
        editButton.GetComponentInChildren<Text>().text = newText;
    }
    public void DisableEditButton() 
    {
        editButton.gameObject.SetActive(false);
    }
    public void DisableDetailButton()
    {
        detailButton.gameObject.SetActive(false);
    }
    public void EnableDetailButtton()
    {
        detailButton.gameObject.SetActive(true);
    }
    public void EnableEditButtton()
    {
        editButton.gameObject.SetActive(true);
    }

    public void GenerateContractPreview()
    {
        contractUIHandler.GenerateContractPreview(contractIDText.text);
    }
    public void GenerateContractEditPreview()
    {
        contractUIHandler.GenerateContractEditPreview(contractIDText.text);
    }


}
