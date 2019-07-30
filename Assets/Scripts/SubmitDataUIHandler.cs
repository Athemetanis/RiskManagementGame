using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmitDataUIHandler : MonoBehaviour
{   
    //VARIABLES
    public Text warningText;
    public Button submitButton;

    private SubmitDataManager submitDataManager;
    private GameObject myLocalPlayerObject;

    //GETTERS & SETTERS

    // Start is called before the first frame update
    void Start()
    {
        myLocalPlayerObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        submitDataManager = myLocalPlayerObject.gameObject.GetComponent<SubmitDataManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void SubmitData()
    {
        submitDataManager.MoveToNextQuarter();
    }





}
