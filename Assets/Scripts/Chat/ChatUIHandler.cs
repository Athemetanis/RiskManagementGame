using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatUIHandler : MonoBehaviour
{
    public ToggleGroup toggleGroup;

    public TMP_InputField messageIF;

    public TextMeshProUGUI infoText;
    public GameObject notificationImage;
    public Toggle chatTab;

    public GameObject messageContent;
    public GameObject providerContent;
    public GameObject developerContent;

    public GameObject playerTogglePrefab;
    public GameObject chatMessagePrefab;

    private GameObject myPlayerDataObject;
    private PlayerData playerData;
    private ChatManager chatManager;

    private string gameID;
    private GameData gameData;

    private string recipient;
    

    public void SetRecipient(string playerID) { recipient = playerID; }

    private void Start()
    {   
        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        playerData = myPlayerDataObject.GetComponent<PlayerData>();
        gameID = myPlayerDataObject.GetComponent<PlayerData>().GetGameID();
        gameData = GameHandler.allGames[gameID];

        chatManager = myPlayerDataObject.GetComponent<ChatManager>();
        chatManager.SetChatUIHandler(this);

        GeneratePlayersContent();
    }

    public void GeneratePlayersContent()
    {
        foreach (Transform child in providerContent.transform)
        { GameObject.Destroy(child.gameObject); }
        foreach (Transform child in developerContent.transform)
        { GameObject.Destroy(child.gameObject); }

        string myPlayerID = playerData.GetPlayerID();

        Dictionary<string, GameObject> developers = GameHandler.allGames[gameID].GetDeveloperList();
        Dictionary<string, GameObject> providers = GameHandler.allGames[gameID].GetProviderList();

        foreach (KeyValuePair<string, GameObject> developer in developers)
        {
            PlayerData playerData = developer.Value.GetComponent<PlayerData>();

            string playerID = playerData.GetPlayerID();

            if (myPlayerID == playerID)
            {
                return;
            }

            string firmName = gameData.GetFirmName(playerID);

            GameObject playerToggle = Instantiate(playerTogglePrefab);
            playerToggle.transform.SetParent(developerContent.transform, false);
            ChatPlayerToggleUIHandler playerToggleHandler = playerToggle.GetComponent<ChatPlayerToggleUIHandler>();
            playerToggleHandler.SetChatUIHandler(this);
            playerToggleHandler.SetUpChatPlayerToggle(toggleGroup, playerID, firmName, playerID);
        }

        foreach (KeyValuePair<string, GameObject> provider in providers)
        {
            PlayerData playerData = provider.Value.GetComponent<PlayerData>();
            string playerID = playerData.GetPlayerID();

            if (myPlayerID == playerID)
            {
                return;
            }
            string firmName = gameData.GetFirmName(playerID);

            GameObject playerToggle = Instantiate(playerTogglePrefab);
            playerToggle.transform.SetParent(providerContent.transform, false);
            ChatPlayerToggleUIHandler playerToggleHandler = playerToggle.GetComponent<ChatPlayerToggleUIHandler>();
            playerToggleHandler.SetChatUIHandler(this);
            playerToggleHandler.SetUpChatPlayerToggle(toggleGroup, playerID, firmName, playerID);
        }
    }

    public void GenerateChatContent(string playerID)
    {
        foreach (Transform child in messageContent.transform)
        { GameObject.Destroy(child.gameObject); }

        Dictionary<string, Message> myMessages = chatManager.GetMyMessages();

        foreach (KeyValuePair<string, Message> msg in myMessages)
        {
            string sender = msg.Value.sender;
            string recipent = msg.Value.recipent;
            string message = msg.Value.message;
            if(sender == playerID || recipent == playerID)
            {

                GameObject newMessageOBJ = Instantiate(chatMessagePrefab);
                newMessageOBJ.transform.SetParent(messageContent.transform, false);
                ChatMessageUIHandler chatMessageUIHandler = newMessageOBJ.GetComponent<ChatMessageUIHandler>();

                if(sender == playerData.GetPlayerID())
                {
                    sender = "Me";
                    
                }
                chatMessageUIHandler.SetUpMessageText(sender, message);
            }
        }
    }

    public void SendChatMessage()
    {   
        if(recipient != null)
        {
            chatManager.SendMessage(recipient, messageIF.text);
        }       
    }

    public void ShowMessageNotification(string firmName)
    {
        infoText.text = "New message from " + firmName + ".";
        infoText.enabled = true;
        ChatNotificationON();
    }

    public void ChatNotificationON()
    {
        notificationImage.SetActive(true);
        if (chatTab.isOn == true)
        {
            StartCoroutine(Wait5SecondsAndDisable());
        }
    }

    public void ChatNotificationOFF() //trigered by UI when contractTabActivated or ked je contract tab aktivne a vyprsi urcity casovy limit
    {
        if (notificationImage.activeSelf == true)
        {
            notificationImage.SetActive(false);
        }
        if(infoText.enabled == true)
        {
            infoText.enabled = false;
        }
    }

    IEnumerator Wait5SecondsAndDisable()
    {
        yield return new WaitForSeconds(5);
        ChatNotificationOFF();
    }
}
