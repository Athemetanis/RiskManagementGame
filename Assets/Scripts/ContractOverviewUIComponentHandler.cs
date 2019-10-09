using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContractOverviewUIComponentHandler : MonoBehaviour
{
    public TextMeshProUGUI contractIDText;
    public TextMeshProUGUI featureIDText;
    public TextMeshProUGUI deliveryTimeText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI individualCustomersQuadrantCountText;
    public TextMeshProUGUI businessCustomersQuadrantCountText;
    public TextMeshProUGUI enterpriseCustomersQuadrantCountText;

    public TextMeshProUGUI individualCustomersOverallCountText;
    public TextMeshProUGUI businessCustomersOverallCountText;
    public TextMeshProUGUI enterpriseCustomersOverallCountText;

    public TextMeshProUGUI individualCustomersRevenueText;
    public TextMeshProUGUI businessCustomersRevenueText;
    public TextMeshProUGUI enterpriseCustomersRevenueText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetUpContractOverview(string contractID, string featureID, int deliveryTime, int price,  int individualCustomersOverallCount, int businessCustomerOverallCount, int enterpriseCustomersOverallCount, int individualCustomersPrice, int businessCustomersPrice, int enterpriseCustomersPrice)
    {
        contractIDText.text = contractID;
        featureIDText.text = featureID;
        deliveryTimeText.text = deliveryTime.ToString();
        priceText.text = price.ToString();

        int individualCustomersQCount = (individualCustomersOverallCount / 60 ) * deliveryTime;
        int businessCustomerQCount = (businessCustomerOverallCount / 60 ) * deliveryTime;
        int enterpriseCustomersQCount = (enterpriseCustomersOverallCount / 60 ) * deliveryTime;
        individualCustomersQuadrantCountText.text = individualCustomersQCount.ToString();
        businessCustomersQuadrantCountText.text = businessCustomerQCount.ToString();
        enterpriseCustomersQuadrantCountText.text = enterpriseCustomersQCount.ToString();

        individualCustomersOverallCountText.text = individualCustomersOverallCount.ToString();
        businessCustomersOverallCountText.text = businessCustomerOverallCount.ToString();
        enterpriseCustomersOverallCountText.text = enterpriseCustomersOverallCount.ToString();


        int individualsCutomersRevenue = individualCustomersPrice * individualCustomersQCount;
        int businessCustomersRevenue = businessCustomersPrice * businessCustomerQCount;
        int enterpriseCustomersRevenue = enterpriseCustomersPrice  * enterpriseCustomersQCount;
        individualCustomersRevenueText.text = individualsCutomersRevenue.ToString();
        businessCustomersRevenueText.text = businessCustomersRevenue.ToString();
        enterpriseCustomersRevenueText.text = enterpriseCustomersRevenue.ToString();
    }


}
