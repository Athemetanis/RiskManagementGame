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

    public void SetEndEntCustomers(int endCount) { beginningEnterpriseCustomers.text = endCount.ToString(); }
    public void SetEndBusCutomers(int endCount) { beginningBusinessCustomers.text = endCount.ToString(); }
    public void SetEndIndCustomers(int endCount) { beginningIndividualsCustmers.text = endCount.ToString(); }

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
    }



}
