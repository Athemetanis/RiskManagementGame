using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticationUIHandler : MonoBehaviour {


    public GameObject signPanel;
    public GameObject signUpPanel;
    public GameObject signInPanel;
    public GameObject loggedInPanel;
    public Text infoText;



    public Text UserName;

    // Use this for initialization
    void Start() {
        UserName.text = "";
    }

    // Update is called once per frame
    void Update() {

    }

    public void ShowSignPanel()
    {
        DisableAllPanels();
        signPanel.SetActive(true);
    }

    public void ShowSignUpPanel()
    {
        DisableAllPanels();
        signUpPanel.SetActive(true);
    }

    public void ShowSignInPanel()
    {
        DisableAllPanels();
        signInPanel.SetActive(true);
    }


    public void ShowLoggedInPanel()
    {
        DisableAllPanels();
        loggedInPanel.SetActive(true);
    }


    public void DisableAllPanels()
    {
        infoText.text = "666666";
        signPanel.SetActive(false);
        signInPanel.SetActive(false);
        signUpPanel.SetActive(false);
        loggedInPanel.SetActive(false);
    }

    public void ChangeWarningText(string text)
    {
        
        infoText.text = text;
        infoText.gameObject.SetActive(false);

    }

        
    
}
