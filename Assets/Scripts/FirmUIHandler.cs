using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirmUIHandler : MonoBehaviour
{
    
    public InputField firmNameIF;
    public InputField firmDescriptionIF;

    private FirmManager firmManager;

    private void Awake()
    {
        firmManager = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject().GetComponent<FirmManager>();
        firmManager.SetFirmUIHandler(this);
    }
    // Updating gui according data on start
    void Start()
    {
        firmNameIF.text = firmManager.GetFirmName();
        firmDescriptionIF.text = firmManager.GetFirmDescription();
    }

    public void SetFirmName()
    {
        firmManager.SetFirmName(firmNameIF.text);
        
    }

    public void SetFirmDescription()
    {
        firmManager.SetFirmDescription(firmDescriptionIF.text);
    }

    public void SetUIFirmName(string firmName)
    {
        firmNameIF.text = firmName;
    }

    public void SetUIFirmDescription(string firmDescription)
    {
        firmDescriptionIF.text = firmDescription;
    }



}
