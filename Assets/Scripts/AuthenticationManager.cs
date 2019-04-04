using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using System;
//using System.Windows.Forms;

public class AuthenticationManager : MonoBehaviour {
    /// <summary>
    /// This class conatians functionality needed for authentization
    /// </summary>

    //VARIABLES
    private AuthenticationUIHandler authForm;
    private PlayerManager playerManager;

    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    private string email;
    private string password;
    private string confirmPassword;

    private string warningText;
    //GETTERS & SETTERS
    public string GetConfirmPassword() { return confirmPassword; }
    public string GetUserID() { return user.UserId; }

    public void SetPlayerManager(PlayerManager playerManager) { this.playerManager = playerManager; }
    public void SetEmail(InputField inputEmail) { email = inputEmail.text; }
    public void SetPassword(InputField inputPassword) { password = inputPassword.text; }
    public void SetConfirmPassword(InputField inputConfirmPassword)
    {
        confirmPassword = inputConfirmPassword.text;
        Debug.Log(GetConfirmPassword());
    }

    

    private void Awake()
    {
        authForm = this.gameObject.GetComponentInChildren<AuthenticationUIHandler>();
        Debug.Log("authForm" + authForm.name);

    }


    private void Start()
    {
        InitializeFirebase();
    }


    //METHODS      
    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);

    }

    private void OnDestroy()
    {
        if (auth != null)
        {
            auth.StateChanged -= AuthStateChanged;
        }

    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                authForm.InfoText.text = "1111";
                Debug.Log("Signed out " + user.UserId);
                authForm.ShowSignPanel();
                playerManager.SetPlayerID("");
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                authForm.InfoText.text = "222222";
                Debug.Log("Signed in " + user.UserId);
                authForm.ShowLoggedInPanel();
                Debug.Log("OK-1");
                ShowUserName();
                Debug.Log("OK");
                playerManager.SetPlayerID(user.UserId);
                Debug.Log("OK2");

            }
        }
    }

    public void TrySingUp()
    {
        if (string.Equals(password, confirmPassword) == false)
        {
            authForm.InfoText.text = "Passwords are not equal";
            Debug.Log("passwords are not equal");
            return;

        }
        Debug.Log(password);

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {   
                //Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                authForm.InfoText.text = "Creating user was cannceled";
                return;
            }
            if (task.IsFaulted)
            {
                authForm.InfoText.text = "Creating user error: " + task.Exception;
                //Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                /*FirebaseException error = task.Exception.InnerExceptions[0] as FirebaseException;
                string errorMsg = error.ToString();
                authForm.InfoText.text = "errorMsg";*/
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

        });

    }

    public void TrySignIn()
    {            
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                authForm.ChangeWarningText("Sign in canceled.") ;
                //Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                //authForm.ChangeWarningText("Sign in error: " + task.Exception);
                //Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                /*FirebaseException error = task.Exception.InnerExceptions[0] as FirebaseException;
                string errorMsg = error.ToString();
                authForm.InfoText.text = "errorMsg";*/
                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    string authErrorCode = "";
                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        authErrorCode = String.Format("AuthError.{0}: ",
                          ((Firebase.Auth.AuthError)firebaseEx.ErrorCode).ToString());
                    }
                    Debug.Log(authErrorCode + exception.ToString());
                    authForm.ChangeWarningText(exception.ToString());
                    warningText = exception.ToString();

                    //authForm.InfoText.Invoke( (MethodInvoker)delegate { authForm.InfoText.text = exception.ToString();  });
                    Invoke("ChangeText", 1 );
                }
                return;
            }
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }

    void ChangeText()
    {
        authForm.ChangeWarningText(warningText);
    }




    public void SignOutUser()
    {
        authForm.InfoText.text = "33333 ";
        playerManager.SetPlayerID("");
        playerManager.SetPlayerGameID("");
        auth.SignOut();

    }

    public void ShowUserName()
    {   
        authForm.UserName.text = user.Email;
    }



    public void SentPasswordRessetEmail()
    {
        if (user != null)
        {
            auth.SendPasswordResetEmailAsync(email).ContinueWith(task => {
                if (task.IsCanceled)
                {
                authForm.InfoText.text = "SendPasswordResetEmailAsync was canceled.";
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
                    /*FirebaseException error = task.Exception.InnerExceptions[0] as FirebaseException;
                    string errorMsg = error.ToString();
                    authForm.InfoText.text = "errorMsg";*/
                    return;
                }
                authForm.InfoText.text = "Password reset email sent successfully.";
                Debug.Log("Password reset email sent successfully.");
            });
        }


    }
    
}
