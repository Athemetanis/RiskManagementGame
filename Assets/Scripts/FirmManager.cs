using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirmManager : MonoBehaviour
{
    
    //VARIABLES
    private string firmName;
    private string firmDescription;

    private FirmUIHandler firmUIHandler;

    //GETTERS & SETTERS

    public void SetFirmName(string firmName) { this.firmName = firmName; }
    public string GetFirmName() { return firmName; }
    public void SetFirmDescription(string firmDescription) { this.firmDescription = firmDescription; }
    public string GetFirmDescription() { return firmDescription; }
    public void SetFirmUIHandler(FirmUIHandler firmUIHandler) { this.firmUIHandler = firmUIHandler; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
