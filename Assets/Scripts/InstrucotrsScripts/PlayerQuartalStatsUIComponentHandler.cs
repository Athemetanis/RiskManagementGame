using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerQuartalStatsUIComponentHandler : MonoBehaviour
{
    public TextMeshProUGUI firmName;
    public TextMeshProUGUI valueQ1;
    public TextMeshProUGUI valueQ2;
    public TextMeshProUGUI valueQ3;
    public TextMeshProUGUI valueQ4;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetUpPlayerQuartalStatsUIComponent(string firmName, string valueQ1, string valueQ2, string valueQ3, string valueQ4)
    {
        this.firmName.text = firmName;
        this.valueQ1.text = valueQ1;
        this.valueQ2.text = valueQ2;
        this.valueQ3.text = valueQ3;
        this.valueQ4.text = valueQ4;


    }
}
