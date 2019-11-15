using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerQuarterStats3UIComponent : MonoBehaviour
{
    public TextMeshProUGUI firmName;
    public TextMeshProUGUI value1Q1;
    public TextMeshProUGUI value2Q1;
    public TextMeshProUGUI value3Q1;

    public TextMeshProUGUI value1Q2;
    public TextMeshProUGUI value2Q2;
    public TextMeshProUGUI value3Q2;

    public TextMeshProUGUI value1Q3;
    public TextMeshProUGUI value2Q3;
    public TextMeshProUGUI value3Q3;

    public TextMeshProUGUI value1Q4;
    public TextMeshProUGUI value2Q4;
    public TextMeshProUGUI value3Q4;



    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetUpPlayerStatsQ1(string firmName, string value1, string value2, string value3)
    {
        value1Q1.text = value1;
        value2Q1.text = value2;
        value3Q1.text = value3;
    }

    public void SetUpPlayerStatsQ2(string firmName, string value1, string value2, string value3)
    {
        value1Q2.text = value1;
        value2Q2.text = value2;
        value3Q2.text = value3;
    }

    public void SetUpPlayerStatsQ3(string firmName, string value1, string value2, string value3)
    {
        value1Q3.text = value1;
        value2Q3.text = value2;
        value3Q3.text = value3;
    }

    public void SetUpPlayerStatsQ4(string firmName, string value1, string value2, string value3)
    {
        value1Q4.text = value1;
        value2Q4.text = value2;
        value3Q4.text = value3;
    }

}
