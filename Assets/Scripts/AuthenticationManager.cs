using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;

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


    //GETTERS & SETTERS
    public string GetConfirmPassword() { return confirmPassword; }

    public string GetUserID() { return user.UserId; }


    public void SetEmail(InputField inputEmail) { email = inputEmail.text; }
    public void SetPassword(InputField inputPassword) { password = inputPassword.text; }
    public void SetConfirmPassword(InputField inputConfirmPassword)
    {
        confirmPassword = inputConfirmPassword.text;
        Debug.Log(GetConfirmPassword());
    }

    public void SetPlayerManager(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
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
                Debug.Log("Signed out " + user.UserId);
                authForm.ShowSignPanel();
                playerManager.SetPlayerID("");
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
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

            Debug.Log("passwords are not equal");
            return;

        }
        Debug.Log(password);

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
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
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);


        });

    }

    public void SignOutUser()
    {
        playerManager.SetPlayerID("");
        auth.SignOut();

    }

    public void ShowUserName()
    {
        authForm.UserName.text = user.Email;
    }
    
}
