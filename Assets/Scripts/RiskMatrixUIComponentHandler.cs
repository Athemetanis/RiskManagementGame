﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RiskMatrixUIComponentHandler : MonoBehaviour
{

    public Image imageColor;
    public TextMeshProUGUI riskNameID;
    public Dropdown likelihoodIF;
    public Dropdown impactIF;
    public Toggle monitorToggle;

    private string riskID;
    private Color riskColor;
    private bool initialized;

    //REFERENCES
    private GameObject myPlayerDataObject;
    private RiskManager riskManager;
    private RiskUIHandler riskUIHandler;


    //METHODS
    public void Start() //on clients only because its UI
    {
        Debug.Log("RISK UI COMPONENT START");
        if (this.gameObject.activeSelf)
        {
            Debug.Log("RISK UI COMPONENT ACTIVE SELF");
            initialized = false;
            riskColor = imageColor.color;
            riskID = riskNameID.text;
            myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
            riskManager = myPlayerDataObject.GetComponent<RiskManager>();
            riskUIHandler = this.gameObject.transform.root.GetComponent<RiskUIHandler>();

            if (riskManager.ContainsRisk(riskNameID.text))
            {
                likelihoodIF.value = riskManager.GetLikelihood(riskID);
                impactIF.value = riskManager.GetImpact(riskID);
                monitorToggle.isOn = riskManager.GetMonitor(riskID);
            }
            else
            {
                riskManager.AddRisk(riskNameID.text, 0, 0, false, riskColor);
            }
            initialized = true;
        }
    }
    


    public void SetLikelihood() //trigered by event - OnEndEdit
    {
        //Debug.Log("liekliness ui changed");
        string riskID = riskNameID.text;
        int likelihood = likelihoodIF.value;
        if (initialized)
        {
            //Debug.Log("likeliness change riskmanager");
            riskManager.UpdateLikelihood(riskID, likelihood);
        }
    }

    public void SetImpact() //trigered by event
    {
        string riskID = riskNameID.text;
        int impact = impactIF.value;
        if (initialized)
        {
            riskManager.UpdateImpact(riskID, impact);
        }
    }

    public void SetForMonitor()  //trigered by event
    {
        if (initialized)
        {
            riskManager.UpdateMonitor(riskID, monitorToggle.isOn);
        }
       
    }

    public void HighlightRisk()  //trigered by event  - on cursor hover
    {
        string riskID = riskNameID.text;
        if (riskUIHandler != null)
        {
            riskUIHandler.HighlightRisk(riskID);
        }
    }

    public void EndHighLighRisk()
    {
        if (riskUIHandler != null)
        {
            riskUIHandler.EndHighlightRisk();
        }
    }
   


}
