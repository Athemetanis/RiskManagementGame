using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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


    public GameObject riskUIComponentPrefab;
    public GameObject riskImagePrefab;

    public GameObject riskListContent;
    public GameObject riskGraphContent;


    private Dictionary<string, RectTransform> graphImagePoints = new Dictionary<string, RectTransform>();
    private Dictionary<string, Image> allGraphImages = new Dictionary<string, Image>();

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
    public void SetUpQuarterRisksDescriptions(string risk1d,string risk2d, string risk3d)
    {
        risk1DescriptionText.text = risk1d;
        risk2DescriptionText.text = risk2d;
        risk3DescriptionText.text = risk3d;
        risk1ActionText.gameObject.SetActive(false);
        risk2ActionText.gameObject.SetActive(false);
        risk3ActionText.gameObject.SetActive(false);
    }


    public void SetUpQuarterMatrix(     )
    {


    }

    public void UpdateGraphPoints( )
    {
        /*//Debug.Log("updating graph points");
        foreach (Transform child in riskGraphContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        graphImagePoints.Clear();
        allGraphImages.Clear();
        //Debug.Log(riskManager.GetRisks().Distinct().ToList().Count());
        foreach (string riskID in riskManager.GetRisks().Distinct().ToList())
        {

            GameObject riskGraphImage = Instantiate(riskGraphImagePrefab);
            riskGraphImage.transform.SetParent(riskGraphContent.transform, false);
            RectTransform graphPointRT = riskGraphImage.GetComponent<RectTransform>();
            Image pointI = riskGraphImage.GetComponent<Image>();
            graphImagePoints.Add(riskID, graphPointRT);
            allGraphImages.Add(riskID, pointI);
            //Debug.Log("L" + riskManager.GetLikelihood(riskID));
            // Debug.Log(riskManager.GetImpact(riskID));
            riskGraphImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(riskManager.GetLikelihood(riskID) * xAxisScaler, riskManager.GetImpact(riskID) * yAxisScaler);
            pointI.color = riskManager.GetColor(riskID);
        }*/
    }

    public void HighlightRisk(string riskID)
    {
        foreach (Image image in allGraphImages.Values)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0.4f);
        }
        Image highlightedImage = allGraphImages[riskID];
        highlightedImage.color = new Color(highlightedImage.color.r, highlightedImage.color.g, highlightedImage.color.b, 1f);
        graphImagePoints[riskID].SetAsLastSibling();
    }

    public void EndHighlightRisk()
    {
        foreach (Image image in allGraphImages.Values)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        }
    }

}
