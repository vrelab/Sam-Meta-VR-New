using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserManager : MonoBehaviour
{
    public static UserManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(this);
        }
    }
    public bool isUserSignIn;
    public string myUserName;
    public Button loginBtnInSettingPannel, loginBtnInStartPannel;
    public TextMeshProUGUI loginBtnTextInSettingPannel, loginBtnTextInStartPannel;
    public Camera mainCamera;

    public GameObject left,right;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
