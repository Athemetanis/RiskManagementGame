using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MarketingManager : NetworkBehaviour
{   //VARIABLES
    [SyncVar(hook = "OnChangeAdvertisement")]
    private int advertisementCoverage;   //0,25,50,75,100,
    [SyncVar(hook = "OnChangeIndividualPrice")]
    private int individualPrice;        // 7-15
    [SyncVar(hook = "OnChangeBusinessPrice")]
    private int businessPrice;          //20-40
    [SyncVar(hook = "OnChangeEnterprisePrice")]
    private int enterprisePrice;        //50-80

    private int advertisement0Price = 0;
    private int advertisement25Price = 10000;
    private int advertisement50Price = 30000;
    private int advertisement75Price = 60000;
    private int advertisement100Price = 90000;

    //REFERENCEs
    private MarketingUIHandler marketingUIHandler;
    private ContractUIHandler contractUIHandler;
    private ProviderAccountingManager providerAccountingManager;

    //GETTERS & SETTERS
    public void SetMarketingUIHandler(MarketingUIHandler marketingUIHandler) { this.marketingUIHandler = marketingUIHandler; }
    public void SetContractUIHandler(ContractUIHandler contractUIHandler) { this.contractUIHandler = contractUIHandler; }
    public int GetAdvertisementCoverage() { return advertisementCoverage; }
    public int GetIndividualsPrice() { return individualPrice; }
    public int GetBusinessPrice() { return businessPrice; }
    public int GetEnterprisePrice() { return enterprisePrice; }
    public int GetAdvertisement0Price() { return advertisement0Price; }
    public int GetAdvertisement25Price() { return advertisement25Price; }
    public int GetAdvertisement50Price() { return advertisement50Price; }
    public int GetAdvertisement75Price() { return advertisement75Price; }
    public int GetAdvertisement100Price() { return advertisement100Price; }

    public override void OnStartServer()
    {
        if (individualPrice == 0)
        {
            SetUpDefaultValues();
        }
        providerAccountingManager = this.gameObject.GetComponent<ProviderAccountingManager>();
    }
    public override void OnStartClient()
    {
        providerAccountingManager = this.gameObject.GetComponent<ProviderAccountingManager>();
    }
    public void SetUpDefaultValues()
    {
        advertisementCoverage = 0;
        individualPrice = 8;
        businessPrice = 25;
        enterprisePrice = 60;
    }
    //METHODS
    public void ChangeAdvertisementCoverage(int advertisementCoverage)
    {
        CmdChangeAdvertisementCoverage(advertisementCoverage);
    }
    [Command]
    public void CmdChangeAdvertisementCoverage(int advertisementCoverage)
    {
        this.advertisementCoverage = advertisementCoverage;
        providerAccountingManager.UpdateAdvertisementCostServer();
    } 

    public void ChangeIndividualPrice(int individualPrice)
    {
        CmdChangeIndividualPrice(individualPrice);
    }
    [Command]
    public void CmdChangeIndividualPrice(int individualPrice)
    {
        this.individualPrice = individualPrice;
        providerAccountingManager.UpdateRevenueServer();
    }
    public void ChangeBusinessPrice(int businessPrice)
    {
        CmdChangeBusinessPrice(businessPrice);
        
    }
    [Command]
    public void CmdChangeBusinessPrice(int businessPrice)
    {
        this.businessPrice = businessPrice;
        providerAccountingManager.UpdateRevenueServer();
    }
    public void ChangeEnterprisePrice(int enterprisePrice)
    {
        CmdChangeEnterprisePrice(enterprisePrice);
        
    }
    [Command]
    public void CmdChangeEnterprisePrice(int enterprisePrice)
    {
        this.enterprisePrice = enterprisePrice;
        providerAccountingManager.UpdateRevenueServer();
    }

    //HOOKS
    public void OnChangeAdvertisement(int advertisement)
    {
        advertisementCoverage = advertisement;
        if(marketingUIHandler != null)
        {
            marketingUIHandler.UpdateAdvertisementCoverageToggle(advertisement);
            
        }
    }
    public void OnChangeIndividualPrice(int individualPrice)
    {
        this.individualPrice = individualPrice;
        if (marketingUIHandler != null)
        {
            marketingUIHandler.UpdateIndividualPriceSlider(individualPrice);
            contractUIHandler.UpdateContractOverview();

        }
    }
    public void OnChangeBusinessPrice(int businessPrice)
    {
        this.businessPrice = businessPrice;
        if (marketingUIHandler != null)
        {
            marketingUIHandler.UpdateBusinessPriceSlider(businessPrice);
            contractUIHandler.UpdateContractOverview();
        }
    }
    public void OnChangeEnterprisePrice(int enterprisePrice)
    {
        this.enterprisePrice = enterprisePrice;
        if (marketingUIHandler != null)
        {
            marketingUIHandler.UpdateEnterprisePriceSlider(enterprisePrice);
            contractUIHandler.UpdateContractOverview();
        }
    }
}
