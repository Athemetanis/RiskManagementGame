using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FirmUIHandler : MonoBehaviour
{
    public TMP_InputField firmNameIF;
    public TMP_InputField firmDescriptionIF;
    public TextMeshProUGUI errorMessageText;

    public TMP_InputField playerNameIF;

    public Button confirmFirmNameChangeButton;
    public Button confirmPlayerNameChangeButton;

    
    public Toggle marketSize1;
    public Toggle marketSize2;
    public Toggle marketSize3;

    public Toggle competitivePosture1;
    public Toggle competitivePosture2;
    public Toggle competitivePosture3;
    public Toggle competitivePosture4;
    public Toggle competitivePosture5;

    public Toggle distictiveComp1;
    public Toggle distictiveComp2;
    public Toggle distictiveComp3;
    public Toggle distictiveComp4;
    public Toggle distictiveComp5;

    public Toggle businesspartnerDiversity1;
    public Toggle businesspartnerDiversity2;
    public Toggle businesspartnerDiversity3;

    public Toggle contractPriorities1;
    public Toggle contractPriorities2;
    public Toggle contractPriorities3;
    public Toggle contractPriorities4;

    public Toggle accountingStrategies1;
    public Toggle accountingStrategies2;
    public Toggle accountingStrategies3;

    public Toggle growthStrategies1;
    public Toggle growthStrategies2;
    public Toggle growthStrategies3;

    public Toggle developmentStrategies1;
    public Toggle developmentStrategies2;
    public Toggle developmentStrategies3;




    private FirmManager firmManager;

    // Updating gui according saved data on start
    void Start()
    {
        firmManager = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject().GetComponent<FirmManager>();
        firmManager.SetFirmUIHandler(this);
        //firmNameIF.text = firmManager.GetFirmName();
        firmDescriptionIF.text = firmManager.GetFirmDescription();

        if (firmManager.GetFirmNameChanged())
        {
            DisableChangingFirmName();
        }

        if (firmManager.GetPlayerNameChanged())
        {
            DisableChangePlayerName();
        }
    }

    // If gui changed call this - try to set new firmName/description
    public void SetFirmName()
    {   
        if(firmNameIF.text == "")
        {
            return;
        }
        firmManager.TryToChangeFirmName(firmNameIF.text);
    }

    public void SetFirmDescription()
    {
        firmManager.ChangeFirmDescription(firmDescriptionIF.text);

    }

    public void SetPlayerName()
    {
        if (playerNameIF.text == "")
        {
            return;
        }
        firmManager.SetPlayerName(playerNameIF.text);
    }

    //methods activated by toggles onValueChanged
    public void SetMarketsize()
    {
        firmManager.SetFirmDecisionsMarketSize(marketSize1.isOn, marketSize2.isOn, marketSize3.isOn);
    }
    public void SetCompetitivePosture()
    {
        firmManager.SetCompetitivePosture(competitivePosture1.isOn, competitivePosture2.isOn, competitivePosture3.isOn, competitivePosture4.isOn, competitivePosture5.isOn);
    }
    public void SetDistinctiveCompetencies()
    {
        firmManager.SetDistictiveCompetencies(distictiveComp1.isOn, distictiveComp2.isOn, distictiveComp3.isOn, distictiveComp4.isOn, distictiveComp5.isOn);
    }
    public void SetBusinessPartnerDiversity()
    {
        firmManager.SetBusinessDiversity(businesspartnerDiversity1.isOn, businesspartnerDiversity2.isOn, businesspartnerDiversity3.isOn);
    }
    public void SetContractPriorities()
    {
        firmManager.SetContractPrioryties(contractPriorities1.isOn, contractPriorities2.isOn, contractPriorities3.isOn, contractPriorities4.isOn);
    }
    public void SetAccountingStrategies()
    {
        firmManager.SetAccountingStrategies(accountingStrategies1.isOn, accountingStrategies2.isOn, accountingStrategies3.isOn);
    }
    public void SetGrowthStrategies()
    {
        firmManager.SetGrowthStategies(growthStrategies1.isOn, growthStrategies2.isOn, growthStrategies3.isOn);
    }
    public void SetDevelopmentStrategies()
    {
        firmManager.SetDevelopmentStrategies(developmentStrategies1.isOn, developmentStrategies2.isOn, developmentStrategies3.isOn);
    }



    //Methods for updating UI values
    public void SetUIFirmName(string firmName)
    {
        firmNameIF.text = firmName;
    }
    public void SetUIFirmDescription(string firmDescription)
    {
        firmDescriptionIF.text = firmDescription;
        
    }
    public void SetPlayerName(string playerName)
    {
        playerNameIF.text = playerName;
        playerNameIF.interactable = false;

    }
    public void SetUIFirmNameOld(string oldFirmName)
    {
        Debug.Log("nastavujem stare meno");
        firmNameIF.text = oldFirmName;
        errorMessageText.gameObject.SetActive(true);        
    }
    public void DisableChangingFirmName()
    {
        firmNameIF.interactable = false;
        confirmFirmNameChangeButton.gameObject.SetActive(false);
    }

    public void DisableChangePlayerName()
    {
        playerNameIF.interactable = false;
        confirmPlayerNameChangeButton.gameObject.SetActive(false);
    }

    public void SetUIFirmDecisions()
    {



    }







}
