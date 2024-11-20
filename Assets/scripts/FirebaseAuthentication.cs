using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using System;

public class FirebaseAuthentication : MonoBehaviour
{
    [Header("FireBase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;

    [Space]
    [Header("Signin InputField")]
    public TMP_InputField _UserId;
    public TMP_InputField _SigninPassword;
    public string email => _UserId.text;
    public string userPassword => _SigninPassword.text;

    [Space]
    [Header("Reg InputField")]
    public TMP_InputField _userName;
    public TMP_InputField _password, _verifyPassword,_nameField;
    public string userName => _userName.text;
    public string password => _password.text;
    public string verifyPassword => _verifyPassword.text;
    public string nameField => _nameField.text;

    public Canvas signInCanvas;
    public NetworkManager networkManager;
    public VRnetworkplayerspawner vRnetworkplayerspawner; 

    private void Awake()
    {
        
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(Task =>
        {
            dependencyStatus = Task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                IntializeFirebase();
            }
            else
            {
                Debug.Log("Cannot resolve dependency"+dependencyStatus);
            }
        });
    }

    void IntializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;

        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender,EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if(!signedIn && user != null)
            {
                Debug.Log("signed out" + user.UserId);
            }

            user = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log("signed in" + user.UserId);
            }
        }
    }

    public void LogIn()
    {
        StartCoroutine(LogInAsync(email, userPassword));
    }
    private IEnumerator LogInAsync(string email,string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            Debug.LogError(loginTask.Exception);

            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;

            string failedMessage = "Login Faild! Bzs ";
            switch (authError)
            {
                case AuthError.InvalidEmail:
                    failedMessage += "invalid Email";
                    break;
                case AuthError.WrongPassword:
                    failedMessage += "invalid WrongPassword";
                    break;
                case AuthError.MissingEmail:
                    failedMessage += "invalid MissingEmail";
                    break;
                case AuthError.MissingPassword:
                    failedMessage += "invalid MissingPassword";
                    break;
            }

            Debug.LogError(failedMessage);
        }
        else
        {
            user = loginTask.Result.User;

            Debug.LogFormat("{0} you are successfully logged In", user.DisplayName);
            UserManager.Instance.myUserName = user.DisplayName;
            networkManager.enabled = true;
            vRnetworkplayerspawner.enabled = true;
            signInCanvas.enabled = false;
            UserManager.Instance.isUserSignIn = true;
        }
    }
    public void Register()
    {
        StartCoroutine(RegisterAsync(nameField, userName, password, verifyPassword));
    }

    private IEnumerator RegisterAsync(string name,string email, string password,string confirmPassword)
    {
        if (name == "")
        {
            Debug.LogError("empty username");
        }
        if (email == "")
        {
            Debug.LogError("empty email");
        }
        if (password == "")
        {
            Debug.LogError("empty password");
        }
        if (confirmPassword == "")
        {
            Debug.LogError("empty confirmPassword");
        }
        if (password != confirmPassword)
        {
            Debug.LogError("empty username");
        }
        else
        {
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(() => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                Debug.LogError(registerTask.Exception);

                FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;

                string failedMessage = "Login Faild! Bzs ";
                switch (authError)
                {
                    case AuthError.InvalidEmail:
                        failedMessage += "invalid Email";
                        break;
                    case AuthError.WrongPassword:
                        failedMessage += "invalid WrongPassword";
                        break;
                    case AuthError.MissingEmail:
                        failedMessage += "invalid MissingEmail";
                        break;
                    case AuthError.MissingPassword:
                        failedMessage += "invalid MissingPassword";
                        break;
                }

                Debug.LogError(failedMessage);
            }
            else
            {
                user = registerTask.Result.User;

                UserProfile userProfile = new UserProfile { DisplayName = name };

                var updateProfileTasak = user.UpdateUserProfileAsync(userProfile);

                yield return new WaitUntil(() => updateProfileTasak.IsCompleted);

                if (updateProfileTasak.Exception != null)
                {
                    user.DeleteAsync();
                    Debug.LogError(updateProfileTasak.Exception);

                    FirebaseException firebaseException = updateProfileTasak.Exception.GetBaseException() as FirebaseException;
                    AuthError authError = (AuthError)firebaseException.ErrorCode;

                    string failedMessage = "Login Faild! Bzs ";
                    switch (authError)
                    {
                        case AuthError.InvalidEmail:
                            failedMessage += "invalid Email";
                            break;
                        case AuthError.WrongPassword:
                            failedMessage += "invalid WrongPassword";
                            break;
                        case AuthError.MissingEmail:
                            failedMessage += "invalid MissingEmail";
                            break;
                        case AuthError.MissingPassword:
                            failedMessage += "invalid MissingPassword";
                            break;
                        default:
                            failedMessage += "failed update";
                            break;
                    }

                    Debug.LogError(failedMessage);
                }
                else
                {
                    Debug.Log("reg successfull welcome " + user.DisplayName);
                    UserManager.Instance.myUserName = user.DisplayName;
                    networkManager.enabled = true;
                    vRnetworkplayerspawner.enabled = true;
                    signInCanvas.enabled = false;
                    UserManager.Instance.isUserSignIn = true;
                }
            }
        }
    }
}
