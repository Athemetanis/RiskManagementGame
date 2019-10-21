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
    private

    void Start()
    {
        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        gameID = myPlayerDataObject.GetComponent<PlayerData>().GetGameID();
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        researchManager = myPlayerDataObject.GetComponent<ResearchManager>();

    }

    public void GetHistoryData()
    {
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




}
