using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerGameStateUIComponentHandler : MonoBehaviour
{
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI firmName;
    public Toggle playerGameState;

    public void SetUpPlayerGameStateUIComponent(string playerName, string firmName, bool playerGameState)
    {
        this.playerName.text = playerName;
        this.firmName.text = firmName;
        this.playerGameState.isOn = playerGameState;
    }
}
