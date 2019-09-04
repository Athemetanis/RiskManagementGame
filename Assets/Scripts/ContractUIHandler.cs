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
    //public GameObject featureOutsourcingListContent;

    public GameObject contractProposalPrefab; //zatiaľ nepoužívaš - budeš vôbec??? áno!!!
    
    
    //UI elements
    public Dropdown selectedDeveloperDropdown;
    public Dropdown selectedFeatureDropdown;
    public Button createContractButton;

    //REFERENCES FOR OTHER MANAGERS
    private GameObject myPlayerDataObject;
    private ContractManager contractManager;
    private FeatureManager featureManager;
    private FirmManager firmManager;
        

    // Start is called before the first frame update
    void Start()
    {
        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        contractManager = myPlayerDataObject.GetComponent<ContractManager>();
        contractManager.SetContractUIHandler(this);
        featureManager = myPlayerDataObject.GetComponent<FeatureManager>();
        featureManager.SetContractUIHandler(this);
        featureManager.SetContractManager(contractManager);
        firmManager = myPlayerDataObject.GetComponent<FirmManager>();

        if (contractManager.GetPlayerRole() == PlayerRoles.Provider)
        {
            //Debug.Log(contractManager.GetPlayerRole());
            //Debug.Log("Vygeneruj List Developerov");
            GenerateFeatureDropdownOptions(new List<string>(featureManager.GetOutsourcedFeatures().Keys));
            GenerateDeveloperDropdownOptions();
            UpdateDeveloperFirmList(GameHandler.allGames[contractManager.GetGameID()].GetListDeveloperFirmNameDescription());
            UpdateUIContractListsContents();
        }
        else  //som developer
        {
            UpdateUIContractListsContents();
        }

    }

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

    public void CreateContract()
    {
        contractManager.CreateContract(selectedDeveloperDropdown.options[selectedDeveloperDropdown.value].text, selectedFeatureDropdown.options[selectedFeatureDropdown.value].text);
    }

    public void EnableContractCreation()
    {
        if(selectedFeatureDropdown.options.Count != 0 &&  selectedFeatureDropdown.options.Count != 0)
        {
            createContractButton.interactable = true;
        }
        else
        {
            createContractButton.interactable = false;
        }
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
                Debug.Log(contractManager.GetPlayerRole());
                Debug.Log(contract.GetContractTrun());
                contractUIComponentHandler.SetPartnersNameText(contractManager.GetFirmName(contract.GetDeveloperID()));
              
                if (contract.GetContractTrun() % 2 == 0)  //providers turn
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
                Debug.Log(contractManager.GetPlayerRole());
                Debug.Log(contract.GetContractTrun());
                contractUIComponentHandler.SetPartnersNameText(contractManager.GetFirmName(contract.GetProviderID()));
               
                if (contract.GetContractTrun() % 2 == 0) //providers turn
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
        contractPreviewUIHandler.PreviewContract();
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

        if(contract.GetContractState() == ContractState.Proposal)
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
    }

    public void SendBackContract(string contractID, string price, string delivery)
    {
        contractManager.ModifyContract(contractID, int.Parse(price), int.Parse(delivery));
    }


    public void AcceptContract(string contractID)
    {
        Contract contract = contractManager.GetMyContracts()[contractID];
        if(contract.GetContractState() == ContractState.InNegotiations)
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
}
