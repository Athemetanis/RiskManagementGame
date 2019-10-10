using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CustomersUIHandler : MonoBehaviour
{
    public TextMeshProUGUI beginningEnterpriseCustomers;
    public TextMeshProUGUI beginningBusinessCustomers;
    public TextMeshProUGUI beginningIndividualsCustmers;

    public TextMeshProUGUI enterpriseCustomerAddDuringQ;
    public TextMeshProUGUI businessCustomerAddDuringQ;
    public TextMeshProUGUI individualCustomerAddDuringQ;

    public TextMeshProUGUI enterpriseCustomerAddEndQ;
    public TextMeshProUGUI businessCustomerAddEndQ;
    public TextMeshProUGUI individualCustomerAddEndQ;

    public TextMeshProUGUI endEnterpriseCustomers;
    public TextMeshProUGUI endBusinessCustomers;
    public TextMeshProUGUI endIndividualCustomers;

    public TextMeshProUGUI enterpriseCustomersAdvLoss;
    public TextMeshProUGUI businessCustomersAdvLoss;
    public TextMeshProUGUI individualCustomersAdvLoss;

    public TextMeshProUGUI enterpriseCustomersAdvAdd;
    public TextMeshProUGUI businessCustomersAdvAdd;
    public TextMeshProUGUI individualCustomersAdvAdd;

    public void SetBeginingEntCustomers(int begininCount) { beginningEnterpriseCustomers.text = begininCount.ToString(); }
    public void SetBeginnigBusCutomers(int beginingCount) { beginningBusinessCustomers.text = beginingCount.ToString(); }
    public void SetBeginingIndCustomers(int beginingCount) { beginningIndividualsCustmers.text = beginingCount.ToString(); }

    public void SetEntCustomersAddDuringQ(int addition) { enterpriseCustomerAddDuringQ.text = addition.ToString(); }
    public void SetBusCustomersAddDuringQ(int addition) { businessCustomerAddDuringQ.text = addition.ToString(); }
    public void SetIndCustomersAddDuringQ(int addition) { individualCustomerAddDuringQ.text = addition.ToString(); }

    public void SetEntCustomersAddEndQ(int addition) { enterpriseCustomerAddEndQ.text = addition.ToString(); }
    public void SetBusCustomersAddEndQ(int addition) { businessCustomerAddEndQ.text = addition.ToString(); }
    public void SetIndCustomersAddEndQ(int addition) { individualCustomerAddEndQ.text = addition.ToString(); }

    public void SetEndEntCustomers(int endCount) { endEnterpriseCustomers.text = endCount.ToString(); }
    public void SetEndBusCutomers(int endCount) { endBusinessCustomers.text = endCount.ToString(); }
    public void SetEndIndCustomers(int endCount) { endIndividualCustomers.text = endCount.ToString(); }

    public void SetEntCostumersAdvLoss(int loss) { enterpriseCustomersAdvLoss.text = loss.ToString(); }
    public void SetBusCustomersAdvLoss(int loss) { businessCustomersAdvLoss.text = loss.ToString(); }
    public void SetIndCustomersAdvLoss(int loss) { individualCustomersAdvLoss.text = loss.ToString(); }
    public void SetEntCustomerAdvAdd(int add) { enterpriseCustomersAdvAdd.text = add.ToString(); }
    public void SetBusCustomersAdvAdd(int add) { businessCustomersAdvAdd.text = add.ToString(); }
    public void SetIndCustomersAdvAdd(int add) { individualCustomersAdvAdd.text = add.ToString(); }

    private CustomersManager customersManager;

    // Start is called before the first frame update
    void Start()
    {
        customersManager = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject().GetComponent<CustomersManager>();
        customersManager.SetCustomersUIHandler(this);
        UpdateAllUIElements();
    }

    public void UpdateAllUIElements()
    {
        beginningEnterpriseCustomers.text = customersManager.GetBeginningEnterpriseCustomers().ToString("n0");
        beginningBusinessCustomers.text = customersManager.GetBeginningBusinessCustomers().ToString("n0");
        beginningIndividualsCustmers.text = customersManager.GetBeginningIndividualCustomers().ToString("n0");
        enterpriseCustomerAddDuringQ.text = customersManager.GetEnterpriseCustomersDuringQ().ToString("n0");
        businessCustomerAddDuringQ.text = customersManager.GetBusinessCustomersDuringQ().ToString("n0");
        individualCustomerAddDuringQ.text = customersManager.GetIndividualCustomersDuringQ().ToString("n0");
        enterpriseCustomerAddEndQ.text = customersManager.GetEnterpriseCustomersEndQ().ToString("n0");
        businessCustomerAddEndQ.text = customersManager.GetBusinessCustomersEndQ().ToString("n0");
        individualCustomerAddEndQ.text = customersManager.GetIndividualCustomersEndQ().ToString("n0");
        endEnterpriseCustomers.text = customersManager.GetEndEnterpriseCustomers().ToString("n0");
        endBusinessCustomers.text = customersManager.GetEndBusinessCustomers().ToString("n0");
        endIndividualCustomers.text = customersManager.GetEndIndividualCustomers().ToString("n0");

        enterpriseCustomersAdvLoss.text = customersManager.GetEnterpriseCustomersAdvLoss().ToString("n0");
        businessCustomersAdvLoss.text = customersManager.GetBusinessCustomersAdvLoss().ToString("n0");
        individualCustomersAdvLoss.text = customersManager.GetIndividualCustomersAdvLoss().ToString("n0");
        enterpriseCustomersAdvAdd.text = customersManager.GetEnterpriseCustomersAdvAdd().ToString("n0");
        businessCustomersAdvAdd.text = customersManager.GetBusinessCustomersAdvAdd().ToString("n0");
        individualCustomersAdvAdd.text = customersManager.GetIndividualCustomersAdvAdd().ToString("n0");



    }



}
