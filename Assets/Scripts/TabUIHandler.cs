using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabUIHandler : MonoBehaviour
{
    public Toggle gameBasicsTab;
    public Toggle welcomeTab;
    public Toggle marketOverviewTab;
    public Toggle introQ2Tab;
    public Toggle introQ3Tab;
    public Toggle introQ4Tab;
    public Toggle endEvaluationTab;
    public Toggle submitTab;

    public GameObject gameEnd;

    public GameObject gameBasicsContent;
    public GameObject welcomeContent;
    public GameObject marketOverviewContent;
    public GameObject introQ2Content;
    public GameObject introQ3Content;
    public GameObject introQ4Content;
    public GameObject endEvaluationContent;
    public GameObject submitContent;

    private FirmUIHandler firmUIHandler;
    private ContractUIHandler contractUIHandler;   //tlacitko pre novy kontrakt
    private HumanResourcesUIHandler humanResourcesUIHandler; //talcitka na pridavanie zamest //slidery pre mzdy
    private MarketingUIHandler marketingUIHandeler; //ceny advert
    //tlacitka na kupu researchu
    private DeveloperResearchUIHandler developerResearchUIHandler;
    private ProviderResearchUIHandler providerResearchUIHandler;
    private FinalEvaluationUIHandler finalEvaluationUIHandler;


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

        firmUIHandler = this.gameObject.GetComponent<FirmUIHandler>();
        contractUIHandler = this.gameObject.GetComponent<ContractUIHandler>();
        humanResourcesUIHandler = this.gameObject.GetComponent<HumanResourcesUIHandler>();
        marketingUIHandeler = this.gameObject.GetComponent<MarketingUIHandler>();
        developerResearchUIHandler = this.gameObject.GetComponent<DeveloperResearchUIHandler>();
        providerResearchUIHandler = this.gameObject.GetComponent<ProviderResearchUIHandler>();
        finalEvaluationUIHandler = this.gameObject.GetComponent<FinalEvaluationUIHandler>();


        HandlingQTabs();
    }


    public void HandlingQTabs()
    {
        currentQuarter = gameData.GetGameRound();

        if (currentQuarter == 1)
        {
            gameBasicsTab.isOn = true;
            gameBasicsTab.gameObject.SetActive(true);
            gameBasicsContent.SetActive(true);


            welcomeTab.isOn = false;
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

            welcomeContent.SetActive(false);
            marketOverviewContent.SetActive(false);
            introQ2Content.SetActive(false);
            introQ3Content.SetActive(false);
            introQ4Content.SetActive(false);
            endEvaluationContent.SetActive(false);

        }
        if (currentQuarter == 2)
        {
            gameBasicsTab.isOn = false;
           // gameBasicsTab.gameObject.SetActive(false);
            gameBasicsContent.SetActive(false);

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
            gameBasicsTab.isOn = false;
           // gameBasicsTab.gameObject.SetActive(false);
            gameBasicsContent.SetActive(false);

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
            gameBasicsTab.isOn = false;
           // gameBasicsTab.gameObject.SetActive(false);
            gameBasicsContent.SetActive(false);

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
            gameBasicsTab.isOn = false;
            //gameBasicsTab.gameObject.SetActive(false);
            gameBasicsContent.SetActive(false);

            finalEvaluationUIHandler.GenerateContent();

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

            submitTab.isOn = false;
            submitTab.gameObject.SetActive(false);
            submitContent.SetActive(false);


            welcomeContent.SetActive(false);
            marketOverviewContent.SetActive(false);
            introQ2Content.SetActive(false);
            introQ3Content.SetActive(false);
            introQ4Content.SetActive(false);
            endEvaluationContent.SetActive(true);

            firmUIHandler.DisableEditation();
            contractUIHandler.DisableEditation();

            if (humanResourcesUIHandler != null)
            {
                humanResourcesUIHandler.DisableEditation();
            }
            if (marketingUIHandeler != null)
            {
                marketingUIHandeler.DisableEditation();
            }
            if (developerResearchUIHandler != null)
            {
                developerResearchUIHandler.DisableEditation();
            }
            if (providerResearchUIHandler != null)
            {
                providerResearchUIHandler.DisableEditation();
            }

            finalEvaluationUIHandler.GenerateContent();


        }
        if (currentQuarter == 6)
        {
            gameEnd.SetActive(true);
        }

    }
}
