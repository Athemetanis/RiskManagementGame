using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IndividualDecisionsRiskManagementQuarterHandler : MonoBehaviour
{
    public int correspondingQuarter;

    public TextMeshProUGUI risk1DescriptionText;
    public TextMeshProUGUI risk1ActionText;
    public TextMeshProUGUI risk2DescriptionText;
    public TextMeshProUGUI risk2ActionText;
    public TextMeshProUGUI risk3DescriptionText;
    public TextMeshProUGUI risk3ActionText;
    public TextMeshProUGUI risk4DescriptionText;
    public TextMeshProUGUI risk4ActionText;
    public TextMeshProUGUI risk5DescriptionText;
    public TextMeshProUGUI risk5ActionText;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void SetUpQuarterRisksDescriptions(string risk1d,string risk1a, string risk2d, string risk2a, string risk3d, string risk3a)
    {

        risk1DescriptionText.text = risk1d;
        risk1ActionText.text = risk1a;
        risk2DescriptionText.text = risk2d;
        risk2ActionText.text = risk2a;
        risk3DescriptionText.text = risk3d;
        risk3ActionText.text = risk3a;
    }

    public void SetUpQuarterMetrix()
    {


    }





}
