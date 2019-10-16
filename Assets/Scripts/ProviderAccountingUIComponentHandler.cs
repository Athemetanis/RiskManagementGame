using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProviderAccountingUIComponentHandler : MonoBehaviour
{
    public TextMeshProUGUI beginningCashBalanceText;
    public TextMeshProUGUI revenueText;
    public TextMeshProUGUI individualRevenueText;
    public TextMeshProUGUI businessRevenueText;
    public TextMeshProUGUI enterpriseRevenueText;
    public TextMeshProUGUI advertisementText;
    public TextMeshProUGUI contractPaymentsText;
    public TextMeshProUGUI riskSharingFeesReceivedText;
    public TextMeshProUGUI terminationFeeReceivedText;
    public TextMeshProUGUI marketingResearchText;
    public TextMeshProUGUI borrowEmergencyLoanText;
    public TextMeshProUGUI repayEmergencyLoanText;
    public TextMeshProUGUI endCashBalanceText;

    public int correspondingAccountingQuarter;

    private string gameID;
    private int currentQuarter;
    private GameObject myPlayerDataObject;
    private ProviderAccountingManager providerAccountingManager;

    public void Init()
    {
        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        gameID = myPlayerDataObject.GetComponent<PlayerData>().GetGameID();
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        providerAccountingManager = myPlayerDataObject.GetComponent<ProviderAccountingManager>();
    }

    public void GetHistoryData()
    {
        (int beginningCashBalance, int revenue, int enterpriseRevenue, int businessRevenue, int individualRevenue, int advertismenentCost, int contractPayments, int riskSharingFeeReceived, int terminationFeeReceived, int marketingResearch, int borrowEmergencyLoan, int repayEmergencyLoan, int endCashBalance) = providerAccountingManager.GetCorrespondingQuarterData(correspondingAccountingQuarter);
        beginningCashBalanceText.text = beginningCashBalance.ToString("n0");
        revenueText.text = revenue.ToString("n0");
        enterpriseRevenueText.text = enterpriseRevenue.ToString("n0");
        businessRevenueText.text = businessRevenue.ToString("n0");
        individualRevenueText.text = individualRevenue.ToString("n0");
        advertisementText.text = advertismenentCost.ToString("n0");
        contractPaymentsText.text = contractPayments.ToString("n0");
        riskSharingFeesReceivedText.text = riskSharingFeeReceived.ToString("n0");
        terminationFeeReceivedText.text = terminationFeeReceived.ToString("n0");
        marketingResearchText.text = marketingResearch.ToString("n0");
        borrowEmergencyLoanText.text = borrowEmergencyLoan.ToString("n0");
        repayEmergencyLoanText.text = repayEmergencyLoan.ToString("n0");
        endCashBalanceText.text = endCashBalance.ToString("n0");
    }


    //METHODS FOR UPDATING UI ELEMENTS
    public void UpdateAllElements()
    {
        beginningCashBalanceText.text = providerAccountingManager.GetBeginnigCashBalance().ToString("n0");
        revenueText.text = providerAccountingManager.GetRevenue().ToString("n0");
        individualRevenueText.text = providerAccountingManager.GetIndividualCustomersRevenue().ToString("n0");
        businessRevenueText.text = providerAccountingManager.GetBusinessCustomersRevenue().ToString("n0");
        enterpriseRevenueText.text = providerAccountingManager.GetEnterpriseCustomersRevenue().ToString("n0");
        advertisementText.text = providerAccountingManager.GetAdvertisementCost().ToString("n0");
        contractPaymentsText.text = providerAccountingManager.GetContractPayments().ToString("n0");
        riskSharingFeesReceivedText.text = providerAccountingManager.GetRishSharingFeesReceived().ToString("n0");
        terminationFeeReceivedText.text = providerAccountingManager.GetTerminationFeeReceived().ToString("n0");
        marketingResearchText.text = providerAccountingManager.GetMarketingResearch().ToString("n0");
        borrowEmergencyLoanText.text = providerAccountingManager.GetBorrowEmergencyLoan().ToString("n0");
        repayEmergencyLoanText.text = providerAccountingManager.GetRepayEmergencyLoan().ToString("n0");
        endCashBalanceText.text = providerAccountingManager.GetEndCashBalance().ToString("n0");
    }

    public void UpdateBeginingCashBalanceText(int beginingCashBalance)
    {
        beginningCashBalanceText.text = beginingCashBalance.ToString("n0");
    }
    public void UpdateRevenueText(int revenue)
    {
        revenueText.text = revenue.ToString("n0");
    }
    public void UpdateIndividualRevenue(int individualRevenue)
    {
        individualRevenueText.text = individualRevenue.ToString("n0");
    }
    public void UpdateBusinessRevenue(int businessRevenue)
    {
        businessRevenueText.text = businessRevenue.ToString("n0");
    }
    public void UpdateEnterpriseRevenue(int enterpriseRevenue)
    {
        enterpriseRevenueText.text = enterpriseRevenue.ToString("n0");
    }
    public void UpdateAdvertisementText(int advertisement)
    {
        advertisementText.text = advertisement.ToString("n0");
    }
    public void UpdateContractPayments(int contractPayments)
    {
        contractPaymentsText.text = contractPayments.ToString("n0");
    }
    public void UpdateRiskSharingFee(int riskSharingFee)
    {
        riskSharingFeesReceivedText.text = riskSharingFee.ToString("n0");
    }
    public void UpdateTerminatinFeeReceived(int terminationFee)
    {
        terminationFeeReceivedText.text = terminationFee.ToString("n0");
    }
    public void UpdateMarketingResearch(int marketingResearch)
    {
        marketingResearchText.text = marketingResearch.ToString("n0");
    }
    public void UpdateBorrowEmergencyLoan(int borrowEmergencyLoan)
    {
        borrowEmergencyLoanText.text = borrowEmergencyLoan.ToString("n0");
    }
    public void UpdateRepayEmergencyLoan(int repayEmergencyLoan)
    {
        repayEmergencyLoanText.text = repayEmergencyLoan.ToString("n0");
    }
    public void UpdateEndCashBalanceText(int endCashBalance)
    {
        endCashBalanceText.text = endCashBalance.ToString("n0");
    }
   


}
