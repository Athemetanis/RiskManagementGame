using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProviderResearchUIHandler : MonoBehaviour
{
    public Toggle buyCompetitorsResearchToggle;
    public Toggle buyPossiblePartnersResearchToggle;

    public TextMeshProUGUI availabilityInfoText;

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
        availabilityInfoText.gameObject.SetActive(false);
        EnableCorrespondingQuarterUI(currentQuarter);
    }

    public void EnableCorrespondingQuarterUI (int quarter)
    {
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        switch (quarter)
        {
            case 1:
                availabilityInfoText.gameObject.SetActive(true);
                researchQ4Container.SetActive(false);
                researchQ3Container.SetActive(false);
                researchQ2Container.SetActive(false);
                researchQ1Container.SetActive(false);

                return;
                
            case 2:
                researchQ1Container.SetActive(true);
                providerResearchUIComponentHandlerQ1.SetUpProviderResearchUIComponent();
                return;
            case 3:
                researchQ2Container.SetActive(true);
                researchQ1Container.SetActive(true);
                providerResearchUIComponentHandlerQ1.SetUpProviderResearchUIComponent();
                providerResearchUIComponentHandlerQ2.SetUpProviderResearchUIComponent();
                return;
            case 4:
                researchQ3Container.SetActive(true);
                researchQ2Container.SetActive(true);
                researchQ1Container.SetActive(true);
                providerResearchUIComponentHandlerQ1.SetUpProviderResearchUIComponent();
                providerResearchUIComponentHandlerQ2.SetUpProviderResearchUIComponent();
                providerResearchUIComponentHandlerQ3.SetUpProviderResearchUIComponent();
                return;
            case 5:
                researchQ4Container.SetActive(true);
                researchQ3Container.SetActive(true);
                researchQ2Container.SetActive(true);
                researchQ1Container.SetActive(true);
                providerResearchUIComponentHandlerQ1.SetUpProviderResearchUIComponent();
                providerResearchUIComponentHandlerQ2.SetUpProviderResearchUIComponent();
                providerResearchUIComponentHandlerQ3.SetUpProviderResearchUIComponent();
                providerResearchUIComponentHandlerQ4.SetUpProviderResearchUIComponent();
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

    public void SetAvailabilityText(string text)
    {
        availabilityInfoText.text += "\\n " + text;
    }

  
}
