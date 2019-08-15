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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnChangeBeginningCashBalance(int beginningCashBalance)
    {
        this.beginningCashBalance = beginningCashBalance;
    }
    public void OnChangeRevenue(int revenue)
    {
        this.revenue = revenue;
    }
    public void OnChangeAdvertisement(int advertisement)
    {
        this.advertisement = advertisement;
    }
    public void OnChangeEndCashBalance(int endCashBalance)
    {
        this.endCashBalance = endCashBalance;
    }
}
