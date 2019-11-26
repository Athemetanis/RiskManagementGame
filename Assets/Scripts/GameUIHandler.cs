using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIHandler : MonoBehaviour
{
    //VARIABLES
    
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI IDText;
    public TextMeshProUGUI PlayersCountText;
    public TextMeshProUGUI RoundText;

    //METHODS
    public void ChangeNameText(string name)
    {
        NameText.text = name;
    }

    public void ChangeIDText(string ID)
    {
        IDText.text = ID;
    }

    public void ChangeRoundText(int round)
    {
        RoundText.text = round.ToString();
    }

    public void ChangePlayersCountText(int playersCount)
    {
        PlayersCountText.text = playersCount.ToString();
    }

   



}
