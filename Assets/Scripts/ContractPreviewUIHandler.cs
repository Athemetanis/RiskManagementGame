using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContractPreviewUIHandler : MonoBehaviour
{
    //VARIABLES
    public TextMeshProUGUI contractIDText;
    public TextMeshProUGUI developerFirmText;
    public TextMeshProUGUI providerFirmText;
    public TextMeshProUGUI featureText;
    public TextMeshProUGUI stateText;
    public InputField priceIF;      
    public InputField deliveryIF;
    public GameObject historyContent;
    public InputField riskSharingFeeIF;

    public Button rejectButton;
    public Button acceptButton;
    public Button sendBackButton;
    public Button proposeButton;
    public Button modifyButton;
    public Button cancelChangesButton;
    public Button cancelProposingButton;
    public TextMeshProUGUI scheduleInfoText;
    public TextMeshProUGUI warningText;

    public TextMeshProUGUI developmentPricePerDay;
    public TextMeshProUGUI developmentPriceOverall;
    public TextMeshProUGUI developmentPriceOverallRecommended;

    //-------Results
    public TextMeshProUGUI agreedPriceText;
    public TextMeshProUGUI agreedDeliveryText;
    public TextMeshProUGUI agreedRiskSharingFeeText;
    public TextMeshProUGUI actualDeliveryText;
    public TextMeshProUGUI riskSharingFeePaidText;
    public TextMeshProUGUI terminationFeePaidText;

    public GameObject negotiationContainer;
    public GameObject resultContianer;

    public GameObject historyTextUIComponentPrefab;
    private ContractUIHandler contractUIHandler;

    private string price;
    private string delivery;
    private string riskSharingFee;

    //GETTERS & SETTERS

    public void SetContractIDText(string contractID) { contractIDText.text = contractID; }
    public void SetDeveloperFirmText(string developerFirmName) { developerFirmText.text = developerFirmName; }
    public void SetProviderFirmText(string providerFirmName) { providerFirmText.text = providerFirmName; }
    public void SetFeatureText(string featureName) { featureText.text = featureName; }
    public void SetPrice(int price) { this.price = price.ToString(); }
    public void SetDelivery(int delivery) { this.delivery = delivery.ToString(); }
    public void SetContractUIHandler(ContractUIHandler contractUIHandler) { this.contractUIHandler = contractUIHandler; }
    public void SetState(ContractState state) { stateText.text = state.ToString(); }
    public void SetScheduleInfoText(string scheduleInfo) { scheduleInfoText.text = scheduleInfo; }
    public void SetRiskSharingFee(int riskSharingFee) { this.riskSharingFee = riskSharingFee.ToString(); }

    public void SetAgreedPriceText(int agreedPrice) { agreedPriceText.text = agreedPrice.ToString("n0"); }
    public void SetAgreedDeliveryText(int agreedDelivery) { agreedDeliveryText.text = agreedDelivery.ToString("n0"); }
    public void SetAgreedRiskSharingFee(int riskSharingFee) { agreedRiskSharingFeeText.text = riskSharingFee.ToString("n0"); }
    public void SetActualDelivery(int actualDelivery) { actualDeliveryText.text = actualDelivery.ToString("n0"); }
    public void SetRiskSharingFeePaid(int riskSharingFeePaid) { riskSharingFeePaidText.text = riskSharingFeePaid.ToString("n0"); }
    public void SetTerminationFeePaid(int terminationFeePaid) { terminationFeePaidText.text = terminationFeePaid.ToString("n0"); }


    public void SetDevelopmentPricePerDay(string developmentPricePerDay) { this.developmentPricePerDay.text = developmentPricePerDay; }
    public void SetDevelopmentPriceOverall(string developmentPriceOverall) { this.developmentPriceOverall.text = developmentPriceOverall; }
    public void SetDevelopmentPriceRecommended(string developmentPriceOverallRecommended) { this.developmentPriceOverallRecommended.text = developmentPriceOverallRecommended; }

    //METHODS
    public void DestroyGameObject()
    {
        Destroy(this.gameObject);
    }

    public void ProposalContract()
    {
        rejectButton.gameObject.SetActive(true);
        proposeButton.gameObject.SetActive(true);
        sendBackButton.gameObject.SetActive(false);
        modifyButton.gameObject.SetActive(false);
        acceptButton.gameObject.SetActive(false);
        cancelChangesButton.gameObject.SetActive(false);
        cancelProposingButton.gameObject.SetActive(false);
        priceIF.gameObject.SetActive(false);
        deliveryIF.gameObject.SetActive(false);
        riskSharingFeeIF.gameObject.SetActive(false);
    }
    public void InNegotiationContract()
    {
        acceptButton.gameObject.SetActive(true);
        rejectButton.gameObject.SetActive(true);
        modifyButton.gameObject.SetActive(true);
        proposeButton.gameObject.SetActive(false);
        sendBackButton.gameObject.SetActive(false);
        cancelChangesButton.gameObject.SetActive(false);
        cancelProposingButton.gameObject.SetActive(false);
        priceIF.gameObject.SetActive(true);
        deliveryIF.gameObject.SetActive(true);
        riskSharingFeeIF.gameObject.SetActive(true);
        priceIF.text = price;
        deliveryIF.text = delivery;
        riskSharingFeeIF.text = riskSharingFee;
        priceIF.interactable = false;
        deliveryIF.interactable = false;
        riskSharingFeeIF.interactable = false;
    }
    public void FinalContract()
    {
        rejectButton.gameObject.SetActive(true);
        acceptButton.gameObject.SetActive(true);
        sendBackButton.gameObject.SetActive(false);
        modifyButton.gameObject.SetActive(false);
        proposeButton.gameObject.SetActive(false);
        cancelChangesButton.gameObject.SetActive(false);
        cancelProposingButton.gameObject.SetActive(false);
        priceIF.gameObject.SetActive(true);
        deliveryIF.gameObject.SetActive(true);
        riskSharingFeeIF.gameObject.SetActive(true);
        priceIF.text = price;
        deliveryIF.text = delivery;
        riskSharingFeeIF.text = riskSharingFee;
        priceIF.interactable = false;
        deliveryIF.interactable = false;
        riskSharingFeeIF.interactable = false;


    }
    public void ModifyProposePriceDelivery() //when modify/propose button clicked. Dont forget to set active coresponding CANCEL button in inspector - button OnClick!!!
    {   //setting up inputfields
        priceIF.gameObject.SetActive(true);
        deliveryIF.gameObject.SetActive(true);
        riskSharingFeeIF.gameObject.SetActive(true);
        priceIF.text = price;
        deliveryIF.text = delivery;
        riskSharingFeeIF.text = riskSharingFee;
        priceIF.interactable = true;
        deliveryIF.interactable = true;
        riskSharingFeeIF.interactable = true;
        modifyButton.gameObject.SetActive(false);
        proposeButton.gameObject.SetActive(false);
        sendBackButton.gameObject.SetActive(true);
        acceptButton.gameObject.SetActive(false);
    }
   
    public void CancelModifying()
    {
        priceIF.interactable = false;
        deliveryIF.interactable = false;
        riskSharingFeeIF.interactable = false;
        priceIF.text = price;
        deliveryIF.text = delivery;
        riskSharingFeeIF.text = riskSharingFee;
        modifyButton.gameObject.SetActive(true);
        cancelChangesButton.gameObject.SetActive(false);
        sendBackButton.gameObject.SetActive(false);
        acceptButton.gameObject.SetActive(true);
    }
    public void CancelProposing()
    {
        priceIF.gameObject.SetActive(false);
        deliveryIF.gameObject.SetActive(false);
        riskSharingFeeIF.gameObject.SetActive(false);
        proposeButton.gameObject.SetActive(true);
        cancelProposingButton.gameObject.SetActive(false);
        sendBackButton.gameObject.SetActive(false);
    }
     
    public void GenerateHistoryRecord(List<string> historyList)
    {
        foreach (Transform child in historyContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (string historyRecord in historyList)
        {
            GameObject historyRecordUIComponent = Instantiate(historyTextUIComponentPrefab);
            historyRecordUIComponent.transform.SetParent(historyContent.transform, false);
            historyRecordUIComponent.GetComponent<TextMeshProUGUI>().text = historyRecord;
        }
    }
    
    public void PreviewContract()
    {
        rejectButton.gameObject.SetActive(false);
        acceptButton.gameObject.SetActive(false);
        sendBackButton.gameObject.SetActive(false);
        modifyButton.gameObject.SetActive(false);
        proposeButton.gameObject.SetActive(false);
        cancelChangesButton.gameObject.SetActive(false);
        cancelProposingButton.gameObject.SetActive(false);
        priceIF.gameObject.SetActive(true);
        deliveryIF.gameObject.SetActive(true);
        riskSharingFeeIF.gameObject.SetActive(true);
        priceIF.text = price;
        deliveryIF.text = delivery;
        riskSharingFeeIF.text = riskSharingFee;
        priceIF.interactable = false;
        deliveryIF.interactable = false;
        riskSharingFeeIF.interactable = false;
    }
    public void SendBackContract()
    {
        contractUIHandler.SendBackContract(contractIDText.text, priceIF.text, deliveryIF.text, riskSharingFeeIF.text);
        DestroyGameObject();
    }
    public void AcceptContract()
    {
        contractUIHandler.AcceptContract(contractIDText.text);
        DestroyGameObject();
    }
    public void RejectContract()
    {
        contractUIHandler.RefuseContract(contractIDText.text);
        DestroyGameObject();
    }

    public void DisableScheduleInfoText() { scheduleInfoText.gameObject.SetActive(false); }
    public void CheckDeliveryInput()
    {
        if(int.Parse(deliveryIF.text) > 60)
        {
            deliveryIF.text = "60";
            warningText.gameObject.SetActive(true);

        }
        else
        {
            warningText.gameObject.SetActive(false);
        }
    }
    public void EnableNegotiationContainer() { negotiationContainer.SetActive(true); }
    public void DisableNegotiationContainer() { negotiationContainer.SetActive(false); }
    public void EnadleResultContainer() { resultContianer.SetActive(true); }
    public void DisableResultContainer() { resultContianer.SetActive(false); }
}
