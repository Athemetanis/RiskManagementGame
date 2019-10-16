using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProviderAccountingUIHandler : MonoBehaviour
{
    public GameObject providerAccountingUIContainerQ1;
    public GameObject providerAccountingUIContainerQ2;
    public GameObject providerAccountingUIContainerQ3;
    public GameObject providerAccountingUIContainerQ4;


    public ProviderAccountingUIComponentHandler providerAccountingUIComponentHandlerQ1;
    public ProviderAccountingUIComponentHandler providerAccountingUIComponentHandlerQ2;
    public ProviderAccountingUIComponentHandler providerAccountingUIComponentHandlerQ3;
    public ProviderAccountingUIComponentHandler providerAccountingUIComponentHandlerQ4;

    private ProviderAccountingUIComponentHandler providerAccountingUIComponentHandlerCurrent;

    private string gameID;
    private int currentQuarter;
    private GameObject myPlayerDataObject;
    private ProviderAccountingManager providerAccountingManager;

    private void Start()
    {
        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        gameID = myPlayerDataObject.GetComponent<PlayerData>().GetGameID();
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        providerAccountingManager = myPlayerDataObject.GetComponent<ProviderAccountingManager>();
        providerAccountingManager.SetProviderAccountingUIHandler(this);

        providerAccountingUIComponentHandlerQ1.Init();
        providerAccountingUIComponentHandlerQ2.Init();
        providerAccountingUIComponentHandlerQ3.Init();
        providerAccountingUIComponentHandlerQ4.Init();

        SetReferences(currentQuarter);
    }

    public void SetReferences(int quarter)
    {
        switch (quarter)
        {
            case 1:
                providerAccountingManager.SetCurrentProviderAccountingUIHandler(providerAccountingUIComponentHandlerQ1);
                providerAccountingUIContainerQ1.SetActive(true);
                providerAccountingUIComponentHandlerQ1.UpdateAllElements();
                providerAccountingUIContainerQ2.SetActive(false);
                providerAccountingUIContainerQ3.SetActive(false);
                providerAccountingUIContainerQ4.SetActive(false);

                break;
            case 2:
                providerAccountingManager.SetCurrentProviderAccountingUIHandler(providerAccountingUIComponentHandlerQ2);
                providerAccountingUIContainerQ1.SetActive(true);
                providerAccountingUIComponentHandlerQ1.GetHistoryData();
                providerAccountingUIContainerQ2.SetActive(true);
                providerAccountingUIComponentHandlerQ2.UpdateAllElements();
                providerAccountingUIContainerQ3.SetActive(false);
                providerAccountingUIContainerQ4.SetActive(false);
                break;
            case 3:
                providerAccountingManager.SetCurrentProviderAccountingUIHandler(providerAccountingUIComponentHandlerQ3);
                providerAccountingUIContainerQ1.SetActive(true);
                providerAccountingUIComponentHandlerQ1.GetHistoryData();
                providerAccountingUIContainerQ2.SetActive(true);
                providerAccountingUIComponentHandlerQ2.GetHistoryData();
                providerAccountingUIContainerQ3.SetActive(true);
                providerAccountingUIComponentHandlerQ3.UpdateAllElements();
                providerAccountingUIContainerQ4.SetActive(false);
                break;
            case 4:
                providerAccountingManager.SetCurrentProviderAccountingUIHandler(providerAccountingUIComponentHandlerQ4);
                providerAccountingUIContainerQ1.SetActive(true);
                providerAccountingUIComponentHandlerQ1.GetHistoryData();
                providerAccountingUIContainerQ2.SetActive(true);
                providerAccountingUIComponentHandlerQ2.GetHistoryData();
                providerAccountingUIContainerQ3.SetActive(true);
                providerAccountingUIComponentHandlerQ3.GetHistoryData();
                providerAccountingUIContainerQ4.SetActive(true);
                providerAccountingUIComponentHandlerQ4.UpdateAllElements();
                break;
        }
    }


}
