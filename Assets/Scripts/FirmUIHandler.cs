using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirmUIHandler : MonoBehaviour
{
    public InputField firmNameIF;
    public InputField firmDescriptionIF;
    public Text errorMessageText;


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
    public void SetFirmName(InputField firmName)
    {
        firmManager.TryToChangeFirmName(firmName.text);
        //zavolaj na Firmmanagerovi funkciu ktor8 skusi zmenit firm name
    }

    public void SetFirmDescription(InputField firmDescription)
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



}
