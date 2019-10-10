using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractUIHandler : MonoBehaviour
{   
    //VARIABLES

    public GameObject contractListContent;
    public GameObject rejectedContractListContent;
    public GameObject contractUIComponentPrefab;
    public GameObject contractPreviewUIPrefab;
    public GameObject contractContentUI;

    public GameObject developerListContent;
    public GameObject developerUIComponentPrefab;

    public GameObject contractOverviewUIComponentPrefab;
    public GameObject overviewContractListContent;

    public GameObject NotificationImage;

    public Toggle ContractTabToggle;
    //public GameObject featureOutsourcingListContent;

    //public GameObject contractProposalPrefab; //zatiaľ nepoužívaš - budeš vôbec??? áno!!!
    
    
    //UI elements
    public Dropdown selectedDeveloperDropdown;
    public Dropdown selectedFeatureDropdown;
    public Button createContractButton;


    //REFERENCES FOR OTHER MANAGERS
    private GameObject myPlayerDataObject;
    private ContractManager contractManager;
    private FeatureManager featureManager;
    private FirmManager firmManager;
    private ScheduleManager scheduleManager;
    private MarketingManager marketingManager;


    // Start is called before the first frame update
    void Start()
    {
        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        contractManager = myPlayerDataObject.GetComponent<ContractManager>();
        contractManager.SetContractUIHandler(this);


        firmManager = myPlayerDataObject.GetComponent<FirmManager>();
        scheduleManager = myPlayerDataObject.GetComponent<ScheduleManager>();


        if (contractManager.GetPlayerRole() == PlayerRoles.Provider)
        {
            featureManager = myPlayerDataObject.GetComponent<FeatureManager>();
            featureManager.SetContractUIHandler(this);
            featureManager.SetContractManager(contractManager);
            marketingManager = myPlayerDataObject.GetComponent<MarketingManager>();
            marketingManager.SetContractUIHandler(this);

            GenerateFeatureDropdownOptions(new List<string>(featureManager.GetOutsourcedFeatures().Keys));
            GenerateDeveloperDropdownOptions();
            UpdateDeveloperFirmList(GameHandler.allGames[contractManager.GetGameID()].GetListDeveloperFirmNameDescription());
            UpdateUIContractListsContents();
            UpdateContractOverview();
        }
        else  //som developer
        {
            UpdateUIContractListsContents();
        }
    }

    //METHODS
    public void CreateContract()
    {
        contractManager.CreateContract(selectedDeveloperDropdown.options[selectedDeveloperDropdown.value].text, selectedFeatureDropdown.options[selectedFeatureDropdown.value].text);
    }
    public void SendBackContract(string contractID, string price, string delivery, string riskSharingFee)
    {
        contractManager.ModifyContract(contractID, int.Parse(price), int.Parse(delivery), int.Parse(riskSharingFee));
    }
    public void AcceptContract(string contractID)
    {
        Contract contract = contractManager.GetMyContracts()[contractID];
        if (contract.GetContractState() == ContractState.InNegotiations)
        {
            contractManager.FinalContract(contractID);
        }
        else
        {
            contractManager.AcceptContract(contractID);
        }

    }
    public void RefuseContract(string contractID)
    {
        contractManager.RejectContract(contractID);
    }

    public void ContracNotificationON()
    {
        NotificationImage.SetActive(true);
        if(ContractTabToggle.isOn == true)
        {
            StartCoroutine(Wait5SecondsAndDisable());
        }
    }

    public void ContractNotificationOFF() //trigered by UI when contractTabActivated or ked je contract tab aktivne a vyprsi urcity casovy limit? 
    {
        if (NotificationImage.activeSelf == true)
        {
            NotificationImage.SetActive(false);
        }

    }

    IEnumerator Wait5SecondsAndDisable()
    {
        yield return new WaitForSeconds(5);
        ContractNotificationOFF();
    }

    //GENERATING & UPDATING UI ELEMENTS

        //provider only!!!
    public void GenerateDeveloperDropdownOptions()
    {
        selectedDeveloperDropdown.ClearOptions();
        selectedDeveloperDropdown.AddOptions(firmManager.GetDeveloperList());
    }
    public void UpdateDeveloperDropdownOptions(List<string> developerFirmList)
    {
        selectedDeveloperDropdown.ClearOptions();
        selectedDeveloperDropdown.AddOptions(developerFirmList);
    }

    public void GenerateFeatureDropdownOptions(List<string> featureOptions)
    {
        selectedFeatureDropdown.ClearOptions();
        selectedFeatureDropdown.AddOptions(featureOptions);
    }
    public void UpdateFeatureDropdownOptions(List<string> FeatureOptions)
    {
        GenerateFeatureDropdownOptions(FeatureOptions);
    }
   
    public void UpdateDeveloperFirmList(Dictionary<string, string> developerFirmNameDescription)
    {
        Debug.Log("idem updatovat developerov");

        foreach(Transform child in developerListContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (KeyValuePair<string, string> developerFirm in developerFirmNameDescription)
        {
            Debug.Log("vytvaram prveho developera");
            GameObject developerUIComponent = Instantiate(developerUIComponentPrefab);
            developerUIComponent.transform.SetParent(developerListContent.transform, false);
            DeveloperUIComponentHandler developerUIComponentHandler = developerUIComponent.GetComponent<DeveloperUIComponentHandler>();
            developerUIComponentHandler.SetFirmName(developerFirm.Key);
            developerUIComponentHandler.SetFirmDescription(developerFirm.Value);
            developerUIComponent.SetActive(true);
        }
      
    }

    public void EnableContractCreation() //called from UI
    {
        if (selectedFeatureDropdown.options.Count != 0 && selectedDeveloperDropdown.options.Count != 0)
        {
            createContractButton.interactable = true;
        }
        else
        {
            createContractButton.interactable = false;
        }
    }

    public void UpdateContractOverview()
    {   
        foreach(Transform child in overviewContractListContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        Dictionary<string, Contract> myContracts = contractManager.GetMyContracts();
        foreach (Contract contract in myContracts.Values)
        {   if((contract.GetContractState() == ContractState.InNegotiations) || (contract.GetContractState() == ContractState.Final) || (contract.GetContractState() == ContractState.Accepted))
            {
                GameObject contractOverviewUIComponent = Instantiate(contractOverviewUIComponentPrefab);
                contractOverviewUIComponent.transform.SetParent(overviewContractListContent.transform, false);
                ContractOverviewUIComponentHandler contractOverviewUIComponentHandler = contractOverviewUIComponent.GetComponent<ContractOverviewUIComponentHandler>();
                contractOverviewUIComponentHandler.SetUpContractOverview(contract.GetContractID(), contract.GetContractFeature().nameID, contract.GetContractDelivery(), contract.GetContractPrice(), contract.GetContractFeature().individualCustomers, contract.GetContractFeature().businessCustomers, contract.GetContractFeature().enterpriseCustomers, marketingManager.GetIndividualsPrice(), marketingManager.GetBusinessPrice(), marketingManager.GetEnterprisePrice());
            }
        }
    }


    //provider & developer

    public void UpdateUIContractListsContents()
    {
        Debug.Log("ResetingContractUIList");
        Dictionary<string, Contract> myContracts = contractManager.GetMyContracts();

        foreach (Transform child in contractListContent.transform)
        { GameObject.Destroy(child.gameObject); }

        foreach (Contract contract in myContracts.Values)
        {   
            GameObject contractUIComponent = Instantiate(contractUIComponentPrefab);
            contractUIComponent.transform.SetParent(contractListContent.transform, false);
            ContractUIComponentHandler contractUIComponentHandler = contractUIComponent.GetComponent<ContractUIComponentHandler>();
            contractUIComponentHandler.SetContractUIHandler(this);
            contractUIComponentHandler.SetContractIDText(contract.GetContractID());
            contractUIComponentHandler.SetStatus(contract.GetContractState().ToString());

            //PROVIDER
            if (contractManager.GetPlayerRole() == PlayerRoles.Provider)
            {
                //Debug.Log(contractManager.GetPlayerRole());
                //Debug.Log(contract.GetContractTrun());
                contractUIComponentHandler.SetPartnersNameText(contractManager.GetFirmName(contract.GetDeveloperID()));
              
                if (contract.GetContractTurn() % 2 == 0)  //providers turn
                {
                    contractUIComponentHandler.SetPlayerTurnText("Your turn");
                    contractUIComponentHandler.DisableDetailButton();
                }
                else
                {
                    contractUIComponentHandler.SetPlayerTurnText("Developers turn");
                    contractUIComponentHandler.DisableEditButton();
                }
                
            }
            else //DEVELOPER
            {
                //Debug.Log(contractManager.GetPlayerRole());
                //Debug.Log(contract.GetContractTrun());
                //Debug.Log(contract.GetProviderID());
                contractUIComponentHandler.SetPartnersNameText(contractManager.GetFirmName(contract.GetProviderID()));
               
                if (contract.GetContractTurn() % 2 == 0) //providers turn
                {
                    contractUIComponentHandler.SetPlayerTurnText("ProvidersTurn");
                    contractUIComponentHandler.DisableEditButton();
                }
                else
                {
                    contractUIComponentHandler.SetPlayerTurnText("Your Turn");
                    contractUIComponentHandler.DisableDetailButton();
                }
            }
            if (contract.GetContractState() == ContractState.Accepted || contract.GetContractState() == ContractState.Done || contract.GetContractState() == ContractState.Rejected)
            {
                contractUIComponentHandler.DisableEditButton();
                contractUIComponentHandler.EnableDetailButtton();
                contractUIComponentHandler.SetPlayerTurnText("none");

            }
        }
    }

    public void GenerateContractPreview(string contractID)
    {
        Contract contract = contractManager.GetMyContracts()[contractID];
        GameObject contractPreviewUI = Instantiate(contractPreviewUIPrefab);
        contractPreviewUI.transform.SetParent(contractContentUI.transform, false);
        ContractPreviewUIHandler contractPreviewUIHandler = contractPreviewUI.GetComponent<ContractPreviewUIHandler>();
        contractPreviewUIHandler.SetContractUIHandler(this);
        contractPreviewUIHandler.SetContractIDText(contract.GetContractID());
        contractPreviewUIHandler.SetDeveloperFirmText(contractManager.GetFirmName(contract.GetDeveloperID()));
        contractPreviewUIHandler.SetProviderFirmText(contractManager.GetFirmName(contract.GetProviderID()));
        contractPreviewUIHandler.SetFeatureText(contract.GetContractFeature().nameID);
        contractPreviewUIHandler.SetDelivery(contract.GetContractDelivery());
        contractPreviewUIHandler.SetPrice(contract.GetContractPrice());
        contractPreviewUIHandler.SetState(contract.GetContractState());
        contractPreviewUIHandler.GenerateHistoryRecord(contract.GetContractHistory());
        contractPreviewUIHandler.SetRiskSharingFee(contract.GetContractRiskSharingFee());
        contractPreviewUIHandler.PreviewContract();
        if(scheduleManager != null) //Developer
        {
            if (scheduleManager.GetScheduleDevelopmentEndDay().ContainsKey(contractID))
            {
                int developmentEndDay = scheduleManager.GetScheduleDevelopmentEndDay()[contractID];
                if (developmentEndDay > 60)
                {
                    contractPreviewUIHandler.SetScheduleInfoText("SCHEDULE: Feature can not be completed in one quarter.");
                }
                contractPreviewUIHandler.SetScheduleInfoText("SCHEDULE: Contract's end day of developmet: " + developmentEndDay);
            }
            else
            {
                contractPreviewUIHandler.SetScheduleInfoText("SCHEDULE: Contract was not scheduled for development. ");
            }
        }
        else { contractPreviewUIHandler.DisableScheduleInfoText(); }
    }
    public void GenerateContractEditPreview(string contractID)
    {
        Contract contract = contractManager.GetMyContracts()[contractID];
        GameObject contractPreviewUI = Instantiate(contractPreviewUIPrefab);
        contractPreviewUI.transform.SetParent(contractContentUI.transform, false);
        ContractPreviewUIHandler contractPreviewUIHandler = contractPreviewUI.GetComponent<ContractPreviewUIHandler>();
        contractPreviewUIHandler.SetContractUIHandler(this);
        contractPreviewUIHandler.SetContractIDText(contract.GetContractID());
        contractPreviewUIHandler.SetDeveloperFirmText(contractManager.GetFirmName(contract.GetDeveloperID()));
        contractPreviewUIHandler.SetProviderFirmText(contractManager.GetFirmName(contract.GetProviderID()));
        contractPreviewUIHandler.SetFeatureText(contract.GetContractFeature().nameID);
        contractPreviewUIHandler.SetDelivery(contract.GetContractDelivery());
        contractPreviewUIHandler.SetPrice(contract.GetContractPrice());
        contractPreviewUIHandler.SetState(contract.GetContractState());
        contractPreviewUIHandler.GenerateHistoryRecord(contract.GetContractHistory());
        contractPreviewUIHandler.SetRiskSharingFee(contract.GetContractRiskSharingFee());

        if (contract.GetContractState() == ContractState.Proposal)
        {
            contractPreviewUIHandler.ProposalContract();
        }
        else if (contract.GetContractState() == ContractState.InNegotiations)
        {
            contractPreviewUIHandler.InNegotiationContract();
        }
        else if (contract.GetContractState() == ContractState.Final)
        {
            contractPreviewUIHandler.FinalContract();
        }
        if (scheduleManager != null) //Developer
        {
            if (scheduleManager.GetScheduleDevelopmentEndDay().ContainsKey(contractID))
            {
                int developmentEndDay = scheduleManager.GetScheduleDevelopmentEndDay()[contractID];
                if (developmentEndDay > 60)
                {
                    contractPreviewUIHandler.SetScheduleInfoText("SCHEDULE: Feature can not be completed in one quarter.");
                }else
                contractPreviewUIHandler.SetScheduleInfoText("SCHEDULE: Contract's end day of developmet: " + developmentEndDay);
            }
            else
            {
                contractPreviewUIHandler.SetScheduleInfoText("SCHEDULE: Contract was not scheduled for development. ");
            }
        }
        else { contractPreviewUIHandler.DisableScheduleInfoText(); }
    }



}
