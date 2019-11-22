using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContractDeveloperUIComponentHandler : MonoBehaviour
{
    public TextMeshProUGUI developersFirm;

    public GameObject contractsCountValuesContainer;
    public GameObject contractCountTextComponent;
       
    public void SetDevelopersFirmName(string developersFirm)
    {
        this.developersFirm.text = developersFirm;
    }

    public void CreateNewContractCountValue(int value)
    {
        GameObject contractCount = Instantiate(contractCountTextComponent);
        contractCount.transform.SetParent(contractsCountValuesContainer.transform, false);
        ContractCountTextHandler handler = contractCount.GetComponent<ContractCountTextHandler>();
        handler.SetContractCount(value);
    }
}
