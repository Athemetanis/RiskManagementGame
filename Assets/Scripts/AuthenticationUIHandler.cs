﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticationUIHandler : MonoBehaviour {


    public GameObject signPanel;
    public GameObject signUpPanel;
    public GameObject signInPanel;
    public GameObject loggedInPanel;
    public GameObject authImage;
    public Text infoText;
    public AssincTextRewriter assincTextRewriter;



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
        assincTextRewriter.gameObject.SetActive(true);
        signUpPanel.SetActive(true);
    }

    public void ShowSignInPanel()
    {
        DisableAllPanels();
        assincTextRewriter.gameObject.SetActive(true);
        signInPanel.SetActive(true);
    }


    public void ShowLoggedInPanel()
    {        
        DisableAllPanels();
        authImage.SetActive(false);
        loggedInPanel.SetActive(true);
    }


    public void DisableAllPanels()
    {
        authImage.SetActive(true);
        EnableWarningTextChange("");
        assincTextRewriter.gameObject.SetActive(false);
        signPanel.SetActive(false);
        signInPanel.SetActive(false);
        signUpPanel.SetActive(false);
        loggedInPanel.SetActive(false);
    }

    public void EnableWarningTextChange(string text)
    {
        assincTextRewriter.SetWarningString(text);
        
    }

        
    
}
