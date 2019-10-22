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

    public int correspondinqResearchQuarter;

    private string gameID;
    private int currentQuarter;
    private GameObject myPlayerDataObject;
    private ResearchManager researchManager;
    private MarketingManager marketingManager;
    private ProviderResearchUIHandler providerResearchUIHandler;

    void Awake()
    {

        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        gameID = myPlayerDataObject.GetComponent<PlayerData>().GetGameID();
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        researchManager = myPlayerDataObject.GetComponent<ResearchManager>();
        providerResearchUIHandler = this.GetComponent<ProviderResearchUIHandler>();
        Debug.LogWarning(myPlayerDataObject + " , " +  gameID + " , " + currentQuarter + " , " + researchManager + " , " + providerResearchUIHandler);
    }

    public void SetUpProviderResearchUIComponent()
    {
        partnersResearchContainer.SetActive(false);
        competitorsResearchContainer.SetActive(false);
        GetHistoryData();
        GenerateProductList();
        GenerateReliabilitiesList();

        if (researchManager.GetBuyCompertitorsResearchQuarter(correspondinqResearchQuarter))
        {
            partnersResearchContainer.SetActive(true);
            
        }
        else
        {   
            providerResearchUIHandler.SetAvailabilityText("Research on possible partners not bought for quarter " + currentQuarter + ".");
        }
        if (researchManager.GetBuyPossiblePartnersResearchQuarter(correspondinqResearchQuarter))
        {
            competitorsResearchContainer.SetActive(true);
        }
        else
        {   
            providerResearchUIHandler.SetAvailabilityText("Research on competitors not bought for quarter " + currentQuarter + ".");
        }

    }

    public void GetHistoryData()
    {
        Debug.Log(researchManager);
        (int enterprisePriceAverage, int enterprisePriceMin, int enterprisePriceMax, int businessPriceAverage, int businessPriceMin, int businessPriceMax, int individualPriceAverage, int individualPriceMin, int individualPriceMax, int advertisementAverage, int advertisementMin, int advertisementMax) = researchManager.GetCorrespondingQuarterDataDeveloper(correspondinqResearchQuarter);
        this.enterprisePriceAverage.text = enterprisePriceAverage.ToString();
        this.enterprisePriceMin.text = enterprisePriceMin.ToString();
        this.enterprisePriceMax.text = enterprisePriceMax.ToString();
        this.businessPriceAverage.text = businessPriceAverage.ToString();
        this.businessPriceMax.text = businessPriceMax.ToString();
        this.businessPriceMin.text = businessPriceMin.ToString();
        this.individualPriceAverage.text = individualPriceAverage.ToString();
        this.individualPriceMin.text = individualPriceMin.ToString();
        this.individualPriceMax.text = individualPriceMax.ToString();
        this.advertisementAverage.text = advertisementAverage.ToString();
        this.advertisementMin.text = advertisementMin.ToString();
        this.advertisementMax.text = advertisementMax.ToString();   
    }

    public void GenerateProductList()
    {
        //currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        List<GameObject> providers = new List<GameObject>(GameHandler.allGames[gameID].GetProviderList().Values);
        int providerCount = providers.Count;

        foreach (GameObject provider in providers)
        {
            int productFunctionality = provider.GetComponent<ProductManager>().GetFunctionalityQuarter(correspondinqResearchQuarter);
            int productIntegrability = provider.GetComponent<ProductManager>().GetIntegrabilityQuarter(correspondinqResearchQuarter);
            int productUI = provider.GetComponent<ProductManager>().GetUserFriendlinessQuarter(correspondinqResearchQuarter);
            string firmName = provider.GetComponent<FirmManager>().GetFirmName();
                       
            GameObject competitorsProductUIComponent = Instantiate(competitorsProductUIComponentPrefab);
            competitorsProductUIComponent.transform.SetParent(competitorsProductListContentContainer.transform, false);
            ProviderResearchCompetitorsProductUIComponentHandler contractOverviewUIComponentHandler = competitorsProductUIComponent.GetComponent<ProviderResearchCompetitorsProductUIComponentHandler>();

            contractOverviewUIComponentHandler.SetUpProviderResearchUIComponent(firmName, productFunctionality, productIntegrability, productUI);
        }

    }

    public void GenerateReliabilitiesList()
    {
        //int currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        List<GameObject> developers = new List<GameObject>(GameHandler.allGames[gameID].GetDeveloperList().Values);
        int developersCount = developers.Count;

        foreach (GameObject developer in developers)
        {
            int reliability = developer.GetComponent<ContractManager>().GetReliabilityQuater(correspondinqResearchQuarter);
            string firmName = developer.GetComponent<FirmManager>().GetFirmName();

            GameObject possiblePartnersReliability = Instantiate(possiblePartnersReliabilityComponentPrefab);
            possiblePartnersReliability.transform.SetParent(possiblePartnersReliabilityListContentContainer.transform, false);
            ProviderResearchPartnersReliabilityUIComponentHandler providerResearchPartnersReliabilityUIComponentHandler = possiblePartnersReliability.GetComponent<ProviderResearchPartnersReliabilityUIComponentHandler>();
            providerResearchPartnersReliabilityUIComponentHandler.SetUpProviderResearchPartnersReliability(firmName, reliability);
        }


    }





}
