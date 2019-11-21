using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatMessageUIHandler : MonoBehaviour
{
    public TextMeshProUGUI senderText;
    public TextMeshProUGUI messageText;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetUpMessageText(string sender, string message)
    {
        senderText.text = sender;
        if(sender == "Me")
        {
            senderText.color = new Color(252, 3, 82);
        }else
            senderText.color = new Color(7, 160, 242);
        messageText.text = message;
    }
}
