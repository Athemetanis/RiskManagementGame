using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using System;


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

    //private string warningText = "";
    //private bool warningTextChange = false; 

    //GETTERS & SETTERS
    public string GetConfirmPassword() { return confirmPassword; }
    public string GetUserID() { return user.UserId; }

    public void SetPlayerManager(PlayerManager playerManager) { this.playerManager = playerManager; }
    public void SetEmail(InputField inputEmail) { email = inputEmail.text; }
    public void SetPassword(InputField inputPassword) { password = inputPassword.text; }
    public void SetConfirmPassword(InputField inputConfirmPassword){ confirmPassword = inputConfirmPassword.text; }

    

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
                Debug.Log("Signed out " + user.UserId);
                authForm.ShowSignPanel();
                playerManager.SetPlayerFirebaseID("");
                authForm.EnableWarningTextChange("");
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                authForm.ShowLoggedInPanel();
                ShowUserName();
                playerManager.SetPlayerFirebaseID(user.UserId);
                authForm.EnableWarningTextChange("");
                

            }
        }
    }

    public void TrySingUp()
    {
        if (string.Equals(password, confirmPassword) == false)
        {
            authForm.EnableWarningTextChange("Passwords are not equal");
            return;
        }
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                authForm.EnableWarningTextChange("Creating user was cannceled.");
                return;
            }
            if (task.IsFaulted)
            {
                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    string authErrorCode = "";
                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        authErrorCode = String.Format("AuthError.{0}: ",
                          ((Firebase.Auth.AuthError)firebaseEx.ErrorCode).ToString());
                    }
                    string code = exception.ToString().Substring(28);
                    Debug.Log(code);
                    authForm.EnableWarningTextChange(code);
                }
                return;
            }
            
            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
            newUser.DisplayName, newUser.UserId);
            authForm.EnableWarningTextChange("");

        });

        
        

    }

    public void TrySignIn()
    {            
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                authForm.EnableWarningTextChange("Sign in canceled.") ;
                //Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    string authErrorCode = "";
                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        authErrorCode = String.Format("AuthError.{0}: ",
                          ((Firebase.Auth.AuthError)firebaseEx.ErrorCode).ToString());
                    }
                    string code = exception.ToString().Substring(28);
                    Debug.Log(code);
                    authForm.EnableWarningTextChange(code);
                }
                return;
            }
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            authForm.EnableWarningTextChange("");
        });
    }


    public void SignOutUser()
    {
        authForm.EnableWarningTextChange("");
        playerManager.SetPlayerFirebaseID("");
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
                    authForm.EnableWarningTextChange("Sending password reset email was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                    {
                        string authErrorCode = "";
                        Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                        if (firebaseEx != null)
                        {
                            authErrorCode = String.Format("AuthError.{0}: ",
                              ((Firebase.Auth.AuthError)firebaseEx.ErrorCode).ToString());
                        }
                        string code = exception.ToString().Substring(28);
                        Debug.Log(code);
                        authForm.EnableWarningTextChange(code);
                    }
                    return;
                }
                authForm.EnableWarningTextChange("Password reset email sent successfully.");
                Debug.Log("Password reset email sent successfully.");
            });
        }
    }
    
}
