using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DeveloperFinalEmployeeStatsUIHandler : MonoBehaviour
{
    public TextMeshProUGUI firmName;
    public TextMeshProUGUI value1Employees;
    public TextMeshProUGUI value2Employees;
    public TextMeshProUGUI value3Employees;

    public TextMeshProUGUI value1Sal;
    public TextMeshProUGUI value2Sal;
    public TextMeshProUGUI value3Sal;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetupFinalProductStats(string firmName, string v1emp, string v2emp, string v3emp, string v1sal, string v2sal, string v3sal)
    {
        this.firmName.text = firmName;
        value1Employees.text = v1emp;
        value2Employees.text = v2emp;
        value3Employees.text = v3emp;
        value1Sal.text = v1sal;
        value2Sal.text = v2sal;
        value3Sal.text = v3sal;
    }
}
