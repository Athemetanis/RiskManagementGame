using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProviderAccountingUIHandler : MonoBehaviour
{
    public Text beginningCashBalanceText;
    public Text revenueText;
    public Text advertisementText;
    public Text endCashBalanceText;

    private GameObject myPlayerDataObject;
    private ProviderAccountingManager providerAccountingManager;
    private ProductManager productManager;

    // Start is called before the first frame update
    void Start()
    {
        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        providerAccountingManager = myPlayerDataObject.GetComponent<ProviderAccountingManager>();
        providerAccountingManager.SetProviderAccountingUIHandler(this);
    }

    public void UpdateBeginingCashBalanceText(int beginingCashBalance)
    {
        beginningCashBalanceText.text = beginingCashBalance.ToString();
    }
    public void UpdateRevenueText(int revenue)
    {
        revenueText.text = revenue.ToString();
    }
    public void UpdateEndCashBalanceText(int endCashBalance)
    {
        endCashBalanceText.text = endCashBalance.ToString();
    }
    public void UpdateAdvertisementText(int advertisement)
    {
        advertisementText.text = advertisement.ToString();
    }


}
