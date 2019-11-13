using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContractProviderNameTextHandler : MonoBehaviour
{
    public TextMeshProUGUI providerFirm;

    public void SetProviderName(string providerFirm)
    {
        this.providerFirm.text = providerFirm;
    }
}
