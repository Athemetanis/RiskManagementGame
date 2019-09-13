using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RiskUIHandler : MonoBehaviour
{
    public GameObject riskGraphImagePrefab;
    public GameObject riskGraphContent;

    //VARIABLES
    private Dictionary<string, RectTransform> graphImagePoints = new Dictionary<string, RectTransform>();
    private Dictionary<string, Image> allGraphImages = new Dictionary<string, Image>();
    

    private int xAxisScaler = 50;
    private int yAxisScaler = 30;

    //REFERENCES
    private GameObject myPlayerDataObject;
    private RiskManager riskManager;

    
    private void Start()
    {
        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        riskManager = myPlayerDataObject.GetComponent<RiskManager>();
        riskManager.SetRiskUIHandler(this);
        //UpdateGraphPoints();
    }
    
    //UPDATING UI ELEMENTS
    public void UpdateGraphPoints()
    {
        Debug.Log("updating graph points");
        foreach(Transform child in riskGraphContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        graphImagePoints.Clear();
        allGraphImages.Clear();
        Debug.Log(riskManager.GetRisks().Distinct().ToList().Count());
        foreach (string riskID in riskManager.GetRisks().Distinct().ToList())
        {
            
            GameObject riskGraphImage = Instantiate(riskGraphImagePrefab);
            riskGraphImage.transform.SetParent(riskGraphContent.transform, false);
            RectTransform graphPointRT = riskGraphImage.GetComponent<RectTransform>();
            Image pointI = riskGraphImage.GetComponent<Image>();
            graphImagePoints.Add(riskID, graphPointRT);
            allGraphImages.Add(riskID, pointI);
            Debug.Log("L" + riskManager.GetLikelihood(riskID));
            Debug.Log(riskManager.GetImpact(riskID));
            riskGraphImage.GetComponent<RectTransform>().position.Set(riskManager.GetLikelihood(riskID) * xAxisScaler, riskManager.GetImpact(riskID) * yAxisScaler, 0);
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

    }

    public void CreateRiskPoint(Color color)
    {

    }


}
