using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContractCountTextHandler : MonoBehaviour
{
    public TextMeshProUGUI contracCount;

    public void SetContractCount(int contracCount)
    {
        this.contracCount.text = contracCount.ToString("n0");
    }
}
