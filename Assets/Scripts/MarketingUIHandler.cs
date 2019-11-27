using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketingUIHandler : MonoBehaviour
{
    public Toggle advertisementCoverage0Toggle;
    public Toggle advertisementCoverage25Toggle;
    public Toggle advertisementCoverage50Toggle;
    public Toggle advertisementCoverage75Toggle;
    public Toggle advertisementCoverage100Toggle;

    public TextMeshProUGUI advertisement0Price;
    public TextMeshProUGUI advertisement25Price;
    public TextMeshProUGUI advertisement50Price;
    public TextMeshProUGUI advertisement75Price;
    public TextMeshProUGUI advertisement100Price;

    public Slider individualPriceSlider;
    public Slider businessPriceSlider;
    public Slider enterprisePriceSlider;

    public TextMeshProUGUI individualPriceText;
    public TextMeshProUGUI businessPriceText;
    public TextMeshProUGUI enterprisePriceText;

    private bool initialized = false;
    private GameObject myPlayerDataObject;
    private MarketingManager marketingManager; 

    // Start is called before the first frame update
    void Start()
    {
        initialized = false;
        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        marketingManager = myPlayerDataObject.GetComponent<MarketingManager>();
        marketingManager.SetMarketingUIHandler(this);
        UpdateAllUIElements();
        advertisement0Price.text = marketingManager.GetAdvertisement0Price().ToString();
        advertisement25Price.text = marketingManager.GetAdvertisement25Price().ToString();
        advertisement50Price.text = marketingManager.GetAdvertisement50Price().ToString();
        advertisement75Price.text = marketingManager.GetAdvertisement100Price().ToString();
        advertisement100Price.text = marketingManager.GetAdvertisement100Price().ToString();
        initialized = true;
    }

    public void AdvertisementChanged() //trigered from UI elements -  on value change in toggle
    {
        if(advertisementCoverage0Toggle.isOn == true)
        {
            ChangeAdvertisementCoverage(0);
        }
        else if (advertisementCoverage25Toggle.isOn == true)
        {
            ChangeAdvertisementCoverage(25);
        }
        else if (advertisementCoverage50Toggle.isOn == true)
        {
            ChangeAdvertisementCoverage(50);
        }
        else if (advertisementCoverage75Toggle.isOn == true)
        {
            ChangeAdvertisementCoverage(75);
        }
        else if (advertisementCoverage100Toggle.isOn == true)
        {
            ChangeAdvertisementCoverage(100);
        }
    }

    public void ChangeAdvertisementCoverage(int advertisementCoverage)
    {
        if (initialized)
        {
            marketingManager.ChangeAdvertisementCoverage(advertisementCoverage);
        }
    }

    public void ChangeIndividualPrice()
    {
        if (initialized)
        {
            marketingManager.ChangeIndividualPrice((int)individualPriceSlider.value);
            individualPriceText.text = individualPriceSlider.value.ToString();
        }
    }
    public void ChangeBusinessPrice()
    {
        if (initialized)
        {
            marketingManager.ChangeBusinessPrice((int)businessPriceSlider.value);
            businessPriceText.text = businessPriceSlider.value.ToString();
        }
    }
    public void ChangeEnterprisePrice()
    {
        if (initialized)
        {
            marketingManager.ChangeEnterprisePrice((int)enterprisePriceSlider.value);
            enterprisePriceText.text = enterprisePriceSlider.value.ToString();
        }
    }

    //METHODS FOR UPDATING UI ELEMENTS
    public void UpdateAllUIElements()
    {
        UpdateAdvertisementCoverageToggle(marketingManager.GetAdvertisementCoverage());
        individualPriceSlider.value = marketingManager.GetIndividualsPrice();
        individualPriceText.text = individualPriceSlider.value.ToString();
        businessPriceSlider.value = marketingManager.GetBusinessPrice();
        businessPriceText.text = businessPriceSlider.value.ToString();
        enterprisePriceSlider.value = marketingManager.GetEnterprisePrice();
        enterprisePriceText.text = enterprisePriceSlider.value.ToString();
    }

    public void UpdateAdvertisementCoverageToggle(int advertisementCoverage)
    {
            switch (advertisementCoverage)
            {
                case 0:
                    advertisementCoverage0Toggle.enabled = true;
                    break;
                case 25:
                    advertisementCoverage25Toggle.enabled = true;
                    break;
                case 50:
                    advertisementCoverage50Toggle.enabled = true;
                    break;
                case 75:
                    advertisementCoverage75Toggle.enabled = true;
                    break;
                case 100:
                    advertisementCoverage100Toggle.enabled = true;
                    break;
            }
    }

    public void UpdateIndividualPriceSlider(int individualPrice)
    {

        individualPriceSlider.value = individualPrice;
        individualPriceText.text = individualPrice.ToString();

    }
    public void UpdateBusinessPriceSlider(int businessPrice)
    {
        businessPriceSlider.value = businessPrice;
        businessPriceText.text = businessPrice.ToString();
    }
    public void UpdateEnterprisePriceSlider(int enterprisePrice)
    {
        enterprisePriceSlider.value = enterprisePrice;
        enterprisePriceText.text = enterprisePrice.ToString();

    }

    public void DisableEditation()
    {
        advertisementCoverage0Toggle.interactable = false;
        advertisementCoverage25Toggle.interactable = false;
        advertisementCoverage50Toggle.interactable = false;
        advertisementCoverage75Toggle.interactable = false;
        advertisementCoverage100Toggle.interactable = false;

        individualPriceSlider.interactable = false;
        businessPriceSlider.interactable = false;
        enterprisePriceSlider.interactable = false;
    }
}
