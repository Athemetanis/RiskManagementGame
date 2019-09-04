using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProviderAccountingManager : NetworkBehaviour
{
    [SyncVar(hook = "OnChangeBeginningCashBalance")]
    private int beginningCashBalance;
    [SyncVar(hook = "OnChangeRevenue")]
    private int revenue;
    [SyncVar(hook = "OnChangeAdvertisement")]
    private int advertisement;
    [SyncVar(hook = "OnChangeEndCashBalance")]
    private int endCashBalance;

    private ProductManager productManager;
    private ContractManager contractManager;
    private ProviderAccountingUIHandler providerAccountingUIHandler;
    
    public void SetProviderAccountingUIHandler(ProviderAccountingUIHandler providerAccountingUIHandler) { this.providerAccountingUIHandler = providerAccountingUIHandler; }

    // Start is called before the first frame update
    void Start()
    {
        productManager = this.gameObject.GetComponent<ProductManager>();
        contractManager = this.gameObject.GetComponent<ContractManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnChangeBeginningCashBalance(int beginningCashBalance)
    {
        this.beginningCashBalance = beginningCashBalance;
        if( providerAccountingUIHandler != null)
        {
            providerAccountingUIHandler.UpdateBeginingCashBalanceText(this.beginningCashBalance);
        }
    }
    public void OnChangeRevenue(int revenue)
    {
        this.revenue = revenue;
        if (providerAccountingUIHandler != null)
        {
            providerAccountingUIHandler.UpdateRevenueText(this.revenue);
        }
    }
    public void OnChangeAdvertisement(int advertisement)
    {
        this.advertisement = advertisement;
        if (providerAccountingUIHandler != null)
        {
            providerAccountingUIHandler.UpdateAdvertisementText(this.advertisement);
        }
    }
    public void OnChangeEndCashBalance(int endCashBalance)
    {
        this.endCashBalance = endCashBalance;
        if (providerAccountingUIHandler != null)
        {
            providerAccountingUIHandler.UpdateEndCashBalanceText(this.endCashBalance);
        }
    }
}
