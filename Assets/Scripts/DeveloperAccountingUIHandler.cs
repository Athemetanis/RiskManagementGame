using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeveloperAccountingUIHandler : MonoBehaviour
{
    public GameObject developerAccountingUIContainerQ1;
    public GameObject developerAccountingUIContainerQ2;
    public GameObject developerAccountingUIContainerQ3;
    public GameObject developerAccountingUIContainerQ4;
    public GameObject developerAccountingUIContainerQ5;

    public DeveloperAccountingUIComponentHandler developerAccountingUIComponentHandlerQ1;
    public DeveloperAccountingUIComponentHandler developerAccountingUIComponentHandlerQ2;
    public DeveloperAccountingUIComponentHandler developerAccountingUIComponentHandlerQ3;
    public DeveloperAccountingUIComponentHandler developerAccountingUIComponentHandlerQ4;
    public DeveloperAccountingUIComponentHandler developerAccountingUIComponentHandlerQ5;

    // private DeveloperAccountingUIComponentHandler developerAccountingUIComponentHandlerCurrent;

    private string gameID;
    private int currentQuarter;
    private GameObject myPlayerDataObject;
    private DeveloperAccountingManager developerAccountingManager;

    private void Start()
    {
        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        gameID = myPlayerDataObject.GetComponent<PlayerManager>().GetGameID();
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        developerAccountingManager = myPlayerDataObject.GetComponent<DeveloperAccountingManager>();
        developerAccountingManager.SetDeveloperAccountingUIHandler(this);

        developerAccountingUIComponentHandlerQ1.Init();
        developerAccountingUIComponentHandlerQ2.Init();
        developerAccountingUIComponentHandlerQ3.Init();
        developerAccountingUIComponentHandlerQ4.Init();
        developerAccountingUIComponentHandlerQ5.Init();

        SetReferences(currentQuarter);
    }

    public void SetReferences(int quarter)
    {
        switch (quarter)
        {
            case 1:
                developerAccountingManager.SetCurrentDeveloperAccountingUIHandler(developerAccountingUIComponentHandlerQ1);
                developerAccountingUIContainerQ1.SetActive(true);
                developerAccountingUIComponentHandlerQ1.UpdateAllElements();
                developerAccountingUIContainerQ2.SetActive(false);
                developerAccountingUIContainerQ3.SetActive(false);
                developerAccountingUIContainerQ4.SetActive(false);
                developerAccountingUIContainerQ5.SetActive(false);
                break;
            case 2:
                developerAccountingManager.SetCurrentDeveloperAccountingUIHandler(developerAccountingUIComponentHandlerQ2);
                developerAccountingUIContainerQ1.SetActive(true);
                developerAccountingUIComponentHandlerQ1.GetHistoryData();
                developerAccountingUIContainerQ2.SetActive(true);
                developerAccountingUIComponentHandlerQ2.UpdateAllElements();
                developerAccountingUIContainerQ3.SetActive(false);
                developerAccountingUIContainerQ4.SetActive(false);
                developerAccountingUIContainerQ5.SetActive(false);
                break;
            case 3:
                developerAccountingManager.SetCurrentDeveloperAccountingUIHandler(developerAccountingUIComponentHandlerQ3);
                developerAccountingUIContainerQ1.SetActive(true);
                developerAccountingUIComponentHandlerQ1.GetHistoryData();
                developerAccountingUIContainerQ2.SetActive(true);
                developerAccountingUIComponentHandlerQ2.GetHistoryData();
                developerAccountingUIContainerQ3.SetActive(true);
                developerAccountingUIComponentHandlerQ3.UpdateAllElements();
                developerAccountingUIContainerQ4.SetActive(false);
                developerAccountingUIContainerQ5.SetActive(false);
                break;
            case 4:
                developerAccountingManager.SetCurrentDeveloperAccountingUIHandler(developerAccountingUIComponentHandlerQ4);
                developerAccountingUIContainerQ1.SetActive(true);
                developerAccountingUIComponentHandlerQ1.GetHistoryData();
                developerAccountingUIContainerQ2.SetActive(true);
                developerAccountingUIComponentHandlerQ2.GetHistoryData();
                developerAccountingUIContainerQ3.SetActive(true);
                developerAccountingUIComponentHandlerQ3.GetHistoryData();
                developerAccountingUIContainerQ4.SetActive(true);
                developerAccountingUIComponentHandlerQ4.UpdateAllElements();
                developerAccountingUIContainerQ5.SetActive(false);
                break;
            case 5:
                developerAccountingManager.SetCurrentDeveloperAccountingUIHandler(developerAccountingUIComponentHandlerQ5);
                developerAccountingUIContainerQ1.SetActive(true);
                developerAccountingUIComponentHandlerQ1.GetHistoryData();
                developerAccountingUIContainerQ2.SetActive(true);
                developerAccountingUIComponentHandlerQ2.GetHistoryData();
                developerAccountingUIContainerQ3.SetActive(true);
                developerAccountingUIComponentHandlerQ3.GetHistoryData();
                developerAccountingUIContainerQ4.SetActive(true);
                developerAccountingUIComponentHandlerQ4.GetHistoryData();
                developerAccountingUIContainerQ5.SetActive(true);
                developerAccountingUIComponentHandlerQ5.UpdateAllElements();
                break;
        }
    }

}
