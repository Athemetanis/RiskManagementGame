using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerMoneyStatsUIComponentHandler : MonoBehaviour
{
    public TextMeshProUGUI firmName;
    public TextMeshProUGUI moneyQ1;
    public TextMeshProUGUI moneyQ2;
    public TextMeshProUGUI moneyQ3;
    public TextMeshProUGUI moneyQ4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetUpPlayerMoneyUIComponent(string firmName, string moneyQ1, string moneyQ2, string moneyQ3, string moneyQ4)
    {
        this.firmName.text = firmName;
        this.moneyQ1.text = moneyQ1;
        this.moneyQ2.text = moneyQ2;
        this.moneyQ3.text = moneyQ3;
        this.moneyQ4.text = moneyQ4;


    }


}
