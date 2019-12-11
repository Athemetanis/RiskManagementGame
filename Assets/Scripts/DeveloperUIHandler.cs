using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeveloperUIHandler : MonoBehaviour
{
    public Toggle welcomeTab;
    public Toggle marketOverview;
    public Toggle introQ2Tab;
    public Toggle introQ3Tab;
    public Toggle introQ4Tab;
    public Toggle endEvaluationTab;


    public GameObject welcomeContent;
    public GameObject marketOverviewContent;
    public GameObject introQ2Content;
    public GameObject introQ3Content;
    public GameObject introQ4Content;
    public GameObject endEvaluationContent;


    private GameObject myPlayerDataObject;
    private PlayerManager playerData;
    private string gameID;
    private GameData gameData;

    private int currentQuarter;

    private void Start()
    {
        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        playerData = myPlayerDataObject.GetComponent<PlayerManager>();
        gameID = myPlayerDataObject.GetComponent<PlayerManager>().GetGameID();
        gameData = GameHandler.allGames[gameID];

        currentQuarter = gameData.GetGameRound();

    }


    public void HandlingQTabs()
    {
        if(currentQuarter == 1)
        {
            welcomeTab.isOn = true;
            marketOverview.isOn = true;
            introQ2Tab.isOn = false;
            introQ2Tab.gameObject.SetActive(false);
            introQ3Tab.isOn = false;
            introQ3Tab.gameObject.SetActive(false);
            introQ4Tab.isOn = false;
            introQ4Tab.gameObject.SetActive(false);

        }


    }



}
