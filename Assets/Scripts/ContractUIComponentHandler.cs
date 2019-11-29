using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContractUIComponentHandler : MonoBehaviour
{   
    //VARIABLES
    public TextMeshProUGUI partnersNameText;
    public TextMeshProUGUI featureIDText;
    public TextMeshProUGUI contractIDText;
    public TextMeshProUGUI playersTurnText;
    public TextMeshProUGUI statusText;
    public Button editButton;
    public Button detailButton;
    public Button resultButton;
    
    

    private ContractUIHandler contractUIHandler;

    //GETTERS & SETTERS
    public void SetPartnersNameText(string partnersName) { partnersNameText.text = partnersName; }
    public void SetFeatureID(string featureID) { featureIDText.text = featureID; }
    public void SetContractIDText(string contractID) { contractIDText.text = contractID; }
    public void SetPlayerTurnText(string playerTurn) { playersTurnText.text = playerTurn; }
    public void SetStatus(string status)
    {
        if(status  == ContractState.Accepted.ToString())
        {
            statusText.color = Color.green;
        }
        if(status == ContractState.Rejected.ToString() || status == ContractState.Terminated.ToString())
        {
            statusText.color = Color.red;
        }
        statusText.text = status;
    }
    public void SetContractUIHandler(ContractUIHandler contractUIHandler){ this.contractUIHandler = contractUIHandler; }
    
    //Methods
    public void ChangeEditButtonText(string newText)
    {
        editButton.GetComponentInChildren<TextMeshProUGUI>().text = newText;
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
    public void EnableResultsButton()
    {
        resultButton.gameObject.SetActive(true);
    }
    public void DisableResultsButton()
    {
        resultButton.gameObject.SetActive(false);
    }

    public void GenerateContractPreview()
    {
        contractUIHandler.GenerateContractPreview(contractIDText.text);
    }
    public void GenerateContractEditPreview()
    {
        contractUIHandler.GenerateContractEditPreview(contractIDText.text);
    }
    public void GenerateContractResultsPreview()
    {
        contractUIHandler.GeneteContractResultsPreview(contractIDText.text);
    }

}
