using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractUIHandler : MonoBehaviour
{   
    //VARIABLES

    public GameObject currentcontractListContent;
    public GameObject contractHistoryListContent;
    public GameObject contractUIComponentPrefab;
    public GameObject contractPreviewUIPrefab;
    public GameObject contractContentUI;

    public GameObject developerListContent;
    public GameObject providerListContent;
    public GameObject playerFirmUIComponentPrefab;






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
    private DeveloperAccountingManager developerAccountingManager;

    private string gameID;


    // Start is called before the first frame update
    void Start()
    {
        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        contractManager = myPlayerDataObject.GetComponent<ContractManager>();
        contractManager.SetContractUIHandler(this);
        gameID = myPlayerDataObject.GetComponent<PlayerData>().GetGameID();

        firmManager = myPlayerDataObject.GetComponent<FirmManager>();
        scheduleManager = myPlayerDataObject.GetComponent<ScheduleManager>();
        developerAccountingManager = myPlayerDataObject.GetComponent<DeveloperAccountingManager>(); 


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
            UpdateResultContractListContent();
            UpdateContractOverview();
        }
        else  //som developer
        {
            UpdateUIContractListsContents();
            UpdateResultContractListContent();
            UpdateProviderFirmList(GameHandler.allGames[contractManager.GetGameID()].GetListProviderFirmNameDescription());


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
    public void ContractNotificationOFF() //trigered by UI when contractTabActivated or ked je contract tab aktivne a vyprsi urcity casovy limit
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
        selectedDeveloperDropdown.AddOptions(GameHandler.allGames[gameID].GetDevelopersFirms());
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
        foreach(Transform child in developerListContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (KeyValuePair<string, string> developerFirm in developerFirmNameDescription)
        {
            GameObject developerUIComponent = Instantiate(playerFirmUIComponentPrefab);
            developerUIComponent.transform.SetParent(developerListContent.transform, false);
            DeveloperUIComponentHandler developerUIComponentHandler = developerUIComponent.GetComponent<DeveloperUIComponentHandler>();
            developerUIComponentHandler.SetFirmName(developerFirm.Key);
            developerUIComponentHandler.SetFirmDescription(developerFirm.Value);
            developerUIComponent.SetActive(true);
        }
      
    }

    public void UpdateProviderFirmList(Dictionary<string, string> providerFirmNameDescription)
    {
        foreach (Transform child in providerListContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (KeyValuePair<string, string> providerFirm in providerFirmNameDescription)
        {
            GameObject providerUIComponent = Instantiate(playerFirmUIComponentPrefab);
            providerUIComponent.transform.SetParent(providerListContent.transform, false);
            DeveloperUIComponentHandler providerUIComponentHandler = providerUIComponent.GetComponent<DeveloperUIComponentHandler>();
            providerUIComponentHandler.SetFirmName(providerFirm.Key);
            providerUIComponentHandler.SetFirmDescription(providerFirm.Value);
            providerUIComponent.SetActive(true);
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
                string developerName = contractManager.GetFirmName(contract.GetDeveloperID());

                contractOverviewUIComponentHandler.SetUpContractOverview(developerName, contract.GetContractState(), contract.GetContractFeature().nameID, contract.GetContractDelivery(), contract.GetContractPrice(), contract.GetContractFeature().individualCustomers, contract.GetContractFeature().businessCustomers, contract.GetContractFeature().enterpriseCustomers, marketingManager.GetIndividualsPrice(), marketingManager.GetBusinessPrice(), marketingManager.GetEnterprisePrice());
            }
        }
    }


    //provider & developer

    public void UpdateUIContractListsContents()
    {
        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        contractManager = myPlayerDataObject.GetComponent<ContractManager>();
        Dictionary<string, Contract> myContracts = contractManager.GetMyContracts();

        foreach (Transform child in currentcontractListContent.transform)
        { GameObject.Destroy(child.gameObject); }

        foreach (Contract contract in myContracts.Values)
        {   
            GameObject contractUIComponent = Instantiate(contractUIComponentPrefab);
            contractUIComponent.transform.SetParent(currentcontractListContent.transform, false);
            ContractUIComponentHandler contractUIComponentHandler = contractUIComponent.GetComponent<ContractUIComponentHandler>();
            contractUIComponentHandler.SetContractUIHandler(this);
            contractUIComponentHandler.SetContractIDText(contract.GetContractID());
            contractUIComponentHandler.SetStatus(contract.GetContractState().ToString());
            contractUIComponentHandler.SetFeatureID(contract.GetContractFeature().nameID);

            //PROVIDER
            if (contractManager.GetPlayerRole() == PlayerRoles.Provider)
            {
                contractUIComponentHandler.SetPartnersNameText(contractManager.GetFirmName(contract.GetDeveloperID()));
              
                if (contract.GetContractTurn() % 2 == 0)  //providers turn
                {
                    contractUIComponentHandler.SetPlayerTurnText("Your turn");
                    contractUIComponentHandler.DisableDetailButton();
                    contractUIComponentHandler.DisableResultsButton();
                }
                else
                {
                    contractUIComponentHandler.SetPlayerTurnText("Developers turn");
                    contractUIComponentHandler.DisableEditButton();
                    contractUIComponentHandler.DisableResultsButton();
                }
                
            }
            else //DEVELOPER
            {
                contractUIComponentHandler.SetPartnersNameText(contractManager.GetFirmName(contract.GetProviderID()));
               
                if (contract.GetContractTurn() % 2 == 0) //providers turn
                {
                    contractUIComponentHandler.SetPlayerTurnText("ProvidersTurn");
                    contractUIComponentHandler.DisableEditButton();
                    contractUIComponentHandler.DisableResultsButton();
                }
                else
                {
                    contractUIComponentHandler.SetPlayerTurnText("Your Turn");
                    contractUIComponentHandler.DisableDetailButton();
                    contractUIComponentHandler.DisableResultsButton();
                }
            }
            if (contract.GetContractState() == ContractState.Accepted || contract.GetContractState() == ContractState.Rejected)
            {
                contractUIComponentHandler.DisableEditButton();
                contractUIComponentHandler.EnableDetailButtton();
                contractUIComponentHandler.DisableResultsButton();
                contractUIComponentHandler.SetPlayerTurnText("none");

            }
            if (contract.GetContractState() == ContractState.Terminated || contract.GetContractState() == ContractState.Completed)
            {
                contractUIComponentHandler.SetPlayerTurnText("none");
                contractUIComponentHandler.DisableEditButton();
                contractUIComponentHandler.DisableDetailButton();
                contractUIComponentHandler.EnableResultsButton();
            }
        }
    }
    public void UpdateResultContractListContent()
    {
        Dictionary<string, Contract> myContractsHistory = contractManager.GetMyContractsHistory();

        foreach (Transform child in contractHistoryListContent.transform)
        { GameObject.Destroy(child.gameObject); }

        foreach (Contract contract in myContractsHistory.Values)
        {
            GameObject contractUIComponent = Instantiate(contractUIComponentPrefab);
            contractUIComponent.transform.SetParent(contractHistoryListContent.transform, false);
            ContractUIComponentHandler contractUIComponentHandler = contractUIComponent.GetComponent<ContractUIComponentHandler>();
            contractUIComponentHandler.SetContractUIHandler(this);
            contractUIComponentHandler.SetContractIDText(contract.GetContractID());
            contractUIComponentHandler.SetFeatureID(contract.GetContractFeature().nameID);
            contractUIComponentHandler.SetStatus(contract.GetContractState().ToString());
            if (contractManager.GetPlayerRole() == PlayerRoles.Provider)
            {
                contractUIComponentHandler.SetPartnersNameText(contractManager.GetFirmName(contract.GetDeveloperID()));
            }
            else //DEVELOPER
            {
                contractUIComponentHandler.SetPartnersNameText(contractManager.GetFirmName(contract.GetProviderID()));
            }
            contractUIComponentHandler.DisableEditButton();
            contractUIComponentHandler.DisableDetailButton();
            contractUIComponentHandler.EnableResultsButton();
            contractUIComponentHandler.SetPlayerTurnText("none");
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
        contractPreviewUIHandler.SetFunctionality(contract.GetContractFeature().functionality);
        contractPreviewUIHandler.SetIntegrability(contract.GetContractFeature().integrability);
        contractPreviewUIHandler.SetUserFriendliness(contract.GetContractFeature().userfriendliness);
        contractPreviewUIHandler.SetDelivery(contract.GetContractDelivery());
        contractPreviewUIHandler.SetPrice(contract.GetContractPrice());
        contractPreviewUIHandler.SetState(contract.GetContractState());
        contractPreviewUIHandler.GenerateHistoryRecord(contract.GetContractHistory());
        contractPreviewUIHandler.SetRiskSharingFee(contract.GetContractRiskSharingFee());
        if (contract.GetContractState() == ContractState.Proposal)
        {
            contractPreviewUIHandler.ProposalDetailContract();
        }
        else { contractPreviewUIHandler.PreviewContract(); }

        

        if (scheduleManager != null) //Developer
        {
            if (scheduleManager.GetScheduleDevelopmentEndDay().ContainsKey(contractID))
            {
                int developmentEndDay = scheduleManager.GetScheduleDevelopmentEndDay()[contractID];
                if (developmentEndDay > 60)
                {
                    contractPreviewUIHandler.SetScheduleInfoText("SCHEDULE: Feature can not be completed in one quarter.");
                }
                contractPreviewUIHandler.SetScheduleInfoText("SCHEDULE: Contract's end day of developmet: " + developmentEndDay);

                if (developerAccountingManager != null)
                {
                    int salaries = developerAccountingManager.GetSalaries();
                    int developmenyPerDay = (int)System.Math.Round(((float)salaries / 60), System.MidpointRounding.AwayFromZero);
                    int developmentOverallPrice = scheduleManager.GetScheduledFeatureDevelopmentTime(contractID) * developmenyPerDay;
                    int priceRecommendend = developmentOverallPrice * 3;
                    contractPreviewUIHandler.SetDevelopmentPricePerDay("Salary of employees is  " + developmenyPerDay + " per day." + "$");
                    contractPreviewUIHandler.SetDevelopmentPriceOverall("Price of overall development time of feature is " + developmentOverallPrice + "$");
                    contractPreviewUIHandler.SetDevelopmentPriceRecommended("Recommended price of feature is tripple of your development price. This corresponds to " + priceRecommendend + "$");

                }
                else { Debug.LogError("DeveloperAccountinManager in contractUIHandler is null!"); }


            }
            else
            {
                contractPreviewUIHandler.SetScheduleInfoText("SCHEDULE: Contract was not scheduled for development. ");

            }           
        }
        else { contractPreviewUIHandler.DisableScheduleInfoText(); }
        contractPreviewUIHandler.EnableNegotiationContainer();
        contractPreviewUIHandler.DisableResultContainer();
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
        contractPreviewUIHandler.SetFunctionality(contract.GetContractFeature().functionality);
        contractPreviewUIHandler.SetIntegrability(contract.GetContractFeature().integrability);
        contractPreviewUIHandler.SetUserFriendliness(contract.GetContractFeature().userfriendliness);
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
            if (developerAccountingManager != null)
            {
                int salaries = developerAccountingManager.GetSalaries();
                int developmenyPerDay = (int)System.Math.Round(((float)salaries / 60), System.MidpointRounding.AwayFromZero);
                int developmentOverallPrice = scheduleManager.GetScheduledFeatureDevelopmentTime(contractID) * developmenyPerDay;
                int priceRecommendend = developmentOverallPrice * 3;
                contractPreviewUIHandler.SetDevelopmentPricePerDay("Salary of employees is  " + developmenyPerDay + " per day." + "$");
                contractPreviewUIHandler.SetDevelopmentPriceOverall("Price of overall development time of feature is " + developmentOverallPrice + "$");
                contractPreviewUIHandler.SetDevelopmentPriceRecommended("Recommended price of feature is tripple of your overall development price. This corresponds to " + priceRecommendend + "$");

            }
            else { Debug.LogError("DeveloperAccountinManager in contractUIHandler is null!"); }
        }
        else { contractPreviewUIHandler.DisableScheduleInfoText(); }
        contractPreviewUIHandler.EnableNegotiationContainer();
        contractPreviewUIHandler.DisableResultContainer();
    }
    public void GeneteContractResultsPreview(string contractID)
    {
        Contract contract = contractManager.GetMyContractsHistory()[contractID];
        GameObject contractPreviewUI = Instantiate(contractPreviewUIPrefab);
        contractPreviewUI.transform.SetParent(contractContentUI.transform, false);
        ContractPreviewUIHandler contractPreviewUIHandler = contractPreviewUI.GetComponent<ContractPreviewUIHandler>();
        contractPreviewUIHandler.SetContractUIHandler(this);
        contractPreviewUIHandler.SetContractIDText(contract.GetContractID());
        contractPreviewUIHandler.SetDeveloperFirmText(contractManager.GetFirmName(contract.GetDeveloperID()));
        contractPreviewUIHandler.SetProviderFirmText(contractManager.GetFirmName(contract.GetProviderID()));
        contractPreviewUIHandler.SetFeatureText(contract.GetContractFeature().nameID);
        contractPreviewUIHandler.SetFunctionality(contract.GetContractFeature().functionality);
        contractPreviewUIHandler.SetIntegrability(contract.GetContractFeature().integrability);
        contractPreviewUIHandler.SetUserFriendliness(contract.GetContractFeature().userfriendliness);
        contractPreviewUIHandler.SetState(contract.GetContractState());
        contractPreviewUIHandler.GenerateHistoryRecord(contract.GetContractHistory());
        contractPreviewUIHandler.SetAgreedPriceText(contract.GetContractPrice());
        contractPreviewUIHandler.SetAgreedDeliveryText(contract.GetContractDelivery());
        contractPreviewUIHandler.SetAgreedRiskSharingFee(contract.GetContractRiskSharingFee());
        contractPreviewUIHandler.SetActualDelivery(contract.GetTrueDeliveryTime());
        contractPreviewUIHandler.SetRiskSharingFeePaid(contract.GetRiskSharingFeePaid());
        contractPreviewUIHandler.SetTerminationFeePaid(contract.GetTerminationFeePaid());
        contractPreviewUIHandler.EnadleResultContainer();
        contractPreviewUIHandler.DisableNegotiationContainer();

  
    }


    public void DisableEditation()
    {   
        if(createContractButton != null)
        {
            createContractButton.interactable = false;
        }
    }
}
