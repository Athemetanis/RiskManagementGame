using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractPreviewUIHandler : MonoBehaviour
{
    //VARIABLES
    public Text contractIDText;
    public Text developerFirmText;
    public Text providerFirmText;
    public Text featureText;
    public InputField priceIF;
    public InputField deliveryIF;
    public GameObject historyContent;

    public Button refuseButton;
    public Button acceptButton;
    public Button sendBackButton;

    public GameObject historyTextUIComponentPrefab;

    private ContractUIHandler contractUIHandler;

    //GETTERS & SETTERS

    public void SetContractIDText(string contractID) { contractIDText.text = contractID; }
    public void SetDeveloperFirmText(string developerFirmName) { developerFirmText.text = developerFirmName; }
    public void SetProviderFirmText(string providerFirmName) { providerFirmText.text = providerFirmName; }
    public void SetFeatureText(string featureName) { featureText.text = featureName; }
    public void SetPrice(int price) { priceIF.text = price.ToString(); }
    public void SetDelivery(int delivery) { deliveryIF.text = delivery.ToString(); }

    public void SetContractUIHandler(ContractUIHandler contractUIHandler) { this.contractUIHandler = contractUIHandler; }

    void Start()
    {
        sendBackButton.interactable = false;
    }

    //METHODS
    public void PriceDeliveryChanged()
    {
        acceptButton.interactable = false;
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
            historyRecordUIComponent.GetComponent<Text>().text = historyRecord;

        }
    }


    public void DisableButtons()
    {
        refuseButton.gameObject.SetActive(false);
        acceptButton.gameObject.SetActive(false);
        sendBackButton.gameObject.SetActive(false);
    }

    public void DestroyGameObject()
    {
        Destroy(this.gameObject);
    }

    public void DisableAcceptButton()
    {
        acceptButton.gameObject.SetActive(false);
    }

    public void SendBackContract()
    {
        Debug.Log("sending changes");
        contractUIHandler.SendBackContract(contractIDText.text, priceIF.text, deliveryIF.text);
        DestroyGameObject();
    }

    public void AcceptContract()
    {
        contractUIHandler.AcceptContract(contractIDText.text);
        DestroyGameObject();
    }

    public void RefuseContract()
    {
        contractUIHandler.RefuseContract(contractIDText.text);
        DestroyGameObject();
    }

}
