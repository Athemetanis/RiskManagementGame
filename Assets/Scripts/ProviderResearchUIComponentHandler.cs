using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProviderResearchUIComponentHandler : MonoBehaviour
{
    public GameObject competitorsProductUIComponentPrefab;
    public GameObject competitorsProductListContentContainer;

    public GameObject possiblePartnersReliabilityComponentPrefab;
    public GameObject possiblePartnersReliabilityListContentContainer;

    public GameObject partnersResearchContainer;
    public GameObject competitorsResearchContainer;

    public TextMeshProUGUI notAvialableInfo;

    public TextMeshProUGUI enterprisePriceMin;
    public TextMeshProUGUI enterprisePriceMax;
    public TextMeshProUGUI enterprisePriceAverage;
    public TextMeshProUGUI enterprisePriceMine;

    public TextMeshProUGUI businessPriceMin;
    public TextMeshProUGUI businessPriceMax;
    public TextMeshProUGUI businessPriceAverage;
    public TextMeshProUGUI businessPriceMine;

    public TextMeshProUGUI individualPriceMin;
    public TextMeshProUGUI individualPriceMax;
    public TextMeshProUGUI individualPriceAverage;
    public TextMeshProUGUI individualPriceMine;

    public TextMeshProUGUI advertisementMin;
    public TextMeshProUGUI advertisementMax;
    public TextMeshProUGUI advertisementAverage;
    public TextMeshProUGUI advertisementMine;

    public int correspondingResearchQuarter;

    private string gameID;
    private int currentQuarter;
    private GameObject myPlayerDataObject;
    private ResearchManager researchManager;
    private MarketingManager marketingManager;
    private ProviderResearchUIHandler providerResearchUIHandler;

    public void SerProviderResearchUIHandler(ProviderResearchUIHandler providerResearchUIHandler) { this.providerResearchUIHandler = providerResearchUIHandler; }

    public void Initialization()
    {

        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        gameID = myPlayerDataObject.GetComponent<PlayerData>().GetGameID();
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        researchManager = myPlayerDataObject.GetComponent<ResearchManager>();
        marketingManager = myPlayerDataObject.GetComponent<MarketingManager>();
    }

    public void SetUpProviderResearchUIComponent()
    {
        partnersResearchContainer.SetActive(false);
        competitorsResearchContainer.SetActive(false);
        GetHistoryData();
        GenerateProductList();
        GenerateReliabilitiesList();
        notAvialableInfo.text = "";

        if (researchManager.GetBuyCompertitorsResearchQuarter(correspondingResearchQuarter))
        {
            competitorsResearchContainer.SetActive(true);
        }
        else
        {
            notAvialableInfo.text += "Research on competitors was not bought for this quarter.";
        }
        if (researchManager.GetBuyPossiblePartnersResearchQuarter(correspondingResearchQuarter))
        {
            partnersResearchContainer.SetActive(true);
        }
        else
        {
            if (notAvialableInfo.text.Length != 0)
            {
                notAvialableInfo.text += " \n ";
            }
            notAvialableInfo.text += "Research on possible business partners was not bought for this quarter.";
        }
    }

    public void GetHistoryData()
    {
        Debug.Log(researchManager);
        (int enterprisePriceAverage, int enterprisePriceMin, int enterprisePriceMax, int businessPriceAverage, int businessPriceMin, int businessPriceMax, int individualPriceAverage, int individualPriceMin, int individualPriceMax, int advertisementAverage, int advertisementMin, int advertisementMax) = researchManager.GetCorrespondingQuarterDataProvider(correspondingResearchQuarter);
        this.enterprisePriceAverage.text = enterprisePriceAverage.ToString("n0");
        this.enterprisePriceMin.text = enterprisePriceMin.ToString("n0");
        this.enterprisePriceMax.text = enterprisePriceMax.ToString("n0");
        this.businessPriceAverage.text = businessPriceAverage.ToString("n0");
        this.businessPriceMax.text = businessPriceMax.ToString("n0");
        this.businessPriceMin.text = businessPriceMin.ToString("n0");
        this.individualPriceAverage.text = individualPriceAverage.ToString("n0");
        this.individualPriceMin.text = individualPriceMin.ToString("n0");
        this.individualPriceMax.text = individualPriceMax.ToString("n0");
        this.advertisementAverage.text = advertisementAverage.ToString("n0");
        this.advertisementMin.text = advertisementMin.ToString("n0");
        this.advertisementMax.text = advertisementMax.ToString("n0");
        enterprisePriceMine.text = marketingManager.GetEnterprisePriceQuarter(correspondingResearchQuarter).ToString("n0");
        businessPriceMine.text = marketingManager.GetBusinessPriceQuarter(correspondingResearchQuarter).ToString("n0");
        individualPriceMine.text = marketingManager.GetIndividualPriceQuarter(correspondingResearchQuarter).ToString("n0");
        advertisementMine.text = marketingManager.GetAdvertismenetCoverageQuarters(correspondingResearchQuarter).ToString();
    }

    public void GenerateProductList()
    {
        foreach (Transform child in competitorsProductListContentContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        List<GameObject> providers = new List<GameObject>(GameHandler.allGames[gameID].GetProviderList().Values);
        Debug.Log(providers.Count);
        foreach (GameObject provider in providers)
        {
            int productFunctionality = provider.GetComponent<ProductManager>().GetFunctionalityQuarter(correspondingResearchQuarter);
            int productIntegrability = provider.GetComponent<ProductManager>().GetIntegrabilityQuarter(correspondingResearchQuarter);
            int productUI = provider.GetComponent<ProductManager>().GetUserFriendlinessQuarter(correspondingResearchQuarter);
            string firmName = provider.GetComponent<FirmManager>().GetFirmName();
                       
            GameObject competitorsProductUIComponent = Instantiate(competitorsProductUIComponentPrefab);
            competitorsProductUIComponent.transform.SetParent(competitorsProductListContentContainer.transform, false);
            ResearchProductUIComponentHandler researchProductUIComponentHandler = competitorsProductUIComponent.GetComponent<ResearchProductUIComponentHandler>();

            researchProductUIComponentHandler.SetUpProductResearchUIComponent(firmName, productFunctionality, productIntegrability, productUI);
        }
    }

    public void GenerateReliabilitiesList()
    {
        foreach (Transform child in possiblePartnersReliabilityListContentContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        List<GameObject> developers = new List<GameObject>(GameHandler.allGames[gameID].GetDeveloperList().Values);
      
        foreach (GameObject developer in developers)
        {
            int reliability = developer.GetComponent<ContractManager>().GetReliabilityQuater(correspondingResearchQuarter);
            string firmName = developer.GetComponent<FirmManager>().GetFirmName();
            int employeesCount = developer.GetComponent<HumanResourcesManager>().GetEmployeesCountQuater(correspondingResearchQuarter);
            GameObject possiblePartnersReliability = Instantiate(possiblePartnersReliabilityComponentPrefab);
            possiblePartnersReliability.transform.SetParent(possiblePartnersReliabilityListContentContainer.transform, false);
            ProviderResearchPartnersReliabilityUIComponentHandler providerResearchPartnersReliabilityUIComponentHandler = possiblePartnersReliability.GetComponent<ProviderResearchPartnersReliabilityUIComponentHandler>();
            providerResearchPartnersReliabilityUIComponentHandler.SetUpProviderResearchPartnersReliability(firmName, reliability, employeesCount);
        }
    }
}
