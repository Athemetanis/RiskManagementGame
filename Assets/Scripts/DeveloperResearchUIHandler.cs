using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeveloperResearchUIHandler : MonoBehaviour
{
    public Toggle buyCompetitorsResearchToggle;
    public Toggle buyPossiblePartnersResearchToggle;

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
        researchManager.SetDeveloperResearchUIHandler(this);

    }

    public void EnableCorrespondingQuarterUI(int quarter)
    {

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
