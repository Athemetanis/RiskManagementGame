using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This class manages values of UI text elements of UI element which represents the games. 
/// This UI representation of the game is showed in the UI List of games aviables for PLAYER 
/// </summary>
public class GameUIHandler : MonoBehaviour
{
    //VARIABLES
    
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI IDText;
    public TextMeshProUGUI PlayersCountText;
    public TextMeshProUGUI RoundText;

    //METHODS
    /// <summary>
    /// Changes Name Text of UI element which represents certain game
    /// </summary>
    /// <param name="name"> Name of the game displayed for user to see</param>
    public void ChangeNameText(string name)
    {
        NameText.text = name;
    }

    /// <summary>
    /// Changes ID Text of UI element which represents certain game 
    /// </summary>
    /// <param name="ID"> ID of the game displayed for user to see </param>
    public void ChangeIDText(string ID)
    {
        IDText.text = ID;
    }
    /// <summary>
    /// Changes ROUND Text of UI element which represents certain game  
    /// </summary>
    /// <param name="round">Round of the game displayed for user to see</param>
    public void ChangeRoundText(int round)
    {
        RoundText.text = round.ToString();
    }

    /// <summary>
    /// Changes Player's Count Text of UI element which represents certain game  
    /// </summary>
    /// <param name="playersCount">Player count of the game displayed for user to see</param>
    public void ChangePlayersCountText(int playersCount)
    {
        PlayersCountText.text = playersCount.ToString();
    }

   



}
