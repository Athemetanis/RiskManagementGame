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

    public Button confirmNameChangeButton;


    private FirmManager firmManager;

    // Updating gui according saved data on start
    void Start()
    {
        firmManager = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject().GetComponent<FirmManager>();
        firmManager.SetFirmUIHandler(this);
        firmNameIF.text = firmManager.GetFirmName();
        firmDescriptionIF.text = firmManager.GetFirmDescription();
    }

    // If gui changed call this - try to set new firmName/description
    public void SetFirmName(TMP_InputField firmName)
    {
        firmManager.TryToChangeFirmName(firmName.text);
        //zavolaj na Firmmanagerovi funkciu ktor8 skusi zmenit firm name
    }

    public void SetFirmDescription(TMP_InputField firmDescription)
    {
        Debug.Log(firmDescription.text);
        firmManager.ChangeFirmDescription(firmDescription.text);
        
        //firmManager.CmdSetFirmsDescription("bla bla");
    }
    
    //------nastavenie hodnoty do GUI inputfieldov
    public void SetUIFirmName(string firmName)
    {
        firmNameIF.text = firmName;
    }

    public void SetUIFirmDescription(string firmDescription)
    {
        firmDescriptionIF.text = firmDescription;
        
    }

    public void SetUIFirmNameOld(string oldFirmName)
    {
        Debug.Log("nastavujem stare meno");
        firmNameIF.text = oldFirmName;
        errorMessageText.gameObject.SetActive(true);        
    }


    public void DisableChangingName()
    {
        firmNameIF.interactable = false;
        confirmNameChangeButton.gameObject.SetActive(false);

    }



}
