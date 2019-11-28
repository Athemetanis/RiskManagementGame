using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ProviderFinalProductsStatsUIHandler : MonoBehaviour
{
    public TextMeshProUGUI firmName;
    public TextMeshProUGUI value1Product;
    public TextMeshProUGUI value2Product;
    public TextMeshProUGUI value3Product;

    public TextMeshProUGUI value1Price;
    public TextMeshProUGUI value2Price;
    public TextMeshProUGUI value3Price;

    public TextMeshProUGUI value1Customers;
    public TextMeshProUGUI value2Customers;
    public TextMeshProUGUI value3Customers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

  public void SetupFinalProductStats(string firmName, string v1prod, string v2prod, string v3prod, string v1price, string v2price, string v3price, string v1cust, string v2cust, string v3cust)
    {
        this.firmName.text = firmName;
        value1Product.text = v1prod;
        value2Product.text = v2prod;
        value3Product.text = v3prod;
        value1Price.text = v1price;
        value2Price.text = v2price;
        value3Price.text = v3price;
        value1Customers.text = v1cust;
        value2Customers.text = v2cust;
        value3Customers.text = v3cust;

  }
}
