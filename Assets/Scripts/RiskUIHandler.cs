using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RiskUIHandler : MonoBehaviour
{
    //REFERENCES
    public GameObject riskQ1Container;
    public GameObject riskQ2Container;
    public GameObject riskQ3Container;
    public GameObject riskQ4Container;
    public GameObject riskQ5Container;

    public GameObject riskGraphContentQ3;
    public GameObject riskGraphContentQ4;
    public GameObject riskGraphImagePrefab;

    //VARIABLES
    private GameObject riskGraphContent;

    private Dictionary<string, RectTransform> graphImagePoints = new Dictionary<string, RectTransform>();
    private Dictionary<string, Image> allGraphImages = new Dictionary<string, Image>();
    
    private int xAxisScaler = 50;
    private int yAxisScaler = 30;

    private string gameID;
    private int currentQuarter;

    //REFERENCES
    private GameObject myPlayerDataObject;
    private RiskManager riskManager;

    private void Start()
    {        
        
        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        gameID = myPlayerDataObject.GetComponent<PlayerData>().GetGameID();
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        riskManager = myPlayerDataObject.GetComponent<RiskManager>();
        riskManager.SetRiskUIHandler(this);

        EnableCorrespondingUI(currentQuarter);
        //UpdateGraphPoints();
    }

    //UPDATING UI ELEMENTS
    public void EnableCorrespondingUI(int currentQuarter)
    {
        riskQ1Container.SetActive(false);
        riskQ2Container.SetActive(false);
        riskQ3Container.SetActive(false);
        riskQ4Container.SetActive(false);
        riskQ5Container.SetActive(false);
        if (currentQuarter == 1)
        {
           riskQ1Container.SetActive(true);
        }
        if (currentQuarter == 2)
        {
           riskQ2Container.SetActive(true);
        }
        if (currentQuarter == 3)
        {
            riskGraphContent = riskGraphContentQ3;
            riskQ3Container.SetActive(true);
            UpdateGraphPoints();
        }
        if (currentQuarter == 4)
        {
            riskGraphContent = riskGraphContentQ4;
            riskQ4Container.SetActive(true);
            UpdateGraphPoints();
        }
        if(currentQuarter == 5)
        {
            riskQ5Container.SetActive(true);
        }
    }

    public void UpdateGraphPoints()
    {
        //Debug.Log("updating graph points");
        foreach(Transform child in riskGraphContent.transform)
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
        }
    }

    public void UpdateLikelihood(string riskID, int likelihood )
    {
        graphImagePoints[riskID].position.Set( likelihood * xAxisScaler, graphImagePoints[riskID].position.y, 0);
    }

    public void UpdateImpact(string riskID, int impact)
    {
        graphImagePoints[riskID].position.Set(graphImagePoints[riskID].position.x, impact * yAxisScaler,  0);
    }

    public void HighlightRisk(string riskID)
    {
        foreach(Image image in allGraphImages.Values)
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
