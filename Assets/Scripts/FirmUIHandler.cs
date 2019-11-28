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
                        

        if (firmManager.GetFirmNameChanged())
        {
            firmNameIF.text = firmManager.GetFirmName();
        }
        if (firmManager.GetPlayerNameChanged())
        {
            playerNameIF.text = firmManager.GetPlayerName();
        }

        firmDescriptionIF.text = firmManager.GetFirmDescription();

        if (firmManager.GetFirmNameChanged())
        {
            DisableChangingFirmName();
        }

        if (firmManager.GetPlayerNameChanged())
        {
            DisableChangePlayerName();
        }
        SetUIFirmDecisions();
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

        SetMarketsize(firmManager.GetMarketSize());
        SetCompetitivePosture(firmManager.GetCompetitivePosture());
        SetContractPriorities(firmManager.GetContractPriorities());
        SetDistinctiveCompetencies(firmManager.GetDistinctiveCompetenceis());
        SetBusinessPartnerDiversity(firmManager.GetBusinessPartnerDiversity());
        SetAccountingStrategies(firmManager.GetFirmAccouningStrategies());
        SetGrowthStrategies(firmManager.GetGrowthStrategies());
        SetDevelopmentStrategies(firmManager.GetDevelopmentStrategies());
    }

    public void SetMarketsize((bool ms1, bool ms2, bool ms3) marketSize)
    {
        this.marketSize1.isOn = marketSize.ms1;
        this.marketSize2.isOn = marketSize.ms2;
        this.marketSize3.isOn = marketSize.ms3;
    }
    public void SetCompetitivePosture((bool cp1, bool cp2, bool cp3, bool cp4, bool cp5) cp)
    {
        competitivePosture1.isOn = cp.cp1;
        competitivePosture2.isOn = cp.cp2;
        competitivePosture3.isOn = cp.cp3;
        competitivePosture4.isOn = cp.cp4;
        competitivePosture5.isOn = cp.cp5;
    }
    public void SetDistinctiveCompetencies((bool dc1, bool dc2, bool dc3, bool dc4, bool dc5) dc)
    {
        distictiveComp1.isOn = dc.dc1;
        distictiveComp2.isOn = dc.dc2;
        distictiveComp3.isOn = dc.dc3;
        distictiveComp4.isOn = dc.dc4;
        distictiveComp4.isOn = dc.dc5;
    }
    public void SetBusinessPartnerDiversity((bool bpd1, bool bpd2, bool bpd3) bp)
    {
        businesspartnerDiversity1.isOn = bp.bpd1;
        businesspartnerDiversity2.isOn = bp.bpd2;
        businesspartnerDiversity3.isOn = bp.bpd3;
    }
    public void SetContractPriorities((bool cp1, bool cp2, bool cp3, bool cp4) cp)
    {
        contractPriorities1.isOn = cp.cp1;
        contractPriorities2.isOn = cp.cp2;
        contractPriorities3.isOn = cp.cp3;
        contractPriorities4.isOn = cp.cp4;
    }
    public void SetAccountingStrategies((bool as1, bool as2, bool as3) ast)
    {
        accountingStrategies1.isOn = ast.as1;
        accountingStrategies2.isOn = ast.as2;
        accountingStrategies3.isOn = ast.as3;
    }
    public void SetGrowthStrategies((bool gs1, bool gs2, bool gs3) gs)
    {
        growthStrategies1.isOn = gs.gs1;
        growthStrategies2.isOn = gs.gs2;
        growthStrategies3.isOn = gs.gs3;
    }
    public void SetDevelopmentStrategies((bool ds1, bool ds2, bool ds3) ds)
    {
        developmentStrategies1.isOn = ds.ds1;
        developmentStrategies2.isOn = ds.ds2;
        developmentStrategies3.isOn = ds.ds3;
    }


    public void DisableEditation()
    {
        marketSize1.interactable = false;
        marketSize2.interactable = false;
        marketSize3.interactable = false;

        competitivePosture1.interactable = false;
        competitivePosture2.interactable = false;
        competitivePosture3.interactable = false;
        competitivePosture4.interactable = false;
        competitivePosture5.interactable = false;

        distictiveComp1.interactable = false;
        distictiveComp2.interactable = false;
        distictiveComp3.interactable = false;
        distictiveComp4.interactable = false;
        distictiveComp5.interactable = false;

        businesspartnerDiversity1.interactable = false;
        businesspartnerDiversity2.interactable = false;
        businesspartnerDiversity3.interactable = false;

        contractPriorities1.interactable = false;
        contractPriorities2.interactable = false;
        contractPriorities3.interactable = false;
        contractPriorities4.interactable = false;

        accountingStrategies1.interactable = false;
        accountingStrategies2.interactable = false;
        accountingStrategies3.interactable = false;

        growthStrategies1.interactable = false;
        growthStrategies2.interactable = false;
        growthStrategies3.interactable = false;

        developmentStrategies1.interactable = false;
        developmentStrategies2.interactable = false;
        developmentStrategies3.interactable = false;
    }
}
