using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractUIHandler : MonoBehaviour
{
    public GameObject contractListContent;
    public GameObject rejectedContractListContent;
    public GameObject developerListContent;
    //public GameObject featureOutsourcingListContent;

    public GameObject contractProposalPrefab;
    public GameObject contractDetailPrefab;

    public GameObject contractUIPrefab;
    public GameObject developerUIPrefab;

    //DROPDOWN MENU
    public Dropdown selectedDeveloperDropdown;
    public Dropdown selectedFeatureDropdown;

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
            Debug.Log(contractManager.GetPlayerRole());
            Debug.Log("Vygeneruj List Developerov");
            GenerateFeatureDropdownOptions(new List<string>(featureManager.GetOutsourcedFeatures().Keys));
            GenerateDeveloperDropdownOptions();
        }
        else
        {
            Debug.Log(contractManager.GetPlayerRole());
            Debug.Log("Som developer - negenerujem nic");
        }


    }

    

    // Update is called once per frame
    void Update()
    {
        
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



}
