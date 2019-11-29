using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatPlayerToggleUIHandler : MonoBehaviour
{
    public TextMeshProUGUI firmName;
    public TextMeshProUGUI playerName;

    private Toggle playerToggle;
    private string playerID;

    public GameObject notificationImage;

    private ChatUIHandler chatUIHandler;

    public void SetChatUIHandler(ChatUIHandler chatUIHandler) { this.chatUIHandler = chatUIHandler; }

    public void SetUpChatPlayerToggle(ToggleGroup tg, string playerID, string firmName, string playerName)
    {
        playerToggle = this.GetComponent<Toggle>();
        playerToggle.group = tg;

        this.playerID = playerID;
        this.firmName.text = firmName;
        this.playerName.text = playerName;
        playerToggle.onValueChanged.AddListener(delegate { ToggleValueChanged(playerToggle); });
    }

    public void ToggleValueChanged(Toggle change)
    {
        if (playerToggle.isOn)
        {
            chatUIHandler.GenerateChatContent(playerID);
            chatUIHandler.SetRecipient(playerID);
        }

    }

    public void ShowNotificationImage()
    {
        notificationImage.SetActive(true);
    }

    public void DisableNotificationImageAfterClick()
    {
        notificationImage.SetActive(false);
    }


}
