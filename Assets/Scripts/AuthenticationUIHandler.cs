using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticationUIHandler : MonoBehaviour {


    public GameObject SignPanel;
    public GameObject SignUpPanel;
    public GameObject SignInPanel;
    public GameObject LoggedInPanel;
    public Text InfoText;


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
        DisableAllPAnels();
        SignPanel.SetActive(true);
    }

    public void ShowSignUpPanel()
    {
        DisableAllPAnels();
        SignUpPanel.SetActive(true);
    }

    public void ShowSignInPanel()
    {
        DisableAllPAnels();
        SignInPanel.SetActive(true);
    }


    public void ShowLoggedInPanel()
    {
        DisableAllPAnels();
        LoggedInPanel.SetActive(true);
    }


    public void DisableAllPAnels()
    {
        InfoText.text = "666666";
        SignPanel.SetActive(false);
        SignInPanel.SetActive(false);
        SignUpPanel.SetActive(false);
        LoggedInPanel.SetActive(false);
    }

    public void ChangeWarningText(string text)
    {
        InfoText.text = text;
    }

        
    
}
