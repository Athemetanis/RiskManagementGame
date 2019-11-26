using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeveloperResearchUIComponentHandler : MonoBehaviour
{
    public GameObject competitorsEmployeesUIComponentPrefab;
    public GameObject competitorsEmployeesContentContainer;

    public GameObject possiblePartnersProductComponentPrefab;
    public GameObject possiblePartnersProductListContentContainer;

    public GameObject partnersResearchContainer;
    public GameObject competitorsResearchContainer;

    public TextMeshProUGUI notAvialableInfo;

    public TextMeshProUGUI programmersSalaryMin;
    public TextMeshProUGUI programmersSalaryMax;
    public TextMeshProUGUI programmersSalaryAverage;
    public TextMeshProUGUI programmersSalaryMine;

    public TextMeshProUGUI integrabilitySpecialistsSalaryMin;
    public TextMeshProUGUI integrabilitySpecialistsSalaryMax;
    public TextMeshProUGUI integrabilitySpecialistsSalaryAverage;
    public TextMeshProUGUI integrabilitySpecialistsSalaryMine;

    public TextMeshProUGUI uiSpecialistsSalaryMin;
    public TextMeshProUGUI uiSpecialistsSalaryMax;
    public TextMeshProUGUI uiSpecialistsSalaryAverage;
    public TextMeshProUGUI uiSpecialistsSalaryMine;

    public TextMeshProUGUI reliabilityMin;
    public TextMeshProUGUI reliabilityMax;
    public TextMeshProUGUI reliabilityAverage;
    public TextMeshProUGUI reliabilityMine;

    public int correspondingResearchQuarter;

    private string gameID;
    private int currentQuarter;
    private GameObject myPlayerDataObject;
    private ResearchManager researchManager;
    private HumanResourcesManager humanResourcesManager;
    private ContractManager contractManager;
    private DeveloperResearchUIHandler developerResearchUIHandler;

    public void SetDeveloperResearchUIHandler(DeveloperResearchUIHandler developerResearchUIHandler) { this.developerResearchUIHandler = developerResearchUIHandler; }

    public void Initialization()
    {
        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        gameID = myPlayerDataObject.GetComponent<PlayerData>().GetGameID();
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        researchManager = myPlayerDataObject.GetComponent<ResearchManager>();
        humanResourcesManager = myPlayerDataObject.GetComponent<HumanResourcesManager>();
        contractManager = myPlayerDataObject.GetComponent<ContractManager>();
    }

    public void SetUpDeveloperResearchUIComponent()
    {
        partnersResearchContainer.SetActive(false);
        competitorsResearchContainer.SetActive(false);
        GetHistoryData();
        GenerateEmployeesList();
        GenerateProductList();

        if (researchManager.GetBuyCompertitorsResearchQuarter(correspondingResearchQuarter))
        {
            partnersResearchContainer.SetActive(true);
        }
        else
        {
            notAvialableInfo.text += "Research on possible business partners was not bought for this quarter.";
        }
        if (researchManager.GetBuyPossiblePartnersResearchQuarter(correspondingResearchQuarter))
        {
            competitorsResearchContainer.SetActive(true);
        }
        else
        {   if(notAvialableInfo.text.Length != 0)
            {
                notAvialableInfo.text += "\\n ";
            }
            notAvialableInfo.text += "Research on competitors was not bought for this quarter.";
        }

    }

    public void GetHistoryData()
    {
        (int programmersSalaryAverage, int programmersSalaryMin, int programmersSalaryMax, int integrabilitySpecialistsSalaryAverage, int integrabilitySpecialistsSalaryMin, int integrabilitySpecialistsSalaryMax, int uiSpecialistsSalaryAverage, int uiSpecialistsSalaryMin, int uiSpecialistsSalaryMax, int reliabilityAverage, int reliabilityMin, int reliabilityMax) = researchManager.GetCorrespondingQuarterDataDeveloper(correspondingResearchQuarter);
        this.programmersSalaryAverage.text = programmersSalaryAverage.ToString("n0");
        this.programmersSalaryMin.text = programmersSalaryMin.ToString("n0");
        this.programmersSalaryMax.text = programmersSalaryMax.ToString("n0");
        this.integrabilitySpecialistsSalaryAverage.text = integrabilitySpecialistsSalaryAverage.ToString("n0");
        this.integrabilitySpecialistsSalaryMin.text = integrabilitySpecialistsSalaryMin.ToString("n0");
        this.integrabilitySpecialistsSalaryMax.text = integrabilitySpecialistsSalaryMax.ToString("n0");
        this.uiSpecialistsSalaryAverage.text = uiSpecialistsSalaryAverage.ToString("n0");
        this.uiSpecialistsSalaryMin.text = uiSpecialistsSalaryMin.ToString("n0");
        this.uiSpecialistsSalaryMax.text = uiSpecialistsSalaryMax.ToString("n0");
        this.reliabilityAverage.text = reliabilityAverage.ToString("n0");
        this.reliabilityMin.text = reliabilityMin.ToString("n0");
        this.reliabilityMax.text = reliabilityMax.ToString("n0");

        programmersSalaryMine.text = humanResourcesManager.GetProgrammersSalaryQuarter(correspondingResearchQuarter).ToString("n0");
        integrabilitySpecialistsSalaryMine.text = humanResourcesManager.GetIntegrabilitySpecialistSalaryQuarter(correspondingResearchQuarter).ToString("n0");
        uiSpecialistsSalaryMine.text = humanResourcesManager.GetUISpecialistSalaryQuarter(correspondingResearchQuarter).ToString("n0");
        reliabilityMine.text = contractManager.GetReliabilityQuater(correspondingResearchQuarter).ToString();

    }

    public void GenerateEmployeesList()
    {
        foreach (Transform child in competitorsEmployeesContentContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        List<GameObject> developers = new List<GameObject>(GameHandler.allGames[gameID].GetDeveloperList().Values);

        //Debug.Log(developers.Count);
        foreach (GameObject developer in developers)
        {
            int employeesCount = developer.GetComponent<HumanResourcesManager>().GetEmployeesCountQuater(correspondingResearchQuarter);
            string firmName = developer.GetComponent<FirmManager>().GetFirmName();

            GameObject competitorsEmployeesUIComponent = Instantiate(competitorsEmployeesUIComponentPrefab);
            competitorsEmployeesUIComponent.transform.SetParent(competitorsEmployeesContentContainer.transform, false);
            DeveloperResearchCompetitorsEmployeesUIComponentHandler competitorsEmpolyeesUIComponentHandler = competitorsEmployeesUIComponent.GetComponent<DeveloperResearchCompetitorsEmployeesUIComponentHandler>();

            competitorsEmpolyeesUIComponentHandler.SetUpDeveloperResearchCompetitorsEmployeesUIComponent(firmName, employeesCount);
        }
    }

    public void GenerateProductList()
    {
        foreach (Transform child in possiblePartnersProductListContentContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        List<GameObject> providers = new List<GameObject>(GameHandler.allGames[gameID].GetProviderList().Values);
        int providerCount = providers.Count;
        Debug.Log(providers.Count);
        foreach (GameObject provider in providers)
        {
            int productFunctionality = provider.GetComponent<ProductManager>().GetFunctionalityQuarter(correspondingResearchQuarter);
            int productIntegrability = provider.GetComponent<ProductManager>().GetIntegrabilityQuarter(correspondingResearchQuarter);
            int productUI = provider.GetComponent<ProductManager>().GetUserFriendlinessQuarter(correspondingResearchQuarter);
            string firmName = provider.GetComponent<FirmManager>().GetFirmName();

            GameObject competitorsProductUIComponent = Instantiate(possiblePartnersProductComponentPrefab);
            competitorsProductUIComponent.transform.SetParent(possiblePartnersProductListContentContainer.transform, false);
            ResearchProductUIComponentHandler researchProductIComponentHandler = competitorsProductUIComponent.GetComponent<ResearchProductUIComponentHandler>();

            researchProductIComponentHandler.SetUpProductResearchUIComponent(firmName, productFunctionality, productIntegrability, productUI);
        }
    }

}

