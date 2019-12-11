using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class NetworkUIHandler : MonoBehaviour
{
    public CustomNetworkManager manager;

    // public GameObject networkUIgameObject;

    public GameObject quitButton;
    public GameObject startServerUI;
    public GameObject connectingUI;
    public GameObject connectedUI;
    public GameObject serverActive;

    private static NetworkUIHandler networkUI;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (networkUI == null)
        {
            networkUI = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
    private void Start()
    {
        if(manager == null)
        {
            GameObject networkManagerObject = GameObject.FindGameObjectWithTag("CustomNetworkManage");
            manager = networkManagerObject.GetComponent<CustomNetworkManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {   
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            if (!NetworkClient.active)
            {
                quitButton.SetActive(true);
                startServerUI.SetActive(true);

                connectedUI.SetActive(false);
                connectingUI.SetActive(false);
                serverActive.SetActive(false);

            }
            else
            {
                quitButton.SetActive(false);
                connectingUI.SetActive(true);
                startServerUI.SetActive(false);
                connectedUI.SetActive(false);
                serverActive.SetActive(false);
            }

        }
        else
        {
            if (NetworkClient.isConnected)
            {
                quitButton.SetActive(false);
                connectedUI.SetActive(true);
                connectingUI.SetActive(false);
                startServerUI.SetActive(false);
                serverActive.SetActive(false);
            }
            if (NetworkServer.active)
            {
                quitButton.SetActive(false);
                connectedUI.SetActive(false);
                connectingUI.SetActive(false);
                startServerUI.SetActive(false);
                serverActive.SetActive(true);
            }

        }
    }

    public void MyStartClient()
    {
        if (manager == null)
        {
            GameObject networkManagerObject = GameObject.FindGameObjectWithTag("CustomNetworkManager");
            manager = networkManagerObject.GetComponent<CustomNetworkManager>();
        }
        manager.StartClient();
    }

    public void MyStartServer()
    {
        if (manager == null)
        {
            GameObject networkManagerObject = GameObject.FindGameObjectWithTag("CustomNetworkManager");
            manager = networkManagerObject.GetComponent<CustomNetworkManager>();
        }
        manager.StartMyServer();
    }

    public void MyStopCliet()
    {
        if (manager == null)
        {
            GameObject networkManagerObject = GameObject.FindGameObjectWithTag("CustomNetworkManager");
            manager = networkManagerObject.GetComponent<CustomNetworkManager>();
        }
        manager.StopClient();
      //  Destroy(networkUIgameObject);
    }

    public void MyStopServer()
    {
        if (manager == null)
        {
            GameObject networkManagerObject = GameObject.FindGameObjectWithTag("CustomNetworkManager");
            manager = networkManagerObject.GetComponent<CustomNetworkManager>();
        }
        manager.StopHost();
      //  Destroy(networkUIgameObject);
    }


    public void Quit()
    {
        Application.Quit();
    }

           
}
