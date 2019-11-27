using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalEvaluationUIHandler : MonoBehaviour
{
    public GameObject playerMoneyListContent;
    public GameObject playerMoneyStatsUIComponentPrefab;

    private GameObject myPlayerDataObject;
    private string gameID;

    void Start()
    {
        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        gameID = myPlayerDataObject.GetComponent<PlayerData>().GetGameID();

    }

    public void GenerateContent()
    {
        GeneratePlayerMoneyList();
    }

    public void GeneratePlayerMoneyList()
    {
        foreach (Transform child in playerMoneyListContent.transform)
        { GameObject.Destroy(child.gameObject); }

        Dictionary<string, GameObject> players = GameHandler.allGames[gameID].GetPlayerList();

        foreach (KeyValuePair<string, GameObject> player in players)
        {
            string moneyQ1 = "-";
            string moneyQ2 = "-";
            string moneyQ3 = "-";
            string moneyQ4 = "-";

            PlayerData playerData = player.Value.GetComponent<PlayerData>();
            PlayerRoles playerRole = playerData.GetPlayerRole();

            if (playerRole == PlayerRoles.Developer)
            {
                DeveloperAccountingManager devAccMan = player.Value.GetComponent<DeveloperAccountingManager>();

                moneyQ1 = devAccMan.GetEndCashBalanceQuarter(1).ToString("n0");

                moneyQ2 = devAccMan.GetEndCashBalanceQuarter(2).ToString("n0");

                moneyQ3 = devAccMan.GetEndCashBalanceQuarter(3).ToString("n0");
                moneyQ4 = devAccMan.GetEndCashBalanceQuarter(4).ToString("n0");

            }
            else
            {
                ProviderAccountingManager provAccMan = player.Value.GetComponent<ProviderAccountingManager>();

                moneyQ1 = provAccMan.GetEndCashBalanceQuarter(1).ToString("n0");

                moneyQ2 = provAccMan.GetEndCashBalanceQuarter(2).ToString("n0");

                moneyQ3 = provAccMan.GetEndCashBalanceQuarter(3).ToString("n0");

                moneyQ4 = provAccMan.GetEndCashBalanceQuarter(4).ToString("n0");

            }
            GameObject playerMoneyListUIComponent = Instantiate(playerMoneyStatsUIComponentPrefab);
            playerMoneyListUIComponent.transform.SetParent(playerMoneyListContent.transform, false);
            PlayerMoneyStatsUIComponentHandler playerMoneyStatsUIComponentHandler = playerMoneyListUIComponent.GetComponent<PlayerMoneyStatsUIComponentHandler>();

            string firmName = GameHandler.allGames[gameID].GetFirmName(player.Key);
            playerMoneyStatsUIComponentHandler.SetUpPlayerMoneyUIComponent(firmName, moneyQ1, moneyQ2, moneyQ3, moneyQ4);
        }
    }

}
