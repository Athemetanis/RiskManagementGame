using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public struct Message
{
    public readonly string sender;
    public readonly string recipent;
    public readonly string message;

    public Message (string recipent, string sender, string message)
    {
        this.sender = sender;
        this.recipent = recipent;
        this.message = message;
    }

    public override bool Equals(object obj)
    {
        if (Object.ReferenceEquals(obj, null))
        {
            return false;
        }
        if (Object.ReferenceEquals(this, obj))
        {
            return true;
        }
        if (this.GetType() != obj.GetType())
        {
            return false;
        }
        if (obj is Message)
        {
            Message msg = (Message)obj;
            return (this.recipent == msg.recipent) && (this.sender == msg.sender) && (this.message == msg.message);
        }
        else
        {
            return false;
        }
    }
    public bool Equals(Message msg)
    {
        if (Object.ReferenceEquals(msg, null))
        {
            return false;
        }
        if (Object.ReferenceEquals(this, msg))
        {
            return true;
        }
        if (this.GetType() != msg.GetType())
        {
            return false;
        }
        return (this.recipent == msg.recipent) && (this.sender == msg.sender) && (this.message == msg.message);
    }

    public override int GetHashCode()
    {
        var hashCode = 1047090843;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(sender);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(recipent);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(message);

        return hashCode;
    }
}

public class SyncDictionaryMessage : SyncDictionary<string, Message> { };

public class ChatManager : NetworkBehaviour
{
    private SyncDictionaryMessage myMessages = new SyncDictionaryMessage();
    private SyncListString messageOrder = new SyncListString();

    private string gameID;
    private PlayerData playerData;
    private FirmManager firmManager;

    private ChatUIHandler chatUIHandler;

    public void SetChatUIHandler(ChatUIHandler chatUIHandler) { this.chatUIHandler = chatUIHandler; }
    public Dictionary<string, Message> GetMyMessages() { return new Dictionary<string, Message>(myMessages); }
    
    void Start() { }
    public override void OnStartServer()
    {
        playerData = this.gameObject.GetComponent<PlayerData>();
        gameID = this.gameObject.GetComponent<PlayerData>().GetGameID();
        firmManager = this.gameObject.GetComponent<FirmManager>();
    }
    public override void OnStartClient()
    {
        playerData = this.gameObject.GetComponent<PlayerData>();
        gameID = this.gameObject.GetComponent<PlayerData>().GetGameID();
        firmManager = this.gameObject.GetComponent<FirmManager>();

        myMessages.Callback += OnChangeMyMessages;

    }

    public void SendMessage(string recipientfirmName, string messageText)
    {
        Message message = new Message(firmManager.GetFirmName(), recipientfirmName, messageText);
        CmdSendMessage(message);
    }

    [Command]
    public void CmdSendMessage(Message message)
    {   
        string messageID = GameHandler.singleton.GenerateUniqueID();
        string recipientPlayerID = GameHandler.allGames[gameID].GetFirmPlayerID(message.recipent);
        string senderPlayerID = GameHandler.allGames[gameID].GetFirmPlayerID(message.sender);

        ChatManager recipentChatManager = GameHandler.allGames[gameID].GetPlayer(recipientPlayerID).GetComponent<ChatManager>();
        ChatManager senderChatManager = GameHandler.allGames[gameID].GetPlayer(senderPlayerID).GetComponent<ChatManager>();
        recipentChatManager.AddMessageToMyMessages(messageID, message);
        senderChatManager.AddMessageToMyMessages(messageID, message);
    }

    public void AddMessageToMyMessages(string messageID, Message message)
    {
        if (myMessages.Count == 500) ///deletes oldes message
        {
            myMessages.Remove(messageOrder[0]);
            messageOrder.RemoveAt(0);
            messageOrder.Add(messageID);
            myMessages.Add(messageID, message);
        }
        else
        {
            messageOrder.Add(messageID);
            myMessages.Add(messageID, message);
        }
    }

    public void OnChangeMyMessages(SyncDictionaryMessage.Operation op, string key, Message msg)
    {
        if(op == SyncDictionary<string, Message>.Operation.OP_ADD)
        {
            if (msg.sender != playerData.GetPlayerID())
            {
                ShowNotificationChat(msg.sender);
            }            
        }

    }

    public void ShowNotificationChat(string playerID)
    {
        string sender = GameHandler.allGames[gameID].GetFirmName(playerID);
        if (chatUIHandler != null)
        {
            chatUIHandler.ShowMessageNotification(sender);
        }
    }

}
