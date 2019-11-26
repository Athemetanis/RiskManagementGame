using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabUIHandler : MonoBehaviour
{
    public Toggle welcomeTab;
    public Toggle marketOverviewTab;
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
    private PlayerData playerData;
    private string gameID;
    private GameData gameData;

    private int currentQuarter;

    private void Start()
    {
        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        playerData = myPlayerDataObject.GetComponent<PlayerData>();
        gameID = myPlayerDataObject.GetComponent<PlayerData>().GetGameID();
        gameData = GameHandler.allGames[gameID];

        currentQuarter = gameData.GetGameRound();

        HandlingQTabs();
    }


    public void HandlingQTabs()
    {
        currentQuarter = gameData.GetGameRound();

        if (currentQuarter == 1)
        {
            welcomeTab.isOn = true;
            welcomeTab.gameObject.SetActive(true);
            marketOverviewTab.isOn = false;
            marketOverviewTab.gameObject.SetActive(true);

            introQ2Tab.isOn = false;
            introQ2Tab.gameObject.SetActive(false);

            introQ3Tab.isOn = false;
            introQ3Tab.gameObject.SetActive(false);

            introQ4Tab.isOn = false;
            introQ4Tab.gameObject.SetActive(false);

            endEvaluationTab.isOn = false;
            endEvaluationTab.gameObject.SetActive(false);

            welcomeContent.SetActive(true);
            marketOverviewContent.SetActive(false);
            introQ2Content.SetActive(false);
            introQ3Content.SetActive(false);
            introQ4Content.SetActive(false);
            endEvaluationContent.SetActive(false);

        }
        if (currentQuarter == 2)
        {               
            welcomeTab.isOn = false;
            welcomeTab.gameObject.SetActive(false);
            marketOverviewTab.isOn = false;
            marketOverviewTab.gameObject.SetActive(false);

            introQ2Tab.isOn = true;
            introQ2Tab.gameObject.SetActive(true);


            introQ3Tab.isOn = false;
            introQ3Tab.gameObject.SetActive(false);

            introQ4Tab.isOn = false;
            introQ4Tab.gameObject.SetActive(false);

            endEvaluationTab.isOn = false;
            endEvaluationTab.gameObject.SetActive(false);

            welcomeContent.SetActive(false);
            marketOverviewContent.SetActive(false);
            introQ2Content.SetActive(true);
            introQ3Content.SetActive(false);
            introQ4Content.SetActive(false);
            endEvaluationContent.SetActive(false);

        }
        if (currentQuarter == 3)
        {
            welcomeTab.isOn = false;
            welcomeTab.gameObject.SetActive(false);
            marketOverviewTab.isOn = false;
            marketOverviewTab.gameObject.SetActive(false);

            introQ2Tab.isOn = false;
            introQ2Tab.gameObject.SetActive(false);

            introQ3Tab.isOn = true;
            introQ3Tab.gameObject.SetActive(true);

            introQ4Tab.isOn = false;
            introQ4Tab.gameObject.SetActive(false);
            endEvaluationTab.isOn = false;
            endEvaluationTab.gameObject.SetActive(false);

            welcomeContent.SetActive(false);
            marketOverviewContent.SetActive(false);
            introQ2Content.SetActive(false);
            introQ3Content.SetActive(true);
            introQ4Content.SetActive(false);
            endEvaluationContent.SetActive(false);
        }
        if (currentQuarter == 4)
        {
            welcomeTab.isOn = false;
            welcomeTab.gameObject.SetActive(false);
            marketOverviewTab.isOn = false;
            marketOverviewTab.gameObject.SetActive(false);


            introQ2Tab.isOn = false;
            introQ2Tab.gameObject.SetActive(false);
            introQ3Tab.isOn = false;
            introQ3Tab.gameObject.SetActive(false);

            introQ4Tab.isOn = true;
            introQ4Tab.gameObject.SetActive(true);

            endEvaluationTab.isOn = false;
            endEvaluationTab.gameObject.SetActive(false);

            welcomeContent.SetActive(false);
            marketOverviewContent.SetActive(false);
            introQ2Content.SetActive(false);
            introQ3Content.SetActive(false);
            introQ4Content.SetActive(true);
            endEvaluationContent.SetActive(false);
        }

        if (currentQuarter == 5)
        {
            welcomeTab.isOn = false;
            welcomeTab.gameObject.SetActive(false);
            marketOverviewTab.isOn = false;
            marketOverviewTab.gameObject.SetActive(false);

            introQ2Tab.isOn = false;
            introQ2Tab.gameObject.SetActive(false);
            introQ3Tab.isOn = false;
            introQ3Tab.gameObject.SetActive(false);
            introQ4Tab.isOn = false;
            introQ4Tab.gameObject.SetActive(false);

            endEvaluationTab.isOn = true;
            endEvaluationTab.gameObject.SetActive(true);

            
            welcomeContent.SetActive(false);
            marketOverviewContent.SetActive(false);
            introQ2Content.SetActive(false);
            introQ3Content.SetActive(false);
            introQ4Content.SetActive(false);
            endEvaluationContent.SetActive(true);
        }
    }
}
