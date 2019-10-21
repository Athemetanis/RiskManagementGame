using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProviderResearchUIHandler : MonoBehaviour
{
    public Toggle buyCompetitorsResearchToggle;
    public Toggle buyPossiblePartnersResearchToggle;

    public TextMeshProUGUI aviabilityInfoText;

    public GameObject researchQ1Container;
    public GameObject researchQ2Container;
    public GameObject researchQ3Container;
    public GameObject researchQ4Container;

    public ProviderResearchUIComponentHandler providerResearchUIComponentHandlerQ1;
    public ProviderResearchUIComponentHandler providerResearchUIComponentHandlerQ2;
    public ProviderResearchUIComponentHandler providerResearchUIComponentHandlerQ3;
    public ProviderResearchUIComponentHandler providerResearchUIComponentHandlerQ4;

    private string gameID;
    private int currentQuarter;
    private GameObject myPlayerDataObject;
    private ResearchManager researchManager;


    // Start is called before the first frame update
    void Start()
    {
        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        gameID = myPlayerDataObject.GetComponent<PlayerData>().GetGameID();
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        researchManager = myPlayerDataObject.GetComponent<ResearchManager>();
        researchManager.SetProviderResearchUIHandler(this);
        aviabilityInfoText.gameObject.SetActive(false);
        EnableCorrespondingQuarterUI(currentQuarter);
    }

public void EnableCorrespondingQuarterUI (int quarter)
    {   
        switch (quarter)
        {
            case 1:
                aviabilityInfoText.gameObject.SetActive(true);
                return;
                
            case 2:
                if (buyCompetitorsResearchToggle.isOn || buyPossiblePartnersResearchToggle.isOn)
                {
                    researchQ1Container.SetActive(true);
                }
                else
                {
                    aviabilityInfoText.gameObject.SetActive(true);
                }           
                return;
            case 3:
                if (buyCompetitorsResearchToggle.isOn || buyPossiblePartnersResearchToggle.isOn)
                {
                    researchQ2Container.SetActive(true);
                }
                else
                {
                    aviabilityInfoText.gameObject.SetActive(true);
                }
                return;
            case 4:
                if (buyCompetitorsResearchToggle.isOn || buyPossiblePartnersResearchToggle.isOn)
                {
                    researchQ3Container.SetActive(true);
                }
                else
                {
                    aviabilityInfoText.gameObject.SetActive(true);
                }
                return;
            case 5:
                if (buyCompetitorsResearchToggle.isOn || buyPossiblePartnersResearchToggle.isOn)
                {
                    researchQ4Container.SetActive(true);
                }
                else
                {
                    aviabilityInfoText.gameObject.SetActive(true);
                }
                return;
        }

    }

    public void ChangeBuyCompetitorsResearch()
    {
        researchManager.SetBuyCompetitorsResearch(buyCompetitorsResearchToggle.isOn);
    }
    public void ChangeBuyPossiblePartnersResearch()
    {
        researchManager.SetBuyPossiblePartnersResearch(buyPossiblePartnersResearchToggle.isOn);
    }

  
}
