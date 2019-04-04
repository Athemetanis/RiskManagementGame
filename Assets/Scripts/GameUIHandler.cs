using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIHandler : MonoBehaviour
{
    //VARIABLES

    public Text NameText;
    public Text IDText;
    public Text PlayersCountText;
    public Text RoundText;

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
