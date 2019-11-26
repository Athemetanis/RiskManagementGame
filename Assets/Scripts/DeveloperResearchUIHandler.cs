using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeveloperResearchUIHandler : MonoBehaviour
{
    public Toggle buyCompetitorsResearchToggle;
    public Toggle buyPossiblePartnersResearchToggle;
    public Toggle previusResearchResultsToggle;

    public TextMeshProUGUI availabilityInfoText;

    public GameObject researchQ1Container;
    public GameObject researchQ2Container;
    public GameObject researchQ3Container;
    public GameObject researchQ4Container;

    public DeveloperResearchUIComponentHandler developerResearchUIComponentHandlerQ1;
    public DeveloperResearchUIComponentHandler developerResearchUIComponentHandlerQ2;
    public DeveloperResearchUIComponentHandler developerResearchUIComponentHandlerQ3;
    public DeveloperResearchUIComponentHandler developerResearchUIComponentHandlerQ4;

    private string gameID;
    private int currentQuarter;
    private GameObject myPlayerDataObject;
    private ResearchManager researchManager;


    void Start()
    {
        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        gameID = myPlayerDataObject.GetComponent<PlayerData>().GetGameID();
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        researchManager = myPlayerDataObject.GetComponent<ResearchManager>();
        researchManager.SetDeveloperResearchUIHandler(this);
        availabilityInfoText.gameObject.SetActive(false);
        developerResearchUIComponentHandlerQ1.SetDeveloperResearchUIHandler(this);
        developerResearchUIComponentHandlerQ2.SetDeveloperResearchUIHandler(this);
        developerResearchUIComponentHandlerQ3.SetDeveloperResearchUIHandler(this);
        developerResearchUIComponentHandlerQ4.SetDeveloperResearchUIHandler(this);
        developerResearchUIComponentHandlerQ1.Initialization();
        developerResearchUIComponentHandlerQ2.Initialization();
        developerResearchUIComponentHandlerQ3.Initialization();
        developerResearchUIComponentHandlerQ4.Initialization();
        EnableCorrespondingQuarterUI(currentQuarter);
    }

    public void EnableCorrespondingQuarterUI(int quarter)
    {
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        switch (quarter)
        {
            case 1:
                //availabilityInfoText.gameObject.SetActive(true);
                //availabilityInfoText.text = "Results for previus Quarter not avialable";
                previusResearchResultsToggle.gameObject.SetActive(false);
                researchQ4Container.SetActive(false);
                researchQ3Container.SetActive(false);
                researchQ2Container.SetActive(false);
                researchQ1Container.SetActive(false);

                return;

            case 2:
                researchQ1Container.SetActive(true);
                previusResearchResultsToggle.gameObject.SetActive(false);
                developerResearchUIComponentHandlerQ1.SetUpDeveloperResearchUIComponent();
                return;
            case 3:
                researchQ2Container.SetActive(true);
                //researchQ1Container.SetActive(true);
                previusResearchResultsToggle.gameObject.SetActive(true);
                developerResearchUIComponentHandlerQ1.SetUpDeveloperResearchUIComponent();
                developerResearchUIComponentHandlerQ2.SetUpDeveloperResearchUIComponent();
                return;
            case 4:
                researchQ3Container.SetActive(true);
                // researchQ2Container.SetActive(true);
                //researchQ1Container.SetActive(true);
                previusResearchResultsToggle.gameObject.SetActive(true);
                developerResearchUIComponentHandlerQ1.SetUpDeveloperResearchUIComponent();
                developerResearchUIComponentHandlerQ2.SetUpDeveloperResearchUIComponent();
                developerResearchUIComponentHandlerQ3.SetUpDeveloperResearchUIComponent();
                return;
            case 5:
                researchQ4Container.SetActive(true);
                //researchQ3Container.SetActive(true);
                //researchQ2Container.SetActive(true);
                //researchQ1Container.SetActive(true);
                previusResearchResultsToggle.gameObject.SetActive(true);
                developerResearchUIComponentHandlerQ1.SetUpDeveloperResearchUIComponent();
                developerResearchUIComponentHandlerQ2.SetUpDeveloperResearchUIComponent();
                developerResearchUIComponentHandlerQ3.SetUpDeveloperResearchUIComponent();
                developerResearchUIComponentHandlerQ4.SetUpDeveloperResearchUIComponent();
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

    public void ShowPreviousResults()
    {
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        if (previusResearchResultsToggle.isOn)
        {
            switch (currentQuarter)
            {
                case 3:
                    researchQ1Container.SetActive(true);
                    return;
                case 4:
                    researchQ2Container.SetActive(true);
                    researchQ1Container.SetActive(true);
                    return;
                case 5:
                    researchQ3Container.SetActive(true);
                    researchQ2Container.SetActive(true);
                    researchQ1Container.SetActive(true);
                    return;
            }
        }
        else
        {
            switch (currentQuarter)
            {
                case 3:
                    researchQ1Container.SetActive(false);
                    return;
                case 4:
                    researchQ2Container.SetActive(false);
                    researchQ1Container.SetActive(false);
                    return;
                case 5:
                    researchQ3Container.SetActive(false);
                    researchQ2Container.SetActive(false);
                    researchQ1Container.SetActive(false);
                    return;
            }
        }
    }

}
