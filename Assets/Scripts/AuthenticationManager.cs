using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Firebase;
using System;

/// <summary>
/// This class have been implemented according documentation provided by firebase for Unity projects.
/// https://firebase.google.com/docs/auth/unity/start
/// </summary>

public class AuthenticationManager : MonoBehaviour {
    /// <summary>
    /// This class conatians functionality needed for authentization
    /// </summary>

    //VARIABLES
    private AuthenticationUIHandler authForm;
    private ConnectionManager playerManager;

    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    //This variables are obtained from UI 
    private string email;
    private string password;
    private string confirmPassword;

    //private string warningText = "";
    //private bool warningTextChange = false; 

    //GETTERS & SETTERS
    public string GetConfirmPassword() { return confirmPassword; }
    public string GetUserID() { return user.UserId; }

    public void SetPlayerManager(ConnectionManager playerManager) { this.playerManager = playerManager; }
    public void SetEmail(InputField inputEmail) { email = inputEmail.text; }
    public void SetPassword(InputField inputPassword) { password = inputPassword.text; }
    public void SetConfirmPassword(InputField inputConfirmPassword){ confirmPassword = inputConfirmPassword.text; }

    
    /// <summary>
    /// Obtainging reference on UI handler responsible for authentication UI
    /// </summary>
    private void Awake()
    {
        authForm = this.gameObject.GetComponentInChildren<AuthenticationUIHandler>();
        //Debug.Log("authForm" + authForm.name);


    }

    /// <summary>
    /// Firebase is being intialized. For more information see documentation for firebase used in Unity project - authentication part.
    /// https://firebase.google.com/docs/auth/unity/start
    /// </summary>
    private void Start()
    {
        InitializeFirebase();

        auth.SignOut(); //remove if not playing from editor / multiple instances of game
    }

    /// <summary>
    /// When application is closed players is automaticaly signed out
    /// </summary>
    private void OnApplicationQuit()
    {
        auth.SignOut();
    }

    //METHODS   
    /// <summary>
    /// Firebase initialization. For more information see Firebase documentation.
    /// </summary>
    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    /// <summary>
    /// This method is called when player returns to menu
    /// </summary>
    private void OnDestroy()
    {
        if (auth != null)
        {
            auth.StateChanged -= AuthStateChanged;
        }
    }

    /// <summary>
    /// Invoked when authentication state is changed  - user signed in/out etc.
    /// For more inforamtion see Firebase documentation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventArgs"> </param>
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
    /// <summary>
    /// Tries to sign up user if it is possible. Otherwise shows warning text why this was not possible such as wuser already exists.
    /// Form more information see documentation for Firebase.
    /// </summary>

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

    /// <summary>
    /// Tries to sign in user if it is possible. Otherwise shows warning text why this was not possible such as wrong password. 
    /// Form more information see documentation for Firebase.
    /// </summary>
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
    
    /// <summary>
    /// Deleting input fields and variables obtained from currently logged user and signing auoutt this user. 
    /// </summary>
    public void SignOutUser()
    {
        authForm.EnableWarningTextChange("");
        playerManager.SetPlayerFirebaseID("");
        playerManager.SetPlayerGameID("");
        auth.SignOut();

    }

    /// <summary>
    /// Shows username in UI panel at the top of the screen
    /// </summary>
    public void ShowUserName()
    {   
        authForm.UserName.text = auth.CurrentUser.Email;
    }

    /// <summary>
    /// Sends password reseting link on email adress if it is obtained from UI
    /// </summary>
    public void SentPasswordRessetEmail()
    {
        if(email != "")
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
