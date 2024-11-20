using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Database;
using Proyecto26;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Extensions;
using UnityEngine.SceneManagement;

public class DataSaver : MonoBehaviour
{
    public static DataSaver Instance;
    public TMP_InputField username;
    public TMP_InputField mobileNo;
    public TMP_InputField password;
    public TMP_InputField confirmPassword;

    public TMP_InputField loginMobileNumber;
    public TMP_InputField loginPassword;
    public TMP_Text warningRegisteredUserText;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;
    public DataOfUser[] userDatas;

    private DatabaseReference dbRef;
    private DependencyStatus dependencyStatus;
    private string firebaseDatabaseURL = "https://fir-authtest-766ed-default-rtdb.asia-southeast1.firebasedatabase.app/";

    public List<DataOfUser> ofUsers = new List<DataOfUser>();
    public DataOfUser userData = new DataOfUser();
    private DataOfUser data = new DataOfUser();
    bool isMobileNumberExists;

    public string userName;
    public string characterVersion = "VrNetworkplayer_V1";
    public GameObject loadingScreen, characterSelectionScreen;
    //public NetworkManager networkManager;
    //public NetworkPlayerSpawner playerSpawner;
    //public Canvas logInCanvas;

    private void Awake()
    {
#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
        Debug.unityLogger.logEnabled = false;
#endif
        OVRPlugin.suggestedCpuPerfLevel = OVRPlugin.ProcessorPerformanceLevel.Boost;
        OVRPlugin.systemDisplayFrequency = 72.0f;
        OVRPlugin.suggestedGpuPerfLevel = OVRPlugin.ProcessorPerformanceLevel.Boost;

        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this.gameObject);

        FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                dbRef = FirebaseDatabase.DefaultInstance.RootReference;
                LoadData(); // Fetch data when Firebase is initialized
                Debug.Log("Database instance fetch");
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    public void SaveData()
    {
        userData.userName = username.text;
        userData.password = password.text;
        userData.mobileNumber = mobileNo.text;

        for (int i = 0; i < ofUsers.Count; i++)
        {
            if (ofUsers[i].mobileNumber == userData.mobileNumber)
            {
                warningRegisteredUserText.text = "Mobile number already a registered user";
                ClearRegisterFeilds();
                isMobileNumberExists = true;
                break;
            }
            else
            {
                isMobileNumberExists = false;

                if (password.text == confirmPassword.text)
                    AddDataToDatabase();
                else
                    warningRegisteredUserText.text = "Incorrect Password";
            }
        }

    }


    void AddDataToDatabase()
    {
        if (!isMobileNumberExists)
        {
            isMobileNumberExists = true;

            if (userData.userName == "" || userData.mobileNumber == "")
                warningRegisteredUserText.text = "please enter proper details";

            if (userData.userName != "" && userData.mobileNumber != "")
            {
                string json = JsonUtility.ToJson(userData);

                if (dependencyStatus == DependencyStatus.Available)
                {
                    dbRef.Child("User").Child(userData.userName).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
                    {
                        if (task.IsCompleted)
                        {
                            Debug.Log("Data successfully saved: " + json);
                            LoadData();
                            userName = userData.userName;
                            characterSelectionScreen.SetActive(true);
                            //StartCoroutine(LoadYourAsyncScene());
                            //logInCanvas.enabled = false;
                            //networkManager.enabled = true;
                            //playerSpawner.enabled = true;
                        }
                        else if (task.IsFaulted)
                        {
                            Debug.LogError("Error saving data: " + task.Exception);
                        }
                    });
                }

                UIManager.instance.LoginScreen();
                ClearRegisterFeilds();
            }
        }
    }


    void LoadData(Action callBack = null)
    {
        dbRef.Child("User").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error retrieving data");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                ofUsers.Clear();
                foreach (DataSnapshot user in snapshot.Children)
                {
                    IDictionary dictUser = (IDictionary)user.Value;
                    var dummymap = new DataOfUser
                    {
                        userName = dictUser["userName"].ToString(),
                        password = dictUser["password"].ToString(),
                        mobileNumber = dictUser["mobileNumber"].ToString()
                    };
                    ofUsers.Add(dummymap);
                }
                userDatas = ofUsers.ToArray();
            }
        });
        callBack?.Invoke();
    }


    public void CheckData()
    {
        var mobileNumber = loginMobileNumber.text;
        var password = loginPassword.text;
        LoadData(() =>
        {
            for (int i = 0; i < ofUsers.Count; i++)
                if (ofUsers[i].mobileNumber == mobileNumber)
                {
                    if (ofUsers[i].password == password)
                    {
                        warningLoginText.text = "";
                        confirmLoginText.text = "Successfully logged in";
                        userName = ofUsers[i].userName;
                        characterSelectionScreen.SetActive(true);
                        //StartCoroutine(LoadYourAsyncScene());
                        //logInCanvas.enabled = false;
                        //networkManager.enabled = true;
                        //playerSpawner.enabled = true;
                        //joystickCanvas.SetActive(true);
                    }
                    else
                    {
                        confirmLoginText.text = "";
                        warningLoginText.text = "Incorrect password";
                        ClearLoginFeilds();
                    }
                    break;
                }
                else
                {
                    confirmLoginText.text = "";
                    warningLoginText.text = "Not a valid user.Register as new user ";
                    ClearLoginFeilds();
                }
        });
        //for (int i = 0; i < ofUsers.Count; i++)
        //{
        //    warningLoginText.text = "";
        //    confirmLoginText.text = "";

        //    if (ofUsers[i].mobileNumber == mobileNumber)
        //    {
        //        RestClient.Get<DataOfUser>(firebaseDatabaseURL + "/Users/" + ofUsers[i].userName + ".json").Then(response =>
        //        {
        //            data = response;
        //            if (data.password == password)
        //            {
        //                warningLoginText.text = "";
        //                confirmLoginText.text = "Successfully logged in";
        //                //logInCanvas.enabled = false;
        //                //networkManager.enabled = true;
        //                //playerSpawner.enabled = true;
        //                //joystickCanvas.SetActive(true);
        //            }
        //            else
        //            {
        //                confirmLoginText.text = "";
        //                warningLoginText.text = "Incorrect password";
        //                ClearLoginFeilds();
        //            }
        //        });
        //    }
        //    else
        //    {
        //        confirmLoginText.text = "";
        //        warningLoginText.text = "Not a valid user.Register as new user ";
        //        ClearLoginFeilds();
        //    }
        //}
    }

    public void SelectedSpawnCharacter(string character)
    {
        characterVersion = character;
        loadingScreen.SetActive(true);
        StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Metaverse");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void ClearLoginFeilds()
    {
        loginMobileNumber.text = "";
        loginPassword.text = "";
    }


    public void ClearRegisterFeilds()
    {
        username.text = "";
        mobileNo.text = "";
        password.text = "";
        confirmPassword.text = "";
    }
}


[Serializable]
public class DataOfUser
{
    public string userName;
    public string password;
    public string mobileNumber;
}