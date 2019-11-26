using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmitDataUIHandler : MonoBehaviour
{   
    //VARIABLES
   // public Text warningText;  WARNIGN MESSAGES NOT IMPLEMENTED YET
    public Button submitButton;

    public GameObject submitingDataImage;

    private SubmitDataManager submitDataManager;
    private GameObject myLocalPlayerObject;

    //GETTERS & SETTERS

    // Start is called before the first frame update
    void Start()
    {
        myLocalPlayerObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        submitDataManager = myLocalPlayerObject.gameObject.GetComponent<SubmitDataManager>();
        submitDataManager.SetSubmitDataUIHandler(this);

        if (submitDataManager.GetSubmitData())
        {
            EnableSubmitDataImage();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


   public void SubmitData()
    {
        submitDataManager.SubmitData();
    }

    public void EnableSubmitDataImage()
    {
        submitingDataImage.SetActive(true);
    }
    public void DisableSubmitDataImage()
    {
        submitingDataImage.SetActive(false);
    }


}
